// eUseControl.Data/Repository/TeamMemberRepository.cs
using System.Collections.Generic;
using System.Linq;
using eUseControl.Data.Context;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;

namespace eUseControl.Data.Repository
{
    public class TeamMemberRepository : ITeamMemberRepository
    {
        private readonly AppDbContext _db;

        public TeamMemberRepository()
        {
            _db = new AppDbContext();
        }

        public List<TeamMember> GetAllTeamMembers()
        {
            return _db.TeamMember.OrderBy(t => t.DisplayOrder).ToList();
        }

        public List<TeamMember> GetActiveTeamMembers()
        {
            return _db.TeamMember.Where(t => t.IsActive).OrderBy(t => t.DisplayOrder).ToList();
        }
    }
}