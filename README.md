# .NET 8 ile Mikroservis Mimarisi

## Genel Bakış

Bu proje .NET 8 kullanılarak geliştirilmiştir. Bu projeyi geliştirirken, mikroservisler arasında senkron ve asenkron iletişim, OAuth 2.0 ve OpenID Connect protokollerinin implementasyonu, Eventual Consistency modeli ve Docker kullanımı gibi önemli konularda bilgi sahibi oldum.

## Mikroservisler

### Catalog Microservice
- Kurs bilgilerinin tutulmasından ve sunulmasından sorumlu mikroservis.
- **Veritabanı:** MongoDB
- Bu mikroservis, MongoDB kullanarak kurs bilgilerini yönetir ve sunar.

### Basket Microservice
- Sepet işlemlerinden sorumlu mikroservis.
- **Veritabanı:** RedisDB
- RedisDB kullanarak sepet işlemlerini yönetir ve kullanıcı sepetlerini depolar.

### Discount Microservice
- Kullanıcılara tanımlanacak indirim kuponlarından sorumlu mikroservis.
- **Veritabanı:** PostgreSQL
- PostgreSQL kullanarak indirim kuponlarını yönetir ve kullanıcılarla ilişkilendirir.

### Order Microservice
- Sipariş işlemlerinden sorumlu mikroservis. Domain Driven Design ve CQRS (MediatR ile) kullanılarak geliştirilmiştir.
- **Veritabanı:** SQL Server
- Sipariş süreçlerini yönetir ve CQRS ile komut ve sorgu işlemlerini ayrıştırır.

### FakePayment Microservice
- Ödeme işlemlerinden sorumlu mikroservis.
- Kullanıcıların ödeme işlemlerini simüle eder ve diğer mikroservislerle entegre çalışır.

### IdentityServer Microservice
- Kullanıcı verilerinin tutulması ve token üretiminden sorumlu mikroservis.
- **Veritabanı:** SQL Server
- IdentityServer kullanarak kullanıcı doğrulama işlemlerini yönetir ve token üretir.

### PhotoStock Microservice
- Kurs fotoğraflarının tutulması ve sunulmasından sorumlu mikroservis.
- Fotoğrafların yönetimini ve sunumunu sağlar.

### API Gateway
- Ocelot Kütüphanesi kullanarak istekleri ilgili mikroservislere yönlendirir.
- Mikroservisler arasındaki trafiği yönetir ve yönlendirir.

### Message Broker
- Mikroservisler arası iletişim için RabbitMQ kullanır ve MassTransit kütüphanesi ile entegre edilmiştir.
- RabbitMQ kullanarak mikroservisler arasında mesaj iletimini sağlar.

### Asp.Net Core MVC Microservice
- Kullanıcı arayüzü mikroservisi olup, diğer mikroservislerden aldığı verileri kullanıcıya gösterir ve kullanıcı ile etkileşimi sağlar.
- Kullanıcı arayüzünü yönetir ve mikroservislerle entegre çalışır.

## Teknolojiler ve Kütüphaneler

- **.NET 8**
- **ASP.NET Core MVC**
- **RabbitMQ (MassTransit Library)**
- **IdentityServer v4**
- **OAuth 2.0 / OpenID Connect**
- **Domain Driven Design**
- **CQRS Pattern**
- **PostgreSQL**
- **MongoDB**
- **SQL Server**
- **Docker & Docker Compose**
- **API Gateway (Ocelot Library)**

## Nasıl Çalıştırılır

1. Depoyu klonlayın:
   ```sh
   git clone https://github.com/KarabulutSibel/Microservices.git

2. Proje Dizinine gidin:
   ```sh
   cd your-repo-name

3. Docker containerlarını oluşturup çalıştırın:
   ```sh
   docker-compose up --build
