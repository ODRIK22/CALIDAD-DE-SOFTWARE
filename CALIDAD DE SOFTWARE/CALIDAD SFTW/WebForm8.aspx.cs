using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace CALIDAD_DE_SOFTWARE.CALIDAD_SFTW
{
    public partial class WebForm8 : System.Web.UI.Page
    {
        private readonly string cadenaConexion = "Server=ODRIK\\SQLEXPRESS01;Database=CalidadSFTW;User ID=sa;Password=22;TrustServerCertificate=True;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarVentas();
                using (SqlConnection con = new SqlConnection(cadenaConexion))
                {
                    try
                    {
                        string query = "SELECT id_rol FROM Sesion";
                        SqlCommand cmd = new SqlCommand(query, con);

                        con.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            int rolId = Convert.ToInt32(result);

                            if (rolId != 1)
                            {
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
        }
        protected void btnGenerarInforme_Click(object sender, EventArgs e)
        {
            // Consulta para obtener los datos de las ventas
            string query = @"
    SELECT 
        v.id AS VentaID,
        v.fecha_venta AS FechaVenta,
        v.total AS TotalVenta,
        dv.id_producto AS ProductoID,
        p.nombre AS NombreProducto,
        dv.cantidad AS Cantidad,
        dv.precio_unitario AS PrecioUnitario,
        p.precio_compra AS PrecioCompra,
        (dv.precio_unitario - p.precio_compra) * dv.cantidad AS Ganancia
    FROM Venta v
    INNER JOIN DetalleVenta dv ON v.id = dv.id_venta
    INNER JOIN Producto p ON dv.id_producto = p.id
    ORDER BY v.fecha_venta DESC;";

            DataTable dt = new DataTable();

            // Ejecutar la consulta y llenar el DataTable
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            // Calcular el total de ganancias
            decimal totalGanancias = dt.AsEnumerable().Sum(row => row.Field<decimal>("Ganancia"));

            // Crear el documento PDF
            PdfDocument pdfDocument = new PdfDocument();
            PdfPage page = pdfDocument.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Usar una fuente básica
            XFont font = new XFont("Arial", 10);
            XFont fontBold = new XFont("Arial", 10);

            int yPoint = 40; // Coordenada Y inicial

            // Título
            gfx.DrawString("Informe de Ventas", fontBold, XBrushes.Black, new XRect(0, yPoint, page.Width, page.Height), XStringFormats.TopCenter);
            yPoint += 20;

            gfx.DrawString($"Fecha: {DateTime.Now:dd/MM/yyyy}", font, XBrushes.Black, new XRect(0, yPoint, page.Width, page.Height), XStringFormats.TopCenter);
            yPoint += 30;

            // Cabecera de tabla
            int[] columnPositions = { 10, 50, 150, 250, 300, 400, 450, 500 }; // Posiciones X para cada columna

            gfx.DrawString("ID Venta", fontBold, XBrushes.Black, new XRect(columnPositions[0], yPoint, 40, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Fecha Venta", fontBold, XBrushes.Black, new XRect(columnPositions[1], yPoint, 100, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Total Venta", fontBold, XBrushes.Black, new XRect(columnPositions[2], yPoint, 100, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Producto ID", fontBold, XBrushes.Black, new XRect(columnPositions[3], yPoint, 100, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Nombre Producto", fontBold, XBrushes.Black, new XRect(columnPositions[4], yPoint, 100, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Cantidad", fontBold, XBrushes.Black, new XRect(columnPositions[5], yPoint, 100, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Precio Unitario", fontBold, XBrushes.Black, new XRect(columnPositions[6], yPoint, 100, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Ganancia", fontBold, XBrushes.Black, new XRect(columnPositions[7], yPoint, 100, page.Height), XStringFormats.TopLeft);
            yPoint += 20;

            // Contenido de la tabla
            foreach (DataRow row in dt.Rows)
            {
                gfx.DrawString(row["VentaID"].ToString(), font, XBrushes.Black, new XRect(columnPositions[0], yPoint, 40, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(Convert.ToDateTime(row["FechaVenta"]).ToString("dd/MM/yyyy"), font, XBrushes.Black, new XRect(columnPositions[1], yPoint, 100, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(string.Format("{0:C}", row["TotalVenta"]), font, XBrushes.Black, new XRect(columnPositions[2], yPoint, 100, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(row["ProductoID"].ToString(), font, XBrushes.Black, new XRect(columnPositions[3], yPoint, 100, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(row["NombreProducto"].ToString(), font, XBrushes.Black, new XRect(columnPositions[4], yPoint, 100, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(row["Cantidad"].ToString(), font, XBrushes.Black, new XRect(columnPositions[5], yPoint, 100, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(string.Format("{0:C}", row["PrecioUnitario"]), font, XBrushes.Black, new XRect(columnPositions[6], yPoint, 100, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(string.Format("{0:C}", row["Ganancia"]), font, XBrushes.Black, new XRect(columnPositions[7], yPoint, 100, page.Height), XStringFormats.TopLeft);
                yPoint += 20;

                // Crear una nueva página si el contenido excede la altura de la página
                if (yPoint > page.Height - 50)
                {
                    page = pdfDocument.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    yPoint = 40;
                }
            }

            // Mostrar el total de ganancias al final del documento
            yPoint += 20;
            gfx.DrawString($"Total de Ganancias: {totalGanancias:C}", fontBold, XBrushes.Black, new XRect(10, yPoint, page.Width, page.Height), XStringFormats.TopLeft);

            // Guardar el PDF en un archivo temporal y descargarlo
            string tempFile = Path.GetTempFileName() + ".pdf";
            pdfDocument.Save(tempFile);
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=InformeVentas.pdf");
            Response.WriteFile(tempFile);
            Response.End();
        }
        private void CargarVentas()
        {
            string query = @"
                SELECT 
                    v.id AS VentaID,
                    v.fecha_venta AS FechaVenta,
                    v.total AS TotalVenta,
                    dv.id_producto AS ProductoID,
                    p.nombre AS NombreProducto,
                    dv.cantidad AS Cantidad,
                    dv.precio_unitario AS PrecioUnitario,
                    p.precio_compra AS PrecioCompra,
                    (dv.precio_unitario - p.precio_compra) * dv.cantidad AS Ganancia
                FROM Venta v
                INNER JOIN DetalleVenta dv ON v.id = dv.id_venta
                INNER JOIN Producto p ON dv.id_producto = p.id
                ORDER BY v.fecha_venta DESC;";

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvVentas.DataSource = dt;
                gvVentas.DataBind();

                decimal gananciaTotal = dt.AsEnumerable().Sum(row => row.Field<decimal>("Ganancia"));
                lblGananciaTotal.Text = $"Ganancia Total: {gananciaTotal:C}";
            }
        }
        protected void gvVentas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvVentas.PageIndex = e.NewPageIndex;
            CargarVentas(); // Recargar los datos en el GridView
        }
    }
}