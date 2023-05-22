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
    public class MessageController : ControllerBase
    {
        private readonly MessageService _data;
        public MessageController(MessageService data){
            _data = data;
        }

        //Find all messages between two users endpoint
        [HttpGet]
        [Route("GetAllMsgs2Users/{User1Id}/{User2Id}")]
        public IEnumerable<MessageModel> GetAllMsgs2Users(int User1Id, int User2Id){
            return _data.GetAllMsgs2Users(User1Id, User2Id);
        }

        //Send a message to a user endpoint
        [HttpPost]
        [Route("SendMsg")]
        public bool SendMsg(MessageDTO Message){
            return _data.SendMsg(Message);
        }
    }
}