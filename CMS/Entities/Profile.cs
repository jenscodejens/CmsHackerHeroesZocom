using Microsoft.AspNetCore.Identity;
using CMS.Data;

namespace CMS.Entities;

public class Profile
{   
    public int id {get ;set;}
    public string UserId {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string Email {get; set;}
    public DateOnly CreationDate {get; set;}
    public ApplicationUser User {get; set;}
}
