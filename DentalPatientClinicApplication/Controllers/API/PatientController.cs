using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DentalPatientClinicApplication.Models;

namespace DentalPatientClinicApplication.Controllers.API
{

    public class PatientController : ApiController
    {
        private ClinicDbContext _context;
        public PatientController()
        {
            _context = new ClinicDbContext();
        }
        public IEnumerable<Patient> GetPatients()
        {
            return _context.Patients.ToList();
        }
        public Patient GetPatient(int Id)
        {
            var Customer = _context.Patients.SingleOrDefault(m => m.PatientId == Id);

            if(Customer!=null)
            {
                return Customer;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            
        }

        // Delete Patient 
        [HttpDelete]
        public void DeletePatient(int id)
        {
            var P = _context.Patients.SingleOrDefault(m => m.PatientId == id);
            if(P!=null)
            {
                _context.Patients.Remove(P);
                _context.SaveChanges();
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}
