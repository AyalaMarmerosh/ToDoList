using Tasks.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Tasks.Services;

namespace Tasks.Interfaces
{
    public interface ITaskService
    {
        string Login(User user);
        List<Task> GetAll();
        Task Get(int id);
        void Add(Task task);
        void Delete(int id);
        void Update(Task task);
        // int Count {get;}
        
        //user פונקציות לניהול
        List<User> GetAllUsers();
        User GetUser(int id);
        User GetCurrentUser();
        User AddUser(User user);
        void DeleteUser(int id);
        int Count {get;}
    }
}