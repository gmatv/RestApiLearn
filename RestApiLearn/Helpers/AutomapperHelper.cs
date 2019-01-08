using AutoMapper;
using RestApiLearn.Dto;
using RestApiLearn.Entities;

namespace RestApiLearn.Helpers
{
    public static class AutomapperHelper
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Author, AuthorDto>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(a => $"{a.FirstName} {a.LastName}"))
                    .ForMember(dest => dest.Age, opt => opt.MapFrom(a => a.DateOfBirth.GetCurrentAge()));

                cfg.CreateMap<Book, BookDto>();

                cfg.CreateMap<CreateAuthorDto, Author>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore());

                cfg.CreateMap<CreateBookDto, Book>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.Author, opt => opt.Ignore())
                    .ForMember(dest => dest.AuthorId, opt => opt.Ignore());
            });
        }
    }
}
