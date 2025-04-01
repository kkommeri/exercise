using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using NodaMoney;
using System.Globalization;

namespace Services.Dto
{
    public class SalesDto
    {
        public string Segment { get; set; }
        public string Country { get; set; }
        public string Product { get; set; }
        public string DiscountBand { get; set; }
        public double UnitsSold { get; set; }
        public Money ManufacturingPrice { get; set; }
        public Money SalePrice { get; set; }
        public DateOnly Date { get; set; }
    }

    public class SalesDtoMap : ClassMap<SalesDto>
    {
        public SalesDtoMap()
        {
            Map(m => m.Segment).Name("Segment");
            Map(m => m.Country).Name("Country");
            Map(m => m.Product).Name("Product");
            Map(m => m.DiscountBand).Name("Discount Band");
            Map(m => m.UnitsSold).Name("Units Sold").TypeConverter<DoubleConverter>();
            Map(m => m.ManufacturingPrice).Name("Manufacturing Price").TypeConverter<MoneyConverter>();
            Map(m => m.SalePrice).Name("Sale Price").TypeConverter<MoneyConverter>();
            Map(m => m.Date).Name("Date").TypeConverterOption.Format("MM/dd/yyyy");
        }
    }

    public class MoneyConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                text = "0"; //If we do not do this Money.Parse will throw invalid argument when the value is empty.
            }

            if (text.Contains("�"))
            {
                //This is a hack to remove the � character from the text, And it only happens when i load the file from the embedded resource for testing
                //for example line from embeeded resource => Government,Canada, Carretera , None ,1618.5,�3.00,�20.00,01/01/2014
                //NodaMoney will then throws the error => System.FormatException : � is an unknown currency symbol or code!
                text = text.Replace("�", "");
            }

            return Money.Parse(text);
        }
    }

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
