using System;
using System.Web.Mvc;
using FinalTask.Domain.Abstract;
using FinalTask.Domain.Entities;
using FinalTask.WebUI.Controllers;
using FinalTask.WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinalTask.Tests
{
    [TestClass]
    public class VisitTests
    {
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddVisit_ExpectedReturnedException()
        {
            var mock = new Mock<ITaskRepository>();
            mock.Setup(
                x => x.AddCity(new City()
                {
                    Id = 1,
                    Latitude = 10.000,
                    Longitude = 10.000,
                    Name = "City"
                }));
            mock.Setup(
                x => x.SaveVisit(new Visit()
                {
                    Id = 1,
                    CityId = 1,
                    IsVisited = true,
                    Date = DateTime.Now,
                    Comment = "Nice",
                    Rate = 9
                }
                ));
           var controller = new VisitController(mock.Object);

            var result = controller.AddVisit(new AddVisitViewModel()
            {
                CityName = "City",
                Comment = "Nice",
                Date = DateTime.Now,
                IsVisited = true,
                Latitude = 10.000,
                Longitude = 10.000,
                Rate = 9
            });
            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }
    }
}
