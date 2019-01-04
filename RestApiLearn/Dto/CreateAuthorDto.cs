using System;

namespace RestApiLearn.Dto
{
    public class CreateAuthorDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTimeOffset DateOfBirth { get; set; }

        public string Genre { get; set; }
    }
}
