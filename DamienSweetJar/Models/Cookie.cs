namespace ICSI418Y_Cookies.Models;

public class Cookie
{
    public string CookieName { get; set; }
    
    public float CookiePrice { get; set; }
    
    public int CookiesLeft { get; set; }
    
    public bool InStock { get; set; }
    
    public int CookieID { get; set; }

    public Cookie(int id, string name, float price, int left, bool instock)
    {
        CookieID = id;
        CookieName = name;
        CookiePrice = price;
        CookiesLeft = left;
        InStock = instock;
    }
}