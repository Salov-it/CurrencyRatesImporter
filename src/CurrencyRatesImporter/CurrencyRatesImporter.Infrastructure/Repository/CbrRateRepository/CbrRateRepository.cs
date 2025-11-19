using CurrencyRatesImporter.Infrastructure.Common.db;
using CurrencyRatesImporter.Infrastructure.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;


namespace CurrencyRatesImporter.Infrastructure.Repository.CbrRateRepository
{
    internal class CbrRateRepository : ICbrRateRepository
    {
        private readonly ISqlConnectionFactory _factory;

        public CbrRateRepository(ISqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task SaveRatesAsync(DateTime date, List<CbrRateXmlModel> rates)
        {
            using var conn = await _factory.CreateAsync();

            foreach (var rate in rates)
            {
                var currencyId = await GetOrCreateCurrencyAsync(conn, rate);

                await UpsertRateAsync(conn, currencyId, date, rate);
            }
        }

        private async Task<int> GetOrCreateCurrencyAsync(SqlConnection conn, CbrRateXmlModel model)
        {
            var sql = @"
                IF NOT EXISTS (SELECT 1 FROM Currency WHERE CharCode = @CharCode)
                BEGIN
                    INSERT INTO Currency (ID, NumCode, CharCode)
                    VALUES (@ID, @NumCode, @CharCode)
                END

                SELECT CurrencyID FROM Currency WHERE CharCode = @CharCode
                ";

            return await conn.ExecuteScalarAsync<int>(
                sql,
                new
                {
                    ID = model.CharCode, // XML.ID у нас нет → берём CharCode
                    model.NumCode,
                    model.CharCode
                }
            );
        }

        private async Task UpsertRateAsync(SqlConnection conn, int currencyId, DateTime date, CbrRateXmlModel model)
        {
            var sql = @"
                IF EXISTS (SELECT 1 FROM Rate WHERE CurrencyID = @CurrencyID AND [Date] = @Date)
                BEGIN
                    UPDATE Rate 
                    SET Nominal = @Nominal,
                        Value = @Value
                    WHERE CurrencyID = @CurrencyID AND [Date] = @Date
                END
                ELSE
                BEGIN
                    INSERT INTO Rate (CurrencyID, [Date], Nominal, Value)
                    VALUES (@CurrencyID, @Date, @Nominal, @Value)
                END
                ";

            await conn.ExecuteAsync(
                sql,
                new
                {
                    CurrencyID = currencyId,
                    Date = date,
                    model.Nominal,
                    model.Value
                }
            );
        }
    }

}
