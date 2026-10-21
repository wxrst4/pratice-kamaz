package org.example.catalogmoviesapi.infrastructure.repository;

import org.example.catalogmoviesapi.domain.entity.Movie;
import org.example.catalogmoviesapi.presentation.http.dto.GenreRatingStats;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.UUID;

@Repository
public interface MovieRepository extends JpaRepository<Movie, UUID> {

    @Query("""
            select m FROM Movie m
            join fetch m.genre g
            where g.id = :genreId
            """)
    List<Movie> findAllByGenreId(UUID genreId);

    @Query("""
            select m from Movie m
            order by m.rating desc, m.id asc
            limit 5
            """)
    List<Movie> findTop5ByOrderByRatingDescIdAsc();

    @Query(value = """
            select
                m.genre_id as genre_id,
                g.name as genre_name,
                round(avg(m.rating), 2) as avg_rating
            from catalog_movies.Movies m
            join catalog_movies.genres g on m.genre_id = g.id
            group by m.genre_id, g.name
            """, nativeQuery = true)
    List<GenreRatingStats> findAllAvgRatingGroupByGenre();

    @Query(value = """
                    select * from catalog_movies.Movies m
                    where m.title ilike %:query% or m.director ilike %:query%
            """, nativeQuery = true)
    List<Movie> findByTitleContainingIgnoreCaseOrDirectorContainingIgnoreCase(String query);
}
