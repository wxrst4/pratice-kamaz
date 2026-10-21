insert into catalog_movies.genres (name)
values ('Фантастика'),
       ('Драма'),
       ('Комедия'),
       ('Боевик'),
       ('Триллер'),
       ('Ужасы'),
       ('Мелодрама'),
       ('Приключения'),
       ('Мультфильм'),
       ('Документальный')
on conflict (name) do nothing;
