using Microsoft.AspNet.Identity.EntityFramework;

namespace FinalTask.Domain.Entities
{
    public class AppRole: IdentityRole
    {
        public AppRole()
        { }

        public AppRole(string name)
            : base(name)
        { }
    }
}
