using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace PizzaSalesBackend.Helper
{
    public class FlexibleTimeSpanConverter : TimeSpanConverter
    {
        public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
                return TimeSpan.Zero;

            // Try direct parse (handles "11:38:36", "9:52:21", etc.)
            if (TimeSpan.TryParse(text, out var ts))
                return ts;

            // Try parsing as DateTime, then extract time part
            if (DateTime.TryParse(text, out var dt))
                return dt.TimeOfDay;

            throw new TypeConverterException(this, memberMapData, text, row.Context, $"Invalid time format: {text}");
        }
    }
}
