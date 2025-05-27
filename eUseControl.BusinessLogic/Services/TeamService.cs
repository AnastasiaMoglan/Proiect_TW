// eUseControl.BusinessLogic/Services/TeamService.cs
using System;
using System.Collections.Generic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;

namespace eUseControl.BusinessLogic.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamMemberRepository _teamMemberRepository;

        public TeamService(ITeamMemberRepository teamMemberRepository)
        {
            _teamMemberRepository = teamMemberRepository ?? throw new ArgumentNullException(nameof(teamMemberRepository));
        }

        public List<TeamMember> GetTeamMembers()
        {
            return _teamMemberRepository.GetActiveTeamMembers();
        }
    }
}