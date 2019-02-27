using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DentalPatientClinicApplication.Models;
using DentalPatientClinicApplication.Models.Viewmodel;
using Microsoft.AspNet.Identity;

namespace DentalPatientClinicApplication.Controllers
{
    public class UseraccessController : Controller
    {
        private ClinicDbContext _context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UseraccessController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {


        }

    }
}