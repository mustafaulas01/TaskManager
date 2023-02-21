using BusinessPlanning.Data;
using BusinessPlanning.Entities;
using BusinessPlanning.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessPlanning.Controllers
{
    public class ProfileController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<AppUser> _userManager;
        public ProfileController(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            AppUser user = _userManager.Users.SingleOrDefault(a => a.UserName == HttpContext.User.Identity.Name);
            if (user==null)
            {
                return View("Error");
            }
            if (_userManager.IsInRoleAsync(user,"Manager").Result)
            {
                var itusers = _userManager.Users.Where(a => a.UserName!="mustafaulas");
              

                List<UserTaskCountModel> listTaskCount = new List<UserTaskCountModel>();

                foreach (var ituser in itusers)
                {
                    UserTaskCountModel mod = CreateTaskCountModel(ituser.Id);
                    listTaskCount.Add(mod);

                }


                ManagerViewModel model = new ManagerViewModel() {
                    ActiveUser = user,
                    ITUsers = itusers,
                    ITUserList = itusers.Select(a => new SelectListItem
                    {
                        Value = a.Id,
                        Text = $"{a.FirstName} {a.LastName}"

                    }).ToList(),
                    UserTaskCountList=listTaskCount
                   
                
                };

                return View("Manager",model);
            }
            else
            {
                NormalUserViewModel model = new NormalUserViewModel() { 
                ActiveUser=user
                };
                return View("NormalUser",model);
            }

            //kullanıcı buraya istek yaptıgında rol kontroö edilir ına göre yönlendirme yapılır
          
        }



        private UserTaskCountModel CreateTaskCountModel( string id)
        {

        
                return new UserTaskCountModel()
                {
                    UserId = id,
                    Count = _context.AppTasks.Where(a => a.IsCompleted == false && a.IsDeleted == false && a.AppUser.Id == id).Count()

                };
     
        }
        public IActionResult Manager()
        {
            return View();
        }

        public IActionResult NormalUser()
        {
            return View();
        }

        public IActionResult testsayfa()
        {
            return View();
        }
    }
}
