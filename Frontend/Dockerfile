# frontend/Dockerfile
FROM node:25-alpine

# Set working directory
WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .

EXPOSE 3000

CMD ["npm", "run dev"]
