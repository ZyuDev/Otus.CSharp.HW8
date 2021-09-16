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
using Otus.Teaching.Concurrency.Import.Loader.Helper;
using System.Threading;
using System.Linq;

namespace Otus.Teaching.Concurrency.Import.Loader.Loaders
{
    public class CsvDataLoader : IDataLoader
    {
        private readonly string _pathToFile;
        private readonly string _connectionString;
        private readonly int _threadCount;
        private readonly int _loadMethod;

        public CsvDataLoader(string path, string connectionString, int threadCount, int loadMethod)
        {
            _pathToFile = $"{path}.csv";
            _connectionString = connectionString;
            _threadCount = threadCount;
            _loadMethod = loadMethod;
        }

        public void LoadData()
        {

            if (PrepareCustomersTable() != 0)
            {
                return;
            }

            var content = File.ReadAllText(_pathToFile);

            var parser = new CsvParser();
            var collection = parser.Parse(content);

            var chanks = ChankHelper.SplitToChanks(collection, _threadCount);

            Console.WriteLine($"Save {collection.Count()} Customers to DB...");
            var sw = new Stopwatch();
            sw.Start();

            if (_loadMethod == 0)
            {
                // Use Thread pool.

                Console.WriteLine("Use ThreadPool.");
                var waitHandles = new WaitHandle[_threadCount];

                var i = 0;
                foreach (var chank in chanks)
                {
                    var waitEvent = new AutoResetEvent(false);

                    ThreadPool.QueueUserWorkItem(x => LoadPortion(chank, waitEvent));

                    waitHandles[i] = waitEvent;
                    i++;
                }

                WaitHandle.WaitAll(waitHandles);
            }
            else if (_loadMethod == 1)
            {
                // Create Threads. 
                 var threads = new List<Thread>();

                Console.WriteLine($"Use Threads created manually.");

                foreach (var chank in chanks)
                {
                    var thread = new Thread(new ParameterizedThreadStart(LoadInThread));
                    thread.Name = $"_chank_load_{Guid.NewGuid()}";

                    threads.Add(thread);

                    thread.Start(new { Name = thread.Name, Data = chank });

                }

                foreach (var thread in threads)
                {
                    thread.Join();
                }

            }
            else
            {
                Console.WriteLine($"Sequential data load.");
                LoadPortion(collection, null);

            }

            sw.Stop();
            Console.WriteLine($"Time, ms: {sw.Elapsed.TotalSeconds:F}");
            PrintCount();

        }

        private List<IEnumerable<Customer>> PrepareChanks()
        {
            var content = File.ReadAllText(_pathToFile);

            var parser = new CsvParser();
            var collection = parser.Parse(content);

            var chanks = ChankHelper.SplitToChanks(collection, _threadCount);

            return chanks;
        }

        private int LoadPortion(IEnumerable<Customer> collection, AutoResetEvent waitHandle)
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

            if(waitHandle != null)
            {
                waitHandle.Set();
            }

            return created;
        }

        private void LoadInThread(object args)
        {
            dynamic d = args;

            string threadName = d.Name;
            IEnumerable<Customer> data = d.Data;

            LoadPortion(data, null);
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

        private void PrintCount()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {

                var customerRepo = new CustomerRepository(connection);
                Console.WriteLine($"Customers count:{customerRepo.Count()}");

            }
        }
    }
}
