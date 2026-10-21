package org.example.catalogmoviesapi.application.service;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.example.catalogmoviesapi.application.exceptions.GenreNotFoundException;
import org.example.catalogmoviesapi.application.exceptions.MovieNotFoundException;
import org.example.catalogmoviesapi.domain.entity.Genre;
import org.example.catalogmoviesapi.domain.entity.Movie;
import org.example.catalogmoviesapi.infrastructure.repository.GenreRepository;
import org.example.catalogmoviesapi.infrastructure.repository.MovieRepository;
import org.example.catalogmoviesapi.presentation.http.dto.CreateMovieRequest;
import org.example.catalogmoviesapi.presentation.http.dto.GenreRatingStats;
import org.springframework.data.domain.PageRequest;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.util.List;
import java.util.UUID;

@Slf4j
@Service
@RequiredArgsConstructor
public class MovieService {

    private final MovieRepository movieRepository;
    private final GenreRepository genreRepository;

    @Transactional
    public Movie createMovie(CreateMovieRequest request) {
        Genre genre = genreRepository
                .findById(request.genreId())
                .orElseThrow(GenreNotFoundException::new);

        Movie movie = new Movie(
                request.title(),
                request.director(),
                request.year(),
                genre,
                request.rating()
        );

        return movieRepository.save(movie);
    }

    public List<Movie> findAllMovies(int page, int size) {
        PageRequest pageRequest = PageRequest.of(page, size);
        return movieRepository.findAll(pageRequest).stream().toList();
    }

    @Transactional
    public void deleteMovieById(UUID id) {
        movieRepository.deleteById(id);
    }

    @Transactional
    public Movie setRatingMovieById(UUID movieId, BigDecimal rating) {
        Movie movie = movieRepository
                .findById(movieId)
                .orElseThrow(() -> new MovieNotFoundException(movieId));

        movie.setRating(rating);

        return movie;
    }

    public List<Movie> findByTitleContainingIgnoreCaseOrDirectorContainingIgnoreCase(
            String query
    ) {
        if (query == null || query.trim().isEmpty()) {
            throw new IllegalArgumentException("Title or director cannot be null or empty");
        }

        return movieRepository.findByTitleContainingIgnoreCaseOrDirectorContainingIgnoreCase(query);
    }

    public List<Movie> findMovieByGenreId(UUID genreId) {
        return movieRepository.findAllByGenreId(genreId);
    }

    public List<Movie> findTop5ByOrderByRatingDescIdAsc() {
        return movieRepository.findTop5ByOrderByRatingDescIdAsc();
    }

    public List<GenreRatingStats> findAllAvgRatingGroupByGenre() {
        return movieRepository.findAllAvgRatingGroupByGenre();
    }
}
