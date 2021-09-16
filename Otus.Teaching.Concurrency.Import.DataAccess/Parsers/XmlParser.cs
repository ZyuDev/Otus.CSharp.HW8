using System.Collections.Generic;
using Otus.Teaching.Concurrency.Import.Core.Parsers;
using Otus.Teaching.Concurrency.Import.Handler.Entities;

namespace Otus.Teaching.Concurrency.Import.DataAccess.Parsers
{
    public class XmlParser
        : IDataParser<Customer>
    {
        public IList<Customer> Parse(string content)
        {
            //Parse data
            return new List<Customer>();
        }
    }
}