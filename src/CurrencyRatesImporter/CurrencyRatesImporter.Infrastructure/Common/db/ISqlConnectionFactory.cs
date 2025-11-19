using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyRatesImporter.Infrastructure.Common.db
{
    public interface ISqlConnectionFactory
    {
        Task<SqlConnection> CreateAsync();
    }
}
