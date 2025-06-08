using AssignmentLibrary.Api.Models;
using AssignmentLibrary.Core.Interfaces;
using AssignmentLibrary.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentLibrary.Api.Controllers
{
    /// <summary>
    /// Controller managing assignment API endpoints.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _service;

        /// <summary>
        /// Initialize new instance of <see cref="AssignmentController"/> class.
        /// </summary>
        /// <param name="service">Service for assignment operations.</param>
        public AssignmentController(IAssignmentService service)
        {
            _service = service;
        }

        /// <summary>
        /// Add new assignment.
        /// </summary>
        /// <param name="dto">The assignment data transfer object.</param>
        /// <returns>
        /// <c>201 Created</c> if successful, <c>409 Conflict</c> if duplicate title, or <c>400 Bad Request</c> if validation fails.
        /// </returns>
        [HttpPost]
        public IActionResult AddAssignment(AssignmentDto dto)
        {
            try
            {
                var assignmentdto = new Assignment(dto.Title, dto.Description, dto.Notes, dto.IsCompleted, dto.Priority);
                var success = _service.AddAssignment(assignmentdto);
                if (!success)
                    return Conflict("Assignment with this title already exists.");
                return CreatedAtAction(nameof(GetByTitle), new { title = dto.Title }, dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Adds/updates note for assignment.
        /// </summary>
        /// <param name="title">The title of the assignment.</param>
        /// <param name="note">The note to add or update.</param>
        /// <returns><c>200 OK</c> if updated; <c>404 Not Found</c> if assignment is missing.</returns>
        [HttpPatch("{title}/note")]
        public IActionResult AddNoteToAssignment(string title, [FromBody] string note)
        {
            var assignment = _service.FindAssignmentByTitle(title);
            if (assignment == null)
            {
                return NotFound("Assignment not found.");
            }

            assignment.UpdateNote(note ?? string.Empty);
            return Ok("Note added or updated successfully.");
        }

        /// <summary>
        /// Retrieves all assignments.
        /// </summary>
        /// <returns>Lists all assignments.</returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.ListAll());
        }

        /// <summary>
        /// Retrieves all incomplete assignments.
        /// </summary>
        /// <returns>Lists all incomplete assignment.</returns>
        [HttpGet("incomplete")]
        public IActionResult GetIncomplete()
        {
            return Ok(_service.ListIncomplete());
        }

        /// <summary>
        /// Retrieves all assignments ordered by priority (highest to lowest).
        /// </summary>
        /// <returns>List assignments by priority.</returns>
        [HttpGet("by-priority")]
        public IActionResult GetByPriority()
        {
            var sorted = _service.ListAssignmentsByPriority();
            return Ok(sorted);
        }

        /// <summary>
        /// Retrieve assignment by title.
        /// </summary>
        /// <param name="title">Title to find assignment.</param>
        /// <returns><c>200 OK</c> with assignment if found, <c>404 Not Found</c> otherwise.</returns>
        [HttpGet("{title}")]
        public IActionResult GetByTitle(string title)
        {
            var assignment = _service.FindAssignmentByTitle(title);
            return assignment == null ? NotFound() : Ok(assignment);
        }

        /// <summary>
        /// Updates assignment.
        /// </summary>
        /// <param name="title">Title to find assignment.</param>
        /// <param name="dto">The assignment data transfer object.</param>
        /// <returns>
        /// <c>204 No Content</c> if updated, <c>404 Not Found</c> if missing, <c>400 Bad Request</c> if validation fails.
        /// </returns>
        [HttpPut("{title}")]
        public IActionResult UpdateAssignment(string title, AssignmentDto dto)
        {
            try
            {
                var updated = _service.UpdateAssignment(title, dto.Title, dto.Description, dto.Notes, dto.IsCompleted, dto.Priority);
                return updated ? NoContent() : NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Mark assignmetn complete.
        /// </summary>
        /// <param name="title">Title to find assignment.</param>
        /// <returns>
        /// <c>200 OK</c> with updated assignment if successful, <c>404 Not Found</c> if assignment does not exist.
        /// </returns>
        [HttpPatch("{title}/complete")]
        public IActionResult MarkAssignmentComplete(string title)
        {
            var success = _service.MarkAssignmentComplete(title);
            if (!success) return NotFound();

            var assignment = _service.FindAssignmentByTitle(title);
            return Ok(assignment);
        }

        /// <summary>
        /// Deletes assignment by title.
        /// </summary>
        /// <param name="title">Title to find assignment.</param>
        /// <returns><c>204 No Content</c> if deleted; <c>404 Not Found</c> if not found.</returns>
        [HttpDelete("{title}")]
        public IActionResult Delete(string title)
        {
            var success = _service.DeleteAssignment(title);
            return success ? NoContent() : NotFound();
        }
    }
}
