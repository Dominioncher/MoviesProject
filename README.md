# MoviesProject

### Компоненты решения:
1. OracleDB + Minio
2. ASP.NET Core API
3. WinForms Client
4. ETL Piplenes


### Поднятие проекта локально
```
cd Docker
docker-compose up -d
```

### Сброс данных
```
docker-compose down -v
```

### Пересборка моделей EF
```
Scaffold-DbContext "User Id=video_rent;Password=video_rent;Data Source=localhost:1521/XEPDB1;" Oracle.EntityFrameworkCore -OutputDir DB
```
