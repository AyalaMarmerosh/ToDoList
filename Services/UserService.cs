using System.Collections.Generic;
using System.Text.Json;
using Tasks.Interfaces;
using System.Linq;
using System.IO;
using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Tasks.Models;



namespace Tasks.Services
{
    public class UserService : IUserService
    {
        List<User> Users { get; }
        static int counter = 1;
        // private static string fileName = "User.json";
        private IWebHostEnvironment webHost;
        private string filePath;
        public UserService(IWebHostEnvironment webHost)
        {
            this.webHost=webHost;
            filePath=Path.Combine(webHost.ContentRootPath,"user.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                Users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            if(Users.Count()>0){
                counter=Users[Users.Count()-1].Id+1;
            }
        }
        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(Users));
        }
        public List<User> GetAll() => Users;

        public User Get(int id) => Users.FirstOrDefault(p => p.Id == id);

        public void Add(User user)
        {
            user.Id = Count;
            Users.Add(user);
            saveToFile();
        }
        public void Update(User user)
        {
            var index = Users.FindIndex(u => u.password.Equals(user.password));
            if (index == -1)
                return;
            Users[index] = user;
            saveToFile();
        }
        public void Delete(int id)
        {
            var user = Get(id);
            if (user is null)
                return;
            Users.Remove(user);
            saveToFile();
        }

        public bool isExist(string name, string password)
        {
            foreach (var user in Users)
            {
                if (user.Name.Equals(name) && user.password.Equals(password))
                {
                    return true;
                }
            }
            return false;
        }
        public int findId(string name, string password)
        {
            var idUser = Users.FirstOrDefault(u => u.Name.Equals(name) && u.password.Equals(password));
            return idUser.Id;
        }
        public int Count => counter++;
    }
}