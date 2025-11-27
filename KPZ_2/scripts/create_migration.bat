@echo off
REM Скрипт для створення та застосування міграції Entity Framework Code First (Windows)

echo === Створення міграції Entity Framework ===
echo.

REM Перевірка чи вказано ім'я міграції
if "%1"=="" (
    echo Використання: create_migration.bat ^<MigrationName^>
    echo Приклад: create_migration.bat InitialCreate
    echo Приклад: create_migration.bat AddBioAndCreatedAtToDoctor
    exit /b 1
)

set MIGRATION_NAME=%1

echo Створення міграції: %MIGRATION_NAME%
dotnet ef migrations add %MIGRATION_NAME% --project KPZ_2 --startup-project KPZ_2 --context TelemedicineDbContext

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Міграцію успішно створено!
    echo.
    echo Застосування міграції до бази даних...
    dotnet ef database update --project KPZ_2 --startup-project KPZ_2 --context TelemedicineDbContext
    
    if %ERRORLEVEL% EQU 0 (
        echo.
        echo Міграцію успішно застосовано!
    ) else (
        echo.
        echo Помилка при застосуванні міграції!
    )
) else (
    echo.
    echo Помилка при створенні міграції!
)
