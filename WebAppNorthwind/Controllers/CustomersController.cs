using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppNorthwind.Database;

namespace WebAppNorthwind.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly NWContext _context;

        public CustomersController(NWContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customers>>> GetAllCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        [HttpGet("by-customer-id/{CustomerID}")]
        public async Task<ActionResult<Customers>> GetCustomerById(string CustomerID)
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }
        [HttpGet("by-phone/{Phone}")]
        public async Task<ActionResult<Customers>> GetCustomerByPhone(string Phone)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Phone == Phone.Trim());
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }
        private string GenerateRandomCustomerID()
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var charIndex = random.Next(chars.Length);
            return chars[charIndex].ToString();
        }
        [HttpPost]
        public async Task<ActionResult<Customers>> AddCustomer(Customers customer)
        {
            var name = customer.CompanyName.Split(" ");
            string firstSuff = name.Length > 0 ? name[0].Substring(0, Math.Min(2, name[0].Length)) : "XX";
            string lastSuff = name.Length > 1 ? name[^1].Substring(0, Math.Min(2, name[^1].Length)) : "YY";

            customer.CustomerID = $"{firstSuff + lastSuff + GenerateRandomCustomerID()}".ToUpper();
            var existingCustomer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerID == customer.CustomerID);
            if (existingCustomer != null)
            {
                return BadRequest("Customers ID already exists.");
            }
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCustomerById), new {CustomerID = customer.CustomerID},customer);
        }
        [HttpPut("by-customer-id/{CustomerID}")]
        public async Task<IActionResult> UpdateCustomer(string CustomerID, Customers customer)
        {
            if (CustomerID != customer.CustomerID)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(CustomerID))
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }
        [HttpDelete("by-customer-id/{CustomerID}")]
        public async Task<IActionResult> DeleteCustomer(string CustomerID)
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            if (customer == null)
            {
                return NotFound();
            }
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool CustomerExists(string CustomerID) {
            return _context.Customers.Any(e => e.CustomerID == CustomerID);
        }
    }
}
