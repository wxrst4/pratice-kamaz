create table if not exists catalog_movies.movies
(
    id       uuid default gen_random_uuid() primary key,
    title    varchar(255)   not null,
    director varchar(255)   not null,
    year     bigint         not null check ( year >= 1900 and year <= 2025 ),
    genre_id uuid           not null references catalog_movies.genres (id) on delete restrict,
    rating   numeric(12, 3) not null check ( rating >= 0.0 and rating <= 10.0 )
);

create index if not exists idx_movies_title
    on catalog_movies.movies (title);

create index if not exists idx_movies_genre_id
    on catalog_movies.movies (genre_id);