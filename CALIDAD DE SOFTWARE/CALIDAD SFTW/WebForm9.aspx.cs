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
    public partial class WebForm9 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
        private readonly string cadenaConexion = "Server=ODRIK\\SQLEXPRESS01;Database=CalidadSFTW;User ID=sa;Password=22;TrustServerCertificate=True;";

        private void BuscarProductoPorCodigo(string codigoBarras)
        {
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                try
                {
                    string query = "SELECT nombre, precio_venta, codigo_barras, stock FROM Producto WHERE codigo_barras = @codigoBarras";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@codigoBarras", codigoBarras);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        gvResultados.DataSource = dt;
                        gvResultados.DataBind();
                        lblResultado.Text = "Producto encontrado.";
                        lblResultado.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        gvResultados.DataSource = null;
                        gvResultados.DataBind();
                        lblResultado.Text = "Producto no encontrado.";
                        lblResultado.ForeColor = System.Drawing.Color.Red;
                    }
                }
                catch (Exception ex)
                {
                    lblResultado.Text = "Error al buscar el producto: " + ex.Message;
                    lblResultado.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        protected void txtCodigoBarras_TextChanged(object sender, EventArgs e)
            {
                string codigoBarras = txtCodigoBarras.Text.Trim();

                if (string.IsNullOrEmpty(codigoBarras))
                {
                    lblResultado.Text = "Por favor, ingresa o escanea un código.";
                    lblResultado.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                BuscarProductoPorCodigo(codigoBarras);
            }
        }
    }