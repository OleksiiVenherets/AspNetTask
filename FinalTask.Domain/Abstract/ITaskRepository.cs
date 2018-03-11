using System.Collections.Generic;
using FinalTask.Domain.Entities;

namespace FinalTask.Domain.Abstract
{
    public interface ITaskRepository
    {
        IEnumerable<AppUser> Users { get; }
        IEnumerable<Visit> Visits { get; }
        IEnumerable<City> Cities { get; }
        IEnumerable<Photo> Photos { get; }

        void SaveVisit (Visit visit);
        Visit DeleteVisit(int visitId);
        void AddCity(City city);
        void AddPhoto(Photo photo);
        Photo DeletePhoto(int photoId);
        bool IsNewCity(string name);

    }
}
