using BusinessPlanning.Data;
using BusinessPlanning.Entities;
using BusinessPlanning.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
//using System.Net;
//using System.Net.Mail;
namespace BusinessPlanning.Controllers
{
    public class AppointmentController : Controller
    {
        private ApplicationDbContext _context;

        private UserManager<AppUser> _userManager;
        public AppointmentController(ApplicationDbContext context, UserManager<AppUser> usermanager)
        {
            _context = context;
            _userManager = usermanager;
        }


        public ActionResult UserAppointment(string UserId = "")
        {
            var user = _userManager.FindByIdAsync(UserId).Result;

            if (user != null)
            {

                PersonSpesificModelView nmodel = new PersonSpesificModelView()
                {
                    ActiveUser = user,
                    CompletedTask = _context.AppTasks.Where(a => a.AppUserId == user.Id && a.IsCompleted == true).Count(),
                    AwaitingTask = _context.AppTasks.Where(a => a.AppUserId == user.Id && a.IsCompleted == false && a.IsDeleted == false).Count(),
                    DeletedTask = _context.AppTasks.Where(a => a.AppUserId == UserId && a.IsDeleted == true).Count(),
                    LastCompletedTask = _context.AppTasks.Where(a => a.AppUserId == UserId && a.IsCompleted == true).OrderByDescending(a => a.UpdatedDate).FirstOrDefault()


                };

                return View(nmodel);
            }

            return View("Error");
        }

        public JsonResult GetAppointments()
        {

            //ilgili appointmentleri userları ile birlikte getir ama selectle özelleştir yani
            var model = _context.AppTasks.Where(a => a.IsCompleted == false && a.IsDeleted == false).Include(a => a.AppUser).Select(a => new AppointmentViewModel
            {
                Id = a.Id,
                UserName = a.AppUser.FirstName + " " + a.AppUser.LastName,
                TaskTitle = a.TitleName,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                TaskDescription = a.Decription,
                UserId = a.AppUser.Id,
                Color = a.AppUser.Color


            });

            return Json(model);
        }

        //[HttpGet("{menuId}")]
        public JsonResult GetAppointmentsbyIT(string userId = "")
        {

            //ilgili appointmentleri userları ile birlikte getir ama selectle özelleştir yani
            var model = _context.AppTasks.Where(a => a.AppUser.UserName == userId && a.IsCompleted == false && a.IsDeleted == false).Include(a => a.AppUser).Select(a => new AppointmentViewModel
            {
                Id = a.Id,
                UserName = a.AppUser.FirstName + " " + a.AppUser.LastName,
                TaskTitle = a.TitleName,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                TaskDescription = a.Decription,
                UserId = a.AppUser.Id,
                Color = a.AppUser.Color


            });

            return Json(model);
        }


        [HttpPost]
        public async Task<JsonResult> AddOrUpdateAppoinment(AddOrUpdateAppointmentViewModel model)
        {

            var userID = "";

            if (model.UserId == null || model.UserId == "")
            {
                //var user = _userManager.FindByNameAsync(HttpContext.User.Identity.Name.ToString());
                userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            else
            {
                userID = model.UserId;
            }

            //ekleme
            if (model.Id == 0)
            {
                if (model.UserId == "1")
                {

                    var users = _context.Users.Where(A => A.IsManager == false).ToList();

                    foreach (var usr in users)
                    {
                        AppTask apptask = new AppTask()
                        {
                            CreatedDate = DateTime.Now,
                            StartDate = model.StartDate,
                            EndDate = model.EndDate,
                            TitleName = model.TitleName,

                            Decription = model.Decription,

                            AppUserId = usr.Id,
                            IsCompleted = false,
                            IsDeleted = false

                        };
                        _context.Add(apptask);
                        _context.SaveChanges();

                        await SendMailtoPerson(usr.Id);


                    }
                }

                else
                {
                    AppTask apptask = new AppTask()
                    {
                        CreatedDate = DateTime.Now,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        TitleName = model.TitleName,

                        Decription = model.Decription,

                        AppUserId = userID,
                        IsCompleted = false,
                        IsDeleted = false

                    };
                    _context.Add(apptask);
                    if (model.UserId != null)
                        await SendMailtoPerson(model.UserId);
                    _context.SaveChanges();
                }


            }
            else
            {
                var entity = _context.AppTasks.SingleOrDefault(a => a.Id == model.Id);

                if (entity == null)
                {
                    return Json("Güncellenecek veri bulnamadı");
                }
                entity.UpdatedDate = DateTime.Now;
                entity.StartDate = model.StartDate;
                entity.EndDate = model.EndDate;
                entity.Decription = model.Decription;
                entity.TitleName = model.TitleName;
                entity.IsCompleted = model.IsCompleted;
                entity.IsDeleted = model.IsDeleted;
                entity.AppUserId = userID;
                _context.SaveChanges();
                //güncelleme
            }

            return Json("200");
        }


        public JsonResult DeleteAppointment(int id = 0)
        {

            var entity = _context.AppTasks.SingleOrDefault(a => a.Id == id);

            if (entity == null)
            {
                return Json("Kayıt bulunamadı");
            }

            entity.UpdatedDate = DateTime.Now;
            entity.IsDeleted = true;
            _context.SaveChanges();
            return Json("200");
        }


        public JsonResult CompleteAppointment(int id = 0)
        {

            var entity = _context.AppTasks.SingleOrDefault(a => a.Id == id);

            if (entity == null)
            {
                return Json("Kayıt bulunamadı");
            }
            entity.IsCompleted = true;
            entity.UpdatedDate = DateTime.Now;

            _context.SaveChanges();
            return Json("200");
        }


        public async Task SendMailtoPerson(string userid = "")
        {

            //bunu kullanıcid alarak mail gönderme revize yapılcaktır.
            var currentUser = await _userManager.FindByIdAsync(userid);
            string emailadres = currentUser.Email;
            if (emailadres == null) return;
            //Profile/Index olmalı domain sonuna
            var domainurl = _context.AppSettings.Where(a => a.Id == 1).FirstOrDefault();
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("no-reply@mustafaulas.net"));
            email.To.Add(MailboxAddress.Parse(emailadres));
            email.Subject = "Yeni Görev Bildirimi";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $"There is some new task which has been added from your administrator.To see that Click <a href='{domainurl.DomainUrl}'> <b> Here<b> </a> ..."
            };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("no-reply@mustafaulas.net", "XXXXXX");
            smtp.Send(email);
            smtp.Disconnect(true);

        }


        public ActionResult CompletedTask(string UserId = "")
        {
            var completedtasks = _context.AppTasks.Where(a => a.AppUserId == UserId && a.IsCompleted == true).ToList();
            TaskViewModel model = new TaskViewModel() { CompletedTasks = completedtasks };

            return View(model);
        }
    }
}
