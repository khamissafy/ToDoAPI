# ToDoListAPI

## Installation

Follow these steps to install and run the application:

1. Clone the repository:
   ```sh
   git clone <repo-url>
   cd ToDoListAPI
   ```
2. Update the database connection string in `appsettings.Debug.json`:
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Your_Local_DB_Connection_String"
   }
   ```
3. Ensure all dependencies are installed:
   ```sh
   dotnet restore
   ```
4. Run the application:
   ```sh
   dotnet run
   ```
   This will automatically create the database, tables, and seed the required data.

---

## API Usage

### Authentication

To access protected endpoints, you need an authentication token. Obtain a token using the login endpoint:

#### **Login**
- **Endpoint:** `POST /api/Users/login`
- **Request Body:**
  ```json
  {
    "username": "your_username",
    "password": "your_password",
    "rememberMe": <boolean, if you need a refresh token>
  }
  ```
- **Response:**
  ```json
  {
    "data": {
      "token": "your_access_token"
    }
  }
  ```
- **Requires Authentication?** No

Use the returned token for authentication in other endpoints by including it in the `Authorization` header:
```sh
Authorization: Bearer your_access_token
```

### User Management

#### **Get User Logs**
- **Endpoint:** `GET /api/Users/logs`
- **Query Parameters:**
  ```json
  {
    "page": 1,
    "pageSize": 10
  }
  ```
- **Requires Authentication?** Yes

### To-Do List Management

#### **Get To-Do Items**
- **Endpoint:** `GET /api/ToDo`
- **Query Parameters:**
  ```json
  {
    "page": 1,
    "pageSize": 10
  }
  ```
- **Requires Authentication?** Yes

#### **Get To-Do Item By ID**
- **Endpoint:** `GET /api/ToDo/{id}`
- **Requires Authentication?** Yes

#### **Create a To-Do Item**
- **Endpoint:** `POST /api/ToDo`
- **Request Body:**
  ```json
  {
    "title": "New Task",
    "description": "Task details"
  }
  ```
- **Requires Authentication?** Yes

#### **Update a To-Do Item**
- **Endpoint:** `PUT /api/ToDo/{id}`
- **Request Body:**
  ```json
  {
    "title": "Updated Task",
    "description": "Updated details"
    "isCompleted": true
  }
  ```
- **Requires Authentication?** Yes

#### **Delete a To-Do Item**
- **Endpoint:** `DELETE /api/ToDo/{id}`
- **Requires Authentication?** Yes

---

## Future Enhancements

1. Implement localization for error messages and UI text.
2. Apply additional security enhancements.

---

## Notes

1. **Development Time:** This project was built in under 3 hours, and only happy path testing was performed. However, all necessary validations have been applied.
2. **Deployment:** The app is deployed using Docker Compose on a Linux server along with the database.
3. **Access:** You can explore the API documentation at:
   [Swagger UI](http://164.68.96.165:5001/swagger)
4. **Security:** The server exposes its IP address because it is a development server, not intended for production use.

