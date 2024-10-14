using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DamiensCookieJar.Pages;

public class SignOutModel : PageModel
{
    public IActionResult OnGet()
    {
        //if a user is not signed in, send to login
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToPage("/Account/Login");
        }
        //else send the signout page
        return Page();
    }
    public async Task<IActionResult> OnPost()
    {
        //sign out user on post
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
        //return home
        return RedirectToPage("/Index");
    } 
}