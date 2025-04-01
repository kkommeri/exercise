using Services.Dto;

namespace Services
{
    public interface ISalesService
    {
        Task<IEnumerable<SalesDto>> GetSalesDataAsync();
        Task<List<SalesSummaryByProductDto>> GetSalesSummaryByProductAsync(List<SalesDto> sales);
    }
}