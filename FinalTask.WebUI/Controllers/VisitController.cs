using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalTask.Domain.Abstract;
using FinalTask.Domain.Entities;
using FinalTask.WebUI.Models;
using Logger;
using Microsoft.AspNet.Identity;

namespace FinalTask.WebUI.Controllers
{
    /// <summary>
    /// Handles all requests relating to visit
    /// </summary>
    public class VisitController : Controller
    {
        private readonly ITaskRepository _repository;
        private readonly Loger _loger = new Loger(AppDomain.CurrentDomain.BaseDirectory, LogLevel.All, LogFormat.Xml);

        public VisitController(ITaskRepository repo)
        {
            _repository = repo;
        }

        // GET: Visit
        public ActionResult Index()
        {
            var id = User.Identity.GetUserId<string>();
            var visits = _repository.Visits.Where(visit => visit.UserId == id && visit.IsVisited).ToList();
            return View(visits);
        }

        /// <summary>
        /// Dsplay view to add visit
        /// </summary>
        /// <returns></returns>
        // GET: Visit/AddVisit
        public ActionResult AddVisit()
        {
            return View();
        }

        /// <summary>
        /// Add new vivit
        /// </summary>
        /// <param name="model">Visit to add</param>
        /// <returns></returns>
        // POST: Visit/AddVisit
        [HttpPost]
        public ActionResult AddVisit(AddVisitViewModel model)
        {
            if (!ModelState.IsValid)
                View(model);
            var names = model.CityName.Split(',');
            if (_repository.IsNewCity(model.CityName))
            {
                try
                {
                    _repository.AddCity(new City
                    {
                        Name = names[0],
                        Latitude = model.Latitude,
                        Longitude = model.Longitude
                    });
                }
                catch (Exception ex)
                {
                    _loger.Log(ex.Message, LogLevel.Error, DateTime.Now, GetType().ToString());
                    return View("CustomError");
                }

            }

            var visit = new Visit
            {
                Date = model.Date,
                UserId = User.Identity.GetUserId(),
                Comment = model.Comment,
                Rate = model.Rate,
                CityId = _repository.Cities.First(c => c.Name == names[0]).Id,
                IsVisited = true
            };
            try
            {
                _repository.SaveVisit(visit);
            }
            catch (Exception ex)
            {
                _loger.Log(ex.Message, LogLevel.Error, DateTime.Now, GetType().ToString());
                return View("CustomError");
            }
            _loger.Log("User" + User.Identity.Name + "adds a visit", LogLevel.Info, DateTime.Now);
            return RedirectToAction("Index", "Visit");

        }

        /// <summary>
        /// Delete specific visit
        /// </summary>
        /// <param name="id">Visit Id to delete</param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            try
            {
                var visit = _repository.DeleteVisit(id);
                if (visit != null)
                    _loger.Log("User" + User.Identity.Name + "deletes a visit", LogLevel.Info, DateTime.Now);
            }
            catch (Exception ex)
            {
                _loger.Log(ex.Message, LogLevel.Error, DateTime.Now, GetType().ToString());
                return View("CustomError");
            }


            return RedirectToAction("Index", "Visit");
        }

        /// <summary>
        /// Displays details of visit
        /// </summary>
        /// <param name="id">Visit Id to display</param>
        /// <returns></returns>
        // GET: Visit/SeeDetails
        public ActionResult SeeDetails(int id)
        {
            var visit = _repository.Visits.First(v => v.Id == id);
            var city = _repository.Cities.First(c => visit.CityId == c.Id);
            var photoList = _repository.Photos.Where(p => p.VisitId == id);
            return View(new SeeVisitViewModel
            {
                CityName = city.Name,
                Date = visit.Date,
                Id = visit.Id,
                Rate = visit.Rate,
                Comment = visit.Comment,
                Photos = photoList
            });
        }

        /// <summary>
        /// Diplays view to edit visit
        /// </summary>
        /// <param name="id">Visit to edit</param>
        /// <returns></returns>
        // GET: Visit/Edit
        public ActionResult Edit(int id)
        {
            var visit = _repository.Visits.First(v => v.Id == id);
            var model = new EditVisitViewModel {Id = visit.Id, Comment = visit.Comment, Rate = visit.Rate};
            return View(model);
        }

