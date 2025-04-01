using CsvHelper.Configuration;
using NodaMoney;

namespace Services.Dto
{
    public class SalesDataDto
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

    public class SalesDataDtoMap : ClassMap<SalesDataDto>
    {
        public SalesDataDtoMap()
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
}
