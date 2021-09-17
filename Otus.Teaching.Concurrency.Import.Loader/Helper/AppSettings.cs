using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Otus.Teaching.Concurrency.Import.Loader.Helper
{
     public class AppSettings
    {
        public string ConnectionString { get; set; }
        public string DataFileName { get; set; }
        public string LoaderAppPath { get; set; }
        public int ItemsCount { get; set; }
        public int DataGenerationRegime { get; set; }
        public int LoadRegime { get; set; }

        public string DataFilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DataFileName);
    }
}
