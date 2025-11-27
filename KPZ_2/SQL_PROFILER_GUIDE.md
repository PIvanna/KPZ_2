# Інструкція з використання SQL Profiler для перегляду SQL запитів Entity Framework

## Що таке SQL Profiler?

SQL Profiler - це інструмент у SQL Server Management Studio (SSMS), який дозволяє відстежувати та аналізувати SQL запити, що виконуються на сервері бази даних.

## Як відкрити SQL Profiler

1. Відкрийте **SQL Server Management Studio (SSMS)**
2. Підключіться до вашого SQL Server (наприклад, `(localdb)\mssqllocaldb`)
3. Перейдіть до меню: **Tools** → **SQL Server Profiler**
   - Або натисніть `Alt + T` → `P`

## Налаштування SQL Profiler

### Крок 1: Створення нового Trace

1. У SQL Profiler натисніть **File** → **New Trace**
2. Введіть дані для підключення:
   - **Server type**: Database Engine
   - **Server name**: `(localdb)\mssqllocaldb` (або ваш сервер)
   - **Authentication**: Windows Authentication

### Крок 2: Налаштування фільтрів

1. Перейдіть на вкладку **Events Selection**
2. Виберіть потрібні події:
   - ✅ **Stored Procedures** → **RPC:Completed**
   - ✅ **TSQL** → **SQL:BatchCompleted**
   - ✅ **TSQL** → **SQL:StmtCompleted**

3. Натисніть **Column Filters** для додаткових налаштувань:
   - **DatabaseName** → **Like** → `TelemedicineCodeFirst` або `TelemedicineDbFirst`
   - **Duration** → **Greater than or equal** → `0` (щоб бачити всі запити)

### Крок 3: Запуск Trace

1. Натисніть **Run** для запуску профілювання
2. SQL Profiler почне відстежувати всі SQL запити

## Що ви побачите при виконанні CRUD операцій

### CREATE операція (INSERT)

При виклику `CreateDoctorAsync()` ви побачите:
```sql
INSERT INTO [Doctors] ([Id], [FullName], [Specialization], [Rating], [ConsultationFee], [PhotoUrl], [Bio], [CreatedAt])
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7)
```

### READ операція (SELECT)

При виклику `GetAllDoctorsAsync()` ви побачите:
```sql
SELECT [d].[Id], [d].[FullName], [d].[Specialization], [d].[Rating], 
       [d].[ConsultationFee], [d].[PhotoUrl], [d].[Bio], [d].[CreatedAt]
FROM [Doctors] AS [d]
```

При виклику `GetAppointmentsByDoctorIdAsync()` з Include:
```sql
SELECT [a].[Id], [a].[DoctorId], [a].[DoctorFullName], [a].[AppointmentTime], [a].[Status],
       [d].[Id], [d].[FullName], [d].[Specialization], [d].[Rating], ...
FROM [Appointments] AS [a]
LEFT JOIN [Doctors] AS [d] ON [a].[DoctorId] = [d].[Id]
WHERE [a].[DoctorId] = @p0
```

### UPDATE операція

При виклику `UpdateDoctorAsync()` ви побачите:
```sql
UPDATE [Doctors] 
SET [FullName] = @p0, [Specialization] = @p1, [Rating] = @p2, 
    [ConsultationFee] = @p3, [PhotoUrl] = @p4, [Bio] = @p5, [CreatedAt] = @p6
WHERE [Id] = @p7
```

### DELETE операція

При виклику `DeleteDoctorAsync()` ви побачите:
```sql
DELETE FROM [Doctors]
WHERE [Id] = @p0
```

## Міграції та SQL Profiler

При застосуванні міграції через `dotnet ef database update` ви побачите всі SQL команди, що виконуються:

```sql
IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'...')
BEGIN
    ALTER TABLE [Doctors] ADD [Bio] nvarchar(500) NULL;
    ALTER TABLE [Doctors] ADD [CreatedAt] datetime2 NULL;
    ...
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'...', N'9.0.0');
END
```

## Поради для ефективного використання

1. **Фільтрація за тривалістю**: Додайте фільтр `Duration >= 100` (мілісекунди), щоб бачити тільки повільні запити
2. **Збереження Trace**: Можна зберегти результат у файл для подальшого аналізу
3. **Групування**: Використовуйте **Group By** для аналізу найчастіших запитів
4. **Вимкнення шуму**: Фільтруйте системні запити, додавши фільтр `TextData NOT LIKE '%sys%'`

## Альтернативні способи перегляду SQL

### 1. EnableSensitiveDataLogging у DbContext

У `TelemedicineDbContext` вже додано `EnableSensitiveDataLogging()`, що дозволяє бачити параметри запитів у логах.

### 2. Логування через ILogger

Можна додати логування SQL запитів через `ILogger`:

```csharp
optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
```

### 3. SQL Server Extended Events

Для production середовища краще використовувати Extended Events замість Profiler.

## Приклад використання

1. Запустіть SQL Profiler
2. Запустіть ваш додаток
3. Виконайте CRUD операції через `CrudExamples.CodeFirstExample()` або `CrudExamples.DbFirstExample()`
4. Спостерігайте за SQL запитами в реальному часі у SQL Profiler

## Важливо

- SQL Profiler може впливати на продуктивність, тому не використовуйте його на production серверах
- Для production використовуйте Extended Events або Application Insights
- Завжди зупиняйте Trace після завершення аналізу
