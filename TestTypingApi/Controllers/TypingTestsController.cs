using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestTypingApi.Models.Context;
using TestTypingApi.Models.DB;

namespace TestTypingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypingTestsController : ControllerBase
    {
        private readonly TestTypingSpeedsDbContext _context;

        public TypingTestsController(TestTypingSpeedsDbContext context)
        {
            _context = context;
        }

        // GET: api/TypingTests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypingTest>>> GetTypingTests()
        {
            return await _context.TypingTests
                 .Include(t => t.User) // include user details
                 .ToListAsync();
        }

        // GET: api/TypingTests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypingTest>> GetTypingTest(int id)
        {
            var typingTest = await _context.TypingTests
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (typingTest == null)
            {
                return NotFound();
            }

            return typingTest;
        }

        // PUT: api/TypingTests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypingTest(int id, TypingTest typingTest)
        {
            if (id != typingTest.Id)
            {
                return BadRequest();
            }

            _context.Entry(typingTest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypingTestExists(id))
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

        // POST: api/TypingTests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TypingTest>> PostTypingTest(TypingTest typingTest)
        {
            _context.TypingTests.Add(typingTest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTypingTest), new { id = typingTest.Id }, typingTest);
        }

        // DELETE: api/TypingTests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypingTest(int id)
        {
            var typingTest = await _context.TypingTests.FindAsync(id);
            if (typingTest == null)
            {
                return NotFound();
            }

            _context.TypingTests.Remove(typingTest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TypingTestExists(int id)
        {
            return _context.TypingTests.Any(e => e.Id == id);
        }
    }
}
