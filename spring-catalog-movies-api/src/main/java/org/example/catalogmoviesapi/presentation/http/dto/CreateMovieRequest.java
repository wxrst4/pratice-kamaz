package org.example.catalogmoviesapi.presentation.http.dto;

import jakarta.validation.constraints.*;

import java.math.BigDecimal;
import java.util.UUID;

public record CreateMovieRequest(

        @NotBlank(message = "Название фильма не должно быть пустым.")
        @Size(message = "Название фильма не должно привышать длинну более чем 255 символов.")
        String title,

        @NotBlank(message = "Директор фильма не должно быть пустым.")
        @Size(message = "Режисер фильма не должно привышать длинну более чем 255 символов.")
        String director,

        @Min(1900)
        @Max(2025)
        @NotNull(message = "Год фильма не должен быть пустым.")
        Long year,

        @NotNull
        UUID genreId,

        @NotNull(message = "Райтинг фильма не должен быть пустым.")
        @DecimalMin("0.0")
        @DecimalMax("10.0")
        BigDecimal rating
) {
}
