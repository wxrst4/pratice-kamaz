package org.example.catalogmoviesapi.presentation.http.dto;

import java.math.BigDecimal;
import java.util.UUID;

public record MovieResponse(
        UUID id,
        String title,
        String director,
        Long year,
        UUID genreId,
        BigDecimal rating
) {
}
