package org.example.catalogmoviesapi.application.exceptions;

public class GenreNotFoundException extends RuntimeException {
    public GenreNotFoundException() {
        super("Такого жанра не существует.");
    }
}
