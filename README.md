# Jogging Time Tracker API

This is a RESTful API that tracks jogging times of users, implemented using .NET Core 6. The API supports authentication, user management, and CRUD operations on jogging times. It also provides filtering by dates and reporting on average speed and distance per week. Examples for how to use the API are provided using a REST client like Postman or Swagger.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [API Documentation](#api-documentation)
- [Contributing](#contributing)
- [License](#license)

## Installation

To install the Jog Tracker API, you can clone this repository and run it locally on your machine.

First, clone the repository using the following command:

- git clone https://github.com/rofeeal/JoggingTimeTracker.git


Next, navigate to the project directory and restore the required packages using the following command:

- dotnet restore

Finally, run the application using the following command:

- dotnet run

The API should now be running on `http://localhost:5000`.

## Usage

To use the Jog Tracker API, you will need to have a REST client like Postman or Swagger installed on your machine.

Once you have a REST client installed, you can use it to make requests to the API endpoints. The API supports the following endpoints:

- `/api/authenticate`: Authenticates a user and returns a JWT token
- `/api/users`: Allows CRUD operations on users (requires admin access)
- `/api/jogtimes`: Allows CRUD operations on jogging times (requires user or admin access)
- `/api/reports/average-speed`: Returns the average speed per week for all users (requires user or admin access)
- `/api/reports/average-distance`: Returns the average distance per week for all users (requires user or admin access)

To use these endpoints, you will need to include the JWT token in the `Authorization` header of your requests. You can get a JWT token by making a POST request to the `/api/authenticate` endpoint with a valid username and password.

## API Documentation

The API documentation is generated using Swagger and can be accessed by visiting `http://localhost:5000/swagger` when the API is running locally. The Swagger documentation provides detailed information about each endpoint, including example requests and responses.

## Contributing

If you would like to contribute to the Jog Tracker API, please follow these steps:

1. Fork the repository
2. Create a new feature branch (`git checkout -b feature-name`)
3. Implement your feature and write tests
4. Commit your changes with a meaningful commit message (`git commit -m "Added feature-name"`)
5. Push your branch to your fork (`git push origin feature-name`)
6. Create a pull request from your branch to the main branch of this repository

## License

The Jog Tracker API is licensed under the MIT License. See the [LICENSE](LICENSE) file for more information.
