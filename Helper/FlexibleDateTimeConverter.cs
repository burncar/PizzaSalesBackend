using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

namespace PizzaSalesBackend.Helper
{
    public class FlexibleDateTimeConverter : DateTimeConverter
    {
        private static readonly string[] formats = new[]
  {
        "yyyy-MM-dd",
        "yyyy/dd/MM",
        "yyyy/MM/dd",
        "dd-MM-yyyy",
        "dd/MM/yyyy",
        "yyyy-MM-dd HH:mm:ss", 
        "dd/MM/yyyy HH:mm:ss"
    };

        public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
                return default(DateTime);

            // Try each format
            foreach (var fmt in formats)
            {
                if (DateTime.TryParseExact(text, fmt, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                    return dt;
            }

            // Fallback: try general parse (optional)
            if (DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.None, out var fallbackDt))
                return fallbackDt;

            throw new TypeConverterException(this, memberMapData, text, row.Context, $"Invalid date format: {text}");
        }
    }
}
