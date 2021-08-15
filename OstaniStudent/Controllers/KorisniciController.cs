using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OstaniStudent.Database;
using OstaniStudent.Database.Models;

namespace OstaniStudent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KorisniciController : ControllerBase
    {
        private readonly ServiceDb _context;

        public KorisniciController(ServiceDb context)
        {
            _context = context;
        }

        // GET: api/Korisnici
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Korisnici>>> GetKorisnicis()
        {
            return await _context.Korisnicis.ToListAsync();
        }

        // GET: api/Korisnici/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Korisnici>> GetKorisnici(int id)
        {
            var korisnici = await _context.Korisnicis.Where(t => t.Id == id).FirstOrDefaultAsync();

            if (korisnici == null)
            {
                return NotFound();
            }

            return korisnici;
        }

        // PUT: api/Korisnici/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKorisnici(int id, Korisnici korisnici)
        {
            if (id != korisnici.Id)
            {
                return BadRequest();
            }

            _context.Entry(korisnici).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KorisniciExists(id))
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

        // POST: api/Korisnici
        [HttpPost]
        public async Task<ActionResult<Korisnici>> PostKorisnici(Korisnici korisnici)
        {
            _context.Korisnicis.Add(korisnici);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (KorisniciExists(korisnici.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetKorisnici", new { id = korisnici.Id }, korisnici);
        }

        // DELETE: api/Korisnici/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKorisnici(int id)
        {
            var korisnici = await _context.Korisnicis.FindAsync(id);
            if (korisnici == null)
            {
                return NotFound();
            }

            _context.Korisnicis.Remove(korisnici);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KorisniciExists(int id)
        {
            return _context.Korisnicis.Any(e => e.Id == id);
        }
    }
}
