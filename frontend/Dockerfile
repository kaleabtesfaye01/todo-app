FROM node:20 AS build

WORKDIR /app

# Copy package files
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application
COPY . .

# Build the application
RUN npm run build

# Debug: Show build output structure
RUN ls -la dist && ls -la dist/todo-frontend/browser

# Serve with nginx
FROM nginx:alpine

# Copy the built app (updated path for newer Angular versions)
COPY --from=build /app/dist/todo-frontend/browser /usr/share/nginx/html

# Copy nginx configuration
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Create env.template.js
RUN mkdir -p /usr/share/nginx/html/assets && \
    echo "window.env = { API_URL: '${API_URL:-http://localhost:5215/api}' };" > /usr/share/nginx/html/assets/env.template.js

# Create entrypoint script
RUN echo '#!/bin/sh' > /docker-entrypoint.sh && \
    echo 'envsubst < /usr/share/nginx/html/assets/env.template.js > /usr/share/nginx/html/assets/env.js' >> /docker-entrypoint.sh && \
    echo 'nginx -g "daemon off;"' >> /docker-entrypoint.sh && \
    chmod +x /docker-entrypoint.sh

EXPOSE 80

ENTRYPOINT ["/docker-entrypoint.sh"] 