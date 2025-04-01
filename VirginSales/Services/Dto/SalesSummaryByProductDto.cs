namespace Services.Dto
{
    public class SalesSummaryByProductDto
    {
        public string Product { get; internal set; }
        public decimal TotalSalePrice { get; internal set; }
        public decimal TotalManufacturingPrice { get; internal set; }
    }
}