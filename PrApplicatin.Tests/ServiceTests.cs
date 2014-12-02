using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrApplicatin.Tests.PrServiceReference;
using System.ServiceModel;

namespace PrApplicatin.Tests
{
    [TestClass]
    public class ServiceTests
    {
        [TestMethod]
        public void CreatingAnEventAsPrExecutiv_Success()
        {
            //Arrange
            var host = new ServiceHost(typeof(PrServiceClient));
            host.Open();

            var client = new PrServiceClient();


            //counting the current amount of events
            int numOfEvents = client.GetEvents().Length;

            try
            {
                //Act
                client.CreateEvent(@event);

                //Assert

                Assert.AreEqual(numOfEvents + 1, client.GetAllEvents().Count());
            }
            finally
            {
                if (numOfEvents + 1 == client.GetAllEvents().Count())
                {
                    Event[] searchedEvents = client.EventSearchByName("Yoav's Testing Event");
                    client.DeleteEvent(searchedEvents[0].EventId);
                }
                client.Close();
                host.Close();
            }
        }

        [TestMethod]
        public void CreatingAnEventAsPrExecutiv_ExistingEvent_Fail()
        {
            //Arrange

            //open the host
            var host = new ServiceHost(typeof(EventPrService));
            host.Open();

            //opnening a client proxy
            var client = new PrService.EventPrServiceClient();



            //creating the first event to add
            Event @event = new Event()
            {
                Date = DateTime.UtcNow,
                Description = "Testing Event",
                Duration = new TimeSpan(1, 30, 0),
                Location = "Tel-Aviv",
                Name = "Yoav's Testing Event"
            };

            //spining for one second to change the current time
            Thread.Sleep(1000);

            //creating the second event that sould throw the exeption
            Event @event2 = new Event()
            {
                Date = DateTime.UtcNow,
                Description = "Testing Event 2",
                Duration = new TimeSpan(1, 0, 0),
                Location = "Haifa",
                Name = "Yoav's Testing Event"
            };

            //Act
            client.AddEvent(@event); // should work
            try
            {
                client.AddEvent(@event); // should not work
                Assert.Fail(); //if we got to this line, no exeption was thrown, the test faild
            }
            catch (Exception)
            {
                //if we got here - all is well, the test passed
            }
            finally
            {
                //clearing the db from the test events
                Event[] searchedEvents = client.EventSearchByName("Yoav's Testing Event");
                for (int i = 0; i < searchedEvents.Length; i++)
                {
                    client.DeleteEvent(searchedEvents[i].EventId);
                }
                client.Close();
                host.Close();
            }
        }
    }
}
