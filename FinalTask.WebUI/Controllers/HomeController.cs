using System.Linq;
using System.Web.Mvc;
using FinalTask.Domain.Abstract;

namespace FinalTask.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITaskRepository _repository;

        public HomeController(ITaskRepository repo)
        {
            _repository = repo;
        }
        // GET: Home
        public ActionResult Index()
        {
            var photos = _repository.Photos.ToList();
            return View(photos);
        }

        /// <summary>
        /// Diaplays a specific image
        /// </summary>
        /// <param name="id">Id of the image to display</param>
        /// <returns></returns>
        public FilePathResult GetImage(int id)
        {
            var photo = _repository.Photos.SingleOrDefault(p => p.Id == id);

            if (photo != null)
                return File(photo.Link, "image/jpeg");
            return null;
        }
    }
}