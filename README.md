# AuctionApp
Real-Time Auction System Built with Clean Architecture, CQRS, ASP.NET, React & SignalR

# Description
Auction App is a full-stack, real-time auction platform demonstrating modern software architecture and development practices like Clean Architecture and CQRS patterns.
It provides robust backend APIs via ASP.NET Core and React frontend (styled with Tailwind CSS).

Real-time bidding is powered by `SignalR` (using WebSockets).

# Features

- ### Clean Architecture + CQRS
  Maintainable codebase with clear separation of concerns and optimized command/query handling.

- ### Real-Time Bidding
  Seamless real-time communication using SignalR backed by WebSockets for instant updates.

- ### Modularity
  Using practices of CLean Architecture was achieved high level of modularity of the project. Each next layer depends only on the previous and not vice-versa.

# Usage of Swagger
  Basically, repository already includes generated API for front-end client, but if you want to generate it once more, just place `swagger.json` in `AuctionApp.React` directory and use `generateApi.sh` script in the same folder.

# Tech Stack
### Backend
- **.NET 8.0**  
- **CQRS**: MediatR 12.2.0  
- **Real-Time**: SignalR 8.0.6  
- **ORM / Tools**: Entity Framework Core Tools 8.0.6  
- **Authentication**: Identity Core 8.0.6  
- **Swagger/OpenAPI**: Swashbuckle 6.6.2  

### Frontend
- **React 18.2.0**  
- **TypeScript 5.4.5**  
- **Tailwind CSS 3.4.3**  
