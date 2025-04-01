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
        public Task<List<SalesSummaryByProductDto>> GetSalesSummaryByProductAsync(List<SalesDto> sales)
        {
            var summary = from s in sales
                        group s by new {s.Product } into g
                        select new SalesSummaryByProductDto
                        {
                            Product = g.Key.Product,
                            TotalSalePrice = g.Sum(x => x.SalePrice.Amount),
                            TotalManufacturingPrice = g.Sum(x => x.ManufacturingPrice.Amount),
                        };

            return Task.FromResult(summary.ToList());
        }

        public async Task<IEnumerable<SalesDto>> GetSalesDataAsync()
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
                csv.Context.RegisterClassMap<SalesDtoMap>();

                var records = csv.GetRecords<SalesDto>().ToList();
                return await Task.FromResult(records);
            }
        }
    }
}
