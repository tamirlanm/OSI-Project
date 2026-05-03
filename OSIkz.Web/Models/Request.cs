using System.ComponentModel.DataAnnotations;

namespace OSIkz.Web.Models
{
    public class Request
    {
        public int Id {get;set;}

        [Required]
        public string Title {get; set;} = string.Empty;
        public string Description {get;set;} = string.Empty;

        public string Priority {get;set;} = "Regular";
        public string Status {get; set;} = "Open";

        public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
        public string UserId {get;set;} = string.Empty;
        public ApplicationUser? User {get;set;} 
    }
    
}