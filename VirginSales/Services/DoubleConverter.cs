using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

namespace Services
{
    public class DoubleConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                string cleaned = text.Replace(" ", ""); //some units have space between
                if (double.TryParse(cleaned, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                {
                    return result;
                }
            }
            throw new TypeConverterException(this, memberMapData, text, row.Context, "Conversion failed for double value.");
        }
    }
}
