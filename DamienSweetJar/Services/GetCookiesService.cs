using ICSI418Y_Cookies.Models;
using MySqlConnector;

namespace DamiensCookieJar.Services;

public class GetCookiesService
{
    public IEnumerable<Cookie> getCookies()
    {
        ConfigurationBuilder config = new ConfigurationBuilder();
        config.AddJsonFile("appsettings.json"); 
        String dbConnectString = config.Build().GetConnectionString("DamiensCookies");

        using (MySqlConnection dbconnect = new MySqlConnection(dbConnectString))
        {
            dbconnect.Open();
            
            using (MySqlCommand getAllCookiesQuery = new MySqlCommand())
            {
                getAllCookiesQuery.Connection = dbconnect;

                getAllCookiesQuery.CommandText = "SELECT * FROM cookie;";
                LinkedList<Cookie> cookiesList = new LinkedList<Cookie>();
                using (MySqlDataReader results = getAllCookiesQuery.ExecuteReader())
                {
                    while (results.Read())
                    {
                        int cookieId = results.GetInt32("CookieID");
                        string cookieName = results.GetString("CookieName");
                        float cookiePrice = results.GetFloat("CookiePrice");
                        int cookiesLeft = results.GetInt32("InStock");
                        bool inStock = results.GetBoolean("IsInStock");

                        cookiesList.AddLast(new Cookie(cookieId, cookieName, cookiePrice, cookiesLeft, inStock));
                    }
                    results.Close();
                }
                dbconnect.Close();
                return cookiesList;
            }
        }
    }
}