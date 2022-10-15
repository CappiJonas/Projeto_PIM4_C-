namespace PIM_IV.Entities
{
    public class Patient
    {
        public string Name { get; }
        public string CPF { get; }
        public string Phone { get; }
        public string Address { get; }
        public string Neighborhood { get; set; }
        public string CEP { get; }
        public string CityState { get; }
        public DateTime DateOfBirth { get; }
        public int Age { get; private set; }
        public string Email { get; }
        public DateTime DateOfDiagnostic { get; }
        public IList<string> Comorbidities { get; } = new List<string>();
        
        public Patient(string name, string cpf, string phone, string address, string neighborhood, string cep, string cityState, DateTime dateOfBirth, 
            string email, IList<string> comorbidities)
        {
            Name = name;
            CPF = cpf;
            Phone = phone;
            Address = address;
            Neighborhood = neighborhood;
            CEP = cep;
            CityState = cityState;
            DateOfBirth = dateOfBirth;
            Email = email;
            DateOfDiagnostic = DateTime.Now;
            Comorbidities = comorbidities;
        }

        public void CalculateAge()
        {
            var timeSpan = this.DateOfDiagnostic.Subtract(this.DateOfBirth);
            this.Age = new DateTime(timeSpan.Ticks).Year - 1;
        }
    }
}
