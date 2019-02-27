using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DentalPatientClinicApplication.Models;
using DentalPatientClinicApplication.Models.Viewmodel;

namespace DentalPatientClinicApplication.Controllers
{
    [Authorize(Roles ="Doctor")]
    public class DoctorController : Controller
    {
        private ClinicDbContext _context;
        
        public DoctorController()
        {
            _context = new ClinicDbContext();
        }
        // GET: Doctor
        
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult ManageProfile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ManageProfile(ForAdmin profile)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Appointments()
        {
            string Uid = Session["UserId"].ToString();
            var U = _context.Doctors.SingleOrDefault(m => m.UserId == Uid).DoctorId;
            var app = _context.Appointments.Where(m => m.Did == U);
            return View(app);
        }

        [HttpPost]
        public ActionResult deleteappointment(int id)
        {
            var deleterec = _context.Appointments.Find(id);
            _context.Appointments.Remove(deleterec);
            _context.SaveChanges();
            string message = "Appointment deleted successfully";
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Editappointment()
        {
            return View();
        }

        public ActionResult listofreceptionist()
        {
            var reclist = _context.Receptionists.ToList();
            return View(reclist);
        }

       [HttpPost]
        public ActionResult deletereceptionist(int id)
        {
            var deleterec = _context.Receptionists.Find(id);
            _context.Receptionists.Remove(deleterec);
            _context.SaveChanges();
            string message = "Receptionist deleted successfully";
            return Json(message,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditReceptionist(int id)
        {
          
           var rec = _context.Receptionists.Single(m => m.ReceptionistId == id);
            TempData["rid"] = rec.ReceptionistId;
            var uid = rec.UserId;
            TempData["uid"] = uid;
            var user = _context.AspNetUsers.Single(m => m.Id == uid);
            
            var viewmodel = new ForAdmin()
            {
                receptionist = rec,
                user = user
            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditReceptionist(ForAdmin rec)
        {
            if (ModelState.IsValid)
            {
                var rid = Convert.ToInt32(TempData["rid"]);
                var receptionist = _context.Receptionists.Single(m => m.ReceptionistId == rid);
                var uid = TempData["uid"].ToString();
                var user = _context.AspNetUsers.Single(m => m.Id == uid);



                receptionist.ReceptionistName = rec.receptionist.ReceptionistName;
                receptionist.DateOfBirth = rec.receptionist.DateOfBirth;
                receptionist.PhoneNumber = rec.receptionist.PhoneNumber;
                receptionist.Email = rec.receptionist.Email;
                _context.SaveChanges();

                user.UserName = rec.user.UserName;

                _context.SaveChanges();

                return RedirectToAction("listofreceptionist", "Doctor");
            }
            return View();
        }


        [HttpGet]
        public ActionResult AddServices()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddServices(AddServiceView Addservice)
        {
            if (ModelState.IsValid)
            {
                DoctorService Ds = new DoctorService();
                Ds.ServiceName = Addservice.ServiceName;
                Ds.ServicePrice = Addservice.Price;
                Ds.ServiceDetails = Addservice.Discription;
                _context.DoctorServices.Add(Ds);
                _context.SaveChanges();
                return RedirectToAction("Services", "Doctor");
            }
            else
            {
                return View(Addservice);
            }

        }


        public ActionResult Services()
        {
            var Servicelist = _context.DoctorServices.ToList();
            return View(Servicelist);
        }

        

        [HttpGet]
        public ActionResult EditService(int? id)
        {
            if (id!= null)
            {
                DoctorService dr = _context.DoctorServices.Find(id);
                return View(dr);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditService(DoctorService Ds)
        {
            if(ModelState.IsValid)
            {
                var service = _context.DoctorServices.Single(m => m.ServiceId == Ds.ServiceId);
                service.ServiceName = Ds.ServiceName;
                service.ServicePrice = Ds.ServicePrice;
                service.ServiceDetails = Ds.ServiceDetails;
                _context.SaveChanges();

                return RedirectToAction("Services", "Doctor");
            }
            else
            {
                return View(Ds);
            }
            
        }

        [HttpPost]
        public ActionResult Deleteservice(int id)
        {
            DoctorService Service = _context.DoctorServices.Find(id);
            _context.DoctorServices.Remove(Service);
            _context.SaveChanges();
            string message = "Service deleted successfully";
            return Json(message);
        }

        [HttpGet]
        public ActionResult AddSchedule()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSchedule(DoctorScheduleview doctorScheduleview)
        {
            string Doctorid = Session["UserId"].ToString();
            var Doctor = _context.Doctors.Single(m => m.UserId == Doctorid); // Getting authenticated doctor information in doctor variable 
            if (ModelState.IsValid)
            {

                DoctorSchedule doctorSchedule = new DoctorSchedule();
                doctorSchedule.DoctorId = Doctor.DoctorId; // Assigned DoctorId to DoctorId column of DoctorSchedule table 
                doctorSchedule.AvailableDate = doctorScheduleview.AvailableDate;
                doctorSchedule.AvailableTime = doctorScheduleview.AvailableTime;
                _context.DoctorSchedules.Add(doctorSchedule);
                _context.SaveChanges();
                return RedirectToAction("Index", "Doctor");
                
            }
            else
            {

            }
            return View();
        }


        //[HttpPost]
        //public ActionResult GetSchedule(int DrID, string availableDate)
        //{

        //    DateTime dt = Convert.ToDateTime(availableDate).Date;
        //    var result = _context.DoctorSchedules.Where(x => x.DoctorId == DrID && x.AvailableDate == dt).ToList();

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}


        public ActionResult Schedules()
        {
            var docid = Session["UserId"].ToString();
            var doc = _context.Doctors.Single(m => m.UserId == docid).DoctorId;
            var docsched = _context.DoctorSchedules.Where(m => m.DoctorId == doc);

            return View(docsched);
        }

        [HttpGet]
        public ActionResult EditSchedule(int id)
        {
            
            var docschedule = _context.DoctorSchedules.Single(m => m.DSid == id);
            return View(docschedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSchedule(DoctorSchedule ds)
        {

            if (ModelState.IsValid) {
                var docschedule = _context.DoctorSchedules.Single(m => m.DSid == ds.DSid);
                
                if (docschedule!=null)
                {
                    docschedule.AvailableDate = ds.AvailableDate;
                    docschedule.AvailableTime = ds.AvailableTime;
                    _context.SaveChanges();
                    return RedirectToAction("Receptionists", "Doctor");
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult deleteschedule(int id)
        {
            DoctorSchedule docschedule = _context.DoctorSchedules.Find(id);
            _context.DoctorSchedules.Remove(docschedule);
            _context.SaveChanges();
            string message = "Service deleted successfully";
            return Json(message);
        }

        public ActionResult UpcomingAppointments()
        {
            var Uid = Session["UserId"].ToString();
            int doc = _context.Doctors.Single(m => m.UserId == Uid).DoctorId;
            var patientapp = _context.Appointments.ToList().Single(m => m.Did == doc);
            return View(patientapp);
        }

    }
}