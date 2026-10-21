create table if not exists catalog_movies.genres
(
    id   uuid default gen_random_uuid() primary key,
    name varchar(255) unique not null
);