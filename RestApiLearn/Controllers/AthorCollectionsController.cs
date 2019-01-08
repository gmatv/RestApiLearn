using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestApiLearn.Dto;
using RestApiLearn.Entities;
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

        public IActionResult CreateAuthorCollection([FromBody] IEnumerable<CreateAuthorDto> authorDtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var authors = _mapper.Map<IEnumerable<Author>>(authorDtos);

            foreach (var author in authors)
            {
                _libraryRepository.AddAuthor(author);
            }

            _libraryRepository.Save();

            return Ok();
        }
    }
}
