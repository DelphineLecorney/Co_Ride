# 🏠 Co_Ride Documentation

> Plateforme moderne de covoiturage avec architecture microservices

**Dernière mise à jour** : 08/04/2026

---

## 🎯 Démarrage rapide

1. **[Contexte](./00%20-%20Vision/Contexte.md)** : Comprendre pourquoi Co_Ride existe.
2. **[Plugins](./03%20-%20Documentation/Plugins.md)**  : Liste des outils utilisés pour la doc.

---

## 📊 Dashboard Projet

### Services Microservices


```mermaid

`graph LR`

`A[Blazor WASM] --> B[YARP Gateway]`

`B --> C[Identity API]`

`B --> D[Trip API]`

`B --> E[Booking API]`

`D -.Events.-> F[(RabbitMQ)]`

`F -.-> E`

`F -.-> G[Notification]`

`style D fill:#4CAF50`

`style E fill:#2196F3`

`style F fill:#FF9800`
```


---

### Stack Technique

| Couche             | Technologies                              |
| ------------------ | ----------------------------------------- |
| **Frontend**       | Blazor WebAssembly, Tailwind CSS          |
| **API Gateway**    | YARP (Yet Another Reverse Proxy)          |
| **Services**       | ASP.NET Core 8, MediatR, FluentValidation |
| **Event Sourcing** | Marten (Trip Service)                     |
| **ORM**            | Entity Framework Core (Booking Service)   |
| **Messaging**      | RabbitMQ + MassTransit                    |
| **Databases**      | PostgreSQL, SQL Server, MongoDB           |
| **Orchestration**  | Docker Compose (dev)                      |

---

## 🗂️ Structure Documentation

```

📁 Del_Ride.Obsidian/

├── 📂 00-Vision/ # Pourquoi & Pour qui

│ ├── Contexte.md ✅ (fait)

│ ├── Vision-Produit.md ⏳(en cours)

│ └── Glossaire.md  

├── 📂 01-Architecture/ # Comment (technique)

│ ├── Vue-Ensemble.md

│ ├── Décisions-Architecture.md ⏳

│ └── Patterns-Utilisés.md

├── 📂 02-Services/ # Détails par service

│ ├── Identity/

│ ├── Trip/

│ ├── Booking/

│ └── Notification/

├── 📂 03-Domain/ # Logique métier

│ ├── Ubiquitous-Language.md

│ └── Business-Rules.md

├── 📂 04-Infrastructure/ # DevOps & Infra

├── 📂 05-Frontend/ # Blazor WASM

├── 📂 06-Workflows/ # User Journeys

└── 📂 07-Development/ # Guide dev

```
---

## 🔥 Liens Rapides Développement

### Commandes Utiles

```bash

# Lancer environnement complet

docker-compose up -d

# Lancer uniquement Trip Service

dotnet run --project src/Services/Trip.Service/Trip.API

# Tests unitaires

dotnet test tests/Trip.Tests.Unit

```

---

### Ressources Externes

- [Marten Documentation](https://martendb.io) - Event Store

- [MassTransit Docs](https://masstransit.io) - Service Bus

- [YARP Documentation](https://microsoft.github.io/reverse-proxy/) - Gateway

---

**Tags principaux** : 

`#microservices` `#event-sourcing` `#ddd` `#cqrs` `#marten` `#masstransit` `#blazor`