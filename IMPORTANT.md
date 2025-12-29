# Важные инструкции по запуску проекта

## Требования

1. .NET 8 SDK
2. SQL Server (или SQL Server LocalDB)
3. Visual Studio 2022 или VS Code

## Настройка базы данных

1. Обновите строку подключения в `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ShopDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
   }
   ```

2. При первом запуске база данных создастся автоматически (EnsureCreated).

3. Данные будут автоматически заполнены через SeedData:
   - Роли: Admin, Manager, Customer
   - Администратор: admin@shop.com / Admin123!
   - Тестовые бренды, категории и товары

## Запуск проекта

1. Откройте проект в Visual Studio
2. Нажмите F5 или запустите через терминал:
   ```bash
   dotnet run
   ```
3. Откройте браузер по адресу https://localhost:5001

## Структура ролей

- **Admin** - Полный доступ ко всем функциям
- **Manager** - Управление заказами, просмотр статистики
- **Customer** - Покупки, корзина, личный кабинет

## Основные функции

- ✅ Каталог товаров с фильтрацией
- ✅ Корзина покупок
- ✅ Оформление заказов
- ✅ CRUD для товаров, брендов, категорий
- ✅ Экспорт/импорт Excel
- ✅ Система авторизации с ролями
- ✅ Личный кабинет пользователя

## Примечания

- Все изображения товаров должны быть размещены в папке `wwwroot/images/`
- Для работы с Excel используется библиотека EPPlus
- Интерфейс полностью на русском языке

