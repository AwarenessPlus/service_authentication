using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthenticationService.Data;
using DomainModel;
using DatabaseServices.DTO;
using Newtonsoft.Json;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicsController : ControllerBase
    {
        private readonly AuthenticationServiceContext _context;

        public MedicsController(AuthenticationServiceContext context)
        {
            _context = context;
        }

        // GET: api/Medics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medic>>> GetMedic()
        {
            return await _context.Medic.ToListAsync();
        }

        // GET: api/Medics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Medic>> GetMedic(int id)
        {
            var medic = await _context.Medic.FindAsync(id);

            if (medic == null)
            {
                return NotFound();
            }

            return medic;
        }

        // PUT: api/Medics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedic(int id, Medic medic)
        {
            if (id != medic.MedicID)
            {
                return BadRequest();
            }

            _context.Entry(medic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicExists(id))
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

        // POST: api/Medics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("SignUp")]
        public async Task<ActionResult<Medic>> PostMedic(MedicDTO medic)
        {
            Medic newMedic = new();
            User newUser = new();
            Authentication newAuthentication = new();
            newAuthentication.UserName = medic.Authentication.UserName;
            newAuthentication.Password = medic.Authentication.Password;
            if (_context.Authentication.Any(e => e.UserName == newAuthentication.UserName))
            {
                return Conflict(JsonConvert.SerializeObject("User Already Exist"));
            }
            string[] firsName = medic.FirstName.Split(' ');
            string[] lastName = medic.LastName.Split(' ');
            newUser.FirstName = firsName[0];
            newUser.SecondName = firsName[1];
            newUser.Surname = lastName[0];
            newUser.LastName = lastName[1];
            newUser.Age = medic.Age;
            newMedic.MedicData = newUser;
            newMedic.Rotation = medic.Rotation;
            newMedic.Semester = medic.Semester;
            newMedic.AuthenticationData = newAuthentication;
            newMedic.AuthenticationData.EncryptPassword(newMedic.AuthenticationData.Password);
            _context.Medic.Add(newMedic);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedic", new { id = newMedic.MedicID }, newMedic);
        }

        // DELETE: api/Medics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedic(int id)
        {
            var medic = await _context.Medic.FindAsync(id);
            if (medic == null)
            {
                return NotFound();
            }

            _context.Medic.Remove(medic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicExists(int id)
        {
            return _context.Medic.Any(e => e.MedicID == id);
        }
    }
}
