using Tasks.Models;
using System.Collections.Generic;
using System.Linq;
using Tasks.Interfaces;
using System.Text.Json;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Tasks.Services{
    public class TaskService : ITaskService
    {
        List<Task> Tasks { get; }
        List<User> MyUsers {get; }

        private User currentUser {get; set;}
        static int counterUser = 1;
        static int counterTasks = 1;
        private static string fileName = "task.json";
        private static string fileNameUser = "user.json";
        // private string filePath;
        private IWebHostEnvironment webHost;
        public TaskService(/*IWebHostEnvironment webHost*/)
        {
            // filePath=Path.Combine(webHost.ContentRootPath, "task.json");
            using (var jsonFile = File.OpenText(fileName))
            {
                Tasks = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            using (var jsonFile = File.OpenText(fileNameUser))
            {
                MyUsers = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            if(MyUsers.Count>0){
                counterUser = MyUsers[MyUsers.Count-1].Id+1;
           }
            if (Tasks.Count > 0)
            {
                counterTasks = Tasks[Tasks.Count() - 1].Id + 1;
            }
        }
        public string Login(User user)
        {
              //משתנה שקולט בתוכו את הסיסמא והקוד של המשתמש
              currentUser = MyUsers.FirstOrDefault(u =>u.Name == user.Name && u.password == user.password );
              if(currentUser is null) 
              {
                  return null;
              } 
           //id -ומחזיר את ה
           return currentUser.Id.ToString();
        }
        private void saveToFile()
        {
            File.WriteAllText(fileName, JsonSerializer.Serialize(Tasks));
        }
        private void SaveUsersToFile()
        {
            File.WriteAllText(fileNameUser, JsonSerializer.Serialize(MyUsers));
        }
        public List<Task> GetAll() =>  Tasks.Where(t => t.userId == currentUser.Id)?.ToList();
         public Task Get(int id) => Tasks.FirstOrDefault(
            p =>{
          return p.Id == id && p.userId == currentUser.Id;
            } 
            );

        public void Add(Task task)
        {
            task.Id = counterTasks++;
            task.userId = currentUser.Id;
            Tasks.Add(task);
            saveToFile();
        }
        public void Delete(int id)
        {
            var task = Get(id);
            if (task is null)
                return;

            Tasks.Remove(task);
            saveToFile();
        }
       public void Update(Task task)
        {
            var index = Tasks.FindIndex(t => t.Id == task.Id);
            if (index == -1)
                return;
            task.userId =  Tasks[index].userId;
            Tasks[index] = task;
            saveToFile();
        }

        public int Count => Tasks.Count();
        // public int CountUsers => MyUsers.Count();

        //Get all users
        public List<User> GetAllUsers()
        {
            return MyUsers;
        }
        public User GetUser(int id) => MyUsers.FirstOrDefault(u => u.Id == id);
        public User GetCurrentUser() => currentUser; 
         //post user
      public User AddUser(User user)
        {
            user.Id = counterUser++;
            MyUsers.Add(user);
            SaveUsersToFile();
            return user;
        }


        //Delete user
        public void DeleteUser(int id)
        {
            User user = GetUser(id);
            if(user != null)
            {
                foreach (Task task in Tasks.ToList())
                {
                    if(task.Id == id)
                    {
                        Tasks.Remove(task);
                    }
                }
                MyUsers.Remove(user);
            }
            saveToFile();
            SaveUsersToFile();
        }

    }
}