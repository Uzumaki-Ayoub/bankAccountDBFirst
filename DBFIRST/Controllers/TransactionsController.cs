using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBFIRST.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBFIRST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly DbfirstContext _context;

        public TransactionsController(DbfirstContext context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(string id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // PUT: api/Transactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(string id, Transaction transaction)
        {
            if (id != transaction.AccountNumber)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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

        // POST: api/Transactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction, BankAccount bankAccount)
        {
           while (true)
           {
               string input = transaction.Deposit.ToString();

               if (int.TryParse(input, out int depositAmount))
               {
                   if (depositAmount > 100 || depositAmount < 5)
                   {
                       return BadRequest("Invalid amount, please insert an amount between 100KD and 5KD");
                       //Console.ReadLine();
                   }
                   else
                   {
                       bankAccount.AccountBalance = bankAccount.AccountBalance += depositAmount;

                       _context.Transactions.Add(transaction);
                       await _context.SaveChangesAsync();
                       return CreatedAtAction("GetTransaction", new { id = transaction.AccountNumber }, transaction);
                       //return ($"Amount inserted to your account!\n your balance is: {bankAccount.AccountBalance}");
                       
                   }
               }
               //int depositAmount = int.Parse(Console.ReadLine());
               else
               {
                   return BadRequest("Please enter a valid numeric amount");
               }

               if ()
           }

        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(string id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionExists(string id)
        {
            return _context.Transactions.Any(e => e.AccountNumber == id);
        }
    }
}
