FROM node:18-alpine
 
WORKDIR /app

COPY ./Client/reactclient/package*.json .
COPY ./Client/reactclient/*.env .

ENV VITE_WEB_API_HOST='http://localhost:5000'
ENV VITE_IMAGES_HOST='http://localhost:5000'
 
RUN npm install --force
 
COPY ./Client/reactclient .
 
EXPOSE 6001
 
CMD ["npm", "run","dev"]