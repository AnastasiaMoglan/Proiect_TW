// eUseControl.Web/Controllers/OurTeamController.cs
using System;
using System.Linq;
using System.Web.Mvc;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    public class OurTeamController : Controller
    {
        private readonly ITeamService _teamService;

        // Dependency Injection via constructor
        public OurTeamController(ITeamService teamService)
        {
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
        }

        // GET: OurTeam
        public ActionResult Index()
        {
            try
            {
                var teamMembers = _teamService.GetTeamMembers();
                
                var viewModel = new OurTeamViewModel
                {
                    TeamMembers = teamMembers.Select(m => new Models.TeamMember
                    {
                        Name = m.Name,
                        Position = m.Position,
                        ImageUrl = m.ImageUrl,
                        Description = m.Description,
                        Email = m.Email
                    }).ToList()
                };
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading team members.";
                ViewBag.DetailedError = ex.Message;
                return View(new OurTeamViewModel());
            }
        }
    }
}