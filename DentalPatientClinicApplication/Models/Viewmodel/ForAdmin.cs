using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DentalPatientClinicApplication.Models.Viewmodel
{
    public class ForAdmin
    {
        public List<AspNetUser> userslist { get; set; }
        public List<AspNetRole> roles { get; set; }
        public AspNetRole role { get; set; }
        public AspNetUser user { get; set; }
        public Patient patient { get; set; }
        public List<Patient> patients { get; set; }
        public Receptionist receptionist { get; set; }
        public List<Receptionist> receptionists { get; set; }
        public Appointment appointment { get; set; }
        public List<Appointment> appointments { get; set; }
        public Doctor doctor { get; set; }
        public List<Doctor> doctors { get; set; }
    }
    public class Recapp
    {
        public Appointment appointment { get; set; }
        public List<Appointment> appointments { get; set; }
        public List<Appointment> apps { get; set; }
    }
    public class Patientview
    {
        [Required(ErrorMessage = "Please enter first name")]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name")]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [_18yearandolder]
        [Required(ErrorMessage = "Please select or enter available date")]
        [DisplayFormat(DataFormatString = "{0:mm/dd/yy}")]
        [Column(TypeName = "DateTime2")]
        [Display(Name = "Date Of Birth")]
        public Nullable<System.DateTime> DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please enter email address")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter phone number")]
        [Display(Name = "Phone Number")]
        [StringLength(10)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "No special characters allowed")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter street address")]
        [StringLength(100)]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "Please enter City name")]
        [StringLength(50)]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter State")]
        [StringLength(20)]
        public string State { get; set; }

        [Required(ErrorMessage = "Please enter zipcode")]
        public Nullable<int> ZipCode { get; set; }

        [Required(ErrorMessage = "PLease enter User name")]
        [StringLength(50)]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "please enter password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        [Required(ErrorMessage = "Please enter confirm password")]
        public string ConfirmPassword { get; set; }
    }
    public class Appointmentview
    {
        [Required(ErrorMessage = "Please enter patient name")]
        [StringLength(20)]
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "Please select doctor")]
        [Display(Name = "Doctor")]
        public Nullable<int> Did { get; set; }

        [Required(ErrorMessage = "Please select or enter appointment date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Birthday")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> AppointmentDate { get; set; }

        [Required(ErrorMessage = "Please select available time")]
        [Display(Name = "Appointment Time")]
        public Nullable<System.TimeSpan> AppointmentTime { get; set; }

        [Display(Name = "Reason")]
        public string Reason { get; set; }

        public Doctor doc { get; set; }

    }

    public class SignInview
    {
        [Required(ErrorMessage = "PLease enter username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "PLease enter Password")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        public bool Rememberme { get; set; }
    }
    public class Doctorview
    {
        [Required(ErrorMessage = "Please enter first name")]
        [StringLength(50)]
        public string Doctorname { get; set; }

        [Required(ErrorMessage = "Please Enter Date of birth")]
        [DisplayFormat(DataFormatString = "{0:mm/dd/yy}")]
        [Column(TypeName = "DateTime2")]
        [Display(Name = "Date Of Birth")]
        public DateTime Dateofbirth { get; set; }

        [Required(ErrorMessage = "Please enter email address")]
        [StringLength(50)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a phone number")]
        [Display(Name = "Phone Number")]
        public string Phonenumber { get; set; }

        [Required(ErrorMessage = "Please enter doctor speciality ")]
        public string Speciality { get; set; }

        [Required(ErrorMessage = "PLease enter User name")]
        [StringLength(50)]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "please enter password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        [Required(ErrorMessage = "Please enter confirm password")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordView
    {
        [Required(ErrorMessage ="Please enter current password")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Please enter new password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please enter confirm password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class DoctorScheduleview
    {
        [Required(ErrorMessage = "Please select available date")]
        [DisplayFormat(DataFormatString = "{0:mm/dd/yy}")]
        [Column(TypeName = "DateTime2")]
        [Display(Name = "Available Date")]
        public DateTime AvailableDate { get; set; }

        [Required(ErrorMessage = "Please enter available date")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        [Display(Name = "Available Time")]
        public TimeSpan AvailableTime { get; set; }

    }

    public class ReceptionistSignupview
    {
        [Required(ErrorMessage = "Please enter first name")]
        [StringLength(50)]
        public string Name { get; set; }

        [_18yearandolder]
        [Required(ErrorMessage = "Please select or enter available date")]
        [DisplayFormat(DataFormatString = "{0:mm/dd/yy}")]
        [Column(TypeName = "DateTime2")]
        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please enter email address")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "PLease enter User name")]
        [StringLength(50)]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "please enter password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        [Required(ErrorMessage = "Please enter confirm password")]
        public string ConfirmPassword { get; set; }
    }

    public class appointmentbyreceptionist
    {
        public string PatientId { get; set; }

        [Required(ErrorMessage ="Please enter patient name")]
        [StringLength(20)]
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "Please select doctor")]
        [Display(Name = "Doctor")]
        public Nullable<int> Did { get; set; }

        [Required(ErrorMessage = "Please select a date")]
        [DisplayFormat(DataFormatString = "{0:dd mm yyyy}")]
        [Display(Name = "Date of Birthday")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> AppointmentDate { get; set; }

        [Required(ErrorMessage ="Please select appointment time")]
        [Display(Name = "Appointment Time")]
        public Nullable<System.TimeSpan> AppointmentTime { get; set; }

        [Display(Name = "Reason")]
        public string Reason { get; set; }
    }

    public class AddServiceView
    {
        [Required(ErrorMessage = "Please enter service name")]
        [StringLength(20)]
        public string ServiceName { get; set; }

        [Required(ErrorMessage = "Please enter price")]
        public decimal Price { get; set; }

        public string Discription { get; set; }
    }
    
}