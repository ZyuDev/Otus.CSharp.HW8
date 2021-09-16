using System.Collections.Generic;

namespace Otus.Teaching.Concurrency.Import.Core.Parsers
{
    public interface IDataParser<T>
    {
        IList<T> Parse(string content);
    }
}