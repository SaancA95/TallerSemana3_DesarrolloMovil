using System;
using System.Collections.Generic;
using System.Text;

namespace appIEES.Models
{
    public class Contacto
    {
        public string TipoId { get; set; } = "";
        public string Id { get; set; } = "";
        public string Nombres { get; set; } = "";
        public string Apellidos { get; set; } = "";
        public DateTime Fecha { get; set; }
        public string Correo { get; set; } = "";
        public decimal Salario { get; set; }

        // Aporte personal al IESS: 9.45 % del salario (verifica el porcentaje vigente)
        public const decimal PorcentajeIess = 0.0945m;
        public decimal AporteIess => Math.Round(Salario * PorcentajeIess, 2);
    }
}
