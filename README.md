```markdown
# ToDoList API

This is a simple ToDo list API built using ASP.NET Core. The project includes user authentication, activity logging, and CRUD operations for managing ToDo items. The API also includes validation, and it is designed for development purposes.

## Installation

To install the app, follow these steps:

1. **Clone the repository:**

   ```bash
   git clone <repo-url>
   ```

2. **Configure the connection string:**

   - Open the `appsettings.Debug.json` file and update the connection string to point to your local SQL Server instance.

3. **Install project dependencies:**

   Make sure all project dependencies are installed by running:

   ```bash
   dotnet restore
   ```

4. **Run the application:**

   After configuring the connection string and installing the dependencies, simply run the app using:

   ```bash
   dotnet run
   ```

   The app will create the database, the necessary tables, and seed the required data automatically.

---

## Usage

Below are the available API endpoints, their descriptions, and how to use them.

### User Endpoints

1. **Login** (`POST /api/Users/login`)

   - **Description**: Authenticates the user and returns a token if successful.
   - **Body**:
     ```json
     {
       "username": "string",
       "password": "string"
     }
     ```
   - **Response**:
     - Returns a token if the credentials are valid.
     - On failure, returns a 403 status with an error message.
   - **Authentication**: Not required to access this endpoint.

2. **Get User Logs** (`GET /api/Users/logs`)

   - **Description**: Retrieves all user activity logs.
   - **Query Parameters**: Filters like date range or user ID can be added in the query string.
   - **Response**:
     - Returns a list of logs with pagination.
     - On failure, returns a bad request with an error message.
   - **Authentication**: Requires valid authentication token.

### ToDo Endpoints

1. **Get All ToDos** (`GET /api/ToDo`)

   - **Description**: Retrieves all ToDo items with optional query filters.
   - **Query Parameters**: You can filter the results using parameters like `status`, `dueDate`, etc.
   - **Response**:
     - Returns a list of ToDo items.
     - On failure, returns a bad request with an error message.
   - **Authentication**: Requires a valid authentication token.

2. **Get ToDo by ID** (`GET /api/ToDo/{id}`)

   - **Description**: Retrieves a single ToDo item by its ID.
   - **Response**:
     - Returns the details of the specified ToDo item.
     - On failure, returns a 404 with an error message if the item is not found.
   - **Authentication**: Requires a valid authentication token.

3. **Create ToDo** (`POST /api/ToDo`)

   - **Description**: Creates a new ToDo item.
   - **Body**:
     ```json
     {
       "title": "string",
       "description": "string",
       "dueDate": "date",
       "priority": "int"
     }
     ```
   - **Response**:
     - Returns no content (`204`) on success.
     - On failure, returns a bad request with an error message.
   - **Authentication**: Requires a valid authentication token.

4. **Update ToDo** (`PUT /api/ToDo/{id}`)

   - **Description**: Updates an existing ToDo item.
   - **Body**:
     ```json
     {
       "title": "string",
       "description": "string",
       "dueDate": "date",
       "priority": "int"
     }
     ```
   - **Response**:
     - Returns no content (`204`) on success.
     - On failure, returns a bad request with an error message.
   - **Authentication**: Requires a valid authentication token.

5. **Delete ToDo** (`DELETE /api/ToDo/{id}`)

   - **Description**: Soft deletes a ToDo item by its ID.
   - **Response**:
     - Returns no content (`204`) on success.
     - On failure, returns a bad request with an error message.
   - **Authentication**: Requires a valid authentication token.

---

## Future Enhancements

1. **Localization**: 
   - Implement localization for error messages and other UI components to support multiple languages.

2. **Security Enhancements**:
   - Improve security by implementing additional authentication measures such as OAuth, API rate limiting, and advanced logging for security breaches.

---

## Notes

1. This project was developed within a 3-hour time frame as a test application. Only happy path scenarios were tested for the endpoints, but the necessary validations are applied to ensure proper behavior.
   
2. The application is deployed using Docker Compose on a Linux server, along with the database. You can explore the API using the Swagger UI at:

   [http://164.68.96.165:5001/swagger](http://164.68.96.165:5001/swagger)

3. The server IP is exposed since this is a development server, not intended for production environments.

---

Feel free to contribute or reach out with questions or suggestions!
```

This `README.md` provides a clear guide on installation, usage, future enhancements, and some project notes. Let me know if you need any modifications!
