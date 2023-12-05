The Classical Guitar Music Directory is a web application designed to catalog and provide easy access to a wide range of classical guitar music. Developed using C# and ASP.NET, this application utilizes a SQL Server database to manage its data, offering a robust and user-friendly platform for guitar enthusiasts, musicians, and educators.

**Features**

Browse, search, and filter through a comprehensive list of classical guitar music pieces.
Advanced Search: Utilize advanced search capabilities to find pieces by title, composer, year of birth, instrumentation, and more.
Filtering: View pieces in chronological, reverse chronological, composer a-z, or z-a ordering.
User-Friendly Interface: A clean and intuitive interface ensures ease of navigation and usage.

**Prerequisites**

.NET Core SDK
ASP.NET Runtime
Microsoft SQL Server (Other SQL database management tools can be compatible)
Installation

Clone the Repository:

bash

git clone https://github.com/your-repository/classical-guitar-directory.git

Navigate to the Application Directory:

bash

cd classical-guitar-directory

**Database Setup**

Create a Database:

Use Microsoft SQL Server (or other app) to create a new database for the application.
Database Migration:
Update the connection string in appsettings.json to point to your database.
Use Entity Framework Core to update the database:
sql
dotnet ef database update

Running the Application
Run the Application:
arduino
dotnet run

Access the Application:
Open a web browser and navigate to http://localhost:5000 or the port specified in your environment.
Contributing
Contributions to the Classical Guitar Music Directory are welcome! Please read our contributing guidelines before submitting pull requests.
License
This project is licensed under the MIT License.
Contact
For any questions or feedback, please contact [Your Name] at [your-email@example.com].

