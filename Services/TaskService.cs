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
        static int counter = 0;
        private static string fileName = "Task.json";
        private static string fileNameUser = "user.json";
        private string filePath;
        private IWebHostEnvironment webHost;
        public TaskService(IWebHostEnvironment webHost)
        {
            this.webHost=webHost;
            filePath=Path.Combine(webHost.ContentRootPath, "task.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                Tasks = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            if(MyUsers.Count>0){
                counter = MyUsers[MyUsers.Count-1].Id+1;
           }
            if (Tasks.Count() > 0)
            {
                counter = Tasks[Tasks.Count() - 1].Id + 1;
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
            File.WriteAllText(filePath, JsonSerializer.Serialize(Tasks));
        }
        private void SaveUsersToFile()
        {
            File.WriteAllText(fileNameUser, JsonSerializer.Serialize(MyUsers));
        }
        public List<Task> GetAll(int id) =>  Tasks.Where(t => t.userId == currentUser.Id)?.ToList();
         public Task Get(int id) => Tasks.FirstOrDefault(p => p.Id == id && p.Id == currentUser.Id);

        public void Add(Task task)
        {
            task.Id = Count;
            task.Id = currentUser.Id;
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
            Tasks[index] = task;
            saveToFile();
        }

        public int Count => counter++;
    }
}