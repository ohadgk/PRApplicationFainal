using PRApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PRApplication.Dal.PrDAL
{
    public class PrApplicationDAL
    {
        PRApplicationDBContext context = new PRApplicationDBContext();



        //public ICollection<Guest> GetGuests(int eventId)
        //{
        //    return context.Guests.Include("Event").Where(g => g.EventId == eventId).ToList();
        //}

        public ICollection<Guest> GetGuests(int eventId, string guestFullName)
        {
            return context.Guests.Include("Event").Where(
                g => g.EventId == eventId && (g.FirstName + g.LastName).Trim().Contains(guestFullName.Trim())
                ||
                (g.EventId == eventId && (g.LastName + g.FirstName).Trim().Contains(guestFullName.Trim())))
                .ToList();
        }

        public Guest GetSpecificGuest(int eventId, int guestId)
        {
            return context.Guests.Include("Event").FirstOrDefault(g => g.Id == guestId && g.EventId == eventId);
        }

        public bool ChangeGuestStatus(int eventId, int guestId, bool attended, bool allCompanionsArrived, int companionsThatArrived)
        {
            var currentUser = GetSpecificGuest(eventId, guestId);

            currentUser.AtendedCompanions = companionsThatArrived;
            currentUser.AllCompanionsArrived = allCompanionsArrived;
            currentUser.Atended = attended;

            if (currentUser.AtendedCompanions == currentUser.Companions)
                currentUser.AllCompanionsArrived = true;
            if (allCompanionsArrived)
                currentUser.AtendedCompanions = currentUser.Companions;

            context.Entry<Guest>(currentUser).State = System.Data.Entity.EntityState.Modified;

            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Event GetEvent(int eventId)
        {
            return context.Events.Include("Guests").FirstOrDefault(e => e.Id == eventId);
        }

        public ICollection<Event> GetEvents()
        {
            return context.Events.Include("Guests").ToList();
        }

        public Event GetEvent(string eventName, DateTime eventDate)
        {
            return context.Events.Include("Guests").FirstOrDefault(e => e.Name.Contains(eventName) && e.StartDate.Day == eventDate.Day);
        }

        public ICollection<Event> GetEvents(string eventName)
        {
            return context.Events.Include("Guests").Where(e => e.Name.Contains(eventName)).ToList();
        }

        public ICollection<Event> GetEvents(DateTime eventDate)
        {
            return context.Events.Include("Guests").Where(e => e.StartDate.Day == eventDate.Day).ToList();
        }

        public bool CreateEvent(string eventName, DateTime eventDate)
        {
            Event newEvent = new Event { StartDate = eventDate, Name = eventName };
            try
            {
                context.Events.Add(newEvent);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddGuest(int eventId, Guest guest)
        {
            if (guest.Event == null || string.IsNullOrEmpty(guest.FirstName) || string.IsNullOrEmpty(guest.LastName) || string.IsNullOrEmpty(guest.QRCode))
            {
                return false;
            }
            
            var eventToAdd = guest.Event;

            try
            {
                context.Events.Attach(eventToAdd);

                context.Guests.Add(guest);

                context.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool RemoveGuest(Guest guestToRemove)
        {
            try
            {
                context.Guests.Remove(guestToRemove);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ICollection<User> GetUsers()
        {
            return context.Users.ToList();
        }

        public User GetUserById(int userId)
        {
            return context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public bool AddUser(string userName, string password, bool isAdmin)
        {
            User newUser = new User { IsAdmin = isAdmin, UserName = userName, Password = password };

            try
            {
                context.Users.Add(newUser);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddUser(User newUser)
        {
            try
            {
                context.Users.Add(newUser);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveUser(string userName)
        {
            List<User> usersToRemove = context.Users.Where(u => u.UserName == userName).ToList();

            if (usersToRemove.Count == 0)
                return false;

            try
            {
                if (usersToRemove.Count() == 1)
                    context.Users.Remove(usersToRemove.First());
                else
                    context.Users.RemoveRange(usersToRemove);

                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveEvent(Event eventToRemove)
        {
            try
            {
                context.Events.Remove(eventToRemove);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}