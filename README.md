# Jobsity Challenge

## General Information:

Full Name: Ángel Andrés Mañón <br />
Email: angelandresmanon@gmail.com

# Technologies

### Back-End:

- .NET Core 3.1
- ASP.NET Core 3.1
- RabbitMQ
- Microsoft Sql Server 2017
- Docker

### Front-End:

- jQuery
- Bootstrap

### Libraries:

- Entity Framework Core
- ASP.NET Core SignalR
- Swagger UI (REST API Documentation Tool)
- RabbitMQ .NET Client
- TinyCsvParser


# Architecture

The solution is separated into the following projects:

- **JobsityChat.Core:** The core project basically defines the database entities, the interfaces and the application constants.

- **JobsityChat.Infraestructure:** The infraestructure project basically has the services implementations based on core project interfaces, the database persistence (context and migrations) and message queuing.

- **JobsityChat.StocksBot:** The StockBot project is basically the console project that is listening for incoming stock information request from queue. this project is in charge of searching and parsing the csv file of the requested stock code to push a message into the stock info response queue with the parsed data.

- **JobsityChat.WebApi:** The WebApi project is basically the restful api to the frontend requests like: login, register and chat room, also the WebApi keeps listening for incoming stock information responses from the queue. this project also implements WebSocket endpoints to handle real-time chat features using ASP.NET Core SignalR.

- **JobsityChat.WebUI:** The WebUI project is basically the FrontEnd web application where the user can login, register and send message in the chat room, this project has developed using ASP.NET Core Framework.

# Mandatory Features

- Allow registered users to log in and talk with other users in a chatroom. [x]

- Allow users to post messages as commands into the chatroom with the following format /stock=stock_code . [x]

- Create a decoupled bot that will call an API using the stock_code as a parameter
(https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv, here aapl.us is the stock_code). [x]

- The bot should parse the received CSV file and then it should send a message back into the chatroom using a message broker like RabbitMQ. The message will be a stock quote using the following format: “APPL.US quote is $93.42 per share”. The post owner will be
the bot. [x]

- Have the chat messages ordered by their timestamps and show only the last 50 messages. [x]

- Unit test the functionality you prefer. [ ]

# Bonus (Optional)

- Use .NET identity for users authentication. [x]

- Handle messages that are not understood or any exceptions raised within the bot. [x]

- Build an installer. [ ]

# Project setup

### Runtime and SDKs

- Download and install Docker (https://docs.docker.com/get-docker/)
- Download and install .NET Core 3.1 SDK (https://dotnet.microsoft.com/download)

### Dependencies

- Start Microsoft SQL Server 2017 docker container:

    ````
	docker run -d --hostname mssqldb --name mssqldb_dev -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=P455w0rd123456789" -p 1433:1433 mcr.microsoft.com/mssql/server:2017-CU21-ubuntu-16.04
	````

- Start Rabbit-MQ docker container:
   
    ````
	docker run -d --hostname rabbitmq --name rabbitmq_dev -p 5672:5672 -p 15672:15672 rabbitmq:3-management
	````

### Run projects

After install the dependencies and make sure the docker containers are running now we can continue to run the projects in the following order:

## **StockBot**

- Open the terminal and go to the following repository directory `JobsityChallenge/src/JobsityChat/JobsityChat.StocksBot/`

- Install project dependencies by running command:
    ```
    dotnet restore "JobsityChat.StocksBot.csproj"
    ```
- Set the environment by running command:
    ```
    export NETCORE_ENVIRONMENT=Development
    ```
- Start project by running command:
    ```
    dotnet run "JobsityChat.StocksBot.csproj"
    ```

After executing the previous steps you should be able to see the following message on the console:

[image]

## **WebApi**

- Open the terminal and go to the following repository directory `JobsityChallenge/src/JobsityChat/JobsityChat.WebApi/`

- Install project dependencies by running command:
    ```
    dotnet restore "JobsityChat.WebApi.csproj"
    ```
- Set the environment by running command:
    ```
    export ASPNETCORE_ENVIRONMENT=Development
    ```
- Start project by running command:
    ```
    dotnet run "JobsityChat.WebApi.csproj"
    ```

After executing the previous steps, if you click [here](http://localhost:5000) you should be able to see the Swagger UI (API Documentation Tool) in your browser:

[image]

## **WebUI**

- Open the terminal and go to the following repository directory `JobsityChallenge/src/JobsityChat/JobsityChat.WebUI/`

- Install project dependencies by running command:
    ```
    dotnet restore "JobsityChat.WebUI.csproj"
    ```
- Set the environment by running command:
    ```
    export ASPNETCORE_ENVIRONMENT=Development
    ```
- Start project by running command:
    ```
    dotnet run "JobsityChat.WebUI.csproj"
    ```

After executing the previous steps, if you click [here](http://localhost:4200) you should be able to see the web app running in your browser:

[image]

# How to use

Click [here](http://localhost:4200) to go into the webapp. 

You can register a new account in [here](http://localhost:4200/account/register)
or login [here](http://localhost:4200/account/login) if you already have one account.

You can have multiple user sessions login in from a different browser window (not browser tab).

# Jobsity chat room screen :)

[image]