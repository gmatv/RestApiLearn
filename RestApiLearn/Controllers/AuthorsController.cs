using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestApiLearn.Dto;
using RestApiLearn.Entities;
using RestApiLearn.Services;
using RestApiLearn.Sorting;
using X.PagedList;

namespace RestApiLearn.Controllers
{
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly ILibraryRepository _libraryRepository;
        private readonly IMapper _mapper;

        public AuthorsController(ILibraryRepository libraryRepository, IMapper mapper)
        {
            _libraryRepository = libraryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetAuthors(Pagination pagination, AuthorFilter authorFilter, string orderBy = "Name")
        {
            var orderByEntities = OrderMapper.MapTo<Author>(orderBy);

            IPagedList<Author> authors = _libraryRepository.GetAuthors(pagination, authorFilter, orderByEntities);
            var authorDtos = _mapper.Map<IEnumerable<AuthorDto>>(authors);

            var result = new
            {
                meta = authors.GetMetaData(),
                value = authorDtos
            };

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<AuthorDto> GetAuthor(Guid id)
        {
            var author = _libraryRepository.GetAuthor(id);
            if (author == null)
            {
                return NotFound();
            }

            var authorDto = _mapper.Map<AuthorDto>(author);

            return Ok(authorDto);
        }

        [HttpPost]
        public IActionResult CreateAuthor([FromBody] CreateAuthorDto createAuthorDto)
        {
            if (createAuthorDto == null)
            {
                return BadRequest();
            }

            var author = _mapper.Map<Author>(createAuthorDto);

            _libraryRepository.AddAuthor(author);
            _libraryRepository.Save();

            var authorDto = _mapper.Map<AuthorDto>(author);

            return CreatedAtAction("GetAuthor", new {id = author.Id}, authorDto);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(Guid id)
        {
            var author = _libraryRepository.GetAuthor(id);
            if (author == null)
            {
                return NotFound();
            }

            _libraryRepository.DeleteAuthor(author);
            _libraryRepository.Save();

            return NoContent();
        }
    }
}
