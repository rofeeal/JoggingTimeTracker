# Jogging Tracker RESTful API
This ASP.NET Core 6 project provides a RESTful API for tracking jogging times of users. It allows users to create an account, log in, and log out. Once logged in, users can view, edit, and delete their own jogging time entries. There are three roles implemented with different permission levels: a regular user, a user manager, and an admin. Regular users can only CRUD their own records, user managers can CRUD users, and admins can CRUD all records and users. Each jogging time entry includes a date, distance, and time. The API also allows filtering by dates and generating reports on average speed and distance per week.

# Toolkit Requirements
  This project uses the following toolkit:

    -Backend web framework: .NET Core 6
# Usage
  To use this API, you can either build and run the project on your local machine or deploy it to a server. Once the API is up and running, you can use a REST client like Postman or Swagger to interact with it.

# Default Users
  To make it easier for you to test the API, we have created some default users for you. You can use the following credentials to log in to the API:

    Admin:
    - Username: admin
    - Password: P@ssw0rd
    User Manager:
    - Username: usermanager
    - Password: P@ssw0rd
    Regular User:
    - Username: regularuser
    - Password: P@ssw0rd
# Endpoints
The following endpoints are examples available in this API:

  * /api/Authentication/login (POST): Logs in an existing user.
  * /api/Authentication/logout (POST): Logs out the current user.
  * /api/JoggingTimes/ (GET, POST): Returns a list of all jogging times or creates a new jogging time entry.
  * /api/JoggingTimes/{id} (GET, PUT, DELETE): Returns a single jogging time entry by ID or updates/deletes an existing jogging time entry.
  * /api/users (GET): Returns a list of all users (only available for user managers and admins).
  * /api/users/{id} (GET, PUT, DELETE): Returns a single user by ID or updates/deletes an existing user (only available for user managers and admins).
  * /api/JoggingTimes/weekly-stats (GET): Returns a report on average speed and distance per week.
  For more information on each endpoint, please refer to the Swagger documentation.

# Project Structure
This project is structured as follows:

* JoggingTracker.API: Contains the API controllers and services.
* JoggingTracker.Data: Contains the data context and repositories.
* JoggingTracker.Core: Contains the business logic and models.
* JoggingTracker.sln: The solution file.
# How to Build and Run
To build and run the project on your local machine, please follow these steps:

* Install .NET Core 6 SDK (if not already installed).
* Clone this repository.
* Open a terminal or command prompt and navigate to the JoggingTracker.API folder.
* Run the command dotnet run.
* Open a web browser and navigate to https://localhost:5001/swagger/index.html to access the Swagger documentation and start using the API.
That's it! You should now be able to use the API to track jogging times. If you encounter any issues or have any questions, please feel free to contact us.
