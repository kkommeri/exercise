using Microsoft.Extensions.Options;
using NodaMoney;
using Services.Dto;
using System.Reflection;
using System.Text;

namespace Services.Tests
{
    public class SalesServiceTests
    {
        private string ReadEmbeddedResource(string resourceName)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream, Encoding.GetEncoding(1252)))
            {
                return reader.ReadToEnd();
            }
        }

        [Fact]
        public async Task GetSalesDtoAsync_ReturnsCorrectData()
        {
            // Arrange
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var salesData = ReadEmbeddedResource("Services.Tests.Data.csv");
            var tempFilePath = Path.GetTempFileName();
            await File.WriteAllTextAsync(tempFilePath, salesData, Encoding.GetEncoding(1252));
            var salesSettings = new SalesSettings { SalesDataPath = tempFilePath };
            var options = Options.Create(salesSettings);
            var salesService = new SalesService(options);

            // Act
            var result = await salesService.GetSalesDataAsync();

            // Assert
            Assert.NotNull(result);
            var salesList = result.ToList();
            Assert.Equal(700, salesList.Count);

            Assert.Contains(salesList, item =>
                    item.Segment == "Government" &&
                    item.Country == "Canada" &&
                    item.Product == "Carretera" &&
                    item.DiscountBand == "None" &&
                    item.UnitsSold == 1618.5 &&
                    item.ManufacturingPrice == new Money(3, "GBP") &&
                    item.SalePrice == new Money(20, "GBP") &&
                    item.Date == new DateOnly(2014, 1, 1)
            );

            File.Delete(tempFilePath);
        }

        [Fact]
        public async Task GetSalesSummaryDtoAsync_ReturnsCorrectData()
        {
            // Arrange
            var sales = new List<SalesDto>() {
                    new() { Product = "Carretera", SalePrice = new Money(20, "GBP"), ManufacturingPrice = new Money(3, "GBP") },
                    new() { Product = "Carretera", SalePrice = new Money(30, "GBP"), ManufacturingPrice = new Money(5, "GBP") },
                    new() { Product = "Montana", SalePrice = new Money(40, "GBP"), ManufacturingPrice = new Money(10, "GBP") },
                    new() { Product = "Montana", SalePrice = new Money(50, "GBP"), ManufacturingPrice = new Money(15, "GBP") },
                };

            var salesSettings = new SalesSettings { SalesDataPath = "tempFilePath" };
            var options = Options.Create(salesSettings);

            var salesService = new SalesService(options);

            // Act
            var result = await salesService.GetSalesSummaryByProductAsync(sales);

            //Assert
            Assert.NotNull(result);
            Assert.Collection<SalesSummaryByProductDto>(result, item =>
            {
                Assert.Equal("Carretera", item.Product);
                Assert.Equal(50, item.TotalSalePrice);
                Assert.Equal(8, item.TotalManufacturingPrice);
            }, item =>
            {
                Assert.Equal("Montana", item.Product);
                Assert.Equal(90, item.TotalSalePrice);
                Assert.Equal(25, item.TotalManufacturingPrice);
            });
        }
    }
}
