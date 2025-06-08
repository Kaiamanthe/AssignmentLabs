using AssignmentLibrary.Core;
using AssignmentLibrary.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace AssignmentLibrary.Api.Models
{
    /// <summary>
    /// Data transfer object for creating or updating an assignment via API.
    /// </summary>
    public class AssignmentDto
    {
        /// <summary>
        /// Title of assignment.
        /// Field is required can't be null or empty.
        /// </summary>
        [Required]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Description of assignment.
        /// Field is required can't be null or empty.
        /// </summary>
        [Required]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Optional notes attach to assignment.
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// Assignmetent completion.
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Priority enum of assignment.
        /// Defaults to Medium.
        /// </summary>
        public Priority Priority { get; set; } = Priority.Medium;
    }
}
