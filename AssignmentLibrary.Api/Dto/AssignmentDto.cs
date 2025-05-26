using AssignmentLibrary.Core;
using AssignmentLibrary.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace AssignmentLibrary.Api.Models
{
    public class AssignmentDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        public Priority Priority { get; set; } = Priority.Medium;
    }
}
