using AccountingService.Presentation.DTOs;
using AccountingService.Presentation.DTOs.Requests;
using AccountingService.Presentation.DTOs.Response;
using AutoMapper;
using Common.Entities.PaginationSortSearch;
using Common.Entities.Requests;
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
        // Entity to DTO mappings
        // Note: This area is for <MyClass> to <MyClass>Dto mappings.
        CreateMap<GetBankBook, GetBankBookDto>();
        CreateMap<Pagination, PaginationDto>();
        CreateMap<PaginatedResponse<GetBankBook>, PaginatedResponseDto<GetBankBookDto>>();

        // DTO to entity mappings
        // Note: This area is for <MyClass>Dto to <MyClass> mappings.
        CreateMap<PagedSortedRequestDto, PagedSortedRequest>();

        CreateMap<PagedSortedSearchRequestDto, PagedSortedSearchRequest>()
            .IncludeBase<PagedSortedRequestDto, PagedSortedRequest>();

        CreateMap<GetBankBooksRequestDto, GetBankBooksRequest>()
            .IncludeBase<PagedSortedSearchRequestDto, PagedSortedSearchRequest>();
    }
}