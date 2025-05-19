using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CALIDAD_DE_SOFTWARE.CALIDAD_SFTW
{
    public partial class WebForm5 : System.Web.UI.Page
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
                        if (rolId == 1 || rolId== 2)
                        {
                            
                            Response.Redirect("WebForm7.aspx");
                        }
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error al verificar el rol: " + ex.Message;
                }
            }
        }
        protected void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            string email = txtCorreo.Text.Trim();
            string password = txtContraseña.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                lblMessage.Text = "Por favor, ingresa tu correo y contraseña.";
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    // Consulta para verificar el usuario y obtener datos
                    string query = "SELECT id, nombre, apellido, id_rol FROM Usuario WHERE correo_electronico = @correo AND contraseña = @contraseña";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@correo", email);
                    cmd.Parameters.AddWithValue("@contraseña", password);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Usuario encontrado, guarda datos en sesión
                        int userId = Convert.ToInt32(reader["id"]);
                        string nombreUsuario = reader["nombre"] + " " + reader["apellido"];
                        int rolId = Convert.ToInt32(reader["id_rol"]);

                        Session["UsuarioID"] = userId;
                        Session["NombreUsuario"] = nombreUsuario;
                        Session["RolID"] = rolId;

                        // Registrar la sesión en la base de datos
                        RegistrarSesion(userId, nombreUsuario, rolId);

                        // Redirigir según el rol
                        if (rolId == 1) // Administrador
                        {
                            Response.Redirect("WebForm3.aspx");
                        }
                        else if (rolId == 2) // Usuario
                        {
                            Response.Redirect("WebForm5");
                        }
                        else
                        {
                            Response.Redirect("WebForn5.aspx");
                        }
                    }
                    else
                    {
                        lblMessage.Text = "Correo o contraseña incorrectos.";
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error al iniciar sesión: " + ex.Message;
                }
            }
        }

        private void RegistrarSesion(int userId, string nombreUsuario, int rolId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "INSERT INTO Sesion (id_usuario, nombre_usuario, id_rol) VALUES (@id_usuario, @nombre_usuario, @id_rol)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id_usuario", userId);
                    cmd.Parameters.AddWithValue("@nombre_usuario", nombreUsuario);
                    cmd.Parameters.AddWithValue("@id_rol", rolId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error al registrar la sesión: " + ex.Message;
                }
            }
        }
    }
}