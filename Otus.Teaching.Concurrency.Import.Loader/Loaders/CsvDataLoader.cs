using Otus.Teaching.Concurrency.Import.Core.Loaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Otus.Teaching.Concurrency.Import.DataAccess.Parsers;
using Npgsql;
using Otus.Teaching.Concurrency.Import.DataAccess.Repositories;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using System.Diagnostics;

namespace Otus.Teaching.Concurrency.Import.Loader.Loaders
{
    public class CsvDataLoader : IDataLoader
    {
        private readonly string _pathToFile;
        private readonly string _connectionString;

        public CsvDataLoader(string path, string connectionString)
        {
            _pathToFile = $"{path}.csv";
            _connectionString = connectionString;
        }

        public void LoadData()
        {
            var content = File.ReadAllText(_pathToFile);

            var parser = new CsvParser();
            var collection = parser.Parse(content);

            if (PrepareCustomersTable() != 0)
            {
                return;
            }

            var sw = new Stopwatch();

            Console.WriteLine("Save data to DB...");
            var createdCount = LoadPortion(collection);
            Console.WriteLine($"Created {createdCount} items");
            Console.WriteLine($"Time, s: {sw.Elapsed.TotalSeconds:F}");


        }

        private int LoadPortion(IEnumerable<Customer> collection)
        {
            var created = 0;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var customerRepo = new CustomerRepository(connection);

                foreach (var item in collection)
                {
                    try
                    {
                        customerRepo.AddCustomer(item);
                        created++;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Cannot add customer: {item}. Message: {e.Message}");
                    }
                }

            }

            return created;
        }

        private int PrepareCustomersTable()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var tableService = new CustomerTableService(connection);

                if (!tableService.TableExists())
                {
                    try
                    {
                        tableService.CreateTable();
                        Console.WriteLine("Customers table created");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Cannot create table Customers. Message: {e.Message}");
                        return -1;
                    }
                }

                var customerRepo = new CustomerRepository(connection);

                if (customerRepo.Count() > 0)
                {
                    customerRepo.Clear();
                    Console.WriteLine("Customers table cleared");
                }
            }

            return 0;
        }
    }
}
