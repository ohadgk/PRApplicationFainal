using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCF_Azure_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IPrService
    {

        [OperationContract(Name = "GetGuestsByEventIdAndGuestName")]
        ICollection<Guest> GetGuests(int eventId, string guestFullName);

        [OperationContract]
        string EncryptPass(string password);

        [OperationContract]
        Guest GetSpecificGuest(int eventId, int guestId);

        [OperationContract]
        bool ChangeGuestStatus(int eventId, int guestId, bool attended, bool allCompanionsArrived, int companionsThatArrived);

        [OperationContract(Name = "GetEventByID")]
        Event GetEvent(int eventId);

        [OperationContract]
        Event GetEvent(string eventName, DateTime eventDate);

        [OperationContract]
        bool RemoveEvent(int eventId);

        [OperationContract]
        ICollection<Event> GetEvents();//get all events

        [OperationContract(Name = "GetEventsByEventName")]
        ICollection<Event> GetEvents(string eventName);

        [OperationContract(Name = "GetEventsByEventDate")]
        ICollection<Event> GetEvents(DateTime eventDate);

        [OperationContract]
        bool CreateEvent(string eventName, DateTime eventDate);

        [OperationContract]
        bool AddGuest(int eventId, Guest guest);

        [OperationContract]
        bool RemoveGuest(int eventId, int guestId);

        [OperationContract]
        ICollection<User> GetUsers();

        [OperationContract]
        User GetUserById(int userId);
        [OperationContract]
        bool AddUser(string userName, string password, bool isAdmin);

        [OperationContract(Name = "RemoveUserByUserName")]
        bool RemoveUser(string userName);




    }


}
