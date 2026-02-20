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


        // Model to DTO mappings
        CreateMap<BankBookModel, BankBookDto>();
        CreateMap<PaginationModel, PaginationDto>();
        CreateMap<PaginatedResponseModel<BankBookModel>, PaginatedResponseDto<BankBookDto>>();

        CreateMap<BankBookPositionModel, BankBookPositionDto>();
        CreateMap<PaginatedResponseModel<BankBookPositionModel>, PaginatedResponseDto<BankBookPositionDto>>();

        CreateMap<BankBookExportRequestDto, BankBookExportCreateModel>();
        CreateMap<BankBookPositionExportRequestDto, BankBookPositionExportCreateModel>();

        // DTO to Model mappings
        CreateMap<BankBookPositionCreateDto, BankBookPositionCreateModel>();
        CreateMap<BankBookCreateDto, BankBookCreateModel>();

        CreateMap<PagedSortedRequestDto, PagedSortedRequestModel>();

        CreateMap<PagedSortedSearchRequestDto, PagedSortedSearchRequestModel>()
            .IncludeBase<PagedSortedRequestDto, PagedSortedRequestModel>();

        CreateMap<BankBookQueryDto, BankBookQueryModel>()
            .IncludeBase<PagedSortedSearchRequestDto, PagedSortedSearchRequestModel>();
    }
}