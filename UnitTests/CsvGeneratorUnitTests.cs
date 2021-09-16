using NUnit.Framework;
using Otus.Teaching.Concurrency.Import.DataGenerator.Generators;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    [TestFixture]
    public class CsvGeneratorUnitTests
    {
        [Test]
        public void GenerateToString_ReturnCsvString()
        {
            var csvGenerator = new CsvDataGenerator("test.csv", 2);
            var csvStr = csvGenerator.GenerateToString();

            Console.WriteLine(csvStr);

            Assert.Pass();
        }
    }
}
