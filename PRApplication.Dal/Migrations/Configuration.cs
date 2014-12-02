namespace PRApplication.Dal.Migrations
{
    using PRApplication.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<PRApplication.Dal.PRApplicationDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(PRApplication.Dal.PRApplicationDBContext context)
        {

            string EncryptPasss = EncryptPass("1234");

            List<User> Users = new List<User>
            {
                new User {IsAdmin=true,UserName="Ohad",Password=EncryptPasss},
                new User {IsAdmin=true,UserName="Shalev",Password=EncryptPasss},
                new User {IsAdmin=true,UserName="Tal",Password=EncryptPasss},
                new User {IsAdmin=true,UserName="Or",Password=EncryptPasss},

                new User {IsAdmin=false,UserName="Shontal",Password=EncryptPasss},
                new User {IsAdmin=false,UserName="Shontal2",Password=EncryptPasss}
                

            };
            context.Users.AddOrUpdate(u => u.UserName, Users.ToArray());

            List<Event> Events = new List<Event>
            {
                new Event {Name="First Party",StartDate= new DateTime(2014,3,3)},
                new Event {Name="Second Party",StartDate= new DateTime(2014,5,5)},
                new Event {Name="Shierd Party",StartDate= new DateTime(2014,9,11)}

            };


            context.Events.AddOrUpdate(e => e.Name, Events.ToArray());


            List<Guest> Guests = new List<Guest>
            {
                new Guest {Event = Events[0],EventId=Events[0].Id, FirstName="Eli",LastName="Choen",Companions=3,Atended=false},
                new Guest {Event = Events[0],EventId=Events[0].Id, FirstName="Shalev",LastName="Zahavi",Companions=2,Atended=false},
                new Guest {Event = Events[0],EventId=Events[0].Id, FirstName="Ohad",LastName="Galor",Companions=1,Atended=false},
                new Guest {Event = Events[0],EventId=Events[0].Id, FirstName="Or",LastName="Kadosh",Companions=3,Atended=false},
                new Guest {Event = Events[0],EventId=Events[0].Id, FirstName="Ori",LastName="Kadoshi",Companions=3,Atended=false},

                new Guest {Event = Events[1],EventId=Events[1].Id, FirstName="Tal",LastName="Horowitz",Companions=5,Atended=false},
                new Guest {Event = Events[1],EventId=Events[1].Id, FirstName="Omri",LastName="Nagar",Companions=3,Atended=false},
                new Guest {Event = Events[1],EventId=Events[1].Id, FirstName="Etay",LastName="Peleg",Companions=2,Atended=false},
                new Guest {Event = Events[1],EventId=Events[1].Id, FirstName="Ariel",LastName="Ohayon",Companions=2,Atended=false},

                new Guest {Event = Events[2],EventId=Events[2].Id, FirstName="Yotam",LastName="Yzhaki",Companions=1,Atended=false},
                new Guest {Event = Events[2],EventId=Events[2].Id, FirstName="Dor",LastName="Kave",Companions=3,Atended=false},
                new Guest {Event = Events[2],EventId=Events[2].Id, FirstName="Sami",LastName="Shimon",Companions=1,Atended=false},
                new Guest {Event = Events[2],EventId=Events[2].Id, FirstName="Somo",LastName="Levi",Companions=1,Atended=false}

            };

            context.Guests.AddOrUpdate(g => g.FirstName, Guests.ToArray());

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
