package org.example.catalogmoviesapi.domain.entity;

import jakarta.persistence.*;
import lombok.Getter;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.time.Instant;
import java.util.UUID;

@Getter
@Entity
@NoArgsConstructor
@Table(name = "movies", schema = "catalog_movies")
public class Movie {

    @Id
    @GeneratedValue(strategy = GenerationType.UUID)
    private UUID id;

    private String title;

    private String director;

    private Long year;

    @ManyToOne(fetch = FetchType.LAZY)
    private Genre genre;

    private BigDecimal rating;

    public Movie(String title, String director, Long year, Genre genre, BigDecimal rating) {
        this.title = title;
        this.director = director;
        this.year = year;
        this.genre = genre;
        this.rating = rating;
    }

    public void setRating(BigDecimal rating) {
        if (
                rating == null
                || rating.compareTo(BigDecimal.ZERO) < 0
                || rating.compareTo(BigDecimal.TEN) > 0
        ) {
            throw new IllegalArgumentException("Invalid rating: " + rating);
        }

        this.rating = rating;
    }
}
