using AutoMapper;
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
        // Model to Entity mappings
        CreateMap<BankBookCreated, BankBookDbEntity>();
        CreateMap<BankBookPositionCreated, BankBookPositionDbEntity>();
    }
}