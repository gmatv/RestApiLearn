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
    public class AuthorsController
    {
        private readonly ILibraryRepository _libraryRepository;
        private readonly IMapper _mapper;

        public AuthorsController(ILibraryRepository libraryRepository, IMapper mapper)
        {
            _libraryRepository = libraryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<AuthorDto> GetAuthors()
        {

            //_mapper.Map<>()
            var authorDtos = _libraryRepository
                .GetAuthors()
                .Select(a => _mapper.Map<AuthorDto>(a))
                .ToList();

            return authorDtos;
        }
    }
}
