using System;
using System.Collections.Generic;
using System.Linq;
using FinalTask.Domain.Abstract;
using FinalTask.Domain.Entities;

namespace FinalTask.Domain.Concrete
{
    public class AppIdentityDbRepository: ITaskRepository, IDisposable
    {
        private bool _disposed;
        readonly AppIdentityDbContext _context = new AppIdentityDbContext();

        public IEnumerable<AppUser> Users => _context.Users;
        public IEnumerable<Visit> Visits => _context.Visits;
        public IEnumerable<City> Cities => _context.Cities;
        public IEnumerable<Photo> Photos => _context.Photos;

        public void SaveVisit(Visit visit)
        {
            if (visit.Id == 0)
                _context.Visits.Add(visit);
            else
            {
                var dbEntry = _context.Visits.Find(visit.Id);
                if (dbEntry != null)
                {
                    dbEntry.Comment = visit.Comment;
                    dbEntry.Rate = visit.Rate;
                }
            }
            _context.SaveChanges();
        }
        
        public Visit DeleteVisit(int visitId)
        {
            var dbEntry = _context.Visits.Find(visitId);
            if (dbEntry == null)
                return null;
            _context.Visits.Remove(dbEntry);
            _context.SaveChanges();
            return dbEntry;
        }

        public void AddCity(City city)
        {
            _context.Cities.Add(city);
            _context.SaveChanges();

        }

        public void AddPhoto(Photo photo)
        {
            _context.Photos.Add(photo);
            _context.SaveChanges();
        }

        public Photo DeletePhoto(int photoId)
        {
            var dbEntry = _context.Photos.Find(photoId);
            if (dbEntry == null)
                return null;
            _context.Photos.Remove(dbEntry);
            _context.SaveChanges();
            return dbEntry;
        }

        public bool IsNewCity(string name)
        {
            return Enumerable.All(_context.Cities, city => city.Name != name);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
