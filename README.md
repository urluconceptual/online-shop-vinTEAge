# online-shop-vinTEAge
Group project created in ASP.NET for Web Application Development course. Developed fully functional online shop website for books/tea store.

### Implemented by
-> [@urluconceptual](https://github.com/urluconceptual)  
-> [@MirunaGeorgescu](https://github.com/MirunaGeorgescu)  

## Overview

    * Created using ASP.NET (MVC architecture) and Bootstrap frameworks, C#, CSS, HTML (Razor).
    * Designed local SQL database with CRUD operations using dependency injection.
    * Implemented Role-Based Access Control (4 types of users: unregistered user, regular user, editor, administrator):
      -> Unregistred users  
          -> only see the homepage with a few recommendations of the highest rated products on the website
          -> will be redirected to create an account if they try to add a product to cart  
      -> Regular users
          -> add products to their cart
          -> leave and edit/remove their own reviews
          -> sort by price and rating
          -> search products by keywords  
      -> Editors
          -> add, edit or remove their own products (when adding a product, a request will be sent to the administrator)  
      -> Administrators
          -> edit and remove all products/reviews
          -> change the type of account for all users
          -> their list of products also contains the current requests  
    * The products are part of dynamically created categories, and they require a name, a short description that can be 
    formatted within the Summernote text editor, a picture, price. Their rating and review section will be updated
    every time a review is posted.
