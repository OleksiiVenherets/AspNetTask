using System;
using System.Collections.Generic;
using FinalTask.Domain.Entities;

namespace FinalTask.Tests.Entities
{
    public class EntitiesTest
    {
        public List<AppUser> Users { get; set; }
        public List<Visit> Visits{ get; set; }
        public List<City> Cities { get; set; }
        public List<Photo> Photos {get; set; }


        public EntitiesTest()
        {
            GenerateCities();
            GeneratePhotos();
            GenerateUsers();
            GenerateVisits();
        }

        private void GenerateUsers()
        {
            Users = new List<AppUser>();
            var usertest1 = new AppUser
            {
                Id = "testid1111",
                UserName = "test1@gmail.com",
                Email = "test1@gmail.com",
                Name = "Test1",
                Surname = "Test1",
                Telephone = "0111111111",
                PasswordHash = "test1"                      
            };

            var usertest2 = new AppUser
            {
                Id = "testid2222",
                UserName = "test2@gmail.com",
                Email = "test2@gmail.com",
                Name = "Test2",
                Surname = "Test2",
                Telephone = "0222222222",
                PasswordHash = "test2"
            };
            
            Users.Add(usertest1);
            Users.Add(usertest2);

        }
        private void GenerateVisits()
        {
            Visits = new List<Visit>();
            var visittest1 = new Visit
            {
                Id = 1,
                UserId = "testid1111",
                IsVisited = true,
                CityId = 1,
                Comment = "QWERTY",
                Date = DateTime.Today,
                Rate = 9
            };

            var visittest2 = new Visit
            {
                Id = 2,
                UserId = "testid1111",
                IsVisited = false,
                CityId = 2,
                Comment = null,
                Date = null,
                Rate = null
            };

            var visittest3 = new Visit
            {
                Id = 1,
                UserId = "testid2222",
                IsVisited = true,
                CityId = 1,
                Comment = "QWERTY",
                Date = DateTime.Today,
                Rate = 9
            };

            var visittest4 = new Visit
            {
                Id = 2,
                UserId = "testid2222",
                IsVisited = false,
                CityId = 2,
                Comment = null,
                Date = null,
                Rate = null
            };

            Visits.Add(visittest1);
            Visits.Add(visittest2);
            Visits.Add(visittest3);
            Visits.Add(visittest4);


        }
        private void GenerateCities()
        {
            Cities = new List<City>();
            var citytest1 = new City
            {
                Id = 1,
                Latitude = 49.8382600,
                Longitude = 24.0232400,
                Name = "Lviv"
            };
            var citytest2 = new City
            {
                Id = 2,
                Latitude = 50.4546600,
                Longitude = 30.5238000,
                Name = "Kyiv"
            };
            Cities.Add(citytest1);
            Cities.Add(citytest2);

        }
        private void GeneratePhotos()
        {

        }
    }
}
