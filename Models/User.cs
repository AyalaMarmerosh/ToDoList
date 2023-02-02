
using System;

namespace Tasks.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string password { get; set; }
        public bool Admin{get;set;}
    }
}