using Microsoft.AspNetCore.Mvc;
using SimpleNotes.Api.Interfaces;
using SimpleNotes.Api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimpleNotes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _service;

        public NoteController(INoteService service)
        {
            _service = service;
        }
        public IActionResult GetAll()
        {
            var notes = _service.GetAll();
            return Ok(notes);
        }

        // GET api/<NoteController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var note = _service.GetById(id);
            return note is null ? NotFound() : Ok(note);
        }

        // POST api/<NoteController>
        [HttpPost]
        public IActionResult Create([FromBody]Note note)
        {
            _service.Add(note);
            return CreatedAtAction(nameof(Get), new { id = note.Id }, note);

        }
        // DELETE api/<NoteController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }
    }
}
