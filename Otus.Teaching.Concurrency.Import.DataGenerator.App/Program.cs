using Otus.Teaching.Concurrency.Import.Core.Enums;
using Otus.Teaching.Concurrency.Import.Handler.Data;
using System;
using System.IO;
using Otus.Teaching.Concurrency.Import.DataGenerator.Generators;

namespace Otus.Teaching.Concurrency.Import.XmlGenerator
{
    class Program
    {
        private static readonly string _dataFileDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static string _dataFileName; 
        private static int _dataCount = 100;
        private static GeneratorKinds _generatorKind = GeneratorKinds.Csv;
        
        static void Main(string[] args)
        {
            if (!TryValidateAndParseArgs(args))
                return;

            IDataGenerator generator = null;
            if(_generatorKind == GeneratorKinds.Csv)
            {
                generator = GeneratorFactory.GetCsvGenerator(_dataFileName, _dataCount);
            }
            else if (_generatorKind == GeneratorKinds.Xml)
            {
                generator = GeneratorFactory.GetXmlGenerator(_dataFileName, _dataCount);
            }
            else
            {
                Console.WriteLine("Unknown generator kind");
            }

            if (generator != null)
            {
                Console.WriteLine("Generating data...");
                generator.Generate();
                Console.WriteLine($"Generated data in {_dataFileName}...");
            }

        }

        private static bool TryValidateAndParseArgs(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                var ext = GetFileExt(_generatorKind);
                _dataFileName = Path.Combine(_dataFileDirectory, $"{args[0]}.{ext}");
            }
            else
            {
                Console.WriteLine("Data file name without extension is required");
                return false;
            }
            
            if (args.Length > 1)
            {
                if (!int.TryParse(args[1], out _dataCount))
                {
                    Console.WriteLine("Data must be integer");
                    return false;
                }
            }

            return true;
        }

        private static string GetFileExt(GeneratorKinds generatorKind)
        {
            if (generatorKind == GeneratorKinds.Csv)
            {
                return "csv";
            }
            else if(generatorKind == GeneratorKinds.Xml)
            {
                return "xml";
            }

            return "txt";
        }
    }
}