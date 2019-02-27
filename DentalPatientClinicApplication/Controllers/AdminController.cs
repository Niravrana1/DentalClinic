using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DentalPatientClinicApplication.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DentalPatientClinicApplication.Models.Viewmodel;

namespace DentalPatientClinicApplication.Controllers
{
    
    public class AdminController : Controller
    {
        private ClinicDbContext _context;

       public AdminController()
        {
            _context = new ClinicDbContext();
        }
        // GET: Admin
        public ClinicDbContext db = new ClinicDbContext();
        
        public ActionResult Index()
        {
          
           IEnumerable<AspNetRole> Aroles = db.AspNetRoles.OrderBy(x => x.Name).ToList();
            ViewBag.Roles = Aroles;
            ViewBag.Message = "";
            ViewBag.RoleID = new SelectList(db.AspNetRoles, "Name", "Name");
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "UserName");
            AspNetUser U = new AspNetUser();
            AspNetRole R = new AspNetRole();

            var viewmodel = new ForAdmin
            {
                userslist = db.AspNetUsers.ToList(),
                roles = db.AspNetRoles.ToList(),
                user=U,
                role=R

            };

            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult addrole()
        {
          
            var u = db.AspNetUsers.OrderBy(m => m.UserName);
            return View();
        }
        [HttpPost]
        public ActionResult addrole(AspNetRole Aroles)
        {
            try
            {
                var context = new Models.ApplicationDbContext();


                context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Name = Aroles.Name
                });
                context.SaveChanges();
                ViewBag.Message = "Role created successfully !";
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }
            // GET: AspNetRoles/Create
            public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {

            try
            {
                var context = new Models.ApplicationDbContext();


                context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                context.SaveChanges();
                ViewBag.Message = "Role created successfully !";
                return RedirectToAction("Index","Admin");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(string id)
        {
            AspNetRole aspNetRole = db.AspNetRoles.Find(id);
            db.AspNetRoles.Remove(aspNetRole);
            db.SaveChanges();
            return RedirectToAction("Index","Admin");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [HttpPost]
        public ActionResult RoleAddToUser(string Id, string Name)
        {

            ApplicationDbContext ctx = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ctx));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));
            if (roleManager.RoleExists(Name))
            {
                userManager.AddToRole(Id, Name);
                ViewBag.Message = "Role created successfully !";
            }

            ctx.SaveChanges();
            return Json(new { error = false, message = "done" });
        }

        public ActionResult GetRoles(string Id)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ctx));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));
            var RolesForThisUser = userManager.GetRoles(Id);
            return Json(RolesForThisUser);
        }

        public ActionResult DeleteRoleForUser(string Id, string Name)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ctx));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));
            if (roleManager.RoleExists(Name))
            {
                userManager.RemoveFromRoles(Id, Name);
                ctx.SaveChanges();
                ViewBag.Message = "Role created successfully !";
            }

            return Json(new { error = false, message = "done" });
        }
        public ActionResult Temp()
        {
            ViewBag.roles = _context.AspNetUsers.ToList();
            return View();
        }
        public ActionResult tempajax(string Name)
        {
            string Viewname = Name;
            return Json(Viewname, JsonRequestBehavior.AllowGet);
        }
    }
}