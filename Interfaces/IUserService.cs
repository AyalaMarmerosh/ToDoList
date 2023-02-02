// using User.Models;
using System.Collections.Generic;
using Tasks.Services;
using Tasks.Models;
using Tasks.Controllers;

namespace Tasks.Interfaces
{
    public interface IUserService
    {
        List<User> GetAll();
        User Get(int id);
        void Add(User user);
        void Update(User user);
        void Delete(int id);
        bool isExist(string name, string password);
        int findId(string name, string password);
        // void Update(User user);
        int Count {get;}
    }
}