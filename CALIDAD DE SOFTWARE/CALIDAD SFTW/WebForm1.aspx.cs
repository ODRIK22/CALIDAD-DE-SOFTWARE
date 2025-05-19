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
    public partial class WebForm1 : System.Web.UI.Page
    {
        // Cadena de conexión para la base de datos
        private readonly string cadenaConexion = "Server=ODRIK\\SQLEXPRESS01;Database=CalidadSFTW;User ID=sa;Password=22;TrustServerCertificate=True;";

      

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarRoles();
                CargarCodigosPostales();
            }
        }

        private void CargarRoles()
        {
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                string query = "SELECT id, nombre FROM Rol";
                SqlCommand comando = new SqlCommand(query, conexion);

                try
                {
                    conexion.Open();
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    DataTable dt = new DataTable();
                    adaptador.Fill(dt);

                    ddlRol.DataSource = dt;
                    ddlRol.DataTextField = "nombre";
                    ddlRol.DataValueField = "id";
                    ddlRol.DataBind();
                    ddlRol.Items.Insert(0, new ListItem("-- Seleccione un Rol --", "0"));
                }
                catch (Exception ex)
                {
                    lblMensaje.Text = "Error al cargar los roles: " + ex.Message;
                }
            }
        }

        private void CargarCodigosPostales()
        {
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                string query = "SELECT id, estado FROM CP";
                SqlCommand comando = new SqlCommand(query, conexion);

                try
                {
                    conexion.Open();
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    DataTable dt = new DataTable();
                    adaptador.Fill(dt);

                    ddlCodigoPostal.DataSource = dt;
                    ddlCodigoPostal.DataTextField = "id"; // Código Postal
                    ddlCodigoPostal.DataValueField = "id"; // ID del CP
                    ddlCodigoPostal.DataBind();
                    ddlCodigoPostal.Items.Insert(0, new ListItem("-- Seleccione un Código Postal --", "0"));
                }
                catch (Exception ex)
                {
                    lblMensaje.Text = "Error al cargar los códigos postales: " + ex.Message;
                }
            }
        }

        protected void ddlCodigoPostal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCodigoPostal.SelectedValue != "0")
            {
                using (SqlConnection conexion = new SqlConnection(cadenaConexion))
                {
                    string query = "SELECT estado FROM CP WHERE id = @id";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@id", ddlCodigoPostal.SelectedValue);

                    try
                    {
                        conexion.Open();
                        string estado = comando.ExecuteScalar()?.ToString();
                        txtEstado.Text = estado ?? string.Empty;
                    }
                    catch (Exception ex)
                    {
                        lblMensaje.Text = "Error al cargar el estado: " + ex.Message;
                    }
                }
            }
            else
            {
                txtEstado.Text = string.Empty;
            }
        }

        protected void btnCrearUsuario_Click(object sender, EventArgs e)
        {
            if (ddlRol.SelectedValue == "0" || ddlCodigoPostal.SelectedValue == "0")
            {
                lblMensaje.Text = "Por favor, complete todos los campos correctamente.";
                return;
            }

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                string query = "INSERT INTO Usuario (nombre, apellido, correo_electronico, contraseña, id_rol, id_cp) VALUES (@nombre, @apellido, @correo, @contraseña, @id_rol, @id_cp)";
                SqlCommand comando = new SqlCommand(query, conexion);

                comando.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                comando.Parameters.AddWithValue("@apellido", txtApellido.Text.Trim());
                comando.Parameters.AddWithValue("@correo", txtCorreo.Text.Trim());
                comando.Parameters.AddWithValue("@contraseña", txtContraseña.Text.Trim());
                comando.Parameters.AddWithValue("@id_rol", ddlRol.SelectedValue);
                comando.Parameters.AddWithValue("@id_cp", ddlCodigoPostal.SelectedValue);

                try
                {
                    conexion.Open();
                    int resultado = comando.ExecuteNonQuery();
                    if (resultado > 0)
                    {
                        lblMensaje.ForeColor = System.Drawing.Color.Green;
                        lblMensaje.Text = "Usuario creado exitosamente.";
                        LimpiarCampos();
                    }
                    else
                    {
                        lblMensaje.Text = "No se pudo crear el usuario.";
                    }
                }
                catch (Exception ex)
                {
                    lblMensaje.Text = "Error: " + ex.Message;
                }
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtContraseña.Text = string.Empty;
            txtEstado.Text = string.Empty;
            ddlRol.SelectedIndex = 0;
            ddlCodigoPostal.SelectedIndex = 0;
        }
    }
}