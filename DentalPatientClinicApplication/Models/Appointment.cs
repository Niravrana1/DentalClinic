//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DentalPatientClinicApplication.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Appointment
    {
        public int AppointmentId { get; set; }
        public string AId { get; set; }
        public Nullable<int> PatientId { get; set; }
        public string PatientName { get; set; }
        public Nullable<int> Did { get; set; }
        public string DoctorName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Appointment Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> AppointmentDate { get; set; }
        public Nullable<System.TimeSpan> AppointmentTime { get; set; }
        public string Reason { get; set; }
        public Nullable<bool> AppointmentStatus { get; set; }
        public Nullable<int> ServiceId { get; set; }
    
        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual DoctorService DoctorService { get; set; }
    }
}