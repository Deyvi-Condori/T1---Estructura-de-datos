using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace GestorVentas
{
    public partial class Form1 : Form
    {
        public class Venta
        {
            public string Cliente { get; set; }
            public string Producto { get; set; }
            public int Cantidad { get; set; }
            public decimal PrecioUnitario { get; set; }
            public DateTime Fecha { get; set; }
            public decimal Total => Cantidad * PrecioUnitario;
        }

        private LinkedList<Venta> listaVentas = new LinkedList<Venta>();
        private Venta ventaEditar;

        public Form1()
        {
            InitializeComponent();
            ConfigurarDataGridView();
        }

        private void ConfigurarDataGridView()
        {
            dgvVentas.AutoGenerateColumns = false;
            // Asegúrate de que DataPropertyName coincida con las propiedades de Venta
            dgvVentas.Columns["Precio"].DataPropertyName = "PrecioUnitario";
        }

        private void ActualizarDataGridView()
        {
            dgvVentas.DataSource = listaVentas.ToList();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCampos()) return;

                Venta nuevaVenta = new Venta
                {
                    Cliente = txtCliente.Text,
                    Producto = txtProducto.Text,
                    Cantidad = int.Parse(txtCantidad.Text),
                    PrecioUnitario = decimal.Parse(txtPrecio.Text),
                    Fecha = dtpFecha.Value
                };

                listaVentas.AddLast(nuevaVenta);
                LimpiarCampos();
                ActualizarDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string clienteBuscado = txtBuscarCliente.Text.Trim();
            var resultados = listaVentas.Where(v => v.Cliente.Contains(clienteBuscado)).ToList();
            dgvVentas.DataSource = resultados;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvVentas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una venta");
                return;
            }

            var ventaSeleccionada = (Venta)dgvVentas.SelectedRows[0].DataBoundItem;
            var nodo = listaVentas.Find(ventaSeleccionada);
            if (nodo != null)
            {
                listaVentas.Remove(nodo);
                ActualizarDataGridView();
            }
        }

        private void dgvVentas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            ventaEditar = (Venta)dgvVentas.Rows[e.RowIndex].DataBoundItem;
            txtCliente.Text = ventaEditar.Cliente;
            txtProducto.Text = ventaEditar.Producto;
            txtCantidad.Text = ventaEditar.Cantidad.ToString();
            txtPrecio.Text = ventaEditar.PrecioUnitario.ToString("0.00");
            dtpFecha.Value = ventaEditar.Fecha;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ventaEditar == null) return;

                ventaEditar.Producto = txtProducto.Text;
                ventaEditar.Cantidad = int.Parse(txtCantidad.Text);
                ventaEditar.PrecioUnitario = decimal.Parse(txtPrecio.Text);
                ventaEditar.Fecha = dtpFecha.Value;

                ActualizarDataGridView();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtCliente.Text)) return false;
            if (!int.TryParse(txtCantidad.Text, out _)) return false;
            if (!decimal.TryParse(txtPrecio.Text, out _)) return false;
            return true;
        }

        private void LimpiarCampos()
        {
            txtCliente.Clear();
            txtProducto.Clear();
            txtCantidad.Clear();
            txtPrecio.Clear();
            dtpFecha.Value = DateTime.Now;
        }
    }
}