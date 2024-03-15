# Patient Inc. 

## How to run 

1. Intialize the database by running the following command in the terminal:
```bash
docker-compose up --build -d
```
2. Run the following command to start database migrations:
```bash
cd api
dotnet ef database update
```
3. Run the following command to start the application:
```bash
dotnet run
```
4. Start frontend by running the following command:
```bash
cd frontend
npm install
npm run dev
```
## Folder Structure

- `api` - Contains the backend code in .NET Core
- `frontend` - Contains the frontend code in React
- `docker-compose.yml` - Contains the configuration for the database
- `Readme.md` - Contains the instructions to run the application
- `PatientInc.sln` - Contains the solution file for the .NET Core application
- `report.pdf` - Contains the report for the project
- `database` - Contains the database files (stored procedures, permissions, etc.)

## Ports

- `api` - http://localhost:5231
- `frontend` - http://localhost:5173/
- `database` - http://localhost:1433

## Notes

OpenTelemetry was used with the demo project and tweaking the docker compose by exposing the ports of the OpenTelemetry Collector
