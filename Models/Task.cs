using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace ToDoList.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; } = false;
        public DateTime DueDate { get; set; }
        public string Category { get; set; } = "General";
    }
}
