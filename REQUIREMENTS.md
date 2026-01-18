# SplitCorrect - Gereksinimler

## Sistem Gereksinimleri

### Gerekli Yazılımlar

#### Backend (.NET 8)
- **.NET 8 SDK** (veya üzeri)
  - İndirme: https://dotnet.microsoft.com/download/dotnet/8.0
  - Doğrulama: `dotnet --version`

#### Frontend (React + TypeScript)
- **Node.js** (v18 veya üzeri)
  - İndirme: https://nodejs.org/
  - Doğrulama: `node --version`
- **npm** (v9 veya üzeri, Node.js ile birlikte gelir)
  - Doğrulama: `npm --version`

#### Veritabanı
- **Docker Desktop** (PostgreSQL ve pgAdmin için)
  - İndirme: https://www.docker.com/products/docker-desktop
  - Doğrulama: `docker --version`
- **Docker Compose** (Docker Desktop ile birlikte gelir)
  - Doğrulama: `docker compose version`

## NuGet Paketleri (Backend)

### SplitCorrect.Domain
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.11" />
```

### SplitCorrect.Application
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.11" />
```

### SplitCorrect.Infrastructure
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.11" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Relational" Version="8.0.11" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.11" />
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="8.0.6" />
```

### SplitCorrect.Api
```xml
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.11" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
```

### SplitCorrect.Tests
```xml
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.1" />
<PackageReference Include="xunit" Version="2.9.2" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
<PackageReference Include="coverlet.collector" Version="6.0.2" />
<PackageReference Include="Moq" Version="4.20.72" />
<PackageReference Include="FluentAssertions" Version="6.12.2" />
<PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="8.0.11" />
```

## npm Paketleri (Frontend)

### Dependencies
```json
{
  "react": "^18.3.1",
  "react-dom": "^18.3.1",
  "lucide-react": "^0.469.0"
}
```

### DevDependencies
```json
{
  "@eslint/js": "^9.17.0",
  "@types/react": "^18.3.18",
  "@types/react-dom": "^18.3.5",
  "@vitejs/plugin-react": "^4.3.4",
  "autoprefixer": "^10.4.20",
  "eslint": "^9.17.0",
  "eslint-plugin-react-hooks": "^5.1.0",
  "eslint-plugin-react-refresh": "^0.4.16",
  "globals": "^15.14.0",
  "postcss": "^8.4.49",
  "tailwindcss": "^3.4.17",
  "typescript": "~5.6.2",
  "typescript-eslint": "^8.18.2",
  "vite": "^6.0.5"
}
```

## Docker Servisleri

### PostgreSQL
- **Image**: `postgres:16`
- **Port**: `5432`
- **Database**: `splitcorrect`
- **Username**: `admin`
- **Password**: `admin123`

### pgAdmin
- **Image**: `dpage/pgadmin4:latest`
- **Port**: `5050`
- **Email**: `admin@splitcorrect.com`
- **Password**: `admin123`

## Kurulum Komutları

### Hızlı Kurulum (Otomatik)

#### Windows
```bash
setup.bat
```

#### Linux/macOS
```bash
chmod +x setup.sh
./setup.sh
```

### Manuel Kurulum

### 1. Docker Servislerini Başlat
```bash
docker compose up -d
```

### 2. Backend Bağımlılıklarını Yükle
```bash
cd src/SplitCorrect.Infrastructure
dotnet restore

# Veritabanı Migration'larını Uygula
dotnet ef database update --startup-project ../SplitCorrect.Api
```

### 3. Frontend Bağımlılıklarını Yükle
```bash
cd frontend

# .env dosyasını oluştur (opsiyonel, default değerler çalışır)
cp .env.example .env

npm install
```

### 4. Uygulamayı Çalıştır

#### Hızlı Başlatma (Windows)
```bash
start.bat
```

#### Manuel Başlatma

#### Backend
```bash
cd src/SplitCorrect.Api
dotnet run
# http://localhost:5016 adresinde çalışacak
# Swagger UI: http://localhost:5016/swagger
```

#### Frontend
```bash
cd frontend
npm run dev
# http://localhost:3000 adresinde çalışacak
```

## Test Çalıştırma

### Unit Testler
```bash
cd src/SplitCorrect.Tests
dotnet test
```

### Coverage Report
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Port Kullanımı

- **Backend API**: `http://localhost:5016`
- **Frontend**: `http://localhost:3000`
- **PostgreSQL**: `localhost:5432`
- **pgAdmin**: `http://localhost:5050`

## Ortam Değişkenleri

Backend'de `appsettings.json` dosyasında tanımlı:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=splitcorrect;Username=admin;Password=admin123"
  }
}
```

## Minimum Sistem Gereksinimleri

- **RAM**: 4 GB (8 GB önerilir)
- **Disk**: 2 GB boş alan
- **İşletim Sistemi**: Windows 10/11, macOS 10.15+, Linux (Ubuntu 20.04+)
