using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Otus.Teaching.Concurrency.Import.DataAccess.Repositories
{
    public class CustomerTableService
    {

        protected readonly private NpgsqlConnection _connection;
        protected readonly string _tableName = "customers";


        public CustomerTableService(NpgsqlConnection connection)
        {
            _connection = connection;

        }

        public int CreateTable()
        {

            var query = QuerySource.CreateTableCustomers;
            var result = _connection.Execute(query);

            return result;
        }

        public bool TableExists()
        {
            var query = QuerySource.TableExists;

            var parameters = new DynamicParameters();
            parameters.Add("schemaName", "public", DbType.String);
            parameters.Add("tableName", _tableName, DbType.String);

            var result = _connection.QueryFirstOrDefault<bool>(query, parameters);

            return result;

        }
    }
}
