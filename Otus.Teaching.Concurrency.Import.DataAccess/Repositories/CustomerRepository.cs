using System;
using System.Data;
using Dapper;
using Npgsql;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using Otus.Teaching.Concurrency.Import.Handler.Repositories;

namespace Otus.Teaching.Concurrency.Import.DataAccess.Repositories
{
    public class CustomerRepository
        : ICustomerRepository
    {

        protected readonly private NpgsqlConnection _connection;
        protected readonly string _tableName = "customers";


        public CustomerRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public Customer GetCustomer(int id)
        {
            var query = "SELECT * FROM public.customers WHERE id = @id";
            var parameters = new DynamicParameters();
            parameters.Add("id", id, DbType.Int32);

            var item = _connection.QueryFirstOrDefault<Customer>(query, parameters);

            return item;
        }

        public Customer GetCustomerByEmail(string email)
        {
            var query = "SELECT * FROM public.customers WHERE email = @email";
            var parameters = new DynamicParameters();
            parameters.Add("email", email, DbType.String);

            var item = _connection.QueryFirstOrDefault<Customer>(query, parameters);

            return item;
        }

        public void AddCustomer(Customer item)
        {
            var query = QuerySource.InsertCustomer;
            var parameters = new DynamicParameters();
            parameters.Add("fullname", item.FullName, DbType.String);
            parameters.Add("email", item.Email, DbType.String);
            parameters.Add("phone", item.Phone, DbType.String);

            var result = _connection.Execute(query, parameters);

        }

        public int Clear()
        {
            var query = $"DELETE FROM public.{_tableName}";
            var result = _connection.Execute(query);

            return result;

        }

        public int Count()
        {
            var query = $"SELECT Count(1) as RowsCount FROM {_tableName}";
            var count = _connection.QueryFirstOrDefault<int>(query);

            return count;

        }
    }
}