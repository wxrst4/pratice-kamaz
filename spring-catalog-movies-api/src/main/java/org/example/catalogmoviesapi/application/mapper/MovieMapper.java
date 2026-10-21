package org.example.catalogmoviesapi.application.mapper;

import org.example.catalogmoviesapi.domain.entity.Genre;
import org.example.catalogmoviesapi.domain.entity.Movie;
import org.example.catalogmoviesapi.presentation.http.dto.CreateMovieRequest;
import org.example.catalogmoviesapi.presentation.http.dto.MovieResponse;
import org.springframework.stereotype.Component;

import java.util.List;

@Component
public class MovieMapper {

    public MovieResponse toResponse(Movie entity) {
        return new MovieResponse(
                entity.getId(),
                entity.getTitle(),
                entity.getDirector(),
                entity.getYear(),
                entity.getGenre().getId(),
                entity.getRating()
        );
    }

    public List<MovieResponse> toResponse(List<Movie> movies) {
        return movies.stream()
                .map(this::toResponse)
                .toList();
    }
}
