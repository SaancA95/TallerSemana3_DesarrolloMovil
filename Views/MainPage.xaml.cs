using appIEES.Models;
using System.Globalization;
using System.Text.RegularExpressions;
using appIEES.Models;

namespace appIEES.Views
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            FechaPicker.MaximumDate = DateTime.Today;
            TipoPicker.SelectedIndex = 0;
        }

        // Ajusta teclado y longitud según el tipo de identificación
        private void OnTipoChanged(object? sender, EventArgs e)
        {
            IdentificacionEntry.Text = string.Empty;

            switch (TipoPicker.SelectedItem as string)
            {
                case "CI":
                    IdentificacionEntry.Keyboard = Keyboard.Numeric;
                    IdentificacionEntry.MaxLength = 10;
                    break;
                case "RUC":
                    IdentificacionEntry.Keyboard = Keyboard.Numeric;
                    IdentificacionEntry.MaxLength = 13;
                    break;
                default: // Pasaporte (libre)
                    IdentificacionEntry.Keyboard = Keyboard.Default;
                    IdentificacionEntry.MaxLength = 20;
                    break;
            }
        }

        private async void OnGuardarClicked(object? sender, EventArgs e)
        {
            var errores = new List<string>();
            var tipo = TipoPicker.SelectedItem as string;
            var id = IdentificacionEntry.Text?.Trim() ?? "";
            var nombres = NombresEntry.Text?.Trim() ?? "";
            var apellidos = ApellidosEntry.Text?.Trim() ?? "";
            var correo = CorreoEntry.Text?.Trim() ?? "";

            // Identificación
            if (tipo is null)
                errores.Add("• Seleccione un tipo de identificación.");
            else if (tipo == "CI" && !Regex.IsMatch(id, @"^\d{10}$"))
                errores.Add("• La CI debe tener exactamente 10 números.");
            else if (tipo == "RUC" && !Regex.IsMatch(id, @"^\d{13}$"))
                errores.Add("• El RUC debe tener exactamente 13 números.");
            else if (tipo == "Pasaporte" && string.IsNullOrWhiteSpace(id))
                errores.Add("• Ingrese el número de pasaporte.");

            // Nombres y apellidos (solo letras y espacios)
            var soloLetras = @"^[A-Za-zÁÉÍÓÚáéíóúÑñÜü\s]+$";
            if (!Regex.IsMatch(nombres, soloLetras))
                errores.Add("• Nombres: obligatorio, solo letras.");
            if (!Regex.IsMatch(apellidos, soloLetras))
                errores.Add("• Apellidos: obligatorio, solo letras.");

            // Fecha
            if (FechaPicker.Date > DateTime.Today)
                errores.Add("• La fecha no puede ser futura.");

            // Correo
            if (!Regex.IsMatch(correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                errores.Add("• Correo electrónico inválido.");

            // Salario
            var textoSalario = (SalarioEntry.Text ?? "").Replace(',', '.');
            if (!decimal.TryParse(textoSalario, NumberStyles.Number,
                    CultureInfo.InvariantCulture, out var salario) || salario <= 0)
                errores.Add("• El salario debe ser un número mayor a 0.");

            if (errores.Count > 0)
            {
                ErroresLabel.Text = string.Join(Environment.NewLine, errores);
                return;
            }

            ErroresLabel.Text = string.Empty;

            var contacto = new Contacto
            {
                TipoId = tipo!,
                Id = id,
                Nombres = nombres,
                Apellidos = apellidos,
                Fecha = (DateTime)FechaPicker.Date,
                Correo = correo,
                Salario = salario
            };

            await Navigation.PushAsync(new ResumenPage(contacto));
        }
    
    }
}
