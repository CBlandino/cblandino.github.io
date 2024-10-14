using System.Security.Claims;
using DamiensCookieJar.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;

namespace DamiensCookieJar.Pages;

public class Order : PageModel
{
    [BindProperty] public OrderModel[] CartInputs { get; set; }

    public Order()
    {
        CartInputs = new OrderModel[new GetCookiesService().getCookies().Count()];
        for (int i = 0; i < CartInputs.Length; i++)
        {
            CartInputs[i] = new OrderModel();
        }
    }

    public class OrderModel
    {
        public int cookieID { get; set; }
        
        public string cookieName { get; set; }
        public int quantity { get; set; }
        public float cookiePrice { get; set; }
        
        public int cookiesLeft { get; set; }
    }

    public IActionResult OnGet()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToPage("/Account/Login");
        }

        return Page();
    }
    
    public IActionResult OnPost()
    {
        //grab DB connection string from appsettings.json
        ConfigurationBuilder config = new ConfigurationBuilder();
        config.AddJsonFile("appsettings.json");
        String dbString = config.Build().GetConnectionString("DamiensCookies");


        using (MySqlConnection dbconnect = new MySqlConnection(dbString))
        {
            dbconnect.Open();
            int customerID;
            using (MySqlCommand getCustomerIDQuery = new MySqlCommand())
            {
                getCustomerIDQuery.Connection = dbconnect;
        
                string username = User.FindFirst(ClaimTypes.Name).Value;
                getCustomerIDQuery.CommandText = "SELECT CustomerID from customers WHERE UserName = @username;";
                getCustomerIDQuery.Parameters.AddWithValue("@username", username);
                using (MySqlDataReader results = getCustomerIDQuery.ExecuteReader())
                {
                    results.Read();
        
                    customerID = results.GetInt32("CustomerID");
                    results.Close();
                }
            }
        
            double orderTotal = 0;
            for (int i = 0; i < CartInputs.Length; i++)
            {
                if (CartInputs[i].quantity != 0)
                {
                    orderTotal += CartInputs[i].cookiePrice * CartInputs[i].quantity;
                }
            }

            //validate current order to make sure everything can be fulfilled
            for (int i = 0; i < CartInputs.Length; i++)
            {
                if (CartInputs[i].quantity != 0)
                {
                    if (CartInputs[i].quantity > CartInputs[i].cookiesLeft)
                    {
                        ModelState.AddModelError(string.Empty, 
                            CartInputs[i].quantity + " " + CartInputs[i].cookieName + " cannot currently be fulfilled. please try again");
                        return Page();
                    }
                }
            }
            
            UInt64 orderID;
            using (MySqlCommand initialCartInsert = new MySqlCommand())
            {
                initialCartInsert.Connection = dbconnect;

                initialCartInsert.CommandText =
                    "INSERT INTO cart (CustomerID, OrderDate, Total) VALUES (@custID, LOCALTIMESTAMP, @total);";
                initialCartInsert.Parameters.AddWithValue("@custID", customerID);
                initialCartInsert.Parameters.AddWithValue("@total", orderTotal);

                initialCartInsert.ExecuteNonQuery();

                initialCartInsert.CommandText = "SELECT LAST_INSERT_ID()";
                orderID = (UInt64)initialCartInsert.ExecuteScalar();
            }

            for (int i = 0; i < CartInputs.Length; i++)
            {
                if (CartInputs[i].quantity != 0)
                {
                    //update the selected cookies current stock
                    using (MySqlCommand updateCookieStock = new MySqlCommand())
                    {
                        updateCookieStock.Connection = dbconnect;

                        updateCookieStock.CommandText =
                            "UPDATE cookie SET InStock = @newStock WHERE CookieID = @id;";
                        updateCookieStock.Parameters.AddWithValue("@newStock",
                            CartInputs[i].cookiesLeft - CartInputs[i].quantity);
                        updateCookieStock.Parameters.AddWithValue("@id", CartInputs[i].cookieID);
                        updateCookieStock.ExecuteNonQuery();
                    }

                    using (MySqlCommand orderInsert = new MySqlCommand())
                    {
                        orderInsert.Connection = dbconnect;

                        orderInsert.CommandText = "INSERT INTO cartItems VALUES (@orderid, @cookieID, @cookieAmount);";
                        orderInsert.Parameters.AddWithValue("@orderid", orderID);
                        orderInsert.Parameters.AddWithValue("@cookieID", CartInputs[i].cookieID);
                        orderInsert.Parameters.AddWithValue("@cookieAmount", CartInputs[i].quantity);

                        orderInsert.ExecuteNonQuery();
                    }
                }
            }
            dbconnect.Close();
        }

        return RedirectToPage("/Ordering/Cart");
    }
}