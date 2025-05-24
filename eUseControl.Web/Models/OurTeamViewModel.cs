using System.Collections.Generic;

namespace eUseControl.Web.Models
{
    public class OurTeamViewModel
    {
        public List<TeamMember> TeamMembers { get; set; }

        public OurTeamViewModel()
        {
            TeamMembers = new List<TeamMember>();
        }
    }

    public class TeamMember
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
    }
}