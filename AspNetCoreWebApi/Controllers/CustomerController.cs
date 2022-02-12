using AspNetCoreWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        private readonly ApiDbContext _apiDbContext;
        private string UserID = "ADMIN";
        public CustomerController(ApiDbContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
        }

        [HttpGet]
        //[Route("GetData")]
        public async Task<ActionResult<IEnumerable<Customers>>> GetData()
        {
            try
            {
                var res = await _apiDbContext.Customers.OrderBy(x => x.FirstName).ToListAsync();

                return res;
            }
            catch (Exception ex)
            {

                return NotFound();
            }
        }

        //[HttpGet("{id}")]
        [HttpGet("GetCustomer")]
        //[Route("GetData/{id}")]
        public async Task<ActionResult<IEnumerable<Customers>>> GetData(int id)
        {
            try
            {
                var res = await _apiDbContext.Customers.Where(x => x.CustomerId == id).OrderBy(x => x.FirstName).ToListAsync();

                return res;
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        //[HttpPost("SaveData")]
        [HttpPost]
        //[Route("SaveData")]
        public async Task<ActionResult<Customers>> SaveData(Customers customers)
        {
            customers.CustomerId = (new Random()).Next(100, 1000);
            _apiDbContext.Customers.Add(customers);
            await _apiDbContext.SaveChangesAsync();

            return CreatedAtAction("GetData", new { id = customers.CustomerId }, customers);
        }

        // DELETE: api/Products/5
        //[HttpDelete("{id}")]
        //[Route("DeleteData/{id}")]
        [HttpDelete("DeleteData")]
        public async Task<ActionResult<Customers>> DeleteData(int id)
        {
            var customers = await _apiDbContext.Customers.FindAsync(id);
            if (customers == null)
            {
                return NotFound();
            }
            _apiDbContext.Customers.Remove(customers);
            await _apiDbContext.SaveChangesAsync();

            return customers;
        }
    }
}
