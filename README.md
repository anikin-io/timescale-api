# TimescaleApi

**ASP.NET Core WebAPI для приёма, валидации и агрегации time‑series CSV данных в PostgreSQL (EF Core + Docker + Swagger).**

## Структура репозитория
 ```bash
.
├── TimescaleApi.sln
├── docker-compose.yml
├── TimescaleApi.API
│   ├── Dockerfile
│   ├── Program.cs
│   ├── Controllers
│   ├── Middleware
│   ├── Extensions
│   ├── Models
├── TimescaleApi.Application
│   ├── DTOs
│   ├── Services
├── TimescaleApi.Domain
│   ├── Entities
├── TimescaleApi.Tests
│   ├── Helpers
│   ├── Services
└── TimescaleApi.Infrastructure
    ├── Migrations
    ├── Persistence
    │   ├── DesignTime
    │   ├── Configurations
    │   └── TimescaleDbContext.cs
    └── Services
```

## Запуск проекта

1. **Клонируйте репозиторий**
   
   ```bash
   git clone https://github.com/anikin-io/timescale-api.git
   ```
   
2. **Перейдите в папку**
   
   ```bash
   cd timescale-api
   ```
   
3. **Соберите и запустите контейнеры**

   ```bash
   docker-compose up --build -d
   ```

4. **Откройте Swagger UI в браузере**

   ```bash
   https://localhost:5000/swagger
   ```
