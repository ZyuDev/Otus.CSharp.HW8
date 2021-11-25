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

namespace UsersManage.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly string _connectionString;

        public UsersController(IConfiguration configuration)
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
            Customer item;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var repo = new CustomerRepository(connection);
                item = repo.GetCustomer(id);
            }

            if (item == null)
            {
                return StatusCode(404, $"User with id {id} not found.");
            }
            else
            {
                return Ok(item);
            }
        }

        /// <summary>
        /// Add new User.
        /// </summary>
        /// <param name="item">User</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddItem([FromBody] Customer item)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var repo = new CustomerRepository(connection);
                var savedItem = repo.GetCustomerByEmail(item.Email);

                if (savedItem != null)
                {
                    return StatusCode(409, $"Item {item} already exists.");
                }
                else
                {

                    try
                    {
                        repo.AddCustomer(item);
                        return Ok(item);

                    }
                    catch (Exception e)
                    {

                        return StatusCode(500, $"Cannot add item. Message: {e.Message}");

                    }
                }
            }
        }
    }
}
