Deployed version: https://sparehub.fhallengreen.com/ 

![SpareHub Banner](https://i.imgur.com/8xpieD7.png)

# SpareHub - Ship Spare Parts Management Application

## Overview
SpareHub is a web application designed to manage the dispatch and inventory of ship spare parts efficiently. It provides an interface to view, search, filter, and group orders, along with details about the vessels, stock locations, and statuses. The goal is to streamline the spare parts logistics process, offering real-time access to essential order information.

## Features
- **Order Management**: Display a list of orders with details such as owner, vessel, stock location, and status.
- **Dynamic Grouping**: Group orders by stock location to provide better organization.
- **Search & Filter**: Easily search for specific orders using keywords.
- **Status Indicators**: Visual status indicators for orders.
- **Responsive Design**: Adapts to various screen sizes for a seamless experience.

## Getting Started
### Prerequisites
- **Node.js** (v22 or later)
- **npm** (v7 or later)
- **Docker** (optional, for containerized development)

### Installation
1. Clone this repository:
   ```bash
   git clone https://github.com/FHallengreen/sparehub.git
   cd sparehub
   ```

2. Navigate to the `frontend` directory:
   ```bash
   cd frontend
   ```

3. Install the dependencies:
   ```bash
   npm install
   ```

### Running Locally
To run the frontend application locally without Docker:
1. Navigate to the `frontend` directory (if not already there):
   ```bash
   cd frontend
   ```

2. Start the development server:
   ```bash
   npm run dev
   ```

3. Open a web browser and go to:
   ```
   http://localhost:5173
   ```

### Running with Docker (Optional)
If you want to use Docker for running the application:
1. Make sure Docker and Docker Compose are installed on your system.
2. Navigate to the root of the project.
3. Run the following command:
   ```bash
   docker compose up --build
   ```

4. Access the application in your web browser at:
   ```
   http://localhost:5173
   ```

## Project Structure
```
sparehub/
├── backend/                  # Backend development (To be implemented)
├── frontend/                 # Frontend source code
│   ├── public/
│   ├── src/
│   ├── Dockerfile
│   ├── vite.config.ts
│   └── package.json
├── docker-compose.yml        # Docker Compose configuration
└── README.md
```
