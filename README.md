# online-shop-vinTEAge
## Project created in ASP.NET for Web Application Development course. Developed fully functional online shop website for books/tea store

* Created using ASP.NET (MVC architecture) and Bootstrap frameworks, C#, CSS, HTML (Razor)
* Designed local SQL database with CRUD operations using dependency injection
* Implemented Role-Based Access Control (4 types of users: unregistered user, regular user, editor, administrator)  
  -> Unregistred users can only see the homepage with a few recommendations of the highest rated products on the website and will be redirected to create an account if they try to add a product to cart  
  -> Regular users can add products to their cart, leave and edit/remove their own reviews, sort by price and rating, search products by keywords  
  -> Editors can add, edit or remove their own products (when adding a product, a request will be sent to the administrator)  
  -> Administrators can edit and remove all products/reviews, change type of account for all users and their list of products also contains the current requests  
* The products are part of dynamically created categories, and they require a name, a short description that can formatted within the Summernote text editor, a picture, price. Their rating and review section will be update every time a review is posted.
