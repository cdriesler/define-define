# Create image based on the official Node 12 image from dockerhub
FROM stefanscherer/node-windows:10.15

# Create a directory where our app will be placed
RUN mkdir -p C:/src/app

# Change directory so that our commands run inside this new directory
WORKDIR C:/src/app

# Copy dependency definitions
COPY package.json C:/src/app

# Install dependecies
RUN npm install

# Get all the code needed to run the app
COPY . C:/src/app

# Expose the port the app runs in
EXPOSE 8080

# Serve the app
CMD ["npm", "run", "serve"]