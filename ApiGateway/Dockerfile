# Use the official Nginx image as the base image
FROM nginx:latest

# Copy custom Nginx configuration file
COPY nginx.conf /etc/nginx/nginx.conf

# Expose ports (adjust if necessary)
EXPOSE 8000 443

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]