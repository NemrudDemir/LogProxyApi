using LogProxyApi.Auth;
using System.Collections.Generic;

namespace LogProxyApi.Options
{
    public class AuthorizationOptions
    {
        public IEnumerable<User> AuthorizedUsers { get; set; }
    }
}
