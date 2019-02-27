using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DentalPatientClinicApplication.Models;
using DentalPatientClinicApplication.Models.Viewmodel;

namespace DentalPatientClinicApplication.Controllers
{
    [Authorize(Roles ="Patient")]
    public class PatientController : Controller
    {
        private ClinicDbContext _Context;

        public PatientController()
        {
            _Context = new ClinicDbContext();
        }
        // GET: Patient
        public ActionResult Home()
        {
            var Uid = Session["UserId"].ToString();
            int pid = _Context.Patients.Single(m => m.UserId == Uid).PatientId;
            //var appointment = _Context.Appointments.Where(m => m.PatientId == pid).Distinct().GroupBy(model => model.AppointmentDate).First();
            var appointment = _Context.Appointments.Where(m => m.PatientId == pid).OrderByDescending(m => m.AppointmentDate).FirstOrDefault();

            var viewmodel = new ForAdmin()
            {
                appointment = appointment
            };
            return View(Tuple.Create<ForAdmin, IEnumerable<Appointment>>(viewmodel, _Context.Appointments.ToList()));
            
        }

        //[HttpPost]
        //public ActionResult GetSchedule(int DrID, string availableDate)
        //{

        //    //DateTime dt = Convert.ToDateTime(availableDate).Date;
        //    //var result = _Context.DoctorSchedules.Where(x => x.DoctorId == DrID && x.AvailableDate == dt).ToList();

        //    //return Json(result, JsonRequestBehavior.AllowGet);
        //    ClinicDbContext db = new ClinicDbContext();
        //    DateTime dt = Convert.ToDateTime(availableDate).Date;
        //    //var result = db.DoctorSchedules.Single(x => x.DoctorId == DrID && x.AvailableDate == dt).;

