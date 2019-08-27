using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeltExam.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace BeltExam.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            int? user_id = HttpContext.Session.GetInt32("UserId");
            if (user_id != null)
            {
                return RedirectToAction("Success");
            }
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            int? user_id = HttpContext.Session.GetInt32("UserId");
            if (user_id != null)
            {
                return RedirectToAction("Success");
            }
            return View();
        }

        [HttpPost("processregister")]
        public IActionResult ProccessRegister(User formData)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.users.Any(u => u.Email == formData.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                formData.Password = Hasher.HashPassword(formData, formData.Password);
                dbContext.Add(formData);
                dbContext.SaveChanges();
                System.Console.WriteLine(new string('@', 80));
                System.Console.WriteLine(formData.UserId);

                HttpContext.Session.SetInt32("UserId", formData.UserId);
                return RedirectToAction("Success");
            }
            return View("Index");
        }

        [HttpPost("proccesslogin")]
        public IActionResult ProcessLogin(LoginUser formData)
        {
            if (ModelState.IsValid)
            {
                var userInDb = dbContext.users.FirstOrDefault(u => u.Email == formData.Email);
                if (userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email!");
                    return View("Login");
                }
                var hasher = new PasswordHasher<LoginUser>();

                var result = hasher.VerifyHashedPassword(formData, userInDb.Password, formData.Password);
                if (result == 0)
                {
                    ModelState.AddModelError("Password", "Invalid Password");
                    return View("Login");
                }

                HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                return RedirectToAction("Success");
            }
            ModelState.AddModelError("Email", "Invalid email or password");
            return View("Login");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("/bright_ideas")]
        public IActionResult Success()
        {

            int? User_id = HttpContext.Session.GetInt32("UserId");
            User CurrUser = dbContext.users.FirstOrDefault(u => u.UserId == User_id);
            if (User_id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {

                List<Idea> AllIdeas = dbContext.ideas
                    .Include(i => i.Author)
                    .Include(i => i.LikedBy)
                    .OrderByDescending(i => i.LikedBy.Count)
                    .ToList();

                DashboardViewModel viewModel = new DashboardViewModel();
                viewModel.AllIdeas = AllIdeas;
                viewModel.CurrentUser = CurrUser;
                return View(viewModel);

            }
        }


        [HttpPost("processnewidea")]
        public IActionResult ProcessNewIdea(DashboardViewModel FormData)
        {
            int? User_id = HttpContext.Session.GetInt32("UserId");
            User CurrUser = dbContext.users.FirstOrDefault(u => u.UserId == User_id);
            if (User_id == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                FormData.NewIdea.UserId = CurrUser.UserId;
                dbContext.ideas.Add(FormData.NewIdea);
                dbContext.SaveChanges();
                return RedirectToAction("Success");
            }
            else
            {
                List<Idea> AllIdeas = dbContext.ideas
                    .Include(i => i.Author)
                    .Include(i => i.LikedBy)
                    .OrderByDescending(i => i.LikedBy.Count)
                    .ToList();

                DashboardViewModel viewModel = new DashboardViewModel();
                viewModel.AllIdeas = AllIdeas;
                viewModel.CurrentUser = CurrUser;
                return View("Success", viewModel);
            }
        }

        [HttpGet("like/{id}")]
        public IActionResult LikeIdea(int id)
        {
            int? User_id = HttpContext.Session.GetInt32("UserId");
            User CurrUser = dbContext.users.FirstOrDefault(u => u.UserId == User_id);
            if (User_id == null)
            {
                return RedirectToAction("Index");
            }

            Like newLike = new Like();
            newLike.IdeaId = id;
            newLike.UserId = (int)User_id;
            dbContext.likes.Add(newLike);
            dbContext.SaveChanges();
            return Redirect("/bright_ideas");

        }

        [HttpGet("delete/{id}")]
        public IActionResult DeleteIdea(int id)
        {
            int? CurrUserId = HttpContext.Session.GetInt32("UserId");
            if (CurrUserId == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                Idea ThisIdea = dbContext.ideas
                    .FirstOrDefault(a => a.IdeaId == id);
                dbContext.Remove(ThisIdea);
                dbContext.SaveChanges();
                return RedirectToAction("Success");
            }
        }

        [HttpGet("bright_ideas/{id}")]
        public IActionResult IdeaDetails(int id)
        {
            // DBContext.Ideas.Where(i => i.IdeaId == ideaId).Include(u => u.Owner).Include(l => l.LikedBy).ThenInclude(u => u.User).Single();
            Idea OneIdea = dbContext.ideas 
                .Include(i=>i.Author)
                .Include(i=>i.LikedBy)
                .ThenInclude(u=>u.AssociatedUser)
                .SingleOrDefault(i=>i.IdeaId == id);
            return View(OneIdea);
        }

        [HttpGet("users/{id}")]
        public IActionResult UserDetails(int id)
        {
            User ThisUser = dbContext.users
                .Include(i => i.UserIdeas)
                .Include(l => l.UserLikes)
                .SingleOrDefault(u => u.UserId == id);
            return View(ThisUser);
        }

    }
}

