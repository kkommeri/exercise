using Services.Dto;

namespace Services
{
    public interface ISalesService
    {
        Task<IEnumerable<SalesDataDto>> GetSalesDataAsync();
        Task<List<SalesSummaryByProductDto>> GetSalesSummaryByProductAsync(List<SalesDataDto> sales);
    }
}