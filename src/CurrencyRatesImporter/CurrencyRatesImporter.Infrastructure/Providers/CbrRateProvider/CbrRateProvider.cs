using CurrencyRatesImporter.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CurrencyRatesImporter.Infrastructure.Providers.CbrRateProvider
{
    public class CbrRateProvider : ICbrRateProvider
    {
        private const string CbrUrlTemplate = "https://www.cbr.ru/scripts/XML_daily.asp?date_req={0:dd/MM/yyyy}";

        private readonly HttpClient _http;

        public CbrRateProvider(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<CbrRateXmlModel>> GetRatesAsync(DateTime date, CancellationToken ct = default)
        {
            string url = string.Format(CbrUrlTemplate, date);

            string xml = await _http.GetStringAsync(url, ct);

            var doc = XDocument.Parse(xml);

            var result = new List<CbrRateXmlModel>();

            foreach (var valute in doc.Root!.Elements("Valute"))
            {
                try
                {
                    // Читаем строки
                    string? numCode = valute.Element("NumCode")?.Value;
                    string? charCode = valute.Element("CharCode")?.Value;
                    string? name = valute.Element("Name")?.Value;

                    if (string.IsNullOrWhiteSpace(numCode) ||
                        string.IsNullOrWhiteSpace(charCode) ||
                        string.IsNullOrWhiteSpace(name))
                    {
                        continue; // пропускаем битые записи
                    }

                    // Nominal
                    if (!int.TryParse(valute.Element("Nominal")?.Value, out int nominal) || nominal <= 0)
                        nominal = 1;

                    // Value
                    if (!TryParseCbrDecimal(valute.Element("Value")?.Value, out decimal value))
                        continue; // пропускаем неправильные элементы

                    // VunitRate
                    TryParseCbrDecimal(valute.Element("VunitRate")?.Value, out decimal vunit);

                    result.Add(new CbrRateXmlModel(
                        NumCode: numCode,
                        CharCode: charCode,
                        Nominal: nominal,
                        Name: name,
                        Value: value
                    ));
                }
                catch
                {
                    // Любая ошибка внутри одной валюты — не ломает весь список
                    continue;
                }
            }

            return result.OrderBy(x => x.CharCode).ToList();
        }

        private static bool TryParseCbrDecimal(string? input, out decimal value)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                value = 0;
                return false;
            }

            // ЦБ использует запятую, поэтому делаем замену
            input = input.Replace(",", ".");

            return decimal.TryParse(
                input,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out value);
        }
    }
}
