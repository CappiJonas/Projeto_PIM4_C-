using Microsoft.Extensions.Options;
using PIM_IV.Entities;
using PIM_IV.Options;
using PIM_IV.Services.Interfaces;
using System.Text;

namespace PIM_IV.Services
{
    public class PatientService : IPatientService
    {
        private readonly ComputerPath _computerPath;

        public PatientService(IOptions<ComputerPath> options)
        {
            _computerPath = options.Value;
        }

        public async Task GenerateTxt(Patient patient)
        {
            string fileName = @$"{_computerPath.Path}\{patient.Name}.txt";

            if (File.Exists(fileName))
                File.Delete(fileName);
            try
            {
                using (FileStream fs = File.Create(fileName))
                {
                    Byte[] cep = new UTF8Encoding(true).GetBytes($"CEP: {patient.CEP}\n");
                    await fs.WriteAsync(cep, 0, cep.Length);
                    Byte[] age = new UTF8Encoding(true).GetBytes($"Idade: {patient.Age} anos");
                    await fs.WriteAsync(age, 0, age.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }           
        }
    }
}
