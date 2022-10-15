using PIM_IV.Entities;
using PIM_IV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIM_IV.Controllers
{
    public class PatientController
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public async Task GenerateTxt(Patient patient)
        {
            await _patientService.GenerateTxt(patient);
        }
    }
}
