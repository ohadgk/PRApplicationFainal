using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCF_Azure_Service;

namespace WCF_DataService
{
    //This class is a converter btween the entities in the PRApplication.Entities to the entities created by the PRApplicationEDMX
    public class Converter
    {

        

        public WCF_Azure_Service.User UserEntitiesToWCF(PRApplication.Entities.User entityUser)
        {

            return new WCF_Azure_Service.User
            {
                Id = entityUser.Id,
                IsAdmin = entityUser.IsAdmin,
                Password = entityUser.Password,
                UserName = entityUser.UserName

            };
        }


        public PRApplication.Entities.User UserWCFToEntities(WCF_Azure_Service.User EUser)
        {
            return new PRApplication.Entities.User
            {
                Id = EUser.Id,
                IsAdmin = EUser.IsAdmin,
                Password = EUser.Password,
                UserName = EUser.UserName
            };
        }




        public WCF_Azure_Service.Guest GuestEntitiesToWCF(PRApplication.Entities.Guest entityGuest, bool eventInclude = true)
        {


            var newGuest = new WCF_Azure_Service.Guest
            {
                AllCompanionsArrived = entityGuest.AllCompanionsArrived,
                Atended = entityGuest.Atended,
                AtendedCompanions = entityGuest.AtendedCompanions,
                Companions = entityGuest.Companions,
                EventId = entityGuest.EventId,

                FirstName = entityGuest.FirstName,
                Id = entityGuest.Id,
                Image = entityGuest.Image,
                LastName = entityGuest.LastName,
                QRCode = entityGuest.QRCode


            };
            //Avoid Circular 
            if (eventInclude)
                newGuest.Event = EventEntitiesToWCF(entityGuest.Event);

            return newGuest;
        }

        public PRApplication.Entities.Guest GuestWCFToEntities(WCF_Azure_Service.Guest guest, bool eventInclude = true)
        {
            var newGuest = new PRApplication.Entities.Guest{

            
                AllCompanionsArrived = guest.AllCompanionsArrived,
                Atended = guest.Atended,
                AtendedCompanions = guest.AtendedCompanions,
                Companions = guest.Companions,
                EventId = guest.EventId,
                
                FirstName = guest.FirstName,
                Id = guest.Id,
                Image = guest.Image,
                LastName = guest.LastName,
                QRCode = guest.QRCode


            };
            //Avoid Circular 
            if (eventInclude)
                newGuest.Event = EventWCFToEntities(guest.Event);

            return newGuest;
        }



        public WCF_Azure_Service.Event EventEntitiesToWCF(PRApplication.Entities.Event entityEvent,bool guestsInclude = true)
        {
            var newEvent = 
             new WCF_Azure_Service.Event
            {
                Id = entityEvent.Id,
                Name = entityEvent.Name,
                StartDate = entityEvent.StartDate
            };

            if (guestsInclude)
                newEvent.Guests = GuestsEntitiesToWCF(entityEvent.Guests);

            return newEvent;
        }

        public PRApplication.Entities.Event EventWCFToEntities(WCF_Azure_Service.Event Eevent, bool guestsInclude = true)
        {
            var newEvent=
             new PRApplication.Entities.Event
            {
                Id = Eevent.Id,
                Name = Eevent.Name,
                StartDate = Eevent.StartDate
            };

            if (guestsInclude)
                newEvent.Guests = GuestsWCFToEntities(Eevent.Guests);

            return newEvent;

        }


        public ICollection<WCF_Azure_Service.Guest> GuestsEntitiesToWCF(ICollection<PRApplication.Entities.Guest> collection)
        {
            List<WCF_Azure_Service.Guest> ReturnList = new List<WCF_Azure_Service.Guest>();

            foreach (var Guest in collection)
            {
                ReturnList.Add(GuestEntitiesToWCF(Guest, false));
            }
            return ReturnList;

        }

        public ICollection<WCF_Azure_Service.Event> EventsEntitiesToWCF(ICollection<PRApplication.Entities.Event> collection)
        {
            List<WCF_Azure_Service.Event> ReturnList = new List<WCF_Azure_Service.Event>();
            foreach (var Event in collection)
            {
                ReturnList.Add(EventEntitiesToWCF(Event,false));
            }
            return ReturnList;
        }

        public ICollection<PRApplication.Entities.Guest> GuestsWCFToEntities(ICollection<WCF_Azure_Service.Guest> collection)
        {
            List<PRApplication.Entities.Guest> ReturnList = new List<PRApplication.Entities.Guest>();
            foreach (var Guest in collection)
            {
                ReturnList.Add(GuestWCFToEntities(Guest,false));
            }
            return ReturnList;
        }

        public ICollection<WCF_Azure_Service.User> UsersEntitiesToWCF(ICollection<PRApplication.Entities.User> EUsers)
        {
            List<WCF_Azure_Service.User> returnList = new List<WCF_Azure_Service.User>();
            foreach (var user in EUsers)
            {
                returnList.Add(UserEntitiesToWCF(user));
            }

            return returnList;
        }
    }
}
