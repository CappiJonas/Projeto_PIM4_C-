using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PIM_IV.Controllers;
using PIM_IV.Entities;
using PIM_IV.Options;
using PIM_IV.Services;
using PIM_IV.Services.Interfaces;
using System.Globalization;

var serviceCollection = new ServiceCollection();
ConfigureServices(serviceCollection);
var serviceProvider = serviceCollection.BuildServiceProvider();
var patientController = serviceProvider.GetService<PatientController>();

Console.WriteLine("-- Bem-vindo ao Cappi Hospital System --");
Console.WriteLine();
Console.WriteLine("Entre com seus dados de login");
Console.WriteLine();
Console.Write("Username: ");
string username = Console.ReadLine();
Console.Write("Password: ");
string password = ReturnPassword();

var login = new Login(username, password);

bool continueUsing = false;

do
{
    Console.Clear();

    Console.WriteLine($"-- Bem-vindo, {login.Username} --");
    Console.WriteLine();
    Console.WriteLine("Por favor, insira os dados do paciente");
    Console.Write("Nome completo: ");
    string name = Console.ReadLine();
    Console.Write("CPF: ");
    string cpf = Console.ReadLine();
    Console.Write("Telefone: ");
    string phone = Console.ReadLine();
    Console.Write("Endereço (Rua, número e/ou complemento): ");
    string address = Console.ReadLine();
    Console.Write("Bairro: ");
    string neighborhood = Console.ReadLine();
    Console.Write("CEP: ");
    string cep = Console.ReadLine();
    Console.Write("Cidade/Estado: ");
    string cityState = Console.ReadLine();
    Console.Write("Data de Nascimento (dd/MM/yyy): ");
    DateTime dateOfBirth = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
    Console.Write("E-mail: ");
    string email = Console.ReadLine();
    Console.Write("Comorbidades (Se sim, digitar e se tiver mais de uma separar por espaços. Se não, só digitar Enter): ");
    string[] comorbidities = Console.ReadLine().Split(' ');

    IList<string> list = new List<string>();
    foreach (var comorbidity in comorbidities)
    {
        if (!string.IsNullOrEmpty(comorbidity))
            list.Add(comorbidity);
    }

    Console.WriteLine("Gravando informações no banco de dados...");
    Console.WriteLine();

    var patient = new Patient(name, cpf, phone, address, neighborhood, cep, cityState, dateOfBirth, email, list);
    patient.CalculateAge();

    if (patient.Comorbidities.Count > 0 || patient.Age > 65)
    {
        Console.WriteLine($"Paciente {patient.Name} é do grupo de risco");
        Console.WriteLine("Gerando arquivo para ser enviado à Secretaria da Saúde da cidade");
        patientController.GenerateTxt(patient).Wait();
        Console.WriteLine("Arquivo gerado");
        Console.WriteLine();
    }

    Console.WriteLine("Informações salvas no banco de dados");

    Console.WriteLine();
    Console.Write("Gostaria de cadastrar outro paciente? (S/N) ");
    string answer = Console.ReadLine().ToLowerInvariant();
    continueUsing = answer == "s";
    
} while (continueUsing);



void ConfigureServices(IServiceCollection services)
{
    IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();

    services.AddScoped<IPatientService, PatientService>()
        .AddScoped<PatientController>()
        .Configure<ComputerPath>(options => config.GetSection(nameof(ComputerPath)).Bind(options));
}

string ReturnPassword()
{
    string password = string.Empty;
    ConsoleKey key;
    do
    {
        var keyInfo = Console.ReadKey(intercept: true);
        key = keyInfo.Key;

        if (key == ConsoleKey.Backspace && password.Length > 0)
        {
            Console.Write("\b \b");
            password = password[0..^1];
        }
        else if (!char.IsControl(keyInfo.KeyChar))
        {
            Console.Write("*");
            password += keyInfo.KeyChar;
        }
    } while (key != ConsoleKey.Enter);

    return password;
}