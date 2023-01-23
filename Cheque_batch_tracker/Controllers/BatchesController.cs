using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cheque_batch_tracker.DAL;
using Cheque_batch_tracker.Models;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using System.Data;

namespace Cheque_batch_tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchesController : ControllerBase
    {
        private readonly ChequeBatchContext _context;

        public BatchesController(ChequeBatchContext context)
        {
            _context = context;
        }

        // GET: api/Batches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Batch>>> GetBatch()
        {
            return await _context.Batch.Where(x=> x.Status != 9).ToListAsync();
        }

        // GET: api/Batches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Batch>> GetBatch(int id)
        {
            var batch = await _context.Batch.FindAsync(id);

            if (batch == null)
            {
                return NotFound();
            }

            return batch;
        }

        // PUT: api/Batches/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBatch(int id, Batch batch)
        {
            if (id != batch.ID)
            {
                return BadRequest();
            }
            if (!BatchExists(id))
            {
                return NotFound();
            }
            if (batch.BatchType < 1 )
            {
                batch.BatchType = _context.Batch.FindAsync(id).Result.BatchType;
            }
            int errorNumber = IsBatchValid(batch);
            if (errorNumber > 0)
            {
                return Problem(ModelState.Values.Select(x => x.Errors).ToJson(), "Batch", errorNumber, "CustomErrors");
            }
            batch.Status = 2;
            batch.UpdatedDate = DateTime.Now;
            batch.CheckSum++;
            _context.Entry(batch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BatchExists(id))
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

        // POST: api/Batches
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Batch>> PostBatch(Batch batch)
        {
            //
            int errorNumber = IsBatchValid(batch);
            if (errorNumber > 0)
            {
             return Problem(ModelState.Values.Select(x=> x.Errors).ToJson(), "Batch", errorNumber, "CustomErrors");
            }
            batch.Status = 1;
            batch.UpdatedBy = null;
            batch.CheckSum = 1;

            _context.Batch.Add(batch);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBatch", new { id = batch.ID }, batch);
        }

        // DELETE: api/Batches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBatch(int id)
        {
            var batch = await _context.Batch.FindAsync(id);
            if (batch == null)
            {
                return NotFound();
            }

            //_context.Batch.Remove(batch);
            batch.Status = 9;
            batch.UpdatedDate = DateTime.Now;
            batch.CheckSum++;
            _context.Entry(batch).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BatchExists(int id)
        {
            return _context.Batch.Any(e => e.ID == id);
        }
        private int IsBatchValid(Batch batch)
        {
            //model valadation, range check and req failds
            if (!ModelState.IsValid)
                return 460;
            // check for batch start & end not confuised
            if (batch.BatchEnd <= batch.BatchStart)
            {
                ModelState.AddModelError("Batch", "Start&End! batch start and end are either reverised or wrong.");
                // ModelState.ValidationState
                return 461;
            }

            // cheack for duplicats
            var Exsists = _context.Batch.Where(x => x.BatchStart == batch.BatchStart && x.BatchType == batch.BatchType).FirstOrDefaultAsync();
            if (Exsists.Result != null)
            {
                ModelState.TryAddModelError("Batch", "Duplicate! Either Batch Start or End are already used for the same Type. ");
                return 409;
            }

            //"Batch start is conflicting with an exsisting one"
            var usedBatch = _context.Batch.Where(x => x.BatchEnd > batch.BatchStart && x.BatchType == batch.BatchType).FirstOrDefaultAsync();
            if (usedBatch.Result != null)
            {
                ModelState.TryAddModelError("Batch", "Batch start is conflicting with an exsisting one");
                return 409;
            }
            
            return 0;

        }
    }
    
}
