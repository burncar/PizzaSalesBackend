using CsvHelper.Configuration;

namespace PizzaSalesBackend.Application.Intreface
{
    public interface ICsvHelper
    {
        Task<List<T>> CsvImportAsync<T, TMap>(string filePath) where TMap : ClassMap<T>;
    }
}
