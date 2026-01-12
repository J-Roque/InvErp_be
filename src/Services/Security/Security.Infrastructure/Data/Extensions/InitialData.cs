
using Security.Domain.Models;

namespace Security.Infrastructure.Data.Extensions
{
    internal class InitialData
    {
        public static IEnumerable<Profile> Profiles
        {
            get
            {
                var profiles = new List<Profile>();
                var newProfile1 = Profile.Create(0, "Administrador", true, "ADMIN", null, true);

                profiles.Add(newProfile1);
                return profiles;
            }
        }
    }
}
