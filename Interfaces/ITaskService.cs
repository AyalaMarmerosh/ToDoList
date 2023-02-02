using Tasks.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Tasks.Services;

namespace Tasks.Interfaces
{
    public interface ITaskService
    {
        List<Task> GetAll(int id);
        Task Get(int id);
        void Add(Task task);
        void Delete(int id);
        void Update(Task task);
        void Login(User user);

        // ActionResult<List<Task>> GetAll(string token);

        int Count {get;}
    }
}