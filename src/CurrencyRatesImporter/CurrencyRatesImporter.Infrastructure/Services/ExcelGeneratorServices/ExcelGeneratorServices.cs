using ClosedXML.Excel;
using CurrencyRatesImporter.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyRatesImporter.Infrastructure.Services.ExcelGeneratorServices
{
    public class ExcelGeneratorServices : IExcelGeneratorServices
    {
        public async Task<string> GenerateAsync(DateTime date, List<CbrRateXmlModel> rates)
        {
            if (rates == null || rates.Count == 0)
                throw new ArgumentException("Список курсов пуст.", nameof(rates));

            var sorted = rates.OrderBy(r => r.CharCode).ToList();

            var fileName = $"{date:yyyyMMdd}.xlsx";
            var fullPath = Path.GetFullPath(fileName);

            try
            {
                // Попытка удалить файл, если существует
                if (File.Exists(fullPath))
                {
                    try
                    {
                        File.Delete(fullPath);
                    }
                    catch (IOException)
                    {
                        throw new Exception(
                            $"Файл '{fileName}' не может быть удалён. Возможно, он открыт в другой программе."
                        );
                    }
                    catch (UnauthorizedAccessException)
                    {
                        throw new Exception(
                            $"Нет прав для удаления файла '{fileName}'. Попробуйте запустить программу от администратора."
                        );
                    }
                }

                using var wb = new XLWorkbook();

                // Создаём лист на каждую валюту
                foreach (var baseCurrency in sorted)
                {
                    var ws = wb.Worksheets.Add(baseCurrency.CharCode);

                    // Заголовки
                    ws.Cell(1, 1).Value = "Валюта";
                    ws.Cell(1, 2).Value = $"Курс к {baseCurrency.CharCode}";
                    ws.Range(1, 1, 1, 2).Style.Font.Bold = true;

                    int row = 2;

                    foreach (var target in sorted)
                    {
                        decimal cross = baseCurrency.Value == 0
                            ? 0
                            : target.Value / baseCurrency.Value;

                        ws.Cell(row, 1).Value = target.CharCode;
                        ws.Cell(row, 2).Value = (double)decimal.Round(cross, 6);
                        ws.Cell(row, 2).Style.NumberFormat.Format = "0.000000";

                        row++;
                    }

                    ws.Columns().AdjustToContents();
                }

                
                try
                {
                    await Task.Run(() => wb.SaveAs(fullPath));
                }
                catch (IOException)
                {
                    throw new Exception(
                        $"Не удалось сохранить файл '{fileName}'. Он может быть открыт в другой программе или путь недоступен."
                    );
                }
                catch (UnauthorizedAccessException)
                {
                    throw new Exception(
                        $"Недостаточно прав для записи файла '{fileName}'."
                    );
                }

                return fullPath;
            }
            catch
            {
                throw;
            }
        }
    }
}
