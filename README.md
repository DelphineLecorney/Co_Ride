# 🚗 Plateforme de Covoiturage - Microservices .NET 8

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker)](https://www.docker.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

> Architecture microservices avancée avec DDD, CQRS, Event Sourcing et Saga Patternure microservices avancée avec DDD, CQRS, Event Sourcing et Saga Pattern

---

## 🎯 Vue d'ensemble

Plateforme de covoiturage conçue pour démontrer les meilleures pratiques en matière d'architecture microservices avec .NET 8. Ce projet intègre des concepts avancés tels que Domain-Driven Design (DDD), Command Query Responsibility Segregation (CQRS), Event Sourcing et Saga Pattern pour assurer une scalabilité, une résilience et une maintenabilité optimales.

### Caractéristiques principales

- **Architecture** : Clean Architecture, DDD (Domain-Driven Design)
- **Patterns** : CQRS, Event Sourcing
- **Communication** : RESTful APIs, Async Messaging (RabbitMQ + MassTransit)
- **Base de données** : Polyglotte (SQL Server, MongoDB, PostgreSQL)
- **Frontend** : Blazor WebAssembly
- **Gateway API** : YARP (Yet Another Reverse Proxy)
- **Cache** : Redis distribué
- **Logging** : Seq centralisé
- **Containerisation** : Docker + Docker Compose

---

## 📦 Architecture des Services

```
┌─────────────────────────────────────────────────────────┐
│                    🌐 API Gateway (YARP)                │
│                    Port: 5000                           │
└─────────────────────────────────────────────────────────┘
                            │
        ┌───────────────────┼───────────────────┐
        │                   │                   │
┌───────▼────────┐  ┌──────▼────────┐  ┌──────▼────────┐
│ 🔐 Identity    │  │ 🚗 Trip       │  │ 📅 Booking    │
│ Service        │  │ Service       │  │ Service       │
│ Port: 5001     │  │ Port: 5002    │  │ Port: 5003    │
│                │  │               │  │               │
│ SQL Server     │  │ PostgreSQL +  │  │ SQL Server +  │
│ + EF Core      │  │ Marten (ES)   │  │ MassTransit   │
│ + JWT          │  │ + CQRS        │  │ + Saga        │
└────────────────┘  └───────────────┘  └───────────────┘
                            │
                    ┌───────▼────────┐
                    │ 📧 Notification│
                    │ Service        │
                    │ Port: 5004     │
                    │                │
                    │ MongoDB +      │
                    │ Consumer       │
                    └────────────────┘
```



