using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestApiLearn.Dto;
using RestApiLearn.Entities;
using RestApiLearn.Helpers;
using RestApiLearn.Services;

namespace RestApiLearn.Controllers
{
    [Route("api/[controller]")]
    public class AuthorCollectionsController: ControllerBase
    {
        private readonly ILibraryRepository _libraryRepository;
        private readonly IMapper _mapper;

        public AuthorCollectionsController(ILibraryRepository libraryRepository, IMapper mapper)
        {
            _libraryRepository = libraryRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult CreateAuthorCollection([FromBody] IEnumerable<CreateAuthorDto> createAuthorDtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var authors = _mapper.Map<List<Author>>(createAuthorDtos);

            foreach (var author in authors)
            {
                _libraryRepository.AddAuthor(author);
            }

            _libraryRepository.Save();

            var ids = string.Join(",", authors.Select(a => a.Id));

            var authorDtos = _mapper.Map<IEnumerable<AuthorDto>>(authors);

            return CreatedAtAction("GetAuthorCollection", new { ids }, authorDtos);
        }

        [HttpGet("({ids})")]
        public IActionResult GetAuthorCollection([ModelBinder(typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var authors = _libraryRepository.GetAuthors(ids);

            if (authors.Count() != ids.Count())
            {
                return NotFound();
            }

            var authorDtos = _mapper.Map<IEnumerable<AuthorDto>>(authors);

            return Ok(authorDtos);
        }

        [HttpGet("({ids})")]
        public IActionResult DeleteAuthorCollection([ModelBinder(typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var authors = _libraryRepository.GetAuthors(ids);

            if (authors.Count() != ids.Count())
            {
                return NotFound();
            }

            foreach (var author in authors)
            {
                _libraryRepository.DeleteAuthor(author);
            }

            _libraryRepository.Save();

            return NoContent();
        }
    }
}
