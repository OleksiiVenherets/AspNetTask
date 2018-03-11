using System.Threading.Tasks;
using System.Web.Mvc;
using FinalTask.Domain.Abstract;
using FinalTask.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinalTask.Tests
{
    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void Register_ExpectedReturnedView()

        {
            var mock = new Mock<ITaskRepository>();
            var controller = new AccountController(mock.Object);
            var result = controller.Register(null);
            Assert.IsInstanceOfType(result, typeof(Task<ActionResult>));
        }

        
    }
}
