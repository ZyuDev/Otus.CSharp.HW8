using Otus.Teaching.Concurrency.Import.Handler.Data;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using System;
using System.IO;
using System.Text;

namespace Otus.Teaching.Concurrency.Import.DataGenerator.Generators
{
    public class CsvDataGenerator : IDataGenerator
    {
        private readonly string _fileName;
        private readonly int _dataCount;
        private readonly string _delimiter = ",";


        public CsvDataGenerator(string fileName, int dataCount)
        {
            _fileName = fileName;
            _dataCount = dataCount;
        }

        public void Generate()
        {
            var stringData = GenerateToString();
            File.WriteAllText(_fileName, stringData);
        }

        public string GenerateToString()
        {
            var sb = new StringBuilder();

            PrintHeader(sb);

            var customers = RandomCustomerGenerator.Generate(_dataCount);

            foreach(var item in customers)
            {
                PrintItem(sb, item);
            }

            return sb.ToString();
        }

        private void PrintHeader(StringBuilder sb)
        {
            sb.Append("Id");
            sb.Append(_delimiter);

            sb.Append("FullName");
            sb.Append(_delimiter);

            sb.Append("Email");
            sb.Append(_delimiter);

            sb.Append("Phone");
            sb.Append(Environment.NewLine);
        }

        private void PrintItem(StringBuilder sb, Customer item)
        {
            sb.Append(item.Id);
            sb.Append(_delimiter);

            sb.Append(item.FullName);
            sb.Append(_delimiter);

            sb.Append(item.Email);
            sb.Append(_delimiter);

            sb.Append(item.Phone);
            sb.Append(Environment.NewLine);

        }
    }
}
