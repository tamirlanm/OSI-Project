using Microsoft.AspNetCore.Identity;

namespace OSIkz.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName {get;set;} = string.Empty;
        public string Role {get;set;} = "Tenant"; //Owner, Tenant, Chairman

        public List<Request> Requests {get;set;} = new();
    }
}