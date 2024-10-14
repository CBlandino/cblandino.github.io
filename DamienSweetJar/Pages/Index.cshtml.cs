using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ICSI418Y_Cookies.Models;

public class IndexModel : PageModel
{
    [BindProperty]
    public ContactForm ContactForm { get; set; }

    public void OnGet()
    {
        // You can add backend logic here if needed
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            // Process the form data, send email, save to database, etc.
            // For demonstration purposes, we'll just print the form data to the console.
            Console.WriteLine($"Name: {ContactForm.Name}, Email: {ContactForm.Email}, Message: {ContactForm.Message}");

            // You can redirect to a thank you page or reload the same page
            return RedirectToPage("/Index");
        }

        // If the model state is not valid, stay on the current page
        return Page();
    }
}
