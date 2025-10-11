using AccountingService.Presentation.DTOs;
using AutoMapper;
using Common.Entities.Response;

namespace AccountingService.Presentation.Mappings;

/// <summary>
/// A class responsible for configuring multiple mappings using AutoMapper.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class and configures the mappings.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<GetBankBook, GetBankBooksDto>();
    }
}