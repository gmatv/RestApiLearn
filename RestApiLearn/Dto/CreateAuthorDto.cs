using System;
using System.Collections.Generic;

namespace RestApiLearn.Dto
{
    public class CreateAuthorDto
    {
        public CreateAuthorDto()
        {
            Books = new List<CreateBookDto>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTimeOffset DateOfBirth { get; set; }

        public string Genre { get; set; }

        public List<CreateBookDto> Books { get; set; }
    }
}
