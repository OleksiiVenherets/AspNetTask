using System.Collections.Generic;
using System.Linq;
using FinalTask.Domain.Abstract;
using FinalTask.Domain.Entities;
using FinalTask.Tests.Entities;

namespace FinalTask.Tests.Concrete
{
    public class MockRepository: ITaskRepository
    {
        readonly EntitiesTest _context = new EntitiesTest();
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
                var dbEntry = _context.Visits.Find(v => v.Id == visit.Id);
                if (dbEntry != null)
                {
                    dbEntry.Comment = visit.Comment;
                    dbEntry.Rate = visit.Rate;
                }
            }
        }

        public Visit DeleteVisit(int visitId)
        {
            var dbEntry = _context.Visits.Find(v => v.Id == visitId);
            if (dbEntry == null)
                return null;
            _context.Visits.Remove(dbEntry);
            return dbEntry;
        }

        public void AddCity(City city)
        {
            _context.Cities.Add(city);
        }

        public void AddPhoto(Photo photo)
        {
            _context.Photos.Add(photo);
        }

        public Photo DeletePhoto(int photoId)
        {
            var dbEntry = _context.Photos.Find( p => p.Id == photoId);
            if (dbEntry == null)
                return null;
            _context.Photos.Remove(dbEntry);
            return dbEntry;
        }

        public bool IsNewCity(string name)
        {
            return Cities.All(city => city.Name != name);
        }
    }
}
