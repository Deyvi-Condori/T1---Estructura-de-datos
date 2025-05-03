using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1___Estructura_de_datos
{
    public class Venta
    {
        public string Cliente { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total => Cantidad * PrecioUnitario; // Propiedad calculada
    }
}
