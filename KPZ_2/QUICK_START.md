# Швидкий старт - Entity Framework для Telemedicine Platform

## Крок 1: Встановлення залежностей

Пакети вже додані до `.csproj` файлу. Якщо потрібно встановити:

```bash
dotnet restore
```

## Крок 2: Створення бази даних (Code First)

### Варіант A: Використання скрипта

**Windows:**
```cmd
cd scripts
create_migration.bat InitialCreate
```

**Linux/Mac:**
```bash
cd scripts
./create_migration.sh InitialCreate
```

### Варіант B: Ручне виконання команд

```bash
# Створення початкової міграції
dotnet ef migrations add InitialCreate --project KPZ_2 --startup-project KPZ_2 --context TelemedicineDbContext

# Застосування міграції
dotnet ef database update --project KPZ_2 --startup-project KPZ_2 --context TelemedicineDbContext
```

## Крок 3: Створення міграції після зміни моделі

Після додавання полів `Bio` та `CreatedAt`:

```bash
# Windows
scripts\create_migration.bat AddBioAndCreatedAtToDoctor

# Linux/Mac
./scripts/create_migration.sh AddBioAndCreatedAtToDoctor
```

Або вручну:
```bash
dotnet ef migrations add AddBioAndCreatedAtToDoctor --project KPZ_2 --startup-project KPZ_2 --context TelemedicineDbContext
dotnet ef database update --project KPZ_2 --startup-project KPZ_2 --context TelemedicineDbContext
```

## Крок 4: Використання CRUD операцій

### Code First підхід

```csharp
using MedTeleHelp.WPF.Services;
using MedTeleHelp.WPF.Models;
using MedTeleHelp.WPF.Models.Enums;

var service = new CodeFirstService();

// CREATE
var doctor = new Doctor
{
    Id = Guid.NewGuid(),
    FullName = "Др. Іван Петренко",
    Specialization = "Кардіолог",
    Rating = 4.8,
    ConsultationFee = 1500.00m,
    PhotoUrl = "https://example.com/doctor1.jpg"
};
await service.CreateDoctorAsync(doctor);

// READ
var doctors = await service.GetAllDoctorsAsync();
var doctorById = await service.GetDoctorByIdAsync(doctor.Id);

// UPDATE
doctor.Rating = 4.9;
await service.UpdateDoctorAsync(doctor);

// DELETE
await service.DeleteDoctorAsync(doctor.Id);

service.Dispose();
```

### DB First підхід

```csharp
var service = new DbFirstService();
// Аналогічні операції...
service.Dispose();
```

## Крок 5: Перегляд SQL запитів через SQL Profiler

1. Відкрийте **SQL Server Management Studio**
2. **Tools** → **SQL Server Profiler**
3. Створіть новий Trace
4. Підключіться до `(localdb)\mssqllocaldb`
5. Налаштуйте фільтри:
   - DatabaseName: `TelemedicineCodeFirst` або `TelemedicineDbFirst`
6. Запустіть Trace
7. Виконайте CRUD операції у додатку
8. Спостерігайте SQL запити

Детальні інструкції: `SQL_PROFILER_GUIDE.md`

## Крок 6: Тестування

Використайте приклади з `Examples/CrudExamples.cs`:

```csharp
using MedTeleHelp.WPF.Examples;

// Code First
await CrudExamples.CodeFirstExample();

// DB First
await CrudExamples.DbFirstExample();
```

## Структура файлів

- `Data/Entities/` - моделі Entity Framework
- `Data/CodeFirst/` - DbContext для Code First
- `Data/DbFirst/` - DbContext для DB First
- `Services/CodeFirstService.cs` - CRUD для Code First
- `Services/DbFirstService.cs` - CRUD для DB First
- `Examples/CrudExamples.cs` - приклади використання

## Troubleshooting

### Помилка: "Cannot find design-time services"

Встановіть глобальний tool:
```bash
dotnet tool install --global dotnet-ef
```

### Помилка підключення до бази даних

Перевірте connection string у `TelemedicineDbContext.cs` та `TelemedicineDbFirstContext.cs`

### Міграція не застосовується

Перевірте, чи існує база даних та чи правильний connection string.

## Документація

- Повна документація: `README_ENTITY_FRAMEWORK.md`
- Інструкції з міграцій: `Data/Migrations/README_MIGRATIONS.md`
- SQL Profiler: `SQL_PROFILER_GUIDE.md`
