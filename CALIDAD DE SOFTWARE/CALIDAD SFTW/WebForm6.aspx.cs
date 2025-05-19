using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CALIDAD_DE_SOFTWARE.CALIDAD_SFTW
{
    public partial class WebForm6 : System.Web.UI.Page
    {
        string connectionString = "Server=ODRIK\\SQLEXPRESS01;Database=CalidadSFTW;User ID=sa;Password=22;TrustServerCertificate=True;";
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
                        Response.Redirect("WebForm5.aspx");
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error al verificar el rol: " + ex.Message;
                }
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    // Eliminar todos los registros de la tabla Sesion
                    string query = "DELETE FROM Sesion;";
                    SqlCommand cmd = new SqlCommand(query, con);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = $"{rowsAffected} sesiones eliminadas correctamente.";
                }
                catch (Exception ex)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Error al cerrar las sesiones: " + ex.Message;
                }
            }
            Page_Load(sender,e);
        }
    }
}