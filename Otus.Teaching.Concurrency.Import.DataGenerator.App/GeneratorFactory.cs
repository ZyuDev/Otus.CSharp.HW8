using Otus.Teaching.Concurrency.Import.Handler.Data;
using Otus.Teaching.Concurrency.Import.DataGenerator.Generators;

namespace Otus.Teaching.Concurrency.Import.XmlGenerator
{
    public static class GeneratorFactory
    {
        public static IDataGenerator GetXmlGenerator(string fileName, int dataCount)
        {
            return new XmlDataGenerator(fileName, dataCount);
        }

        public static IDataGenerator GetCsvGenerator(string fileName, int dataCount)
        {
            return new CsvDataGenerator(fileName, dataCount);
        }
    }
}