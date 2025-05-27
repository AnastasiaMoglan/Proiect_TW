using System.Web.Mvc;
using eUseControl.Web.Models;
using System.Collections.Generic;

namespace eUseControl.Web.Controllers
{
    public class OurTeamController : Controller
    {
        // GET: OurTeam
        public ActionResult Index()
        {
            var model = new OurTeamViewModel();
            
            // Add sample team members (in a real application, this would come from a database)
            model.TeamMembers = new List<TeamMember>
            {
                new TeamMember 
                { 
                    Name = "Dr. Jane Smith", 
                    Position = "Chief Ophthalmologist", 
                    ImageUrl = "oftalmolog1.jpg",
                    Description = "Dr. Smith has over 15 years of experience in treating various eye conditions and specializes in laser eye surgery.",
                    Email = "jane.smith@eyecare.com"
                },
                new TeamMember 
                { 
                    Name = "Dr. Lara Davis", 
                    Position = "Optometrist", 
                    ImageUrl = "oftalmolog2.jpg",
                    Description = "Dr. Davis specializes in contact lens fitting and has helped thousands of patients find the perfect lenses for their needs.",
                    Email = "john.davis@eyecare.com"
                },
                new TeamMember 
                { 
                    Name = "Sarah Johnson", 
                    Position = "Optical Technician", 
                    ImageUrl = "oftalmolog3.jpg",
                    Description = "Sarah has been with our team for 8 years and is an expert in frame styling and lens technology.",
                    Email = "sarah.johnson@eyecare.com"
                }
            };
            
            return View(model);
        }
    }
}