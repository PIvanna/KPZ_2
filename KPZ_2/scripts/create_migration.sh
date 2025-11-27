#!/bin/bash

# Скрипт для створення та застосування міграції Entity Framework Code First

echo "=== Створення міграції Entity Framework ==="
echo ""

# Перевірка чи вказано ім'я міграції
if [ -z "$1" ]; then
    echo "Використання: ./create_migration.sh <MigrationName>"
    echo "Приклад: ./create_migration.sh InitialCreate"
    echo "Приклад: ./create_migration.sh AddBioAndCreatedAtToDoctor"
    exit 1
fi

MIGRATION_NAME=$1

echo "Створення міграції: $MIGRATION_NAME"
dotnet ef migrations add $MIGRATION_NAME --project KPZ_2 --startup-project KPZ_2 --context TelemedicineDbContext

if [ $? -eq 0 ]; then
    echo ""
    echo "Міграцію успішно створено!"
    echo ""
    echo "Застосування міграції до бази даних..."
    dotnet ef database update --project KPZ_2 --startup-project KPZ_2 --context TelemedicineDbContext
    
    if [ $? -eq 0 ]; then
        echo ""
        echo "Міграцію успішно застосовано!"
    else
        echo ""
        echo "Помилка при застосуванні міграції!"
    fi
else
    echo ""
    echo "Помилка при створенні міграції!"
fi
