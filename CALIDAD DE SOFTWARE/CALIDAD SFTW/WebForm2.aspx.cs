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
    public partial class WebForm2 : System.Web.UI.Page
    {
        string connectionString = "Server=ODRIK\\SQLEXPRESS01;Database=CalidadSFTW; User ID=sa;Password=22;TrustServerCertificate=True;";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                CargarProductos();
                CargarProveedores();
            }
            using (SqlConnection con = new SqlConnection(connectionString))
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
                        if (rolId == 1)
                        {
                            // Cambia "UserPage.aspx" por la página que deseas redirigir
                        }
                        else {
                            Response.Redirect("WebForm7.aspx");
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

        private void CargarProductos()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT id, nombre_producto, nombre2 FROM CatalogoProductos ORDER BY nombre_producto ASC", con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        ddlNombreProducto.DataSource = reader;
                        ddlNombreProducto.DataTextField = "nombre_producto"; // Mostrar nombres
                        ddlNombreProducto.DataValueField = "id";             // Usar IDs internamente
                        ddlNombreProducto.DataBind();
                    }
                    else
                    {
                        ddlNombreProducto.Items.Clear();
                        ddlNombreProducto.Items.Add(new ListItem("No hay productos disponibles", ""));
                    }

                    ddlNombreProducto.Items.Insert(0, new ListItem("Seleccione un producto", ""));
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error al cargar los productos: " + ex.Message;
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT id, nombre, precio_compra, precio_venta, stock, codigo_barras, id_proveedor, nombre2 FROM Producto";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        try
                        {
                            connection.Open();
                            adapter.Fill(dt);
                            gvProductos.DataSource = dt;
                            gvProductos.DataBind();
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Text = "Error al cargar los productos: " + ex.Message;
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }
        }
        protected void gvProductos_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvProductos.PageIndex = e.NewPageIndex;
            CargarProductos();
        }

        private void CargarProveedores()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT id, nombre FROM Proveedor", con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        ddlProveedor.DataSource = reader;
                        ddlProveedor.DataTextField = "nombre"; // Mostrar nombres
                        ddlProveedor.DataValueField = "id";    // Usar IDs internamente
                        ddlProveedor.DataBind();
                    }
                    else
                    {
                        ddlProveedor.Items.Clear();
                        ddlProveedor.Items.Add(new ListItem("No hay proveedores disponibles", ""));
                    }

                    ddlProveedor.Items.Insert(0, new ListItem("Seleccione un proveedor", ""));
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error al cargar los proveedores: " + ex.Message;
                }
            }
        }

        protected void btnInsertar_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "INSERT INTO Producto (nombre, precio_compra, precio_venta, stock, codigo_barras, id_proveedor, nombre2) " +
                                   "VALUES (@nombre, @precio_compra, @precio_venta, @stock, @codigo_barras, @id_proveedor, @nombre2)";
                    SqlCommand cmd = new SqlCommand(query, con);

                    // Parámetros
                    cmd.Parameters.AddWithValue("@nombre", ddlNombreProducto.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@precio_compra", decimal.Parse(txtPrecioCompra.Text));
                    cmd.Parameters.AddWithValue("@precio_venta", decimal.Parse(txtPrecioVenta.Text));
                    cmd.Parameters.AddWithValue("@stock", int.Parse(txtStock.Text));
                    cmd.Parameters.AddWithValue("@codigo_barras", txtIdProducto.Text); // O ajusta si tienes un campo para código de barras
                    cmd.Parameters.AddWithValue("@id_proveedor", ddlProveedor.SelectedValue == "" ? DBNull.Value : (object)ddlProveedor.SelectedValue);
                    cmd.Parameters.AddWithValue("@nombre2", txtNombreDetallado.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    lblMessage.Text = "Producto insertado con éxito.";
                    CargarProductos();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error al insertar el producto: " + ex.Message;
                }
            }
            ddlNombreProducto.Text = "";
            txtIdProducto.Text = "";
            txtNombreDetallado.Text = "";
            txtPrecioCompra.Text = "";
            txtPrecioVenta.Text = "";
            txtStock.Text = "";
        }

        protected void ddlNombreProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlNombreProducto.SelectedIndex > 0)
            {
                txtIdProducto.Text = ddlNombreProducto.SelectedValue;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("SELECT nombre2 FROM CatalogoProductos WHERE id = @id", con);
                        cmd.Parameters.AddWithValue("@id", ddlNombreProducto.SelectedValue);

                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            txtNombreDetallado.Text = reader["nombre2"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error al cargar el nombre detallado: " + ex.Message;
                    }
                }
            }
            else
            {
                txtIdProducto.Text = string.Empty;
                txtNombreDetallado.Text = string.Empty;
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdProducto.Text))
            {
                lblMessage.Text = "Por favor, selecciona un producto para actualizar.";
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "UPDATE Producto " +
                                   "SET precio_compra = @precio_compra, precio_venta = @precio_venta, " +
                                   "stock = @stock, codigo_barras = @codigo_barras, nombre2 = @nombre2 " +
                                   "WHERE nombre2 = @nombre2";

                    SqlCommand cmd = new SqlCommand(query, con);

                    // Asignación de parámetros
                    cmd.Parameters.AddWithValue("@precio_compra", decimal.Parse(txtPrecioCompra.Text));
                    cmd.Parameters.AddWithValue("@precio_venta", decimal.Parse(txtPrecioVenta.Text));
                    cmd.Parameters.AddWithValue("@stock", int.Parse(txtStock.Text));
                    cmd.Parameters.AddWithValue("@codigo_barras", txtIdProducto.Text);
                    cmd.Parameters.AddWithValue("@id_proveedor", ddlProveedor.SelectedValue == "" ? DBNull.Value : (object)ddlProveedor.SelectedValue);
                    cmd.Parameters.AddWithValue("@nombre2", txtNombreDetallado.Text);
                    cmd.Parameters.AddWithValue("@id", int.Parse(txtIdProducto.Text));

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        lblMessage.Text = "Producto actualizado con éxito.";
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        CargarProductos(); // Recarga los datos en la GridView
                    }
                    else
                    {
                        lblMessage.Text = "No se encontró el producto para actualizar.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error al actualizar el producto: " + ex.Message;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }

            // Limpiar los campos después de actualizar
            ddlNombreProducto.SelectedIndex = 0;
            txtIdProducto.Text = "";
            txtNombreDetallado.Text = "";
            txtPrecioCompra.Text = "";
            txtPrecioVenta.Text = "";
            txtStock.Text = "";
            gvProductos.DataBind();
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombreDetallado.Text))
            {
                lblMessage.Text = "Por favor, selecciona un producto para eliminar.";
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "DELETE FROM Producto WHERE nombre2 = @nombre2";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@nombre2", txtNombreDetallado.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    lblMessage.Text = "Producto eliminado con éxito.";
                    CargarProductos();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error al eliminar el producto: " + ex.Message;
                }
            }
            ddlNombreProducto.Text = "";
            txtIdProducto.Text = "";
            txtNombreDetallado.Text = "";
            txtPrecioCompra.Text = "";
            txtPrecioVenta.Text = "";
            txtStock.Text = "";
        }
        protected void gvProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obtén la fila seleccionada
            GridViewRow selectedRow = gvProductos.SelectedRow;

            // Rellena los campos del formulario con los datos de la fila seleccionada
            txtIdProducto.Text = selectedRow.Cells[5].Text.Replace("$", "").Trim(); // Precio Compra

            txtPrecioCompra.Text = selectedRow.Cells[2].Text.Replace("$", "").Trim(); // Precio Compra
            txtPrecioVenta.Text = selectedRow.Cells[3].Text.Replace("$", "").Trim(); // Precio Venta
            txtStock.Text = selectedRow.Cells[4].Text; // Stock Disponible
                                                       // Asigna el Código de Barras y otros valores
            txtNombreDetallado.Text = selectedRow.Cells[7].Text;


            lblMessage.Text = "Producto seleccionado correctamente.";
            lblMessage.ForeColor = System.Drawing.Color.Green;
        }
    }
    }