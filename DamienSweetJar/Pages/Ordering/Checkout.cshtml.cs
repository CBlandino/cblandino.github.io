using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DamiensCookieJar.Pages;

public class Checkout : PageModel
{

  [BindProperty] public CheckOutInfo info { get; set; }
  
  public class CheckOutInfo
  {
    [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Only alphabetical characters are allowed.")]
    public string fname { get; set; }
    
    [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Only alphabetical characters are allowed.")] 
    public string lname { get; set; }
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Only alphanumerical characters are allowed")]
    public string street { get; set; }
    
    [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only numerical characters are allowed.")]
    public string aptnum { get; set; }
    
    [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Only alphabetical characters are allowed.")]  
    public string city { get; set; }
    
    [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Only alphabetical characters are allowed.")]  
    public string state { get; set; }
    
    [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only numerical characters are allowed")]
    public string zip { get; set; }
  }
  public void OnGet()
  {
    
  }

  public IActionResult OnPost()
  {
    //todo finish here add all addres, name and payment info to the db, then redirect to receipt page
    return Page();
  }
}