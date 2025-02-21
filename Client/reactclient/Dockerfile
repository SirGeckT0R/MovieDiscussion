FROM node:18-alpine
 
WORKDIR /app

COPY ./Client/reactclient/package*.json .
 
RUN npm install
 
COPY ./Client/reactclient .
 
EXPOSE 6001
 
CMD ["npm", "run","dev"]