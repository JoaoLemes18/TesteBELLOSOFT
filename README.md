# 🌦️ API Clima + Autenticação (ASP.NET Core + EF Core + MySQL)

Uma API RESTful em **.NET 8** com **JWT** para autenticação e endpoints para histórico/sincronização de **clima** em **MySQL**.

---

### 🔍 Sobre a API

Esta API segue princípios REST e adota uma arquitetura em camadas para facilitar manutenção, testes e evolução:

- **Camada de Controllers**: recebe as requisições HTTP, valida entrada básica e delega para os serviços.
- **Camada de Services**: concentra a lógica de negócio (regras de autenticação, validações de sincronização, etc.).
- **Camada de Repositories**: acesso a dados usando **EF Core** com **MySQL**.
- **DTOs**: isolam o contrato público (requests/responses) dos modelos de domínio.
- **Middlewares**: tratam **logs** e **erros** de forma centralizada (retornando `ApiResponse<T>` padronizado).
---

## 📂 Estrutura
```
API/
├─ Controllers/ (Auth + Climate)
├─ Data/ (DbContext)
├─ Dtos/
├─ Helpers/ (JWT)
├─ Interfaces/
├─ Middlewares/ (Logs + Erros)
├─ Models/ (User, ClimateRecord)
├─ Repositories/
├─ Services/
└─ API.Tests/ (xUnit)
```
---

## ✨ Funcionalidades
- 🔐 Autenticação JWT (registro e login)
- 🌍 Histórico climático (MySQL)
- 🔄 Sincronização de dados de clima
- 🛡️ Middlewares de logs e erros
- 📑 Respostas padronizadas `ApiResponse<T>`
- 🧪 Testes com xUnit

---

## 📦 Stack
- .NET 8, EF Core, MySQL 8
- System.Text.Json, JWT, Swagger (Swashbuckle)
- xUnit


---


## 🚀 Como rodar

### 1) Clonar o repositório
```bash
git clone https://github.com/seu-usuario/api-clima.git
cd api-clima
dotnet restore
```

### 2) Criar o banco de dados
Execute no MySQL:
```sql
CREATE DATABASE WeatherDb;
USE WeatherDb;

CREATE TABLE ClimateRecords (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CapturedAtUtc DATETIME NOT NULL,
    City VARCHAR(100) NOT NULL,
    Latitude DOUBLE NOT NULL,
    Longitude DOUBLE NOT NULL,
    TemperatureC DOUBLE NOT NULL,
    WindSpeed DOUBLE NOT NULL,
    WindDirection DOUBLE NOT NULL,
    IsDay BOOLEAN NOT NULL
);

CREATE TABLE Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Email VARCHAR(150) NOT NULL UNIQUE,
    Senha VARCHAR(255) NOT NULL
);
```

### 3) Configurar `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=WeatherDb;user=root;password=senha"
  }
}
```

### 4) Executar a API
```bash
dotnet run --project API
```
Disponível em:
- http://localhost:5000
- https://localhost:7000

---

## 🔑 Endpoints

### Autenticação

**Registrar usuário**
```http
POST /api/auth/register
```
**Request**
```json
{
  "nome": "João",
  "email": "joao@email.com",
  "senha": "123456"
}
```
**200/201**
```json
{
  "success": true,
  "message": "Usuário registrado com sucesso.",
  "data": { "id": 1, "nome": "João", "email": "joao@email.com" }
}
```
**400**
```json
{ "success": false, "message": "E-mail já registrado." }
```

### Login
```http
POST /api/auth/login
```
**Request**
```json
{ "email": "joao@email.com", "senha": "123456" }
```
**200**
```json
{
  "success": true,
  "message": "Usuário autenticado com sucesso",
  "data": { "token": "eyJhbGciOi..." }
}
```
**401**
```json
{ "success": false, "message": "Credenciais inválidas." }
```

### Clima

**Sincronização**
```http
POST /api/climate/sync
Authorization: Bearer {seu_token}
```
**Request**
```json
[
  {
    "city": "Cuiabá",
    "latitude": -15.601,
    "longitude": -56.097
  }
]

```
**200**
```json
{
  "success": true,
  "message": "Dados de clima sincronizados com sucesso.",
  "data": [
    { "id": 2, "city": "Cuiabá", "temperatureC": 34.5, "capturedAtUtc": "2025-08-23T18:10:00Z" }
  ]
}
```
**400**
```json
{ "success": false, "message": "Payload inválido." }
```

**Histórico**
```http
GET /api/climate/history
Authorization: Bearer {seu_token}
```
**200**
```json
{
  "success": true,
  "message": "Histórico de clima recuperado",
  "data": [
    { "id": 1, "city": "Cuiabá", "temperatureC": 34.5, "capturedAtUtc": "2025-08-23T18:00:00Z" }
  ]
}
```
**401**
```json
{ "success": false, "message": "Não autorizado." }
```



---

## 🧪 Testes

O projeto utiliza **xUnit** para testes automatizados.  
Eles cobrem principalmente:

- **Autenticação**  
  - Registro de usuário (sucesso e falha com e-mail duplicado)  
  - Login com credenciais válidas  
  - Login com credenciais inválidas  

- **Clima**  
  - Consulta ao histórico de clima (com token válido e inválido)  
  - Sincronização de dados (payload válido e inválido)  

---

### ▶️ Rodar os testes
```bash
cd API.Tests
```
```bash
dotnet test
```
