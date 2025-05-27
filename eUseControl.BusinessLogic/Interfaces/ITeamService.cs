// eUseControl.BusinessLogic/Interfaces/ITeamService.cs
using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface ITeamService
    {
        List<TeamMember> GetTeamMembers();
    }
}