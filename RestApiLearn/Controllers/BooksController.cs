﻿using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestApiLearn.Dto;
using RestApiLearn.Entities;
using RestApiLearn.Services;

namespace RestApiLearn.Controllers
{
    [Route("api/authors/{authorId}/books")]
    public class BooksController : ControllerBase
    {
        private readonly ILibraryRepository _libraryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BooksController> _logger;

        public BooksController(ILibraryRepository libraryRepository, IMapper mapper, ILogger<BooksController> logger)
        {
            _libraryRepository = libraryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetBooksForAuthor(Guid authorId)
        {
            if (!_libraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var booksForAuthor = _mapper.Map<IEnumerable<BookDto>>(_libraryRepository.GetBooksForAuthor(authorId));

            return Ok(booksForAuthor);
        }

        [HttpGet("{id}")]
        public IActionResult GetBookForAuthor(Guid authorId, Guid id)
        {
            if (!_libraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var bookForAuthor = _libraryRepository.GetBookForAuthor(authorId, id);
            if (bookForAuthor == null)
            {
                return NotFound();
            }

            var bookForAuthorDto = _mapper.Map<BookDto>(_libraryRepository.GetBookForAuthor(authorId, id));

            return Ok(bookForAuthorDto);
        }

        [HttpPost]
        public IActionResult AddBookForAuthor(Guid authorId, [FromBody] CreateBookDto createBookDto)
        {
            if (createBookDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var book = new Book
            {
                Id = Guid.NewGuid(),
                AuthorId = authorId,
                Title = createBookDto.Title,
                Description = createBookDto.Description
            };
            _libraryRepository.AddBookForAuthor(authorId, book);
            _libraryRepository.Save();
            var bookForAuthorDto = _mapper.Map<BookDto>(book);
            return CreatedAtAction("GetBookForAuthor", new {authorId, bookId = book.Id}, bookForAuthorDto);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBookForAuthor(Guid authorId, Guid id)
        {
            var book = _libraryRepository.GetBookForAuthor(authorId, id);
            if (book == null)
            {
                return NotFound();
            }

            _libraryRepository.DeleteBook(book);
            _libraryRepository.Save();
            
            _logger.LogInformation("Book {id} for author {authorId} was deleted", id, authorId);

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBookForAuthor(Guid authorId, Guid id, [FromBody] UpdateBookDto updateBookDto)
        {
            if (!_libraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var book = _libraryRepository.GetBookForAuthor(authorId, id);
            if (book == null)
            {
                book = new Book
                {
                    Id = id,
                    AuthorId = authorId,
                    Title = updateBookDto.Title,
                    Description = updateBookDto.Description
                };
                _libraryRepository.AddBookForAuthor(authorId, book);
                _libraryRepository.Save();
                var bookForAuthorDto = _mapper.Map<BookDto>(book);

                return CreatedAtAction("GetBookForAuthor", new { authorId, bookId = book.Id }, bookForAuthorDto);
            }
            else
            {
                _mapper.Map(updateBookDto, book);
                _libraryRepository.Save();

                return NoContent();
            }
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateBookForAuthor(Guid authorId, Guid id, [FromBody] JsonPatchDocument<UpdateBookDto> patchBook)
        {
            var book = _libraryRepository.GetBookForAuthor(authorId, id);

            if (book == null)
            {
                return NotFound();
            }

            var updateBookDto = _mapper.Map<UpdateBookDto>(book);

            patchBook.ApplyTo(updateBookDto, ModelState);

            TryValidateModel(updateBookDto);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(updateBookDto, book);

            _libraryRepository.Save();

            return NoContent();
        }
    }
}
