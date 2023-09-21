using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpliwiseApp.Data;
using SpliwiseApp.Interface;
using SpliwiseApp.Models;
using SpliwiseApp.Service;

namespace SpliwiseApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly SplitContext _context;
        private readonly IBillService _billService;

        public BillsController(SplitContext context, IBillService billService)
        {
            _context = context;
            _billService = billService;
        }

        // GET: api/Bills
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bills>>> GetAllBills()
        {
            var result = await _billService.GetAllBillsAsync();
            return Ok(result);
        }

        // GET: api/Bills/5
        [HttpGet("download/{id}")]
        public async Task<IActionResult> GetBills(int id)
        {
           var result = await _billService.DownloadBill(id);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        //// PUT: api/Bills/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutBills(int id, Bills bills)
        //{
        //    if (id != bills.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(bills).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!BillsExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Bills
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> PostBills(IFormFile file)
        {
          if (file == null)
          {
              return Problem("Entity set 'SplitContext.Bills'  is null.");
          }
           var result = await _billService.UploadFile(file);
            return result;
        }

        //// DELETE: api/Bills/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteBills(int id)
        //{
        //    if (_context.Bills == null)
        //    {
        //        return NotFound();
        //    }
        //    var bills = await _context.Bills.FindAsync(id);
        //    if (bills == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Bills.Remove(bills);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool BillsExists(int id)
        //{
        //    return (_context.Bills?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
