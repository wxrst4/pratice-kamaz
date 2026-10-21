# Каталог фильмов

REST API: добавление и удаление фильмов, изменение рейтинга, поиск по названию, режиссёру и жанру, топ-5 и средний рейтинг по жанрам.

**Стек:** Java 25, Spring Boot 4.1.1, Spring Web MVC, Spring Data JPA, PostgreSQL 16, Flyway, Bean Validation, Lombok, Gradle, Docker Compose.

## Запуск

Нужен работающий Docker с Compose. Все команды выполняются из корня проекта.

Создайте `docker/.env`, если его ещё нет:

```dotenv
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres
POSTGRES_DB=postgres
POSTGRES_PORT=5432
APP_PORT=8080
```

Запустите базу и приложение:

```sh
docker compose --env-file docker/.env -f docker/docker-compose.yml up --build -d
```

API: `http://localhost:8080/api/v1/movies`. Если порты заняты, измените `POSTGRES_PORT` / `APP_PORT`. Flyway автоматически создаёт таблицы и добавляет 10 жанров; фильмов в новой базе нет.

```sh
# Логи приложения
docker compose --env-file docker/.env -f docker/docker-compose.yml logs -f catalog-movies-api
# Остановка
docker compose --env-file docker/.env -f docker/docker-compose.yml down
```

Данные хранятся в именованном Docker-томе. `down` сохраняет данные, `down -v` удаляет их.

## Проверка API

Запросы и проверки ответов находятся в [request.http](request.http) для [IntelliJ IDEA HTTP Client](https://www.jetbrains.com/help/idea/http-client-in-product-code-editor.html).

1. Получите UUID жанра — отдельного API для жанров нет:

   ```sh
   docker compose --env-file docker/.env -f docker/docker-compose.yml exec catalog-movies-db sh -c 'psql -U "$POSTGRES_USER" -d "$POSTGRES_DB" -c "SELECT id, name FROM catalog_movies.genres ORDER BY name;"'
   ```

2. В начале `request.http` задайте `genreId` и при необходимости измените `baseUrl`.
3. Запустите первый запрос. ID созданного фильма сохранится автоматически для следующих запросов.
4. Выполняйте запросы кнопкой ▶ рядом с ними или через **Run All Requests**. В конце сценарий удаляет созданный фильм и проверяет ответ `404`.

Файл покрывает все ручки и проверки ошибок `400`, `404`, `405`. Повторное создание добавляет новый фильм.

## API

Базовый путь: `/api/v1/movies`. Тела запросов и ответов — JSON; для `PATCH` рейтинг передаётся в query-параметре.

| Метод | Путь относительно базового | Назначение |
| --- | --- | --- |
| `POST` | Базовый путь | Создать фильм; ответ `200` |
| `GET` | `?page=0&size=10` | Страница каталога, массив фильмов |
| `PATCH` | `/{id}/rating?rating=9.2` | Изменить рейтинг |
| `DELETE` | `/{id}` | Удалить фильм; ответ `204` |
| `GET` | `/search?query=Нолан` | Поиск по названию или режиссёру |
| `GET` | `/genre/{id}` | Фильмы по UUID жанра |
| `GET` | `/top5-by-rating` | Топ-5 по убыванию рейтинга |
| `GET` | `/avg-rating-by-genre` | Средний рейтинг по жанрам с фильмами |

Поиск работает по части строки без учёта регистра. Страницы нумеруются с `0`. Средний рейтинг округляется до двух знаков; оценки `0` участвуют в статистике.

При создании обязательны `title`, `director`, `year`, `genreId`, `rating`. Год: **1900–2025**, рейтинг: **0–10**, жанр должен существовать. База ограничивает `title` и `director` до 255 символов. `id` генерируется автоматически как UUID.

## Локальная разработка

Нужен **JDK 25**. Остановите API в Docker и запустите базу:

```sh
docker compose --env-file docker/.env -f docker/docker-compose.yml stop catalog-movies-api
docker compose --env-file docker/.env -f docker/docker-compose.yml up -d catalog-movies-db
```

Создайте `.env` в корне проекта с настройками этой базы:

```dotenv
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres
POSTGRES_URL=jdbc:postgresql://localhost:5432/postgres
```

Пользователь и пароль должны совпадать с `docker/.env`; URL содержит внешний порт и имя базы. Сборка: `./gradlew build`, запуск: `./gradlew bootRun`. В Windows используйте `.\gradlew.bat`. Локальный API слушает порт `8080`.

## Структура

Основной пакет: `src/main/java/org/example/catalogmoviesapi`.

| Каталог / файл | Ответственность |
| --- | --- |
| `presentation/http` | Контроллер и DTO запросов/ответов |
| `application` | Сервисы, маппинг, исключения |
| `domain/entity` | JPA-модели `Movie` и `Genre` |
| `infrastructure` | Репозитории и обработка HTTP-ошибок |
| `src/main/resources/application.yaml` | Настройки Spring |
| `src/main/resources/db/migration` | Схема БД и seed жанров |
| `docker/app/Dockerfile` | Сборка и образ приложения |
| `docker/docker-compose.yml` | Приложение, PostgreSQL и том данных |
| `request.http` | Ручная проверка API и проверки ответов |

Поток запроса: `controller → service → repository`. Таблицы `movies` и `genres` находятся в схеме `catalog_movies`; изменения схемы добавляются отдельными миграциями Flyway.
