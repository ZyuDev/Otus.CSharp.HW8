using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Otus.Teaching.Concurrency.Import.DataAccess.Repositories;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersManage.Shared;

namespace UsersManage.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersExampleController : ControllerBase
    {
        private readonly string _connectionString;

        public UsersExampleController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("UsersDb");
        }

        /// <summary>
        /// Get user by Id.
        /// </summary>
        /// <param name="id">Identifier of user</param>
        /// <returns>User</returns>
        [HttpGet("{id}")]
        public IActionResult GetItem(int id)
        {
            var responseDto = new ResponseDto<Customer>();
            Customer item;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var repo = new CustomerRepository(connection);
                item = repo.GetCustomer(id);
            }

            responseDto.Data = item;

            if (item == null)
            {
                responseDto.Status = -1;
                responseDto.Message = $"User with id {id} not found.";
            }

            return Ok(responseDto);
        }

    }
}
