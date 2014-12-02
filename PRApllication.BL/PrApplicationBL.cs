using OnBarcode.Barcode.BarcodeScanner;
using PRApplication.Dal.PrDAL;
using PRApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRApllication.BL
{
    public class PrApplicationBL
    {
        PrApplicationDAL dalObj = new PrApplicationDAL();

        public ICollection<Guest> GetGuests(int eventId, string guestFullName)
        {
            return dalObj.GetGuests(eventId, guestFullName);
        }

        public Guest GetSpecificGuest(int eventId, int guestId)
        {
            return dalObj.GetSpecificGuest(eventId, guestId);
        }
        public bool ChangeGuestStatus(int eventId, int guestId, bool attended, bool allCompanionsArrived, int companionsThatArrived)
        {
            if (attended == null) return false;
            return dalObj.ChangeGuestStatus(eventId, guestId, attended, allCompanionsArrived, companionsThatArrived);
        }
        public Event GetEvent(int eventId)
        {
            return dalObj.GetEvent(eventId);
        }
        public ICollection<Event> GetEvents()
        {
            return dalObj.GetEvents();
        }
        public Event GetEvent(string eventName, DateTime eventDate)
        {
            return dalObj.GetEvent(eventName, eventDate);
        }
        public ICollection<Event> GetEvents(string eventName)
        {
            return dalObj.GetEvents(eventName);
        }
        public ICollection<Event> GetEvents(DateTime eventDate)
        {
            return dalObj.GetEvents(eventDate);
        }
        public bool CreateEvent(string eventName, DateTime eventDate)
        {
            if (string.IsNullOrWhiteSpace(eventName) && eventDate == default(DateTime))
                return false;
            return dalObj.CreateEvent(eventName, eventDate);
        }
        public bool AddGuest(int eventId, Guest guest)
        {
            if (eventId < 1 || guest == null)
                return false;
            return dalObj.AddGuest(eventId, guest);
        }
        public bool RemoveGuest(int eventId, int guestId)
        {
            Guest guestToRemove = dalObj.GetSpecificGuest(eventId, guestId);
            if (guestToRemove == null)
                return false;
            return dalObj.RemoveGuest(guestToRemove);
        }
        public ICollection<User> GetUsers()
        {
            return dalObj.GetUsers();
        }
        public User GetUserById(int userId)
        {
            return dalObj.GetUserById(userId);
        }
        public bool AddUser(string userName, string password, bool isAdmin)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password) || isAdmin == null)
                return false;
            return dalObj.AddUser(userName, password, isAdmin);
        }
        public bool AddUser(User newUser)
        {
            if (newUser == null) return false;
            return dalObj.AddUser(newUser);
        }
        public bool RemoveUser(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return false;
            return dalObj.RemoveUser(userName);
        }
        public bool RemoveEvent(int eventId)
        {
            Event eventToRemove = dalObj.GetEvent(eventId);
            if (eventToRemove == null)
                return false;
            return dalObj.RemoveEvent(eventToRemove);
        }



    }
}
