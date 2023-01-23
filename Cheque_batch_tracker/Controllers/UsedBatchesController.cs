using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cheque_batch_tracker.DAL;
using Cheque_batch_tracker.Models;
using NuGet.Protocol;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cheque_batch_tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsedBatchesController : ControllerBase
    {
        private readonly ChequeBatchContext _context;

        public UsedBatchesController(ChequeBatchContext context)
        {
            _context = context;
        }

        // GET: api/UsedBatches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsedBatches>>> GetUsedBatches()
        {
            return await _context.UsedBatches.Where(x=> x.Status !=9).ToListAsync();
        }

        // GET: api/UsedBatches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsedBatches>> GetUsedBatches(long id)
        {
            var usedBatches = await _context.UsedBatches.FindAsync(id);

            if (usedBatches == null)
            {
                return NotFound();
            }

            return usedBatches;
        }

        // PUT: api/UsedBatches/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsedBatches(long id, UsedBatches usedBatches)
        {
            if (id != usedBatches.ID && ModelState.IsValid)
            {
                return BadRequest();
            }
            int typeOfEdite = 0;
            var Exsists = _context.UsedBatches.Where(x => x.ChequeNumber == usedBatches.ChequeNumber)
                .FirstOrDefaultAsync().Result;
            // cheque number never used beffore 
            if (Exsists == null) 
            {
                typeOfEdite = 1;
            }
            //cheque number used beffore with the same row
            else if (Exsists.ID == usedBatches.ID) 
            {
                typeOfEdite = 2;
            }
            // cheque number used beffore with a diffrent row
            else
            {
                typeOfEdite = 3;
                return Problem("Duplicate Cheque Number", "Batch", 409, "CustomErrors");

            }
            _context.ChangeTracker.Clear();
            usedBatches.Notes = usedBatches.Notes + " {type "+ typeOfEdite + "}";
            usedBatches.Status = 2;
            usedBatches.UpdatedDate = DateTime.Now;
            usedBatches.CheckSum++;
            //_context.UsedBatches.Update(usedBatches);
            _context.Entry(usedBatches).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsedBatchesExists(id))
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

        // POST: api/UsedBatches
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsedBatches>> PostUsedBatches(UsedBatches usedBatches)
        {
            if (!ModelState.IsValid)
            {
             return Problem(ModelState.Values.Select(x=> x.Errors).ToJson(), "Batch", 409, "ModelErrors");
            }
            if (IsUsedBatchesValid(usedBatches) > 0)
            {
             return Problem(ModelState.Values.Select(x=> x.Errors).ToJson(), "Batch", 409, "CustomErrors");
            }
            usedBatches.CheckSum++;
            usedBatches.Status = 1;
            _context.UsedBatches.Add(usedBatches);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsedBatches", new { id = usedBatches.ID }, usedBatches);
        }

        // DELETE: api/UsedBatches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsedBatches(long id)
        {
            var usedBatches = await _context.UsedBatches.FindAsync(id);
            if (usedBatches == null)
            {
                return NotFound();
            }
            usedBatches.Status = 9;
            usedBatches.UpdatedDate = DateTime.Now;
            usedBatches.CheckSum++;
            _context.Entry(usedBatches).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsedBatchesExists(long id)
        {
            return _context.UsedBatches.Any(e => e.ID == id);
        }

        private int IsUsedBatchesValid(UsedBatches usedBatches)
        {
            //model valadation, range check and req failds
            if (!ModelState.IsValid)
                return 460;
            // check for batch start & end not confuised
           
            // cheack for duplicats
            var Exsists = _context.UsedBatches.Where(x => x.ChequeNumber == usedBatches.ChequeNumber
            ).FirstOrDefaultAsync();
            if (Exsists.Result != null)
            {
                ModelState.TryAddModelError("UsedBatches", "Duplicate! Cheque Number is Used Alread.");
                return 409;
            }


            return 0;

        }

    }

}
