using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gameswap_backend.Models;
using gameswap_backend.Models.DTO;
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

        public dynamic GetAllMsgPartners(int userId){
            var allMessagePartners = _context.MessageInfo.Where(item => (item.FromUserId == userId || item.ToUserId == userId)).ToList();

            List<MessagePersonDTO> people = new List<MessagePersonDTO>();
            foreach (var message in allMessagePartners){
                if (message.FromUserId != userId){
                    int searchId = message.FromUserId;
                    MessagePersonDTO personFound = people.Find(person => person.UserId == searchId);
                    if (personFound == null){
                        MessagePersonDTO personToAdd = new MessagePersonDTO
                        {
                            UserId = searchId,
                            Username = message.FromUsername
                        };
                        people.Add(personToAdd);
                    }
                } else {
                    int searchId = message.ToUserId;
                    MessagePersonDTO personFound = people.Find(person => person.UserId == searchId);
                    if (personFound == null){
                        MessagePersonDTO personToAdd = new MessagePersonDTO
                        {
                            UserId = searchId,
                            Username = message.FromUsername
                        };
                        people.Add(personToAdd);
                    }
                }
            }
            return people;
        }

        //function that creates a new message from the front end input via a DTO and saves to the database.
        public bool SendMsg(MessageDTO Message){
            //creating a new, empty instance of our message model for saving the message in the database
            MessageModel newMessage = new MessageModel();
            //putting together the message via the info from the DTO and auto-filling in variables the front end need not deal with at the moment
            newMessage.Id = 0;
            newMessage.FromUserId = Message.FromUserId;
            newMessage.FromUsername = Message.FromUsername;
            newMessage.ToUserId = Message.ToUserId;
            newMessage.ToUsername = Message.ToUsername;
            newMessage.Message = Message.Message;
            newMessage.TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            newMessage.isDeleted = false;
            newMessage.isRead = false;
            //adding message to our database
            _context.Add(newMessage);
            return _context.SaveChanges() != 0;
        }
    }
}