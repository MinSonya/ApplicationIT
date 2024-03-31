using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplicationIT.Models;
using static System.Net.Mime.MediaTypeNames;

namespace ApplicationIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly RequestContext _context;

        public RequestsController(RequestContext context)
        {
            _context = context;
        }

        // GET: api/Requests
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        //{
        //  if (_context.Requests == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.Requests.ToListAsync();
        //}

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(Guid id)
        {
          if (_context.Requests == null)
          {
              return NotFound();
          }
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        [HttpGet]
        [Route("applications")]
        public IActionResult GetUnsubmittedApplications(DateTime unsubmittedOlder)
        {
            // Парсим переданную дату из строки в формат DateTime
            DateTime unsubmittedOlderDate = unsubmittedOlder;

            // Выполняем запрос к базе данных
            var unsubmittedApplications = _context.Requests
                .Where(a => a.submit == false || a.date > unsubmittedOlderDate)
                .ToList();

            // Преобразуем полученные заявки в формат ответа
            var result = unsubmittedApplications.Select(a => a.ToShortForm()).ToList();

            return Ok(result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(Guid id, Request request)
        {
            if (id != request.id)
            {
                return BadRequest("Id");
            }

            if (request.submit == true)
            {
                return BadRequest("Submitted requests cannot be modified.");
            }

            if (request.author == null && (request.activity != null || request.name != null || request.outline != null))
            {
                return Problem("Required fields are not filled in");
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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

        [HttpPost("{Id}/submit")]
        public async Task<IActionResult> SubmitApplication(Guid Id)
        {
            if(!RequestExists(Id))
                return NotFound();

            var request = _context.Requests.FirstOrDefault(r => r.id == Id);

            if (request != null)
            {
                if (request.author == null && request.outline == null && request.activity == null && request.name == null)
                {
                    return Problem("Required fields are not filled in");
                }

                request.submit = true;

                request.submitDate = DateTime.Now.ToUniversalTime();

                // Сохраняем изменения в базе данных
                await _context.SaveChangesAsync();
            }

            return Ok("Application submitted successfully.");
        }

        [HttpGet("submitted-after")]
        public IActionResult GetApplicationsSubmittedAfterDate(DateTime date)
        {
            var applications = _context.Requests
                .Where(a => a.submitDate > date && a.submit == true)
                .ToList();

            return Ok(applications);
        }

        [HttpGet]
        [Route("users/{id}/currentapplication")]
        public IActionResult GetCurrentApplicationForUser(Guid id)
        {
            // Выполняем запрос к базе данных, чтобы найти не поданную заявку для указанного пользователя
            var currentApplication = _context.Requests
                .FirstOrDefault(a => a.author == id && a.submit == false);

            if (currentApplication == null)
            {
                // Если заявка не найдена, возвращаем код состояния 404 Not Found
                return NotFound("No current application found for the user.");
            }

            // Формируем объект ответа в формате JSON на основе найденной заявки
            var result = currentApplication.ToShortForm();

            // Возвращаем найденную заявку в ответе на запрос
            return Ok(result);
        }

        [HttpGet]
        [Route("activities")]
        public IActionResult GetActivities()
        {
            // Предопределенные значения для всех активностей
            var activities = new[]
            {
                 new { activity = "Report", description = "Доклад, 35-45 минут" },
                 new { activity = "Masterclass", description = "Мастеркласс, 1-2 часа" },
                 new { activity = "Discussion", description = "Дискуссия / круглый стол, 40-50 минут" }
            };

            // Возвращаем значения активностей в ответе на запрос
            return Ok(activities);
        }

        // POST: api/Requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
          if (_context.Requests == null)
          {
              return Problem("Entity set 'RequestContext.Requests'  is null.");
          }

            if (_context.Requests != null)
            {
                var existingRequest = await _context.Requests
                                    .Where(r => r.author == request.author && r.submit == false)
                                    .FirstOrDefaultAsync();

                if (existingRequest != null)
                {
                    return Conflict("An unsubmitted request already exists for this author.");
                }
            }

            if(request.author == null && (request.activity != null  || request.name != null || request.outline != null))
            {
                return Problem("Required fields are not filled in");
            }

           // request.id = Guid.NewGuid();
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.id }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(Guid id)
        {
            if (_context.Requests == null)
            {
                return NotFound();
            }
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            if(request.submit == true)
            {
                return BadRequest("You cannot delete an application that has been submitted for review");
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(Guid id)
        {
            return (_context.Requests?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
