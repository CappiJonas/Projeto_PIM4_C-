using PIM_IV.Entities;

namespace PIM_IV.Services.Interfaces
{
    public interface IPatientService
    {
        Task GenerateTxt(Patient patient);
    }
}
