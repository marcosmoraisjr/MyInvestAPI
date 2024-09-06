FROM node:18
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm install -g @angular/cli@17
CMD [ "ng", "serve", "--port", "9090", "--host", "0.0.0.0" ]