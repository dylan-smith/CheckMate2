# CheckMate2

A checklist management app with:
- **Backend:** ASP.NET Core Web API + EF Core + SQL Server
- **Frontend:** React + TypeScript (Vite)

## Features
- Create checklist
- Edit checklist
- Delete checklist
- Enforces unique checklist names

## Run backend
```bash
cd /home/runner/work/CheckMate2/CheckMate2/backend/CheckMate2.Api
dotnet run
```

API base URL (development): `http://localhost:5269`

## Run frontend
```bash
cd /home/runner/work/CheckMate2/CheckMate2/frontend
npm install
npm run dev
```

Set `VITE_API_BASE_URL` if your backend URL is different.
