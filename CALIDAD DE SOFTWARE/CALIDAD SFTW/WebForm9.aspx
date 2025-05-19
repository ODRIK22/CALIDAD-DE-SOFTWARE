<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm9.aspx.cs" Inherits="CALIDAD_DE_SOFTWARE.CALIDAD_SFTW.WebForm9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblMessage" runat="server" Text="Label"></asp:Label>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Escáner de Códigos de Barras</title>
    <style>
        #video {
            width: 100%;
            height: auto;
            border: 1px solid black;
        }
        #resultado {
            margin-top: 20px;
            font-size: 18px;
            font-weight: bold;
            color: blue;
        }
        #mensaje {
            color: green;
            font-weight: bold;
            margin-top: 10px;
        }
        #error {
            color: red;
            font-weight: bold;
            margin-top: 10px;
        }
    </style>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/quagga/0.12.1/quagga.min.js"></script>
</head>
<body>
    <h1>Escáner de Códigos de Barras</h1>
    <video id="video" autoplay></video>
    <div id="resultado">Esperando detección...</div>
    <div id="mensaje"></div>
    <div id="error"></div>

    <!-- TextBox donde se guarda el código escaneado -->
  <asp:TextBox 
    ID="txtCodigoBarras" 
    runat="server" 
    CssClass="textbox" 
    Width="100%" 
    OnTextChanged="txtCodigoBarras_TextChanged" 
    AutoPostBack="true">
</asp:TextBox>


    <!-- Label para mostrar mensajes adicionales -->
    <asp:Label ID="lblResultado" runat="server" CssClass="label"></asp:Label>

    <!-- GridView para mostrar resultados -->
    <asp:GridView ID="gvResultados" runat="server" AutoGenerateColumns="True" CssClass="gridview"></asp:GridView>

    <script>
        let escaneando = true;

        async function iniciarCamaraYEscaner() {
            const mensaje = document.getElementById("mensaje");
            const error = document.getElementById("error");
            const resultado = document.getElementById("resultado");

            try {
                const constraints = { video: { facingMode: "environment" } };
                const stream = await navigator.mediaDevices.getUserMedia(constraints);
                const video = document.getElementById("video");
                video.srcObject = stream;

                mensaje.innerText = "Cámara iniciada correctamente. Escaneando...";
                error.innerText = "";

                Quagga.init({
                    inputStream: {
                        type: "LiveStream",
                        target: document.querySelector("#video"),
                        constraints: {
                            width: 640,
                            height: 480,
                            facingMode: "environment",
                        },
                    },
                    decoder: {
                        readers: ["code_128_reader", "ean_reader", "upc_reader"],
                    }
                }, function (err) {
                    if (err) {
                        console.error("Error al iniciar QuaggaJS:", err);
                        error.innerText = "Error al iniciar el escáner.";
                        mensaje.innerText = "";
                        return;
                    }
                    console.log("QuaggaJS iniciado correctamente.");
                    Quagga.start();
                });

                Quagga.onDetected(function (data) {
                    if (!escaneando) return;

                    const codigoBarras = data.codeResult.code;
                    console.log("Código detectado:", codigoBarras);

                    resultado.innerText = `Código detectado: ${codigoBarras}`;
                    mensaje.innerText = "Código guardado en el campo de texto.";
                    error.innerText = "";

                    // Guardar el código en el TextBox
                    document.getElementById('<%= txtCodigoBarras.ClientID %>').value = codigoBarras;

    // Disparar el evento TextChanged
      document.getElementById('<%= txtCodigoBarras.ClientID %>').dispatchEvent(new Event('change'));

      escaneando = false;

      setTimeout(() => {
          escaneando = true;
      }, 2000);
  });
            } catch (err) {
                console.error("Error al acceder a la cámara:", err);
                error.innerText = "No se pudo acceder a la cámara. Verifica los permisos.";
                mensaje.innerText = "";
            }
        }

        // Iniciar la cámara y el escáner al cargar la página
        window.onload = iniciarCamaraYEscaner;
    </script>
</body>
</html>
</asp:Content>
