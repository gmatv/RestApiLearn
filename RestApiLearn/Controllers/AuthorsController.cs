﻿using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestApiLearn.Dto;
using RestApiLearn.Services;

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
        public IEnumerable<AuthorDto> GetAuthors()
        {
            var authors = _libraryRepository.GetAuthors();
            var authorDtos = _mapper.Map<IEnumerable<AuthorDto>>(authors);

            return authorDtos;
        }

        [HttpGet("{id}")]
        public IActionResult GetAuthor(Guid id)
        {
            return BadRequest(new { error = "Error message" });

            var author = _libraryRepository.GetAuthor(id);
            if (author == null)
            {
                return NotFound();
            }

            var authorDto = _mapper.Map<AuthorDto>(author);

            return Ok(authorDto);
        }
    }
}