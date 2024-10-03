# Use the official Node.js image as the base image
FROM node:18

# Set the working directory inside the container
WORKDIR /app

# Copy the package.json and package-lock.json files
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the application source code to the container
COPY . .

# Expose the port that Vite will run on (default: 5173)
EXPOSE 5173

# Start the Vite development server with host as 0.0.0.0 for live updates
CMD ["npm", "run", "dev", "--", "--host"]
