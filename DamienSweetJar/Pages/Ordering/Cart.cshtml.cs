using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;

namespace DamiensCookieJar.Pages;

public class Cart : PageModel
{
    public LinkedList<CartOrderRow> cartRows { get; set; }

    public float CartTotal;

    public int orderID;

    [BindProperty] public string submit { get; set; }

    public float calcCartTotal()
    {
        float total = 0;
        foreach (var row in cartRows)
        {
            total += row.rowPrice;
        }

        return total;
    }

    public class CartOrderRow
    {
        public CartOrderRow(int id, int quant)
        {
            rowName = GrabName(id);
            rowPrice = calcPrice(quant, id);
            rowQuantity = quant;
        }

        public string GrabName(int id)
        {
            //grab DB connection string from appsettings.json
            ConfigurationBuilder config = new ConfigurationBuilder();
            config.AddJsonFile("appsettings.json");
            String dbString = config.Build().GetConnectionString("DamiensCookies");

            using (MySqlConnection connect = new MySqlConnection(dbString))
            {
                connect.Open();
                using (MySqlCommand nameQuery = new MySqlCommand())
                {
                    nameQuery.Connection = connect;

                    nameQuery.CommandText = "SELECT CookieName FROM cookie WHERE CookieID = @id;";
                    nameQuery.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader results = nameQuery.ExecuteReader())
                    {
                        results.Read();

                        return results.GetString("CookieName");
                    }
                }
            }
        }


        public float calcPrice(int quant, int id)
        {
            //grab DB connection string from appsettings.json
            ConfigurationBuilder config = new ConfigurationBuilder();
            config.AddJsonFile("appsettings.json");
            String dbString = config.Build().GetConnectionString("DamiensCookies");


            float pricePerCookie;
            using (MySqlConnection connect = new MySqlConnection(dbString))
            {
                connect.Open();
                using (MySqlCommand nameQuery = new MySqlCommand())
                {
                    nameQuery.Connection = connect;

                    nameQuery.CommandText = "SELECT CookiePrice FROM cookie WHERE CookieID = @id;";
                    nameQuery.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader results = nameQuery.ExecuteReader())
                    {
                        results.Read();
                        pricePerCookie = results.GetFloat("CookiePrice");
                    }
                }

                connect.Close();
            }

            return pricePerCookie * quant;
        }

        public string rowName { get; set; }

        public float rowPrice { get; set; }

        public int rowQuantity { get; set; }
    }

    public void OnGet()
    {
        cartRows = getCart();
        if (cartRows != null)
        {
            CartTotal = calcCartTotal();
        }
    }

    public LinkedList<CartOrderRow> getCart()
    {
        //grab DB connection string from appsettings.json
        ConfigurationBuilder config = new ConfigurationBuilder();
        config.AddJsonFile("appsettings.json");
        String dbString = config.Build().GetConnectionString("DamiensCookies");

        LinkedList<CartOrderRow> cart = new LinkedList<CartOrderRow>();

        using (MySqlConnection dbconnect = new MySqlConnection(dbString))
        {
            dbconnect.Open();


            using (MySqlCommand orderIDQuery = new MySqlCommand())
            {
                orderIDQuery.Connection = dbconnect;

                int custID = Convert.ToInt32(User.FindFirst(ClaimTypes.GivenName).Value);
                orderIDQuery.CommandText =
                    "SELECT OrderID FROM cart WHERE CustomerID = @custid ORDER BY OrderDate DESC LIMIT 1;";
                orderIDQuery.Parameters.AddWithValue("@custid", custID);

                object result = orderIDQuery.ExecuteScalar();
                if (result == null)
                {
                    return null;
                }

                orderID = Convert.ToInt32(result);
            }


            using (MySqlCommand getOrderQuery = new MySqlCommand())
            {
                getOrderQuery.Connection = dbconnect;

                getOrderQuery.CommandText = "SELECT * FROM cartItems WHERE OrderID = @ordID;";
                getOrderQuery.Parameters.AddWithValue("@ordID", orderID);

                using (MySqlDataReader results = getOrderQuery.ExecuteReader())
                {
                    while (results.Read())
                    {
                        int cookieID = results.GetInt32("CookieID");
                        int quantity = results.GetInt32("CookieAmount");

                        cart.AddLast(new CartOrderRow(cookieID, quantity));
                    }
                }
            }

            dbconnect.Close();
        }

        return cart;
    }

    public IActionResult OnPost()
    {
        if (submit.Equals("checkout"))
        {
            return RedirectToPage("/Ordering/Checkout");
        }

        //todo finish order undo logic aka restock... gather all cookies and quantities from the current cart,
        //todo and update the cookies db to reflect the cookies being un carted

        Dictionary<int, int> orderCookies = new Dictionary<int, int>();
        return RedirectToPage("/Index");
    }
}