using CurrencyRatesImporter.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyRatesImporter.Infrastructure.Repository.CbrRateRepository
{
    public interface ICbrRateRepository
    {
        /// <summary>
        /// Сохраняет котировки в базу:
        /// - добавляет валюты, если их нет
        /// - добавляет или обновляет курсы за дату
        /// </summary>
        Task SaveRatesAsync(DateTime date, List<CbrRateXmlModel> rates);
    }
}
