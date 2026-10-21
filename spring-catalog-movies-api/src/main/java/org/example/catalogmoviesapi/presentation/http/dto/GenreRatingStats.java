package org.example.catalogmoviesapi.presentation.http.dto;

import java.math.BigDecimal;
import java.util.UUID;

public record GenreRatingStats(
        UUID genreId,
        String genreName,
        BigDecimal avgRating
) {
}
