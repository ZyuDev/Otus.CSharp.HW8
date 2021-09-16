using Otus.Teaching.Concurrency.Import.Core.Parsers;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Otus.Teaching.Concurrency.Import.DataAccess.Parsers
{
    public class CsvParser: IDataParser<Customer>
    {

        private readonly string _delimiter = ",";


        public IList<Customer> Parse(string content)
        {

            var collection = new List<Customer>();

            var lines = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 1; i<lines.Length; i++)
            {
                var currentLine = lines[i];

                var item = ParseItem(currentLine);

                if (item != null)
                {
                    collection.Add(item);
                }

            }
            
            return collection;
        }

        private Customer ParseItem(string content)
        {

            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            var item = new Customer();

            var parts = content.Split(_delimiter, StringSplitOptions.RemoveEmptyEntries);

            if(parts.Length >= 4)
            {
                if (!int.TryParse(parts[0], out var id))
                {
                    return null;
                }

                item.Id = id;
                item.FullName = parts[1];
                item.Email = parts[2];
                item.Phone = parts[3];

                return item;
            }
            else
            {
                return null;
            }

        }
       
    }
}
