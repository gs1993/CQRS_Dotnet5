# CQRS starter template

### Current framework version: .NET 5 

Based on: [https://enterprisecraftsmanship.com/ps-cqrs](https://enterprisecraftsmanship.com/ps-cqrs)

--------------

# Features
### Optimized data schemas
* [Entity Framework](https://github.com/dotnet/efcore) - Fully fledged object-database mapper for data manipulation.
* [Dapper](https://github.com/StackExchange/Dapper) - Light and simple object mapper for database reads. Using SQL gives access to complex query optimizaton mechanizms.

### Separation of concerns
* Segregating the read and write sides mean maintainable and flexible models. Most of the complex business logic goes into the write model. The read model can be relatively simple.

### Security
* It's easier to ensure that only the right domain entities are performing writes on the data.

### Independent scaling 
* Split to WRITE and READ databases comming soon

--------------

## Setup

1. Download and install [Docker](https://www.docker.com/products/docker-desktop)
2. Go to file [docker/docker-compose-mssql.yml](https://github.com/gs1993/TemplateCQRS/blob/master/docker/docker-compose-mssql.yml), change SA password and user credentials
3. Execute command: `docker-compose -f docker-compose-mssql.yml up -d` in order to run Sql Server instance in docker container
4. Go to folder [docker/](https://github.com/gs1993/TemplateCQRS/tree/master/docker) and execute create database script: ./entrypoint.sh - SA_password

### UI (MVC)
1. Go to [appsettings.json](https://github.com/gs1993/TemplateCQRS/blob/master/src/api/UI/appsettings.json) and set the connection string to WRITE DATABASE
2. Run UI from [dockerfile](https://github.com/gs1993/TemplateCQRS/blob/master/src/api/UI/Dockerfile) using command: `docker-compose up --build .`
3. Go to app url: [http://localhost:5002](http://localhost:5002)
 
### WebApi
1. Go to [appsettings.json](https://github.com/gs1993/TemplateCQRS/blob/master/src/api/WebApi/appsettings.json) and set the connection string to WRITE DATABASE
2. Run UI from [dockerfile](https://github.com/gs1993/TemplateCQRS/blob/master/src/api/WebApi/Dockerfile) using command: docker-compose up --build.
3. Go to app url: [http://localhost:5000](http://localhost:5000)

<div class="text-blue mb-2">
  .text-blue on white
</div>
