using System.Collections.Generic;
using System.Linq;
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

        public AuthorsController(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        [HttpGet]
        public IEnumerable<AuthorDto> GetAuthors()
        {
            var authorDtos = _libraryRepository
                .GetAuthors()
                .Select(a =>
                    new AuthorDto
                    {
                        Id = a.Id,
                        Name = $"{a.FirstName} {a.LastName}",
                        Genre = a.Genre,
                        Age = a.DateOfBirth.GetCurrentAge()
                    })
                .ToList();

            return authorDtos;
        }
    }
}
