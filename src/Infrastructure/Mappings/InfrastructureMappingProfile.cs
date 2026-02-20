using AutoMapper;
using Common.Domain.BankBook.ResponseModels;
using Common.DomainHelpers;
using Infrastructure.Entities;

namespace Infrastructure.Mappings;

/// <summary>
/// A class resonsible for configuring multiple mappings using AutoMapper.
/// </summary>
public class InfrastructureMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InfrastructureMappingProfile"/> class and configures the mappings.
    /// </summary>
    public InfrastructureMappingProfile()
    {
        // Entity to Model mappings
        CreateMap<BankBookDbEntity, BankBookExportModel>();

        // Model to Entity mappings
        CreateMap<BankBookCreated, BankBookDbEntity>();
        CreateMap<BankBookPositionCreated, BankBookPositionDbEntity>();
        CreateMap<BankBookExportModel, BankBookExportDBEntity>()
            .ForMember(dest => dest.BankBook, opt => opt.Ignore())
            .ForMember(dest => dest.FileContent, opt => opt.Ignore());
    }
}