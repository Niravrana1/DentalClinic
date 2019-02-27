using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DentalPatientClinicApplication.Models;
using DentalPatientClinicApplication.Models.Viewmodel;

namespace DentalPatientClinicApplication.Controllers
{
    public class ReceptionistController : Controller
    {
        private ClinicDbContext _Context;

        public ReceptionistController()
        {
            _Context = new ClinicDbContext();
        }
        // GET: Receptionist
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ManageProfile()
        {
            var uid = Session["UserId"].ToString();
            var U = _Context.AspNetUsers.Single(m => m.Id == uid);
            var R = _Context.Receptionists.Single(m => m.UserId == uid);

            var viewmodel = new ForAdmin
            {
                user = U,
                receptionist = R
            };
            
            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult ManageProfile(ForAdmin data)
        {
            var uid = Session["UserId"].ToString();
            var U = _Context.AspNetUsers.Single(m => m.Id == uid);
            var R = _Context.Receptionists.Single(m => m.UserId == uid);
            if (ModelState.IsValid)
            {
                /* Updating receptionist table */
                R.ReceptionistName = data.receptionist.ReceptionistName;
                R.DateOfBirth = data.receptionist.DateOfBirth;
                R.Email = data.receptionist.Email;
                R.PhoneNumber = data.receptionist.PhoneNumber;
                _Context.SaveChanges();

                /* Updating receptionist data in user table */
                U.UserName = data.user.UserName;
                _Context.SaveChanges();

                return RedirectToAction("Index", "Receptionist");
            }
            else
            {
                return View(data);
            }
        }

        [HttpGet]
        public ActionResult Makeappointment()
        {
            string Uid = Session["UserId"].ToString();
           
           
            ViewBag.doctorlist = _Context.Doctors.ToList();
            //new SelectList(_Context.Doctors, "DoctorId", "DoctorName");

            var noaction = "";
            ViewBag.txtTime = noaction.Select(x => new SelectListItem { Value = "", Text = "" }).Distinct();
            return View();
        }

        [HttpPost]
        public ActionResult Makeappointment(appointmentbyreceptionist ar)
        {
            return View();
        }
           
        [HttpPost]
        public JsonResult checkpatient(string PId)
        {
            var pid = _Context.Patients.Single(m => m.PId == PId);
            //var patient = _Context.Appointments.GroupBy(m => m.PatientId == pid).First();
           
            Appointmentview av = new Appointmentview();
            av.PatientName = pid.FirstName + pid.LastName;
            
            if (pid != null)
            {
                return Json(av, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "patient not exist" }); 
            }
         
        }

        [HttpGet]
        public ActionResult EditAppointment(int id)
        {
            var app = _Context.Appointments.SingleOrDefault(m => m.AppointmentId == id);
            ViewBag.doctorlist = _Context.Doctors.ToList();
            var viewmodel = new Appointmentview()
            {
                PatientName = app.PatientName,
                Did = app.Did,
                AppointmentDate = app.AppointmentDate,
                AppointmentTime = app.AppointmentTime,
                Reason = app.Reason
            };
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult Appointments()
        {
            var allapp = _Context.Appointments.ToList();
            var todayapp = _Context.Appointments.Where(m => m.AppointmentDate == DateTime.Today).ToList();
            var viewmodel = new Recapp()
            {
                appointments = allapp,
                apps = todayapp
            };
            return View(viewmodel);
        }

      

        public ActionResult Services()
        {
            _Context.DoctorServices.ToList();
            return View();
        }

        public ActionResult Bill()
        {
            return View();
        }

        public ActionResult ViewBill()
        {
            
            return View();
        }
    }
}