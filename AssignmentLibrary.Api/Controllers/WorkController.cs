using Microsoft.AspNetCore.Mvc;
using AssignmentLibrary.Api.Interfaces;
using AssignmentLibrary.Api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AssignmentLibrary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkController : ControllerBase
    {
        private readonly IWorkService _service;
        public WorkController(IWorkService service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var works = _service.GetAll();
            return Ok(works);
        }

        // GET api/<NoteController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var work = _service.GetById(id);
            return work is null ? NotFound() : Ok(work);
        }

        // POST api/<NoteController>
        [HttpPost]
        public IActionResult Create([FromBody] Work work)
        {
            _service.Add(work);
            return CreatedAtAction(nameof(Get), new { id = work.Id }, work);
        }

        // DELETE api/<NoteController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }

        // PUT api/<NoteController>/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Work work)
        {
            if (id != work.Id)
            {
                return BadRequest("Note ID mismatch");
            }
            var updated = _service.Update(work);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
