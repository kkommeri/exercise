using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using NodaMoney;

namespace Services
{
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
}
