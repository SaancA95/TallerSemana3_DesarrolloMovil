using System.Globalization;
using System.Text;
using appIEES.Models;

namespace appIEES.Views;

public partial class ResumenPage : ContentPage
{
    private readonly Contacto _contacto;
    private static readonly CultureInfo Cultura = CultureInfo.InvariantCulture;

    public ResumenPage(Contacto contacto)
	{
		InitializeComponent();
        _contacto = contacto;

        IdLabel.Text = $"{contacto.TipoId}: {contacto.Id}";
        NombresLabel.Text = contacto.Nombres;
        ApellidosLabel.Text = contacto.Apellidos;
        FechaLabel.Text = contacto.Fecha.ToString("dd/MM/yyyy");
        CorreoLabel.Text = contacto.Correo;
        SalarioLabel.Text = contacto.Salario.ToString("C2", Cultura);
        AporteLabel.Text = contacto.AporteIess.ToString("C2", Cultura);
    }

    private async void OnExportarClicked(object? sender, EventArgs e)
    {
        try
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== DATOS DEL CONTACTO ===");
            sb.AppendLine($"Tipo de identificación: {_contacto.TipoId}");
            sb.AppendLine($"Identificación: {_contacto.Id}");
            sb.AppendLine($"Nombres: {_contacto.Nombres}");
            sb.AppendLine($"Apellidos: {_contacto.Apellidos}");
            sb.AppendLine($"Fecha: {_contacto.Fecha:dd/MM/yyyy}");
            sb.AppendLine($"Correo: {_contacto.Correo}");
            sb.AppendLine($"Salario: {_contacto.Salario.ToString("C2", Cultura)}");
            sb.AppendLine($"Aporte IESS (9.45%): {_contacto.AporteIess.ToString("C2", Cultura)}");

            var ruta = Path.Combine(FileSystem.AppDataDirectory,
                                    $"contacto_{_contacto.Id}.txt");
            await File.WriteAllTextAsync(ruta, sb.ToString());

            // Permite guardar/compartir el archivo desde el dispositivo
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "Exportar contacto",
                File = new ShareFile(ruta)
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo exportar: {ex.Message}", "OK");
        }
    }
}