using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Options;
using Services.Dto;
using System.Globalization;
using System.Text;

namespace Services
{
    public class SalesService : ISalesService
    {
        private SalesSettings _salesSettings;

        public SalesService(IOptions<SalesSettings> salesSettings)
        {
            _salesSettings = salesSettings.Value;
        }

        public async Task<IEnumerable<SalesDataDto>> GetSalesDataAsync()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                IgnoreBlankLines = true,
            };

            //using (var reader = new StreamReader("C:\\Users\\kkomm\\kkommeri@outlook.onedrive\\OneDrive\\exercise\\docs\\Data.csv", Encoding.GetEncoding(1252)))
            using (var reader = new StreamReader(_salesSettings.SalesDataPath, Encoding.GetEncoding(1252)))
            using (var csv = new CsvReader(reader, csvConfiguration))
            {
                csv.Context.RegisterClassMap<SalesDataDtoMap>();

                var records = csv.GetRecords<SalesDataDto>().ToList();
                return await Task.FromResult(records);
            }
        }

        public Task<List<SalesSummaryByProductDto>> GetSalesSummaryByProductAsync(List<SalesDataDto> sales)
        {
            var summary = from sale in sales
                        group sale by new {sale.Product } into productGroup
                        select new SalesSummaryByProductDto
                        {
                            Product = productGroup.Key.Product,
                            TotalSalePrice = productGroup.Sum(x => x.SalePrice.Amount),
                            TotalManufacturingPrice = productGroup.Sum(x => x.ManufacturingPrice.Amount),
                        };

            return Task.FromResult(summary.ToList());
        }
    }
}