        /// <summary>
        /// Edit visit
        /// </summary>
        /// <param name="model">Visit to edit</param>
        /// <param name="file">Photo to add</param>
        /// <returns></returns>
        // POST: Visit/Edit
        [HttpPost]
        public ActionResult Edit(EditVisitViewModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repository.SaveVisit(new Visit { Comment = model.Comment, Id = model.Id, Rate = model.Rate });
                    _loger.Log("User" + User.Identity.Name + "edits a visit", LogLevel.Info, DateTime.Now);
                }
                catch (Exception ex)
                {
                    _loger.Log(ex.Message, LogLevel.Error, DateTime.Now, GetType().ToString());
                    return View("CustomError");
                }
                
            }
            if (file == null)
                return View(model);
            var filename = Path.GetFileName(file.FileName);

            var path = "/App_Data/" + filename;
            file.SaveAs(Server.MapPath(path));
            var photo = new Photo
            {
                VisitId = model.Id,
                Link = path
            };
            try
            {
                _repository.AddPhoto(photo);
            }
            catch (Exception ex)
            {
                _loger.Log(ex.Message, LogLevel.Error, DateTime.Now, GetType().ToString());
                return View("CustomError");
            }

            return RedirectToAction("SeeDetails", "Visit", new { id = model.Id });

        }

        /// <summary>
        /// Displays visit on the google map
        /// </summary>
        /// <returns></returns>
        // GET: Visit/SeeOnMap
        public ActionResult SeeOnMap()
        {
            var userid = User.Identity.GetUserId<string>();
            var visits = _repository.Visits.Where(o => o.UserId == userid);
            var coordinates = visits.Select(visit => _repository.Cities.First(c => c.Id == visit.CityId))
                .Select(city =>
                    new SeeInMapModel() { Latitude = city.Latitude, Longitude = city.Longitude, Name = city.Name })
                .ToList();
            return View(coordinates);
        }

        /// <summary>
        /// Display scheduled visit
        /// </summary>
        /// <returns></returns>
        // GET: Visit/SeeScheduled
        public ActionResult SeeScheduled()
        {
            var id = User.Identity.GetUserId<string>();
            var visits = _repository.Visits.Where(visit => visit.UserId == id && !visit.IsVisited).ToList();
            return View(visits);
        }

        /// <summary>
        /// Display view to add scheduled visit
        /// </summary>
        /// <returns></returns>
        // GET: Visit/AddVisit
        public ActionResult AddScheduledVisit()
        {
            return View();
        }

        /// <summary>
        /// Add scheduled visit
        /// </summary>
        /// <param name="model">Scheduled visit to add</param>
        /// <returns></returns>
        // POST: Visit/AddVisit
        [HttpPost]
        public ActionResult AddScheduledVisit(AddVisitViewModel model)
        {
            if (!ModelState.IsValid)
                View(model);
            var names = model.CityName.Split(',');
            if (_repository.IsNewCity(model.CityName))
            {
                try
                {
                    _repository.AddCity(new City
                    {
                        Name = names[0],
                        Latitude = model.Latitude,
                        Longitude = model.Longitude
                    });
                }
                catch (Exception ex)
                {
                    _loger.Log(ex.Message, LogLevel.Error, DateTime.Now, GetType().ToString());
                    return View("CustomError");
                }
            }

            var visit = new Visit
            {
                Date = null,
                UserId = User.Identity.GetUserId(),
                Comment = null,
                Rate = null,
                CityId = _repository.Cities.First(c => c.Name == names[0]).Id,
                IsVisited = false
            };
            try
            {
                _repository.SaveVisit(visit);
            }
            catch (Exception ex)
            {
                _loger.Log(ex.Message, LogLevel.Error, DateTime.Now, GetType().ToString());
                return View("CustomError");
            }
            _loger.Log("User" + User.Identity.Name + "adds a visit", LogLevel.Info, DateTime.Now);

            return RedirectToAction("Index", "Visit");

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

        /// <summary>
        /// Disolay wiki info about specific city
        /// </summary>
        /// <param name="id">City Id to display</param>
        /// <returns></returns>
        // GET: Visit/SeeWiki
        public ActionResult SeeWiki(int id)
        {
            var city = _repository.Cities.First(c => c.Id == id);
            ViewBag.City = city.Name;
            return View("SeeWiki");
        }
    }
}