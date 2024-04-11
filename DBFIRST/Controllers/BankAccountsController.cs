using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DBFIRST.Models;

namespace DBFIRST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountsController : ControllerBase
    {
        private readonly DbfirstContext _context;

        public BankAccountsController(DbfirstContext context)
        {
            _context = context;
        }

        // GET: api/BankAccounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankAccount>>> GetBankAccounts()
        {
            return await _context.BankAccounts.ToListAsync();
        }

        // GET: api/BankAccounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BankAccount>> GetBankAccount(int id)
        {
            var bankAccount = await _context.BankAccounts.FindAsync(id);

            if (bankAccount == null)
            {
                return NotFound();
            }

            return bankAccount;
        }

        // PUT: api/BankAccounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBankAccount(int id, BankAccount bankAccount)
        {
            if (id != bankAccount.Id)
            {
                return BadRequest();
            }

            _context.Entry(bankAccount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankAccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BankAccounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BankAccount>> PostBankAccount(BankAccount bankAccount)
        {
            string inputUser = bankAccount.DateOfBirth.ToString("yyyyMMdd");
            //string Date = DateTime.Today.ToString("yyyyMMdd");

            if (DateOnly.TryParseExact(inputUser, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateOnly dOB))
            {
                if (dOB > DateOnly.Parse("1900-01-01"))
                {
                    string nextNumber = GetNextNumber();
                    //string accountNumber = Date + nextNumber;

                    bankAccount.AccountNumber = nextNumber;
                    bankAccount.DateOfBirth = dOB;

                    _context.BankAccounts.Add(bankAccount);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetBankAccount", new { id = bankAccount.Id }, bankAccount);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest("Invalid date format. Please enter the date in the format yyyyMMdd.");
            }
        }

        // DELETE: api/BankAccounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBankAccount(int id)
        {
            var bankAccount = await _context.BankAccounts.FindAsync(id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            _context.BankAccounts.Remove(bankAccount);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BankAccountExists(int id)
        {
            return _context.BankAccounts.Any(e => e.Id == id);
        }

        private string GetNextNumber()
        {
            string maxAccountNumber = GetMaxNumber();

            long startNumber = !string.IsNullOrEmpty(maxAccountNumber) && maxAccountNumber.All(char.IsDigit) ? Convert.ToInt64(maxAccountNumber) + 1 : 1000;

            return startNumber.ToString();
        }

        private string GetMaxNumber()
        {
            var maxAccount = _context.BankAccounts
                .Select(b => b.AccountNumber)
                .OrderByDescending(an => an).FirstOrDefault();
            return maxAccount ?? "";
        }

    }
}
