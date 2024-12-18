![SpareHub Banner](https://i.imgur.com/8xpieD7.png)

# SpareHub Project

## Deployed version can be accessed on: https://sparehub.fhallengreen.com/

## Overview

SpareHub is a full-stack application that involves a frontend (React), a backend (ASP.NET Core), and three databases: MySQL, MongoDB, and Neo4j. This project is configured to run using Docker Compose for easy setup or can be run locally without Docker if needed.

## Prerequisites

To run the project, ensure you have the following installed on your system:

- **Docker** (for Docker Compose setup)
- **Node.js** (for running the frontend locally without Docker)
- **.NET Core SDK** (for running the backend locally without Docker)
- **MySQL** (if running the backend locally without Docker)
- **MongoDB** (if running the backend locally without Docker)
- **Neo4j** (if running the backend locally without Docker)

## .env Configuration

Ensure you have an `.env` file in the root directory of the project with the following format:

```bash
# MYSQL
MYSQL_DATABASE=sparehub
MYSQL_USER=app_user
MYSQL_ROOT_PASSWORD=softE24
MYSQL_PASSWORD=softE24
MYSQL_PORT=3306
MYSQL_HOST=mysql-sparehub

# Frontend
API_URL=http://localhost:8080

# MongoDB
MONGO_INITDB_ROOT_USERNAME=sparehubUser
MONGO_INITDB_ROOT_PASSWORD=softE24
MONGO_INITDB_DATABASE=sparehub

MONGODB_URI=mongodb://sparehubUser:softE24@mongodb-sparehub:27017

## Neo4j
NEO4J_URL=bolt://neo4j:7687
NEO4J_USERNAME=neo4j
NEO4J_PASSWORD=password

# JWT
JWT_SECRET_KEY=XXX
JWT_ISSUER=XXX
JWT_AUDIENCE=XXX

# DHL
DHL_API_KEY=XXX

```

## Running with Docker Compose

### Steps:

1. **Ensure Docker is running** on your system.

2. **Build and run the containers** using Docker Compose:

   ```
   docker compose up --build -d
   ```

> [!NOTE]  
> If Environment variables are not set and it shows and error, do following step:

## ON MacOs/Linux:

```
export MYSQL_DATABASE=XXXX
export MYSQL_USER=XXXX
export MYSQL_PASSWORD=XXXX
```

## ON Windows using Powershell:

> [!IMPORTANT]  
> If you are on Windows, the environment variables are not set, so run following:

```
$env:MYSQL_DATABASE="XXXX"
$env:MYSQL_USER="XXXX"
$env:MYSQL_PASSWORD="XXXX"
```

This will start the following services:

- **frontend**: Running on [http://localhost:5173](http://localhost:5173)
- **backend**: Running on [http://localhost:8080](http://localhost:8080)
- **MySQL**: Exposed on port `3308` (container uses port `3306`)
- **MongoDB**: Exposed on port `27017`
- **Neo4j**: Exposed on port `7687`

3. **Access the frontend** at [http://localhost:5173](http://localhost:5173).

### Stopping the containers:

To stop the Docker containers, run:

```bash
docker compose down
```

### Useful Docker Commands:

- Rebuild the containers after changes:

  ```bash
  docker compose up --build
  ```

- View logs from all services:

  ```bash
  docker compose logs -f
  ```

## Running Locally Without Docker

You can also run the frontend and backend locally without Docker. Ensure you have installed the necessary dependencies before proceeding.

### Backend

1. **MySQL**:

   - Start MySQL on your system with the credentials specified in the `.env` file.
   - run creation script `./database/generate_user_privileges.sh` to create user privileges for the MySQL database.
   - Create a new database named `sparehub` in MySQL.
   - Optionally, use the SQL scripts in `./database/mysql/` to initialize the database schema.

2. **MongoDB**:

   - Start MongoDB on your system with the credentials specified in the `.env` file.
   - You can use the MongoDB scripts in `./database/MongoDB/` to initialize the database.

3. **Neo4j**:

   - Start Neo4j and ensure it's running on the port specified in your `.env` file.

4. **Run the backend**:
   Navigate to the backend folder and run the following command:

   ```bash
   cd backend/SpareHub
   dotnet run
   ```

   This will start the backend API at [http://localhost:8080](http://localhost:8080).

### Frontend

1. **Install dependencies**:

   ```bash
   cd frontend
   npm install
   ```

2. **Run the frontend locally**:

   ```bash
   npm run dev
   ```

   The frontend will be accessible at [http://localhost:5173](http://localhost:5173).

## Troubleshooting

### Common Issues:

- **Ports already in use**: If you encounter a "port already in use" error, ensure no other applications are running on the specified ports (`8080`, `5173`, `3308`, `27017`, etc.).

- **Database connection issues**: Make sure the MySQL, MongoDB, and Neo4j databases are properly configured, and the environment variables match their credentials.

- **Docker rebuild**: If any changes are made to the Dockerfile or the `.env` file, run `docker-compose down` and then `docker-compose up --build` to rebuild the containers.

---

With this setup, you can now run the project both with Docker and without Docker depending on your preference.
