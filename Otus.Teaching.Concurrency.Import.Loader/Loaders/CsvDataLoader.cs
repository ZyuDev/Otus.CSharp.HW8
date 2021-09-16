using Otus.Teaching.Concurrency.Import.Core.Loaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Otus.Teaching.Concurrency.Import.DataAccess.Parsers;

namespace Otus.Teaching.Concurrency.Import.Loader.Loaders
{
    public class CsvDataLoader : IDataLoader
    {
        private string _pathToFile;

        public CsvDataLoader(string path)
        {
            _pathToFile = $"{path}.csv";
        }

        public void LoadData()
        {
            var content = File.ReadAllText(_pathToFile);

            var parser = new CsvParser();
            var collection = parser.Parse(content);

        }
    }
}
