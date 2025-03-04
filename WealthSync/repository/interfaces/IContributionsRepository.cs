using WealthSync.Data;
using WealthSync.Dtos;

namespace WealthSync.repository.interfaces;

public interface IContributionsRepository
{
    Task<bool> AddContributionAsync(CreateContributionDto contributionDto, string userId);
    Task<bool> RemoveContributionAsync(CreateContributionDto contributionDto, string userId);
}