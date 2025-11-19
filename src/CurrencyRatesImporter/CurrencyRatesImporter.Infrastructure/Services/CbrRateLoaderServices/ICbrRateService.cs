using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyRatesImporter.Infrastructure.Services
{
    public interface ICbrRateService
    {
        /// <summary>
        /// Полный процесс:
        /// 1) Загрузка курсов с ЦБ РФ
        /// 2) Сохранение в БД (Currency + Rate)
        /// 3) Генерация Excel-файла (YYYYMMDD.xlsx)
        /// </summary>
        Task<string> ProcessAsync(DateTime date);
    }
}
