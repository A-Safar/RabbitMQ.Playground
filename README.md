# RabbitMQ Playground

A .NET 8 playground project for exploring RabbitMQ messaging patterns and exchange types. This project demonstrates various RabbitMQ exchange patterns including Direct, Fanout, Topic, and Headers exchanges with both message publishing and consuming capabilities.

## 🏗️ Project Structure

```
RabbitMQ.Playground/
├── RabbitMQ.API/              # Web API for publishing messages
├── RabbitMQ.Domain/           # Domain models and entities
├── RabbitMQ.Services/         # RabbitMQ service implementations and consumers
├── docker-compose.yml         # RabbitMQ container setup
└── README.md
```

## 🚀 Features

- **Multiple Exchange Types Support**:
  - Direct Exchange with routing keys
  - Fanout Exchange for broadcast messaging
  - Topic Exchange with pattern-based routing
  - Headers Exchange with header-based routing

- **Message Publishing**: RESTful API endpoints for publishing messages
- **Message Consuming**: Background service for consuming messages
- **Docker Integration**: Easy RabbitMQ setup with Docker Compose
- **Swagger Documentation**: Interactive API documentation

## 🛠️ Technologies Used

- **.NET 8**: Latest .NET framework
- **ASP.NET Core Web API**: For REST endpoints
- **RabbitMQ**: Message broker
- **Docker & Docker Compose**: Container orchestration
- **Swagger/OpenAPI**: API documentation

## 📋 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started) and [Docker Compose](https://docs.docker.com/compose/)
- Your favorite IDE (Visual Studio, VS Code, Rider, etc.)

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/A-Safar/RabbitMQ.Playground.git
cd RabbitMQ.Playground
```

### 2. Start RabbitMQ with Docker

```bash
docker-compose up -d
```

This will start RabbitMQ with:
- **AMQP Port**: `30001` (for application connections)
- **Management UI**: `30002` (accessible at http://localhost:30002)
- **Default Credentials**: `guest/guest`

### 3. Build and Run the Application

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run the API
dotnet run --project RabbitMQ.API
```

The API will be available at:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger UI**: `https://localhost:5001/swagger`

## 📚 API Endpoints

### Direct Exchange
```http
POST /RabbitMQ/direct
Content-Type: application/json

{
  "id": 1,
  "content": "Hello Direct Exchange!"
}
?routingKey=direct-routing-key
```

### Fanout Exchange
```http
POST /RabbitMQ/fanout
Content-Type: application/json

{
  "id": 2,
  "content": "Hello Fanout Exchange!"
}
```

### Topic Exchange
```http
POST /RabbitMQ/topic
Content-Type: application/json

{
  "id": 3,
  "content": "Hello Topic Exchange!"
}
?topicName=topic.routing.pattern
```

### Headers Exchange
```http
POST /RabbitMQ/headers
Content-Type: application/json

{
  "id": 4,
  "content": "Hello Headers Exchange!"
}
```

## 🔧 Configuration

### RabbitMQ Connection Settings

The application connects to RabbitMQ using the following default settings:
- **Host**: `localhost`
- **Port**: `30001`
- **Username**: `guest` (default)
- **Password**: `guest` (default)

### Message Consumer

The `DirectQueue1Consumer` automatically starts when the application runs and listens to the `direct-q-1` queue for incoming messages.

## 📊 RabbitMQ Management

Access the RabbitMQ Management UI at `http://localhost:30002` to:
- Monitor queues and exchanges
- View message statistics
- Manage connections and channels
- Debug message flows

**Default Login**: `guest/guest`

## 🏗️ Architecture

### Domain Layer (`RabbitMQ.Domain`)
- Contains the `Message` entity with `Id` and `Content` properties

### Services Layer (`RabbitMQ.Services`)
- **Contracts**: `IRabbitMQService` interface
- **Implementations**: `RabbitMQService` with all exchange type implementations
- **Consumers**: Background services for message consumption

### API Layer (`RabbitMQ.API`)
- RESTful controllers for message publishing
- Swagger/OpenAPI integration
- Dependency injection setup

## 🧪 Testing the Application

1. **Start the services** using the steps above
2. **Open Swagger UI** at `https://localhost:5001/swagger`
3. **Send test messages** using the API endpoints
4. **Monitor the console** output to see consumed messages
5. **Check RabbitMQ Management UI** for queue statistics

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is open source and available under the [MIT License](LICENSE).

## 📞 Support

If you have any questions or issues, please:
1. Check the [Issues](https://github.com/A-Safar/RabbitMQ.Playground/issues) section
2. Create a new issue with detailed information
3. Provide steps to reproduce any problems

## 🎯 Future Enhancements

- [ ] Add more consumer examples
- [ ] Implement message persistence patterns
- [ ] Add retry mechanisms and dead letter queues
- [ ] Include performance benchmarks
- [ ] Add integration tests
- [ ] Implement message serialization options
- [ ] Add configuration management
- [ ] Include logging and monitoring

---

**Happy Messaging! 🐰**