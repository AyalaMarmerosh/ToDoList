using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Tasks.Models;
using Tasks.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Tasks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private ITaskService TaskService;
        private int userId = 85;
        private string userName;
        public TaskController(ITaskService taskService, IHttpContextAccessor httpContextAccessor)
        {
            // var user = httpContextAccessor.HttpContext.User;
            // userName = user.FindFirst("Name")?.Value;
            // userId = int.Parse(user.FindFirst("Id")?.Value);
            this.TaskService = taskService;
        }

        [HttpGet]
        [Authorize(Policy="User")]
        public ActionResult<List<Task>> Get()
        {
            return TaskService.GetAll(userId);
        }


        [HttpGet("{id}")]//get לפי id מסוים
        [Authorize(Policy = "User")]
        public ActionResult<Task> Get(int id)
        {
            var task = TaskService.Get(id);
            if (task == null)
                return NotFound();
            return task;
        }

        [HttpPost] 
        
        public ActionResult Post(Task task)
        {
            TaskService.Add(task);
            return CreatedAtAction(nameof(Post), new {id=task.Id}, task);

        }

        [HttpPut("{id}")]
        [Authorize(Policy = "User")]
        public ActionResult Update(int id, Task task)
        {
            if (id != task.Id)
                return BadRequest();

            var existingTask = TaskService.Get(id);
            if (existingTask == null)
                return  NotFound();

            TaskService.Update(task);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "User")]
        public ActionResult Delete(int id)
        {
            var task = TaskService.Get(id);
            if (task == null)
                return  NotFound();
            TaskService.Delete(id);
            return Content(TaskService.Count.ToString());
        }
    }
}