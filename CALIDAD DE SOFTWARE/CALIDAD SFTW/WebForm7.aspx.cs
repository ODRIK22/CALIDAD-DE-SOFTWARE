using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CALIDAD_DE_SOFTWARE.CALIDAD_SFTW
{
    public partial class WebForm7 : System.Web.UI.Page
    {
        private readonly string cadenaConexion = "Server=ODRIK\\SQLEXPRESS01;Database=CalidadSFTW;User ID=sa;Password=22;TrustServerCertificate=True;";

        // Carga inicial de la página
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProductos();
                CargarProductosDropdown();
            }
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                try
                {
                    // Consulta para obtener el id_rol del usuario actual
                    string query = "SELECT id_rol FROM Sesion";
                    SqlCommand cmd = new SqlCommand(query, con);

                    con.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        int rolId = Convert.ToInt32(result);

                        // Verificar si el rol es Usuario (id_rol = 2)
                        if (rolId == 2 || rolId == 1)
                        {
                            // Cambia "UserPage.aspx" por la página que deseas redirigir
                        }

                    }
                    else
                    {

                        lblMessage.Text = "No se encontró información del usuario.";
                        Response.Redirect("WebForm5.aspx");
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error al verificar el rol: " + ex.Message;
                }
            }
        }

        // Carga los productos en el GridView
        private void CargarProductos()
        {
            string query = "SELECT id, nombre, precio_venta, stock FROM Producto";

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvProductos.DataSource = dt;
                gvProductos.DataBind();
            }
        }

        // Carga los productos en el DropDownList
        private void CargarProductosDropdown()
        {
            string query = "SELECT id, nombre FROM Producto";

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    ddlProductos.DataSource = dt;
                    ddlProductos.DataTextField = "nombre";
                    ddlProductos.DataValueField = "id";
                    ddlProductos.DataBind();
                }

                ddlProductos.Items.Insert(0, new ListItem("Selecciona un producto", "0"));
            }
        }

        // Botón de compra
        protected void btnComprar_Click(object sender, EventArgs e)
        {
            if (ddlProductos.SelectedValue == "0")
            {
                lblMensaje.Text = "Por favor, selecciona un producto.";
                return;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                lblMensaje.Text = "Por favor, ingresa una cantidad válida mayor a 0.";
                return;
            }

            int productoId = Convert.ToInt32(ddlProductos.SelectedValue);
            decimal precioUnitario = ObtenerPrecioUnitario(productoId);

            if (precioUnitario == -1)
            {
                lblMensaje.Text = "Producto no encontrado.";
                return;
            }

            int stockDisponible = ObtenerStockDisponible(productoId);
            if (cantidad > stockDisponible)
            {
                lblMensaje.Text = $"No hay suficiente stock. Stock disponible: {stockDisponible}.";
                return;
            }

            decimal total = precioUnitario * cantidad;
            if (GuardarVenta(productoId, cantidad, precioUnitario, total))
            {
                lblMensaje.Text = "Compra realizada con éxito.";
                CargarProductos(); // Recargar el GridView con el stock actualizado
            }
            else
            {
                lblMensaje.Text = "Hubo un error al procesar la compra.";
            }
        }

        // Obtiene el precio de venta del producto
        private decimal ObtenerPrecioUnitario(int productoId)
        {
            string query = "SELECT precio_venta FROM Producto WHERE id = @productoId";

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@productoId", productoId);
                conn.Open();
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : -1;
            }
        }

        // Obtiene el stock disponible del producto
        private int ObtenerStockDisponible(int productoId)
        {
            string query = "SELECT stock FROM Producto WHERE id = @productoId";

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@productoId", productoId);
                conn.Open();
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        // Procesa la venta y actualiza la base de datos
        private bool GuardarVenta(int productoId, int cantidad, decimal precioUnitario, decimal total)
        {
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Inserta la venta
                    string insertVentaQuery = "INSERT INTO Venta (id_usuario, fecha_venta, total) VALUES (@idUsuario, @fechaVenta, @total)";
                    SqlCommand cmdVenta = new SqlCommand(insertVentaQuery, conn, transaction);
                    cmdVenta.Parameters.AddWithValue("@idUsuario", 1); // Usuario fijo (ajustar según autenticación)
                    cmdVenta.Parameters.AddWithValue("@fechaVenta", DateTime.Now);
                    cmdVenta.Parameters.AddWithValue("@total", total);
                    cmdVenta.ExecuteNonQuery();

                    // Obtiene el ID de la venta recién insertada
                    string selectVentaQuery = "SELECT TOP 1 id FROM Venta ORDER BY id DESC";
                    SqlCommand cmdSelectVenta = new SqlCommand(selectVentaQuery, conn, transaction);
                    int idVenta = Convert.ToInt32(cmdSelectVenta.ExecuteScalar());

                    // Inserta el detalle de la venta
                    string insertDetalleQuery = "INSERT INTO DetalleVenta (id_venta, id_producto, cantidad, precio_unitario) VALUES (@idVenta, @idProducto, @cantidad, @precioUnitario)";
                    SqlCommand cmdDetalle = new SqlCommand(insertDetalleQuery, conn, transaction);
                    cmdDetalle.Parameters.AddWithValue("@idVenta", idVenta);
                    cmdDetalle.Parameters.AddWithValue("@idProducto", productoId);
                    cmdDetalle.Parameters.AddWithValue("@cantidad", cantidad);
                    cmdDetalle.Parameters.AddWithValue("@precioUnitario", precioUnitario);
                    cmdDetalle.ExecuteNonQuery();

                    // Actualiza el stock del producto
                    string updateStockQuery = "UPDATE Producto SET stock = stock - @cantidad WHERE id = @productoId";
                    SqlCommand cmdStock = new SqlCommand(updateStockQuery, conn, transaction);
                    cmdStock.Parameters.AddWithValue("@cantidad", cantidad);
                    cmdStock.Parameters.AddWithValue("@productoId", productoId);
                    cmdStock.ExecuteNonQuery();

                    // Confirmar la transacción
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        // Manejo de paginación en el GridView
        protected void gvProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProductos.PageIndex = e.NewPageIndex;
            CargarProductos(); // Recargar los productos al cambiar de página
        }
        protected void ddlProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Verifica que se seleccionó un producto válido
            if (ddlProductos.SelectedValue != "0")
            {
                int productoId = Convert.ToInt32(ddlProductos.SelectedValue);

                // Aquí puedes cargar detalles adicionales del producto seleccionado si es necesario
                lblMensaje.Text = $"Producto seleccionado: {ddlProductos.SelectedItem.Text} (ID: {productoId})";
            }
            else
            {
                lblMensaje.Text = "Por favor, selecciona un producto válido.";
            }
        }
    }
}