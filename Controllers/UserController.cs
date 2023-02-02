using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Tasks.Models;
using Tasks.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Tasks.Services;
using System;

namespace Tasks.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private ITokenService TokenService;
        private ITaskService TaskService;
        private IUserService UserService;
        private User user;
         public UserController(IUserService UserService, ITokenService tokenService, ITaskService taskService){
            this.TokenService = tokenService;
            this.TaskService = taskService;
            this.UserService = UserService;        }
        
        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] User user)
        {
            user.Id = UserService.findId(user.Name, user.password);
            var claims = new List<Claim>();

            if(user.Name!="ayala"
            || user.password!="8520741")
            {
                if (UserService.isExist(user.Name, user.password))
                {
                    claims.Add(new Claim("type", "User"));
                    claims.Add(new Claim("name", user.Name));
                    claims.Add(new Claim("user", user.password));
                    claims.Add(new Claim("UserId", user.Id.ToString()));
                    TaskService.Login(user);
                }
                else
                    return Unauthorized();
            }
            else
            {
                claims.Add(new Claim("type", "Admin"));
                claims.Add(new Claim("name", user.Name));
                claims.Add(new Claim("user", user.password));
                claims.Add(new Claim("UserId", user.Id.ToString()));
                TaskService.Login(user);
            }
            var token = TokenService.GetToken(claims);
            return new OkObjectResult(TokenService.WriteToken(token));
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public ActionResult<List<User>> Get(){
            return UserService.GetAll();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Admin")]
        public ActionResult<User> Get(int id)
        {
            var user = UserService.Get(id);

            if (user == null)
                return NotFound();
            return user;
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public ActionResult Post(User user)
        {
            UserService.Add(user: user);
            return CreatedAtAction(nameof(Post), new { id = user.Id }, user);
        }
        
        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            var user = UserService.Get(id);
            if (user == null)
                return NotFound();
            UserService.Delete(id);
            return Content(UserService.Count.ToString());
        }
    }
}