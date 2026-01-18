#!/bin/bash

echo "🚀 SplitCorrect - Quick Setup Script"
echo "===================================="
echo ""

# Check prerequisites
echo "📋 Checking prerequisites..."

if ! command -v docker &> /dev/null; then
    echo "❌ Docker is not installed. Please install Docker Desktop."
    exit 1
fi

if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET 8 SDK is not installed. Please install it from https://dotnet.microsoft.com/download/dotnet/8.0"
    exit 1
fi

if ! command -v node &> /dev/null; then
    echo "❌ Node.js is not installed. Please install it from https://nodejs.org/"
    exit 1
fi

echo "✅ All prerequisites are installed!"
echo ""

# Start Docker services
echo "🐳 Starting Docker services..."
docker compose up -d

echo "⏳ Waiting for PostgreSQL to be ready..."
sleep 10

# Setup Backend
echo ""
echo "🔧 Setting up Backend..."
cd src/SplitCorrect.Infrastructure
dotnet restore
dotnet ef database update --startup-project ../SplitCorrect.Api

# Setup Frontend
echo ""
echo "🎨 Setting up Frontend..."
cd ../../frontend

if [ ! -f .env ]; then
    echo "📝 Creating .env file..."
    cp .env.example .env
fi

npm install

echo ""
echo "✅ Setup complete!"
echo ""
echo "🎯 To start the application:"
echo ""
echo "Terminal 1 (Backend):"
echo "  cd src/SplitCorrect.Api"
echo "  dotnet run"
echo ""
echo "Terminal 2 (Frontend):"
echo "  cd frontend"
echo "  npm run dev"
echo ""
echo "📚 Access points:"
echo "  Frontend:  http://localhost:3000"
echo "  Backend:   http://localhost:5016"
echo "  Swagger:   http://localhost:5016/swagger"
echo "  pgAdmin:   http://localhost:5050"
echo ""
