using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Otus.Teaching.Concurrency.Import.Core.Loaders;
using Otus.Teaching.Concurrency.Import.DataGenerator.Generators;
using Otus.Teaching.Concurrency.Import.Loader.Loaders;
using IniParser;
using IniParser.Model;
using Otus.Teaching.Concurrency.Import.Loader.Helper;

namespace Otus.Teaching.Concurrency.Import.Loader
{
    class Program
    {

        static void Main(string[] args)
        {

            var settings = ReadSettings("configuration.ini");

            if (settings == null)
            {
                Console.WriteLine("Configuration file not found");
                return;
            }

            if (settings.DataGenerationRegime == 0)
            {

                var startInfo = new ProcessStartInfo()
                {
                    ArgumentList = { settings.DataFilePath, settings.ItemsCount.ToString() },
                    FileName = settings.LoaderAppPath
                };

                var processGenerator = Process.Start(startInfo);

                Console.WriteLine($"Data generation started with process Id {processGenerator.Id}...");

                processGenerator.WaitForExit();

                Console.WriteLine("Data generated");
            }
            else if (settings.DataGenerationRegime == 1)
            {
                Console.WriteLine("Data generation by direct method call.");

                var generator = new CsvDataGenerator($"{settings.DataFilePath}.csv", settings.ItemsCount);
                generator.Generate();

                Console.WriteLine("Data generated");

            }

            var loader = new CsvDataLoader(settings.DataFilePath, settings.ConnectionString, 4, settings.LoadRegime);
            loader.LoadData();
        }



        private static AppSettings ReadSettings(string settingsPath)
        {
            if (!File.Exists(settingsPath))
            {
                return null;
            }

            var appSettings = new AppSettings();

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(settingsPath);
            data.ClearAllComments();

            if (int.TryParse(data["Main"]["ItemsCount"], out var itemsCount))
            {
                appSettings.ItemsCount = itemsCount;
            }
            if (int.TryParse(data["Main"]["LoadRegime"], out var loadRegime))
            {
                appSettings.LoadRegime = loadRegime;
            }
            if (int.TryParse(data["Main"]["DataGenerationRegime"], out var dataGenerationRegime))
            {
                appSettings.DataGenerationRegime = dataGenerationRegime;
            }

            appSettings.LoaderAppPath = data["Main"]["LoaderAppPath"].Replace("\"","");
            appSettings.ConnectionString = data["Main"]["ConnectionString"].Replace("\"", "");
            appSettings.DataFileName = data["Main"]["DataFileName"];

            return appSettings;
        }
    }
}