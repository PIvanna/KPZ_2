# Реалізація Entity Framework для Telemedicine Platform

## Огляд

Цей проєкт реалізує роботу з базою даних для платформи телемедицини з використанням Entity Framework Core. Реалізовано два підходи: **DB First** та **Code First**.

## Структура проєкту

```
KPZ_2/
├── Data/
│   ├── Entities/              # Моделі Entity Framework
│   │   ├── DoctorEntity.cs
│   │   └── AppointmentEntity.cs
│   ├── CodeFirst/            # Code First підхід
│   │   └── TelemedicineDbContext.cs
│   ├── DbFirst/              # DB First підхід
│   │   └── TelemedicineDbFirstContext.cs
│   └── Migrations/           # Міграції для Code First
│       └── README_MIGRATIONS.md
├── Services/
│   ├── CodeFirstService.cs   # CRUD операції для Code First
│   ├── DbFirstService.cs     # CRUD операції для DB First
│   └── FileDataService.cs    # Старий сервіс (JSON файли)
├── Examples/
│   └── CrudExamples.cs       # Приклади використання
└── SQL_PROFILER_GUIDE.md     # Інструкція з SQL Profiler
```

## Підходи до роботи з базою даних

### 1. Code First підхід

**Code First** - підхід, де спочатку створюються моделі (класи C#), а потім з них генерується база даних.

**Переваги:**
- Повний контроль над моделлю
- Версіонування через міграції
- Легше рефакторити код

**Використання:**
```csharp
var service = new CodeFirstService();

// CREATE
var doctor = new Doctor { FullName = "Др. Іван Петренко", ... };
await service.CreateDoctorAsync(doctor);

// READ
var doctors = await service.GetAllDoctorsAsync();
var doctor = await service.GetDoctorByIdAsync(id);

// UPDATE
doctor.Rating = 4.9;
await service.UpdateDoctorAsync(doctor);

// DELETE
await service.DeleteDoctorAsync(id);
```

**База даних:** `TelemedicineCodeFirst`

### 2. DB First підхід

**DB First** - підхід, де спочатку існує база даних, а потім з неї генеруються моделі та DbContext.

**Переваги:**
- Працює з існуючою базою даних
- Швидкий старт з готовою БД
- Автоматична генерація моделей

**Генерація з існуючої БД:**
```bash
dotnet ef dbcontext scaffold "Server=(localdb)\mssqllocaldb;Database=TelemedicineDbFirst;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Data/DbFirst -c TelemedicineDbFirstContext --force
```

**Використання:**
```csharp
var service = new DbFirstService();

// Аналогічні CRUD операції
await service.CreateDoctorAsync(doctor);
await service.GetAllDoctorsAsync();
await service.UpdateDoctorAsync(doctor);
await service.DeleteDoctorAsync(id);
```

**База даних:** `TelemedicineDbFirst`

## CRUD операції

Обидва сервіси (`CodeFirstService` та `DbFirstService`) реалізують повний набір CRUD операцій:

### CREATE (Створення)
- `CreateDoctorAsync(Doctor doctor)` - створення лікаря
- `CreateAppointmentAsync(Appointment appointment)` - створення запису

### READ (Читання)
- `GetAllDoctorsAsync()` - отримання всіх лікарів
- `GetDoctorByIdAsync(Guid id)` - отримання лікаря за ID
- `GetAllAppointmentsAsync()` - отримання всіх записів
- `GetAppointmentByIdAsync(Guid id)` - отримання запису за ID
- `GetAppointmentsByDoctorIdAsync(Guid doctorId)` - отримання записів лікаря

### UPDATE (Оновлення)
- `UpdateDoctorAsync(Doctor doctor)` - оновлення лікаря
- `UpdateAppointmentAsync(Appointment appointment)` - оновлення запису

### DELETE (Видалення)
- `DeleteDoctorAsync(Guid id)` - видалення лікаря
- `DeleteAppointmentAsync(Guid id)` - видалення запису

## Міграції Code First

### Створення початкової міграції

```bash
dotnet ef migrations add InitialCreate --project KPZ_2 --startup-project KPZ_2 --context TelemedicineDbContext
dotnet ef database update --project KPZ_2 --startup-project KPZ_2 --context TelemedicineDbContext
```

### Створення міграції після зміни моделі

Після додавання полів `Bio` та `CreatedAt` до `DoctorEntity`:

```bash
dotnet ef migrations add AddBioAndCreatedAtToDoctor --project KPZ_2 --startup-project KPZ_2 --context TelemedicineDbContext
dotnet ef database update --project KPZ_2 --startup-project KPZ_2 --context TelemedicineDbContext
```

Детальні інструкції див. у `Data/Migrations/README_MIGRATIONS.md`

## SQL Profiler

Для перегляду SQL запитів, що генерує Entity Framework, використовуйте **SQL Profiler** з SQL Server Management Studio.

### Швидкий старт:

1. Відкрийте **SQL Server Management Studio**
2. **Tools** → **SQL Server Profiler**
3. Створіть новий Trace
4. Підключіться до `(localdb)\mssqllocaldb`
5. Запустіть Trace
6. Виконайте CRUD операції у додатку
7. Спостерігайте SQL запити в реальному часі

Детальні інструкції див. у `SQL_PROFILER_GUIDE.md`

## Приклади використання

Детальні приклади використання обох підходів знаходяться у файлі `Examples/CrudExamples.cs`:

```csharp
// Code First приклад
await CrudExamples.CodeFirstExample();

// DB First приклад
await CrudExamples.DbFirstExample();
```

## Моделі даних

### DoctorEntity
- `Id` (Guid) - унікальний ідентифікатор
- `FullName` (string, max 200) - повне ім'я
- `Specialization` (string, max 100) - спеціалізація
- `Rating` (double, decimal(3,2)) - рейтинг
- `ConsultationFee` (decimal, decimal(10,2)) - вартість консультації
- `PhotoUrl` (string, max 500, nullable) - URL фото
- `Bio` (string, max 500, nullable) - біографія (додано для міграції)
- `CreatedAt` (DateTime?, nullable) - дата створення (додано для міграції)

### AppointmentEntity
- `Id` (Guid) - унікальний ідентифікатор
- `DoctorId` (Guid) - ID лікаря (Foreign Key)
- `DoctorFullName` (string, max 200) - ім'я лікаря
- `AppointmentTime` (DateTime) - час запису
- `Status` (int) - статус запису (enum AppointmentStatus)

## Connection Strings

Connection strings налаштовані у DbContext класах:

- **Code First**: `Server=(localdb)\mssqllocaldb;Database=TelemedicineCodeFirst;...`
- **DB First**: `Server=(localdb)\mssqllocaldb;Database=TelemedicineDbFirst;...`

Для зміни connection string відредагуйте метод `OnConfiguring` у відповідному DbContext.

## Залежності

Проєкт використовує наступні NuGet пакети:
- `Microsoft.EntityFrameworkCore` (9.0.0)
- `Microsoft.EntityFrameworkCore.SqlServer` (9.0.0)
- `Microsoft.EntityFrameworkCore.Tools` (9.0.0)
- `Microsoft.EntityFrameworkCore.Design` (9.0.0)

## Важливі примітки

1. **EnableSensitiveDataLogging**: Увімкнено для перегляду параметрів SQL запитів (для розробки)
2. **Міграції**: Завжди перевіряйте згенерований код міграції перед застосуванням
3. **SQL Profiler**: Не використовуйте на production серверах через вплив на продуктивність
4. **Dispose**: Не забувайте викликати `Dispose()` на сервісах після використання

## Наступні кроки

1. Створіть початкову міграцію для Code First
2. Застосуйте міграцію до бази даних
3. Створіть базу даних для DB First (якщо потрібно)
4. Запустіть SQL Profiler
5. Виконайте приклади з `CrudExamples.cs`
6. Спостерігайте SQL запити у SQL Profiler
