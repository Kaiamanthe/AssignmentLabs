using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssignmentLibrary.Core.Interfaces;
using AssignmentLibrary.Core.Models;

namespace AssignmentLibrary.UI
{
    public class AssignmentFormatter : IAssignmentFormatter
    {
        public string Format(Assignment assignment)
        {
            return $"Assignment ID: {assignment.Id}, Title: {assignment.Title}, Description: {assignment.Description}";
        }
    }
}
