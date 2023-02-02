
namespace Tasks.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool perform { get; set; }
        public int userId{get;set;}
    }
}