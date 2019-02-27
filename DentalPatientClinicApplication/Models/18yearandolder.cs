using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DentalPatientClinicApplication.Models.Viewmodel;

namespace DentalPatientClinicApplication.Models
{
    public class _18yearandolder : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var patient = (Patientview)validationContext.ObjectInstance;
            if(patient.DateOfBirth == null)
            {
                return new ValidationResult("Birthdate is require"); 
            
            }
            var age = DateTime.Today.Year - patient.DateOfBirth.Value.Year;
            if(age >= 18)
            {
               return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("User must be 18 year and older");
            }
        }
    }
}