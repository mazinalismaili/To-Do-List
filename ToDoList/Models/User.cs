using Microsoft.AspNetCore.Identity;

namespace ToDoList.Models
{
    public enum Gender
    {
        Male,
        Female
    }
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public Gender Gender {  get; set; }

    }
}
