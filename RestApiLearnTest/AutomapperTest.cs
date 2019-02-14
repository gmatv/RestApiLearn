using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using RestApiLearn.Dto;
using RestApiLearn.Entities;
using RestApiLearn.Helpers;
using Xunit;

namespace RestApiLearnTest
{
    public class AutomapperTest
    {
        public AutomapperTest()
        {
            Mapper.Reset();
            AutomapperConfig.Initialize();
        }

        [Fact]
        public void ConfigurationTest()
        {
            Mapper.AssertConfigurationIsValid();
        }

        [Fact]
        public void MapTest()
        {
            var authors = new List<Author>()
            {
                new Author() {LastName = "Last"}
            };

            var authorDtos = Mapper.Map<IEnumerable<AuthorDto>>(authors);

            authorDtos.First().Name.Should().Be(" Last");
        }
    }
}
