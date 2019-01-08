using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public BooksController(ILibraryRepository libraryRepository, IMapper mapper)
        {
            _libraryRepository = libraryRepository;
            _mapper = mapper;
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

        [HttpGet("{bookId}")]
        public IActionResult GetBookForAuthor(Guid authorId, Guid bookId)
        {
            if (!_libraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var bookForAuthor = _libraryRepository.GetBookForAuthor(authorId, bookId);
            if (bookForAuthor == null)
            {
                return NotFound();
            }

            var bookForAuthorDto = _mapper.Map<BookDto>(_libraryRepository.GetBookForAuthor(authorId, bookId));

            return Ok(bookForAuthorDto);
        }

        [HttpPost]
        public IActionResult AddBookForAuthor(Guid authorId, [FromBody] CreateBookDto createBookDto)
        {
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
    }
}
