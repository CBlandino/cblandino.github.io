NOTES FOR PHIPPS:

from Ryan (Backend):

still alot left to do in terms of backend dev so I thought I would leave some notes regarding
my current todos and any thing else that may be relevent.

WHAT I GOT DONE:
1. Account system
    - at Pages/Account/ there are a few razor pages which work with signups, logins and signouts for the site
    - Login (Pages/Account/Login.cshtml.cs) - standard login page, takes in a username and password.
        on post, username and password values are dropped into the page model, which then are validated again in OnPost(),
        check db for matching username, verify hash password, if all credentials are correct the user is authenticated and sent
        back to the home page
    - Signups (Pages/Account/Signup.cshtml.cs) - pretty similar to log ins, users information os posted to the page model, 
        which is then revalidated, if all validation succeeds, the new user information is inserted into the customers table,
        the user is authenticated and sent back to home page
    - Sign outs - if user submits a post on this page, the user is deauthenticated and returned to the homepage
2. Orders (Pages/Ordering/Order.cshtml.cs) 
    - When a user is signed in, they then have access to the orders page, where an order can be placed.
    - the cookies menu is then rendered onto the page from the database, and a user can select the quantities of cookies they
        would like.
    - on post, all of the users cookie choices are sent to the page model, where the users order is inserted into the cart and
        cart items tables, the user is then brought to the cart page.
3. Cart (Pages/Ordering/Cart.cshtml.cs and cshtml)
    - as of right now, this page only displays the cookies that are currently in the users cart
    
4. Services
    - as of right now there is one service (GetCookiesService.cs) that queries the db and returns an iterable list which is 
    used by the order page to render all the cookies in the db onto the page
    
BACKEND TODO's:

1. Add functionality to the buttons on Cart page, checkout takes you to checkout, cancel order cancels the order
    and puts all the cart cookies back in stock
    
2. Finish the checkout page, process all address and personal information, store in the db, complete order and
    show receipt page

3. miscellaneous things that i missed while attempting to get general functionality down
    - add cookie images to orders page
    - increase sight security (make it so checkout page can only be accessed when you have a full cart, etc.)
    - move old orders that have already been processed to new table/storage (txt file?)
    - there are many more
    
4. Admin panel
    - give administrators the ability to login
    - edit inventories and menus
    - access order histories
    
    
NOTE: I am currently working on these todos right now and will commit whatever I manage to finish at midnight.