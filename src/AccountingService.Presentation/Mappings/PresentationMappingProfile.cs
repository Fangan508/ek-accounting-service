using AccountingService.Presentation.DTOs;
using AccountingService.Presentation.DTOs.Requests;
using AccountingService.Presentation.DTOs.Response;
using AutoMapper;
using Common.Domain.BankBook.RequestModels;
using Common.Domain.BankBook.ResponseModels;
using Common.Domain.PaginationSortSearch;

namespace AccountingService.Presentation.Mappings;

/// <summary>
/// A class responsible for configuring multiple mappings using AutoMapper.
/// </summary>
public class PresentationMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PresentationMappingProfile"/> class and configures the mappings.
    /// </summary>
    public PresentationMappingProfile()
    {
        // Entity to DTO mappings
        // Note: This area is for <MyClass> to <MyClass>Dto mappings.
        CreateMap<BankBookModel, BankBookDto>();
        CreateMap<PaginationModel, PaginationDto>();
        CreateMap<PaginatedResponseModel<BankBookModel>, PaginatedResponseDto<BankBookDto>>();

        CreateMap<BankBookPositionModel, BankBookPositionDto>();
        CreateMap<PaginatedResponseModel<BankBookPositionModel>, PaginatedResponseDto<BankBookPositionDto>>();

        // DTO to entity mappings
        // Note: This area is for <MyClass>Dto to <MyClass> mappings.
        CreateMap<PagedSortedRequestDto, PagedSortedRequestModel>();

        CreateMap<PagedSortedSearchRequestDto, PagedSortedSearchRequestModel>()
            .IncludeBase<PagedSortedRequestDto, PagedSortedRequestModel>();

        CreateMap<BankBookQueryDto, BankBookQueryModel>()
            .IncludeBase<PagedSortedSearchRequestDto, PagedSortedSearchRequestModel>();
    }
}