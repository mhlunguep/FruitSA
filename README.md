# Fruit SA Product Management Web Application

This is a web application built using ASP.NET MVC, CSS with Bootstrap, Entity Framework 6, and SQL Server. The application is designed to efficiently manage a list of products and categories associated with a user.

Overview
The objective of this project is to develop a highly scalable and secure web application for managing products and categories. Users will be able to perform CRUD operations on products, categorize them, and associate them with their respective categories.

## Technologies Used

ASP.NET MVC: ASP.NET MVC provides the framework for building scalable and maintainable web applications.
CSS with Bootstrap: Bootstrap is utilized for frontend styling and layout, ensuring a responsive and user-friendly interface.
Entity Framework 6: Entity Framework simplifies data access and management by providing an object-relational mapping (ORM) framework.
SQL Server: SQL Server is used as the backend database management system to store and manage product data efficiently.

## Getting Started

To get started with this project, follow these steps:

1. Clone the Repository: Clone this repository to your local machine using the following command:

   `git clone https://github.com/mhlunguep/FruitSA`

2. Set Up the Database: Set up a SQL Server database and configure the connection string in the appsettings.json file.

3. Add Migrations:  
   Open Package Manager Console in Visual Studio, and make sure to choose **FruitSA.DataAccess** as the Default Project and run the following command:
   a. `add-migration "First Migrations"`
   b. `update-database`

   If you are not sure about the above, see screenshot called PackageManager on the root of this repo

4. Build and Run the Application: Build the solution using Visual Studio or your preferred IDE. Run the application and navigate to the URL to access the product management system.

5. Login using the following details or you can register your own details by clinking the register link on the top menu
   username: admin@fruitsa.com
   password: Admin123\*

6. Upload Products using Excel
   a. First go to the root of this repo and find excel document named Products
   b. Then go to Content Management > Product and upload the Excel file and press the Upload button
   c. Now go to the home page, you should see the products you have just uploaded with their images
   d. To Download the products Content Management > Product you should see a download button at the bottom of the product list
7. To upload your own Products using Excel
   a. First create Categories by navigating to Content Management > Category and click the Create New Category button
   b. Then go to Content Management > Product and upload the Excel file
   **_NB: This file should have these columns in this order: ProductId, ProductCode, Name, Description, CategoryId, Price, ImageUrl, Username, CreatedDate, UpdatedAt_**
   Also the CategoryId on Excel file should match the existing CategoryId on Database
8. You can also create products by cling the Create Products button

If you encounter any issues please do not hesitate to let know.
