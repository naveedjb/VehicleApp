# Vehicle Management Application

A simple full-stack application with a .NET Core Web API backend and Angular frontend for managing vehicles.

## Prerequisites

- **.NET 8.0 SDK** or later 
- **Node.js** (v18 or later) and **npm**

- ## Running the API

### Option 1: Using .NET CLI (Recommended)

1. Navigate to the API directory:
   ```bash
   cd VehicleApp/VehicleApi
   ```

2. Run the API:
   ```bash
   dotnet run
   ```

3. The API will start on `http://localhost:5000`
4. You can access Swagger UI at `http://localhost:5000/swagger` to test the endpoints


### API Endpoints

- **GET** `/api/vehicles` - Returns list of all vehicles
- **POST** `/api/vehicles` - Adds a new vehicle

- ## Running the Frontend

1. Navigate to the frontend directory:
   ```bash
   cd VehicleApp/VehicleUi
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npm start
   ```
4. The application will start on `http://localhost:4200`, open it in browser.

## Testing the Application

### End-to-End Flow

1. **Start the API** (runs on port 5000)
2. **Start the Angular app** (runs on port 4200)
3. **Open browser** to `http://localhost:4200`
4. You should see:
   - A form to add new vehicles at the top
   - A table displaying the existing vehicles (3 pre-loaded vehicles)
5. **Add a vehicle**:
   - Fill in Make, Model, and Year
   - Click "Add Vehicle"
   - The new vehicle should appear in the table immediately
6. **Validation**:
   - Try submitting without filling all fields - you'll see an error
   - Try entering a year outside 1900-2100 range - you'll see an error

You can test the API independently using:

**Swagger UI**: Navigate to `http://localhost:5000/swagger`

# Assumptions Made

1. **In-Memory Storage**: The vehicle list is stored in memory using a static list. Data will be lost when the API is restarted. This was chosen for simplicity as per requirements.

2. **Port Configuration**: 
   - API runs on port 5000 (configured in launchSettings.json)
   - Angular app runs on port 4200 (Angular default)

3. **CORS**: The API is configured to accept requests from `http://localhost:4200` to allow the Angular app to communicate with it.

4. **Validation**:
   - Backend: Uses Data Annotations on the Vehicle model (Required, Range)
   - Frontend: Basic client-side validation with user-friendly error messages
   - Year must be between 2000 and 2100

