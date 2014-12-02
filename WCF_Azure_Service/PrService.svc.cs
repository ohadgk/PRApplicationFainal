using PRApllication.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WCF_DataService;

namespace WCF_Azure_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class PrService : IPrService
    {

        PrApplicationBL BlObj = new PrApplicationBL();
        Converter converter = new Converter();


        public ICollection<Guest> GetGuests(int eventId, string guestFullName)
        {
            return converter.GuestsEntitiesToWCF(BlObj.GetGuests(eventId, guestFullName));
        }



        public Guest GetSpecificGuest(int eventId, int guestId)
        {
            return converter.GuestEntitiesToWCF(BlObj.GetSpecificGuest(eventId, guestId));
        }


        public bool ChangeGuestStatus(int eventId, int guestId, bool attended, bool allCompanionsArrived, int companionsThatArrived)
        {
            return BlObj.ChangeGuestStatus(eventId, guestId, attended, allCompanionsArrived, companionsThatArrived);
        }



        public Event GetEvent(int eventId)
        {
            return converter.EventEntitiesToWCF(BlObj.GetEvent(eventId));
        }



        public bool RemoveEvent(int eventId)
        {
            return BlObj.RemoveEvent(eventId);
        }

        public ICollection<Event> GetEvents()
        {
            return converter.EventsEntitiesToWCF(BlObj.GetEvents());
        }

        public Event GetEvent(string eventName, DateTime eventDate)
        {
            return converter.EventEntitiesToWCF(BlObj.GetEvent(eventName, eventDate));
        }

        public ICollection<Event> GetEvents(string eventName)
        {
            return converter.EventsEntitiesToWCF(BlObj.GetEvents(eventName));
        }

        public ICollection<Event> GetEvents(DateTime eventDate)
        {
            return converter.EventsEntitiesToWCF(BlObj.GetEvents(eventDate));
        }

        public bool CreateEvent(string eventName, DateTime eventDate)
        {
            return BlObj.CreateEvent(eventName, eventDate);
        }

        public bool AddGuest(int eventId, Guest guest)
        {
            if (guest.EventId == 0)
                guest.EventId = eventId;
            if (guest.Event == null)
            {
                guest.Event = GetEvent(eventId);
            }

            var conGuest = converter.GuestWCFToEntities(guest);
            return BlObj.AddGuest(eventId, conGuest);
        }

        public bool RemoveGuest(int eventId, int guestId)
        {
            return BlObj.RemoveGuest(eventId, guestId);
        }

        public ICollection<User> GetUsers()
        {
            return converter.UsersEntitiesToWCF(BlObj.GetUsers());
        }

        public User GetUserById(int userId)
        {
            return converter.UserEntitiesToWCF(BlObj.GetUserById(userId));
        }

        public bool AddUser(string userName, string password, bool isAdmin)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(userName))
                return false;

            password = EncryptPass(password);

            return BlObj.AddUser(userName, password, isAdmin);
        }


        public bool RemoveUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return false;
            return BlObj.RemoveUser(userName);
        }



        public string EncryptPass(string password)
        {
            {
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] hashValue;
                byte[] message = UE.GetBytes(password);

                SHA256Managed hashString = new SHA256Managed();
                string hex = "";

                hashValue = hashString.ComputeHash(message);
                foreach (byte x in hashValue)
                {
                    hex += String.Format("{0:x2}", x);
                }
                return hex;
            }
        }

    }
}

