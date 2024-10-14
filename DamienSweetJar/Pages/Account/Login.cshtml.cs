using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;

public class LoginModel : PageModel
{
    [BindProperty]
    public LoginInputModel InputModel { get; set; }

    public class LoginInputModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Only alphanumeric characters are allowed.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The field must be between 3 and 50 characters.")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[!@#$%^&])(?=.*[A-Z])[A-Za-z0-9!@#$%^&]{10,}$",
            ErrorMessage =
                "Password must be at least 10 characters, with at least 1 number, 1 capital letter and one of [!@#$%^&]")]
        public string Password { get; set; }
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            //Username, password requirements, just in case ModelState is circumvented by the client
            Regex usernameCheck = new Regex("^[a-zA-Z0-9]*$");
            Regex passwordCheck = new Regex("^(?=.*[0-9])(?=.*[!@#$%^&])(?=.*[A-Z])[A-Za-z0-9!@#$%^&]{10,}$");
            
            //check validity of username. if it doesnt pass, return to login with error message
            if (!usernameCheck.IsMatch(InputModel.Username))
            {
                ModelState.AddModelError(string.Empty, "username not found. please try again");
                return Page();
            }
            //check validity of password. if it doesnt pass, return to login with error message
            if (!passwordCheck.IsMatch(InputModel.Password))
            {
                ModelState.AddModelError(string.Empty, "password invalid. Please try again");
                return Page();
            }
            
            //grab DB connection string from appsettings.json
            ConfigurationBuilder config = new ConfigurationBuilder();
            config.AddJsonFile("appsettings.json"); 
            String dbString = config.Build().GetConnectionString("DamiensCookies");

            using (MySqlConnection connect = new MySqlConnection(dbString))
            {
                connect.Open();
                using (MySqlCommand userNameQuery = new MySqlCommand())
                {
                    userNameQuery.Connection = connect;
                    userNameQuery.CommandText = "SELECT PassWordHash, Salt FROM customers WHERE UserName = @user";
                    userNameQuery.Parameters.AddWithValue("@user", InputModel.Username);

                    string userHash;
                    string userSalt;
                    await using (MySqlDataReader results = userNameQuery.ExecuteReader())
                    {
                        if (!results.HasRows)
                        {
                            ModelState.AddModelError(string.Empty,
                                "User Name: " + InputModel.Username + " is not associated with a user account");
                            return Page();
                        }

                        results.Read();

                        userHash = results.GetString(0);
                        userSalt = results.GetString(1);
                        results.Close();
                    }

                    if (Argon2.Verify(userHash, InputModel.Password + userSalt))
                    {
                        //users authentication claims
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, InputModel.Username),
                            new Claim(ClaimTypes.Role, "Customer")
                        };
                        //claims are used to create a users authentication identity
                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        //cookie expires in 30 minutes, and will not persist if the browser is closed
                        var authProperties = new AuthenticationProperties
                        {
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                            IsPersistent = false
                        };
                        //sign the user in
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme, 
                            new ClaimsPrincipal(claimsIdentity), 
                            authProperties);
                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid username or password. Please try again");
                    }
                }
            }
        }
        return Page();
    }
}