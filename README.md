The Classical Guitar Music Directory is a web application designed to catalog and provide easy access to a wide range of classical guitar music. Developed using C# and ASP.NET, this application utilizes a SQL Server database to manage its data, offering a robust and user-friendly platform for guitar enthusiasts, musicians, and educators.

**Features**

Browse, search, and filter through a comprehensive list of classical guitar music pieces.
Advanced Search: Utilize advanced search capabilities to find pieces by title, composer, year of birth, instrumentation, and more.
Filtering: View pieces in chronological, reverse chronological, composer a-z, or z-a ordering.
User-Friendly Interface: A clean and intuitive interface ensures ease of navigation and usage.

**Prerequisites**

.NET Core SDK
ASP.NET Runtime
Visual Studio with ASP.NET and web development workload (other IDEs can also be used)
Microsoft SQL Server (Other SQL database management tools can also be used)
    
**Installation**

Clone the Repository:

    git clone https://github.com/your-repository/classical-guitar-directory.git

Navigate to the Application Directory:

    cd classical-guitar-directory

**Database Setup**

Create a Database:
     Use Microsoft SQL Server to create a new database for the application.
    Alternatively, tools like Azure Data Studio or SQL Server Management Studio (SSMS) can be used.

Configure the Connection String:
    Update the connection string in appsettings.json to point to your newly created database.

 Database Migration:
    In Visual Studio, use the Package Manager Console to update the database:

     Update-Database
     
**Running the Application**

Start the Application:
    In Visual Studio, select IIS Express to run the application.
    Visual Studio will build the application and open it in a web browser.

Access the Application:
     The application will typically start at http://localhost:port where port is dynamically assigned by IIS Express.

Create a Database:
    Use Microsoft SQL Server to create a new database for the application.
    Alternatively, tools like Azure Data Studio or SQL Server Management Studio (SSMS) can be used.

 Configure the Connection String:
     Update the connection string in appsettings.json to point to your newly created database.

Database Migration:
     In Visual Studio, use the Package Manager Console to update the database:
    Update-Database


