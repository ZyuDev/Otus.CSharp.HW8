using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Otus.Teaching.Concurrency.Import.Core.Loaders;
using Otus.Teaching.Concurrency.Import.DataGenerator.Generators;
using Otus.Teaching.Concurrency.Import.Loader.Loaders;

namespace Otus.Teaching.Concurrency.Import.Loader
{
    class Program
    {
        private static string _connectionString = @"Host = localhost; Port = 5432; Database = CustomersDB; User Id = sa; Password = 1;";

        private static string _dataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "customers");
        private static string _loaderAppPath = @"D:\_WORK\REPO_LEARNING\Otus.CSharp.HW8\Otus.Teaching.Concurrency.Import.DataGenerator.App\bin\Debug\netcoreapp3.1\Otus.Teaching.Concurrency.Import.DataGenerator.App.exe";
        private static int _itemsCount = 10000;
        private static int _regime = 1;

        static void Main(string[] args)
        {
            if (args != null && args.Length == 1)
            {
                _dataFilePath = args[0];
            }

            if (_regime == 0)
            {

                var startInfo = new ProcessStartInfo()
                {
                    ArgumentList = { _dataFilePath, _itemsCount.ToString() },
                    FileName = _loaderAppPath
                };

                var processGenerator = Process.Start(startInfo);

                Console.WriteLine($"Data generation started with process Id {processGenerator.Id}...");

                processGenerator.WaitForExit();

                Console.WriteLine("Data generated");
            }
            else if (_regime == 1)
            {
                Console.WriteLine("Data generation by direct method call.");
                GenerateCustomersDataFile();
                Console.WriteLine("Data generated");

            }


            var loader = new CsvDataLoader(_dataFilePath, _connectionString);
            loader.LoadData();
        }

        static void GenerateCustomersDataFile()
        {
            var generator = new CsvDataGenerator($"{_dataFilePath}.csv", _itemsCount);
            generator.Generate();
        }
    }
}