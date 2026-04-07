using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DgAPI.Data;
using DgAPI.Models;

namespace DgAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly DgContext _context;

        public ReportController(DgContext context)
        {
            _context = context;
        }

        // GET: api/Report
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Report>>> GetReport()
        {
            return await _context.Reports.ToListAsync();
        }

        // GET: api/Report/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> GetReport(int id)
        {
            var Report = await _context.Reports.FindAsync(id);

            if (Report == null)
            {
                return NotFound();
            }

            return Report;
        }

        // POST: api/Report
        [HttpPost]
        public async Task<ActionResult<Report>> PostReport(Report Report)
        {
            _context.Reports.Add(Report);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReport), new { id = Report.report_id }, Report);
        }

        // PUT: api/Report/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReport(int id, Report Report)
        {
            if (id != Report.report_id)
            {
                return BadRequest();
            }

            _context.Entry(Report).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Report/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var Report = await _context.Reports.FindAsync(id);
            if (Report == null)
            {
                return NotFound();
            }

            _context.Reports.Remove(Report);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
