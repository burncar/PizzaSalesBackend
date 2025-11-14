using CsvHelper;
using CsvHelper.Configuration;
using PizzaSalesBackend.Application.Intreface;
using PizzaSalesBackend.Model.Map;
using System.Globalization;

namespace PizzaSalesBackend.Application.Services
{
    public class CsvHelperService :ICsvHelper
    {
        public async Task<List<T>> CsvImportAsync<T, TMap>(string filePath) where TMap : ClassMap<T>
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap<TMap>();

            var records = csv.GetRecords<T>().ToList();
            return await Task.FromResult(records);
        }
    }
}
