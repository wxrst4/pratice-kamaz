package org.example.catalogmoviesapi.presentation.http.controller;

import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import org.example.catalogmoviesapi.application.mapper.MovieMapper;
import org.example.catalogmoviesapi.application.service.MovieService;
import org.example.catalogmoviesapi.domain.entity.Movie;
import org.example.catalogmoviesapi.presentation.http.dto.CreateMovieRequest;
import org.example.catalogmoviesapi.presentation.http.dto.GenreRatingStats;
import org.example.catalogmoviesapi.presentation.http.dto.MovieResponse;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.math.BigDecimal;
import java.util.List;
import java.util.UUID;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/v1/movies")
public class MovieController {

    private final MovieService movieService;
    private final MovieMapper movieMapper;

    @PostMapping
    public MovieResponse createMovie(
            @Valid @RequestBody CreateMovieRequest request
    ) {
        Movie movie = movieService.createMovie(request);
        return movieMapper.toResponse(movie);
    }

    @GetMapping
    public List<MovieResponse> findAllMovies(
            @RequestParam(required = false, defaultValue = "0") int page,
            @RequestParam(required = false, defaultValue = "10") int size
    ) {
        List<Movie> movies = movieService.findAllMovies(page, size);
        return movieMapper.toResponse(movies);
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteMovie(
            @PathVariable UUID id
    ) {
        movieService.deleteMovieById(id);
        return ResponseEntity.noContent().build();
    }

    @PatchMapping("/{id}/rating")
    public MovieResponse setRatingById(
            @PathVariable UUID id,
            @RequestParam BigDecimal rating
    ) {
        Movie movie = movieService.setRatingMovieById(id, rating);
        return movieMapper.toResponse(movie);
    }

    @GetMapping("/search")
    public List<MovieResponse> searchMovieByNameOrDirector(
            @RequestParam(required = false) String query
    ) {
        List<Movie> movie = movieService
                .findByTitleContainingIgnoreCaseOrDirectorContainingIgnoreCase(query);
        return movieMapper.toResponse(movie);
    }

    @GetMapping("/genre/{id}")
    public List<MovieResponse> findAllByGenreId(@PathVariable UUID id) {
        List<Movie> movies = movieService.findMovieByGenreId(id);
        return movieMapper.toResponse(movies);
    }

    @GetMapping("/top5-by-rating")
    public List<MovieResponse> findTop5ByOrderByRatingDescIdAsc() {
        List<Movie> movies = movieService.findTop5ByOrderByRatingDescIdAsc();
        return movieMapper.toResponse(movies);
    }

    @GetMapping("/avg-rating-by-genre")
    public List<GenreRatingStats> findAllAvgRatingByGenre() {
        return movieService.findAllAvgRatingGroupByGenre();
    }
}
