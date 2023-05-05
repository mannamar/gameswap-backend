using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gameswap_backend.Models;
using gameswap_backend.Models.DTO;
using gameswap_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace gameswap_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _data;
        public UserController(UserService data){
            _data = data;
        }
        
        //Login user endpoint
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginDTO User){
            return _data.Login(User);
        }

        //Add a user endpoint
        [HttpPost]
        [Route("AddUser")]
        public string AddUser(CreateAccountDTO UserToAdd){
            return _data.AddUser(UserToAdd);
        }
        
        //Update user endpoint - requires information for the ENTIRE user model, including salt & hash - not going to work well for our purposes.
        // [HttpPost]
        // [Route("UpdateUser")]
        // public bool UpdateUser(UserModel userToUpdate){
        //     return _data.UpdateUser(userToUpdate);
        // }

        //Update username endpoint - used for only updating the username so as to bypass needing the salt & hash from the above model.
        [HttpPost]
        [Route("UpdateUser/{id}/{username}")]
        public bool UpdateUsername(int id, string username){
            return _data.UpdateUsername(id, username);
        }
        
        //Endpoint to hard delete a user from the user database. Not suggested to use in the website; just for testing purposes.
        [HttpDelete]
        [Route("HardDeleteUser/{userToDelete}")]
        public bool HardDeleteUser(string userToDelete){
            return _data.HardDeleteUser(userToDelete);
        }

        //Endpoint that soft deletes a user so that they don't appear on the front end, but are still archived in the tables for reference.
        [HttpPost]
        [Route("DeleteUser/{userToDelete}")]
        public bool DeleteUser(string username){
            return _data.HardDeleteUser(username);
        }
    }
}