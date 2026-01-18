# SplitCorrect Frontend

Modern React frontend for the SplitCorrect expense splitting application.

## Features

- 📊 **Groups Management**: Create and manage expense groups
- 👥 **Members**: Add/remove members with email tracking
- 💳 **Expenses**: Track who paid what and split equally
- ⚖️ **Balances**: See who owes whom in real-time
- 💡 **Smart Settlements**: Minimize transactions needed to settle up

## Tech Stack

- **React 18** with TypeScript
- **Vite** for fast development
- **Tailwind CSS** for modern styling
- **Fetch API** for backend communication

## Getting Started

### Prerequisites

- Node.js 18+ installed
- Backend API running on http://localhost:5016

### Installation

```bash
npm install
```

### Development

Start the development server:

```bash
npm run dev
```

The app will be available at http://localhost:3000

### Build

Create a production build:

```bash
npm run build
```

Preview the production build:

```bash
npm run preview
```

## API Integration

The frontend connects to the backend API at `http://localhost:5016/api`. Make sure the backend is running before starting the frontend.

### API Endpoints Used

- `GET /api/groups` - List all groups
- `POST /api/groups` - Create a new group
- `GET /api/members/group/{groupId}` - Get group members
- `POST /api/members` - Add a member
- `GET /api/expenses/group/{groupId}` - Get group expenses
- `POST /api/expenses` - Create an expense
- `GET /api/expenses/group/{groupId}/balances` - Get member balances
- `GET /api/expenses/group/{groupId}/settlements` - Get settlement suggestions

## Project Structure

```
frontend/
├── src/
│   ├── components/
│   │   ├── GroupsList.tsx       # Groups sidebar
│   │   ├── GroupDetails.tsx     # Main group view with tabs
│   │   ├── MembersList.tsx      # Members management
│   │   ├── ExpensesList.tsx     # Expenses tracking
│   │   └── BalancesView.tsx     # Balances & settlements
│   ├── api.ts                   # API client functions
│   ├── App.tsx                  # Root component
│   ├── main.tsx                 # Entry point
│   └── style.css                # Tailwind imports
├── index.html
├── package.json
├── tsconfig.json
├── tailwind.config.js
└── vite.config.ts
```

## Usage

1. **Create a Group**: Click "+ New" in the Groups panel
2. **Add Members**: Select a group and add members in the Members tab
3. **Add Expenses**: Go to Expenses tab and record who paid what
4. **Check Balances**: View the Balances tab to see:
   - Individual member balances (positive = gets back, negative = owes)
   - Smart settlement suggestions (minimum transactions to settle)

## Design Philosophy

- **Clean & Modern**: Gradient backgrounds, smooth transitions, card-based layout
- **Intuitive**: Clear visual hierarchy with emoji icons
- **Responsive**: Works on desktop and mobile devices
- **User-Friendly**: Inline forms, confirmation dialogs, helpful empty states
