// eUseControl.Domain/Interfaces/ITeamMemberRepository.cs
using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Domain.Interfaces
{
    public interface ITeamMemberRepository
    {
        List<TeamMember> GetAllTeamMembers();
        List<TeamMember> GetActiveTeamMembers();
    }
}