        //    var result = from dr in db.DoctorSchedules
        //                 where dr.DoctorId == DrID && dr.AvailableDate == dt
        //                 select new
        //                 {
        //                     AvailableTime = dr.AvailableTime
        //                 };
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult GetSchedule(int DrID, string availableDate)
        {
            ClinicDbContext db = new ClinicDbContext();
            DateTime dt = Convert.ToDateTime(availableDate).Date;
            //var result = db.DoctorSchedules.Single(x => x.DoctorId == DrID && x.AvailableDate == dt).;

            var result = from dr in db.DoctorSchedules
                         where dr.DoctorId == DrID && dr.AvailableDate == dt
                         select new
                         {

                             AvailableTime = dr.AvailableTime
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

            public ActionResult _Home()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Scheduleappointment()
        {
            List<Doctor> lstdr = _Context.Doctors.ToList();
            ViewBag.ddDoctorsList = lstdr.Select(x => new SelectListItem { Value = x.DoctorId.ToString(), Text = x.DoctorName });

            List<Patient> lstpat = _Context.Patients.ToList();
            ViewBag.ddPatientsList = lstpat.Select(x => new SelectListItem { Value = x.PatientId.ToString(), Text = x.FirstName + x.LastName });

            List<DoctorService> lstser = _Context.DoctorServices.ToList();
            ViewBag.ddServicesList = lstser.Select(x => new SelectListItem { Value = x.ServiceId.ToString(), Text = x.ServiceName });

            ViewBag.txtTime = lstpat.Select(x => new SelectListItem { Value = "", Text = "" }).Distinct();

            return View();
        }

        //GET : Patient / Schedule 
        [HttpGet]
        public ActionResult Schedule()
        {
            string Uid = Session["UserId"].ToString();
            var F = _Context.Patients.SingleOrDefault(m => m.UserId == Uid);
            var L = _Context.Patients.SingleOrDefault(m => m.UserId == Uid);
            ViewBag.Firstname = F.FirstName.ToString();
            ViewBag.Lastname = L.LastName.ToString();
            ViewBag.doctorlist = new SelectList(_Context.Doctors, "DoctorId", "DoctorName");
            //new SelectList(_Context.Doctors, "DoctorId", "DoctorName");

            var noaction = "";
            ViewBag.txtTime = noaction.Select(x => new SelectListItem { Value = "", Text = "" }).Distinct();
            return View();
        }

        //POST : Patient / Schedule
        [HttpPost]
        public ActionResult Schedule(Appointmentview av)
        {
            string Uid = Session["UserId"].ToString();
            var U = _Context.Patients.Single(m => m.UserId == Uid);
            
               
                    Appointment Ap = new Appointment();
                    Ap.PatientId = U.PatientId;
                    Ap.PatientName = av.PatientName;
                    Ap.Did = Convert.ToInt32(av.Did);
                    int did = Convert.ToInt32(av.Did);
                    var n = _Context.Doctors.Single(m => m.DoctorId == did);
                    Ap.DoctorName = n.DoctorName;
                    Ap.AppointmentDate = av.AppointmentDate;
                    Ap.AppointmentTime = av.AppointmentTime;
                    Ap.Reason = av.Reason;
                    Ap.AppointmentStatus = false;
                    _Context.Appointments.Add(Ap);
                    _Context.SaveChanges();

                    return RedirectToAction("appointments", "Patient");
                    
           
            
            
        }

        
        [HttpPost]
        public JsonResult ScheduleAppointment(int DrID,string Pname, string AppointmentDate, TimeSpan AppointmentTime, string Reason)
        {
            //AppointmentDate = DateTime.Parse(AppointmentDate).Date.ToShortDateString();
            //DateTime appointDate = Convert.ToDateTime(AppointmentDate);
            var checkdata = from p in _Context.DoctorSchedules.ToList()
                            where p.AvailableDate == Convert.ToDateTime(AppointmentDate).Date && p.AvailableTime == AppointmentTime
                            select p;
            //check schedule....sadasdsad
            if (checkdata.Count() > 0)
            {
                //check doctor availability...
                var checkdravail = from app in _Context.Appointments.ToList()
                                   where app.Did == DrID && app.AppointmentDate == Convert.ToDateTime(AppointmentDate).Date && app.AppointmentTime == AppointmentTime
                                   select app;

                //If Doctor is not Busy book appintment
                if (checkdravail.Count() == 0)
                {
                   var docname = _Context.Doctors.Single(m => m.DoctorId == DrID).DoctorName;

                    var uid = Session["UserId"].ToString();
                    var pid = _Context.Patients.Single(m => m.UserId == uid).PatientId;
                    Appointment apt = new Appointment();
                    apt.PatientId = pid;
                    apt.PatientName = Pname;
                    apt.Did = DrID;
                    apt.DoctorName = docname;
                    apt.AppointmentStatus = false;
                    //apt.ServiceID = ServiceID;
                    apt.AppointmentDate = Convert.ToDateTime(AppointmentDate).Date;
                    apt.AppointmentTime = AppointmentTime;
                    apt.Reason = Reason;


                    _Context.Appointments.Add(apt);
                    _Context.SaveChanges();
                    return Json(new { message = "Appointment Booked Successfully", status = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { message = "Doctor Have Appointment on the Selected Date and Time", status = "success" }, JsonRequestBehavior.AllowGet);

                }

            }
            else
            {
                return Json(new { message = "Doctor Not Available on the Selected Date and Time", status = "success" }, JsonRequestBehavior.AllowGet);

            }
        }

        //GET : Patient / Appointments
        public ActionResult Appointments()
        {
            string Uid = Session["UserId"].ToString();
            var U = _Context.Patients.SingleOrDefault(m => m.UserId == Uid).PatientId;
            var app = _Context.Appointments.Where(m => m.PatientId == U);
            return View(app);
        }

        [HttpPost]
        public ActionResult deleteappointment(int id)
        {
             var deleterec = _Context.Appointments.Find(id);
            _Context.Appointments.Remove(deleterec);
            _Context.SaveChanges();
            string message = "Appointment deleted successfully";
            return Json(message,JsonRequestBehavior.AllowGet);
        }

        public ActionResult _Appointment()
        {
            return View();
        }

        public ActionResult _Appointments()
        {
            string Uid = Session["UserId"].ToString();
            var U = _Context.Patients.SingleOrDefault(m => m.UserId == Uid).PatientId;
            var app = _Context.Appointments.Where(m => m.PatientId == U);

            return View(app);
        }

        //GET : Patient / EditAppointment
        [HttpGet]
        public ActionResult Editappointment(int id)
        {
            var app = _Context.Appointments.SingleOrDefault(m => m.AppointmentId == id);
            ViewBag.doctorlist = _Context.Doctors.ToList();
            var viewmodel = new Appointmentview()
            {
                PatientName = app.PatientName,
                Did = app.Did,
                AppointmentDate = app.AppointmentDate,
                AppointmentTime=app.AppointmentTime,
                Reason = app.Reason
            };
            return View(viewmodel);
        }

        //Post : Patient / EditAppointment
        [HttpPost]
        public ActionResult Editappointment(Appointment appointment)
        {
           
            return View();
        }

       // Using Ajax for deleting appointment
       public ActionResult DeleteAppointment()
        {
            return View();
        }

        // GET : Patient / Profile 
        [HttpGet]
        public ActionResult PatientProfile()
        {
            int patientid = (Int32)Session["UserId"];
            var patient = _Context.Patients.Single(m => m.PatientId == patientid);
            var userid = _Context.Patients.SingleOrDefault(m => m.PatientId == patientid).UserId;
            var user = _Context.AspNetUsers.SingleOrDefault(m => m.Id == userid);

            var viewmodel = new ForAdmin()
            {
                patient = patient,
                user = user
            };
            return PartialView(viewmodel);
        }

        // GET : Patient / EditProfile

        public ActionResult EditPatient()
        {
            return View();
        }

        // POST : Patient / EditProfile

        //GET : Patient / Payment


        // GET : Patient / MyAccount

        public ActionResult MyAccount()
        {
            string Uid = Session["UserId"].ToString();
            string FirstName = _Context.Patients.SingleOrDefault(m => m.UserId == Uid).FirstName;
            string LastName = _Context.Patients.SingleOrDefault(m => m.UserId == Uid).LastName;
            string Id = _Context.Patients.SingleOrDefault(m => m.UserId == Uid).PId;
            string phonenumber = _Context.Patients.SingleOrDefault(m => m.UserId == Uid).PhoneNumber;
            string Email = _Context.Patients.SingleOrDefault(m => m.UserId == Uid).Email;
            ViewBag.Name = FirstName + LastName;
            ViewBag.Id = Id;
            ViewBag.Number = phonenumber;
            ViewBag.Email = Email;
            return View();
        }

        public ActionResult _MyAccount()
        {
            string Uid = Session["UserId"].ToString();
            string FirstName = _Context.Patients.SingleOrDefault(m => m.UserId == Uid).FirstName;
            string LastName = _Context.Patients.SingleOrDefault(m => m.UserId == Uid).LastName;
            string Id = _Context.Patients.SingleOrDefault(m => m.UserId == Uid).PId;
            string phonenumber = _Context.Patients.SingleOrDefault(m => m.UserId == Uid).PhoneNumber;
            string Email = _Context.Patients.SingleOrDefault(m => m.UserId == Uid).Email;
            ViewBag.Name = FirstName + LastName;
            ViewBag.Id = Id;
            ViewBag.Number = phonenumber;
            ViewBag.Email = Email;

            var uid = Session["UserId"].ToString();
            int pid = _Context.Patients.Single(m => m.UserId == uid).PatientId;
            //var appointment = _Context.Appointments.Where(m => m.PatientId == pid).Distinct().GroupBy(model => model.AppointmentDate).First();
            var appointment = _Context.Appointments.Where(m => m.PatientId == pid).OrderByDescending(m => m.AppointmentDate).FirstOrDefault();
            var viewmodel = new ForAdmin()
            {
                appointment = appointment
            };

            return View(viewmodel);
        }

        // GET : Patient / ManageProfile
        [HttpGet]
        public ActionResult ManageProfile()
        {
            string Uid = Session["UserId"].ToString();
            var Patient = _Context.Patients.SingleOrDefault(m => m.UserId == Uid); // Getting patient Information by comparing session variable userid with patient table userid
            string PId = Patient.UserId;                                             // Getting Userid from patient table
            var User = _Context.AspNetUsers.SingleOrDefault(m => m.Id == PId);        // Getting Usertable information of authenticated patient 



            var viewmodel = new ForAdmin()
            {
                patient = Patient,
                user = User
            };
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult _ManageProfile()
        {
            string Uid = Session["UserId"].ToString();
            var Patient = _Context.Patients.SingleOrDefault(m => m.UserId == Uid); // Getting patient Information by comparing session variable userid with patient table userid
            string PId = Patient.UserId;                                             // Getting Userid from patient table
            var User = _Context.AspNetUsers.SingleOrDefault(m => m.Id == PId);        // Getting Usertable information of authenticated patient 

            var uid = Session["UserId"].ToString();
            int pid = _Context.Patients.Single(m => m.UserId == uid).PatientId;
            //var appointment = _Context.Appointments.Where(m => m.PatientId == pid).Distinct().GroupBy(model => model.AppointmentDate).First();
            var appointment = _Context.Appointments.Where(m => m.PatientId == pid).OrderByDescending(m => m.AppointmentDate).FirstOrDefault();


            var viewmodel = new ForAdmin()
            {
                patient = Patient,
                user = User,
                appointment = appointment
                
            };
            return View(viewmodel);
        }

        // Post : Patient / ManageProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProfile(ForAdmin viewmodel)
        {
            string Uid = Session["UserId"].ToString();
            var patientindb = _Context.Patients.Single(m => m.UserId == Uid);  // getting patient details from database where userid in patient table is same as logged in user userid

            if(ModelState.IsValid)
            {
                // Updating patient details 
                patientindb.FirstName = viewmodel.patient.FirstName;
                patientindb.LastName = viewmodel.patient.LastName;
                patientindb.DateOfBirth = viewmodel.patient.DateOfBirth;
                patientindb.Email = viewmodel.patient.Email;
                patientindb.PhoneNumber = viewmodel.patient.PhoneNumber;
                patientindb.StreetAddress = viewmodel.patient.StreetAddress;
                patientindb.City = viewmodel.patient.City;
                patientindb.State = viewmodel.patient.State;
                patientindb.ZipCode = viewmodel.patient.ZipCode;

                _Context.SaveChanges();

                // Updating Usertable 
                string UId = Session["UserId"].ToString();
                var Userindb = _Context.AspNetUsers.Single(m => m.Id == UId );
                Userindb.UserName = viewmodel.user.UserName;
                _Context.SaveChanges();
                return RedirectToAction("Index", "Patient");
            }
            else
            {
                return View();
            }
           
        }
    }
}