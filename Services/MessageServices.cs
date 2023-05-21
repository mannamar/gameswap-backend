using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gameswap_backend.Models;
using gameswap_backend.Services.Context;
using Microsoft.AspNetCore.Mvc;

namespace gameswap_backend.Services
{
    public class MessageService : ControllerBase
    {
        private readonly DataContext _context;
        public MessageService(DataContext context){
            _context = context;
        }

        //function that checks the database for all messages sent between two users
        public IEnumerable<MessageModel> GetAllMsgs2Users(int User1Id, int User2Id){
            return _context.MessageInfo.Where(item => ((item.FromUserId == User1Id && item.ToUserId == User2Id) || (item.FromUserId == User2Id && item.ToUserId == User1Id)));
        }
    }
}