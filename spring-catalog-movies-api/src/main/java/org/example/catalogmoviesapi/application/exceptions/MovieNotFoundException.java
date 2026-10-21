package org.example.catalogmoviesapi.application.exceptions;

import java.util.UUID;

public class MovieNotFoundException extends RuntimeException {
    public MovieNotFoundException(UUID id) {
        super("Фильма с таким id: " + id + " не существует.");
    }
}
