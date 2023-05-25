using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
    [Route("/secret")]
    [ApiController]
    public class SecretsController : ControllerBase
    {
        private readonly SecretContext _context;

        public SecretsController(SecretContext context)
        {
            _context = context;
        }

        /*
        // GET: api/Secrets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Secret>>> GetSecrets()
        {
          if (_context.Secrets == null)
          {
              return NotFound();
          }
            return await _context.Secrets.ToListAsync();
        }*/

        // GET: api/Secrets/5
        /// <summary>
        /// Find a secret by hash
        /// </summary>
        /// <returns>Give back a secret by hash</returns>
        /// <response code="200">Returns the secret</response>
        /// <response code="404">A secret is not found or expired</response>
        /// <param name="hash">Unique hash to identify the secret</param>
        [Produces("application/json", "application/xml")]
        [HttpGet("{hash}")]
        public async Task<ActionResult<Secret>> GetSecret(string hash)
        {
            if (_context.Secrets == null)
            {
                return NotFound();
            }
            
            var secret =  _context.Secrets.Where(s => s.hash == hash).FirstOrDefault();

            if (secret == null)
            {
                return NotFound("Secret not found");
            }

            if (secret.expiresAt == 0)
            {
                if(secret.remainingViews == 0)
                {
                    return BadRequest("Can't see this secret more times.");
                }
                else
                {
                    secret.remainingViews -=1 ;
                    secret.remainingViews -= 1;
                    _context.Entry(secret).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok(secret);
                }
            }
            else
            {
                DateTime expirationDate = DateTime.Parse(secret.createdAt).AddMinutes(secret.expiresAt);
                if (DateTime.Now > expirationDate && (secret.remainingViews == 0))
                {
                    return NotFound($"This secret expired."); //because of both

                }else if(DateTime.Now > expirationDate && (secret.remainingViews != 0)){

                    return NotFound($"This secret expired at {expirationDate}");//because of time
                }
                else if(DateTime.Now < expirationDate && (secret.remainingViews == 0))
                {
                    return NotFound("Can't view more time."); //because of remainingViews
                }
                else if(DateTime.Now < expirationDate && (secret.remainingViews != 0))
                {
                    secret.remainingViews -= 1;
                    _context.Entry(secret).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok(secret);
                }
            }

            secret.remainingViews -= 1;
            _context.Entry(secret).State = EntityState.Modified;
            await _context.SaveChangesAsync();
           
            return Ok(secret);
        }


        /*
        // PUT: api/Secrets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSecret(long id, Secret secret)
        {
           if (id != secret.ID)
           {
               return BadRequest();
           }

           _context.Entry(secret).State = EntityState.Modified;

           try
           {
               await _context.SaveChangesAsync();
           }
           catch (DbUpdateConcurrencyException)
           {
               if (!SecretExists(id))
               {
                   return NotFound();
               }
               else
               {
                   throw;
               }
           }

           return NoContent();
        }*/


        // POST: api/Secrets
        /// <summary>
        /// Add a new secret
        /// </summary>
        /// <response code="201">New secret saved</response>
        /// <param name="hash"></param>
        /// <param name="secretText"> This text will be saved as a secret</param>
        /// <param name="expiresAfterViews">The secret won't be available after the given number of views. It must be greater than 0.</param>
        /// <param name="expiresAfter">The secret won't be available after the given time. The value is provided in minutes. 0 means never expires.</param>
        [ProducesResponseType(201)]
        [Produces("application/json", "application/xml")]
        [HttpPost, Description("Add a new secret")]
        public async Task<ActionResult<Secret>> PostSecret([Required] string hash,[Required]string secretText, [Required] int expiresAfterViews, [Required] int expiresAfter )
        {
            Secret secret = new Secret();
          if (_context.Secrets == null)
          {
              return Problem("Entity set 'SecretContext.Secrets'  is null.");
          }
            secret.hash = hash;
            secret.createdAt = DateTime.Now.ToString("g");
            secret.secretText = secretText;
            secret.remainingViews = expiresAfterViews;
            secret.expiresAt = expiresAfter;

            _context.Secrets.Add(secret);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostSecret", new { secret.hash }, secret);
        }

        /*
        // DELETE: api/Secrets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSecret(long id)
        {
            if (_context.Secrets == null)
            {
                return NotFound();
            }
            var secret = await _context.Secrets.FindAsync(id);
            if (secret == null)
            {
                return NotFound();
            }

            _context.Secrets.Remove(secret);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        */
        private bool SecretExists(long id)
        {
            return (_context.Secrets?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
