using AutoMapper;
using WealthSync.Data;
using WealthSync.Dtos;

namespace WealthSync.Profiles;

public class AutomapperProfiles: Profile
{
    public AutomapperProfiles()
    {
        CreateMap<Saving, SavingsDto>();
        CreateMap<Contribution, ContributionDto>();
    }
}