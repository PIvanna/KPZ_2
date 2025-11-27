# Інструкції з міграцій Entity Framework Code First

## Створення початкової міграції

1. Відкрийте термінал/командний рядок у кореневій папці проєкту (де знаходиться `.csproj` файл)

2. Створіть початкову міграцію:
```bash
dotnet ef migrations add InitialCreate --project KPZ_2 --startup-project KPZ_2 --context TelemedicineDbContext
```

3. Застосуйте міграцію до бази даних:
```bash
dotnet ef database update --project KPZ_2 --startup-project KPZ_2 --context TelemedicineDbContext
```

## Створення міграції після зміни моделі

Після додавання нових полів `Bio` та `CreatedAt` до моделі `DoctorEntity`:

1. Створіть нову міграцію:
```bash
dotnet ef migrations add AddBioAndCreatedAtToDoctor --project KPZ_2 --startup-project KPZ_2 --context TelemedicineDbContext
```

2. Перевірте згенерований код міграції у папці `Migrations/`

3. Застосуйте міграцію:
```bash
dotnet ef database update --project KPZ_2 --startup-project KPZ_2 --context TelemedicineDbContext
```

## Відкат міграції

Якщо потрібно відкатити останню міграцію:
```bash
dotnet ef database update PreviousMigrationName --project KPZ_2 --startup-project KPZ_2 --context TelemedicineDbContext
```

## Видалення останньої міграції (якщо ще не застосована)

```bash
dotnet ef migrations remove --project KPZ_2 --startup-project KPZ_2 --context TelemedicineDbContext
```

## Перегляд SQL скрипту міграції

Для перегляду SQL без застосування:
```bash
dotnet ef migrations script --project KPZ_2 --startup-project KPZ_2 --context TelemedicineDbContext
```

## Важливо

- Перед створенням міграції переконайтеся, що база даних створена або connection string правильний
- Міграції зберігаються у папці `Migrations/`
- Завжди перевіряйте згенерований код міграції перед застосуванням
