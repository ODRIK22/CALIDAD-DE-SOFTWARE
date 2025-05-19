using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CALIDAD_DE_SOFTWARE.CALIDAD_SFTW
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string connectionString = "Server=ODRIK\\SQLEXPRESS01;Database=CalidadSFTW;User ID=sa;Password=22;TrustServerCertificate=True;";
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
            CargarProveedores();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=ODRIK\\SQLEXPRESS01;Database=CalidadSFTW;User ID=sa;Password=22;TrustServerCertificate=True;";
            string query = "INSERT INTO Proveedor (nombre, telefono, empresa) VALUES (@nombre, @telefono, @empresa)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@nombre", txtNombre.Text);
                    command.Parameters.AddWithValue("@telefono", txtTelefono.Text);
                    command.Parameters.AddWithValue("@empresa", txtEmpresa.Text);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    lblMensaje.Text = "Proveedor guardado exitosamente.";

                    // Limpiar campos después de guardar
                    LimpiarCampos();
                    lblMessage.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error: " + ex.Message;
            }
            CargarProveedores();
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=ODRIK\\SQLEXPRESS01;Database=CalidadSFTW;User ID=sa;Password=22;TrustServerCertificate=True;";
            string query = "UPDATE Proveedor SET telefono = @telefono, empresa = @empresa WHERE nombre = @nombre";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@nombre", txtNombre.Text);
                    command.Parameters.AddWithValue("@telefono", txtTelefono.Text);
                    command.Parameters.AddWithValue("@empresa", txtEmpresa.Text);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    lblMensaje.Text = "Proveedor actualizado exitosamente.";

                    // Limpiar campos después de actualizar
                    LimpiarCampos();
                    lblMessage.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error: " + ex.Message;
            }
            CargarProveedores();
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=ODRIK\\SQLEXPRESS01;Database=CalidadSFTW;User ID=sa;Password=22;TrustServerCertificate=True;";
            string query = "DELETE FROM Proveedor WHERE nombre = @nombre";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@nombre", txtNombre.Text);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    lblMensaje.Text = "Proveedor eliminado exitosamente.";

                    // Limpiar campos después de eliminar
                    LimpiarCampos();
                    CargarProveedores();
                    lblMessage.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error: " + ex.Message;
            }
            
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtEmpresa.Text = string.Empty;
        }
        private void CargarProveedores()
        {
            using (SqlConnection con = new SqlConnection("Server=ODRIK\\SQLEXPRESS01;Database=CalidadSFTW;User ID=sa;Password=22;TrustServerCertificate=True;"))
            {
                string query = "SELECT id, nombre, telefono, Empresa FROM Proveedor";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    try
                    {
                        con.Open();
                        adapter.Fill(dt);
                        gvProveedores.DataSource = dt;
                        gvProveedores.DataBind();
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error al cargar los proveedores: " + ex.Message;
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }
        protected void gvProveedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow selectedRow = gvProveedores.SelectedRow;

            txtNombre.Text = selectedRow.Cells[1].Text; // Nombre
            txtTelefono.Text = selectedRow.Cells[2].Text; // Teléfono
            txtEmpresa.Text = selectedRow.Cells[3].Text; // Empresa

            lblMessage.Text = "Proveedor seleccionado correctamente.";
            lblMessage.ForeColor = System.Drawing.Color.Green;
        }
        protected void gvProveedores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProveedores.PageIndex = e.NewPageIndex;
            CargarProveedores(); // Recarga los datos de la tabla
        }
    }
}