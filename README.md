# Каталог фильмов

Проекты для ведения каталога фильмов: добавление и удаление записей, выставление оценок, поиск и статистика.

## Проекты

| Папка | Описание | Технологии |
| --- | --- | --- |
| [spring-catalog-movies-api](spring-catalog-movies-api/README.md) | REST API с хранением данных в PostgreSQL | Java 25, Spring Boot, Gradle, Docker |
| [csharp-catalog-movies-console](csharp-catalog-movies-console/README.md) | Консольное приложение с сохранением каталога в JSON | C#, .NET 10 |

## Запуск

- **Spring API:** нужен Docker. Подготовка настроек и команда запуска — в [README проекта](spring-catalog-movies-api/README.md#запуск). Для проверки ручек используйте [request.http](spring-catalog-movies-api/request.http).
- **Консольное приложение:** нужен .NET 10 SDK. Подготовка файла каталога и запуск — в [README проекта](csharp-catalog-movies-console/README.md#запуск).

Команды из инструкций выполняются внутри папки соответствующего проекта.
