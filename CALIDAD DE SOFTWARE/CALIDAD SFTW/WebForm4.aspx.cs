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
    public partial class WebForm4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
                        if (rolId == 2 || rolId == 1)
                        {
                             // Cambia "UserPage.aspx" por la página que deseas redirigir
                        }
                    }
                    else
                    {
                        lblMessage.Text = "No se encontró información del usuario.";
                        Response.Redirect("Webform5.aspx");
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error al verificar el rol: " + ex.Message;
                }
            }
        }
        string connectionString = "Server=ODRIK\\SQLEXPRESS01;Database=CalidadSFTW; User ID=sa;Password=22;TrustServerCertificate=True;";

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterio = ddlCriterio.SelectedValue; // Obtiene el criterio seleccionado (nombre2 o codigo_barras)
            string busqueda = txtBusqueda.Text.Trim();  // Obtiene el valor ingresado en el campo de búsqueda

            if (string.IsNullOrEmpty(busqueda))
            {
                lblMessage.Text = "Por favor, ingresa un valor para buscar.";
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    // Consulta SQL dinámica basada en el criterio de búsqueda
                    string query = $"SELECT id, nombre, nombre2, codigo_barras, precio_compra, precio_venta, stock FROM Producto WHERE {criterio} LIKE @busqueda";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@busqueda", "%" + busqueda + "%"); // Búsqueda parcial (LIKE)

                    // Ejecutamos la consulta y cargamos los resultados en un DataTable
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Mostrar los resultados en el GridView
                    gvResultados.DataSource = dt;
                    gvResultados.DataBind();

                    lblMessage.Text = $"{dt.Rows.Count} producto(s) encontrado(s).";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error al buscar el producto: " + ex.Message;
                }
            }
        }
    }
}