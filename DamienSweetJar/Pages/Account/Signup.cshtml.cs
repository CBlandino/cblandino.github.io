using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using System.Text.RegularExpressions;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

public class SignupModel : PageModel
{
    [BindProperty] public SignupInputModel InputModel { get; set; }

    public class SignupInputModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Only alphanumeric characters are allowed.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The field must be between 3 and 50 characters.")]
        public string Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[!@#$%^&])(?=.*[A-Z])[A-Za-z0-9!@#$%^&]{10,}$",
            ErrorMessage =
                "Password must be at least 10 characters, with at least 1 number, 1 capital letter and one of [!@#$%^&]")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            //Username, password and email requirements, just in case ModelState is circumvented by the client
            Regex usernameCheck = new Regex("^[a-zA-Z0-9]*$");
            Regex passwordCheck = new Regex("^(?=.*[0-9])(?=.*[!@#$%^&])(?=.*[A-Z])[A-Za-z0-9!@#$%^&]{10,}$");
            Regex emailCheck = new Regex("^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,4}$");

            //check validity of username. if it doesnt pass, return to login with error message
            if (!usernameCheck.IsMatch(InputModel.Username))
            {
                ModelState.AddModelError(string.Empty, "username does not meet the requirements please try again");
                return Page();
            }
            //check validity of password. if it doesnt pass, return to login with error message
            if (!passwordCheck.IsMatch(InputModel.Password))
            {
                ModelState.AddModelError(string.Empty, "password does not meet the requirements. Please try again");
                return Page();
            }
            //check validity of email. if it doesnt pass, return to login with error message
            if (!emailCheck.IsMatch(InputModel.Email))
            {
                ModelState.AddModelError(string.Empty, "invalid email entered, please try again");
                return Page();
            }
            
            //grab DB connection string from appsettings.json
            ConfigurationBuilder config = new ConfigurationBuilder();
            config.AddJsonFile("appsettings.json"); 
            String dbString = config.Build().GetConnectionString("DamiensCookies");
            
            using (MySqlConnection connection = new MySqlConnection(dbString))
            {
                connection.Open();
                
                //query the database to ensure that the submitted username is not already in use
                using (MySqlCommand userNameQuery = new MySqlCommand())
                {
                    userNameQuery.Connection = connection;
                    userNameQuery.CommandText = "SELECT COUNT(*) FROM customers WHERE UserName = @username";
                    userNameQuery.Parameters.AddWithValue("@username", InputModel.Username);

                    bool userNameTaken = (Int64)userNameQuery.ExecuteScalar() != 0;
                    if (userNameTaken)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Username: " + InputModel.Username + " is already taken. please try again");
                        return Page();
                    }
                }
                //query the database to ensure that the submitted email is not already in use
                using (MySqlCommand emailQuery = new MySqlCommand())
                {
                    emailQuery.Connection = connection;
                    emailQuery.CommandText = "SELECT COUNT(*) FROM customers WHERE Email = @email";
                    emailQuery.Parameters.AddWithValue("@email", InputModel.Email);

                    bool emailTaken = (Int64)emailQuery.ExecuteScalar() != 0;
                    if (emailTaken)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Email: " + InputModel.Email + " is already in use. please try again");
                        return Page();
                    }
                }

                //generate pasword salt for the user 
                byte[] saltarr = new byte[16];
                string salt;
                using (RandomNumberGenerator random = RandomNumberGenerator.Create())
                {
                    random.GetBytes(saltarr);
                    salt = Convert.ToBase64String(saltarr);
                }
                //hash user password using Argon2
                string passwordHash = Argon2.Hash(InputModel.Password + salt);
                
                //insert relevant user information into the customers table as a new customer
                object custID;
                using (MySqlCommand query = new MySqlCommand())
                {
                    query.Connection = connection;

                    query.CommandText =
                        "INSERT INTO customers (PassWordHash, Salt, UserName, Email) VALUES (@PasswordHash, @Salt, @uname, @Email)";

                    query.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    query.Parameters.AddWithValue("@Salt", salt);
                    query.Parameters.AddWithValue("@uname", InputModel.Username);
                    query.Parameters.AddWithValue("@Email", InputModel.Email);

                    query.ExecuteNonQuery();

                    query.CommandText = "SELECT LAST_INSERT_ID()";
                    custID = query.ExecuteScalar();
                }

                int customerID = Convert.ToInt32(custID);
                
                //users authentication claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, InputModel.Username),
                    new Claim(ClaimTypes.Role, "Customer"),
                    new Claim(ClaimTypes.GivenName, Convert.ToString(customerID))
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
            }
            //redirect signed in user back to the home page
            return RedirectToPage("/Index");
        }
        return Page();
    }
}