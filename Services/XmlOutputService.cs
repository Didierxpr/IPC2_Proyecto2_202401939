using System.Xml;
using IPC2_Proyecto2_202401939.Domain;

namespace IPC2_Proyecto2_202401939.Services;

/// <summary>
/// Servicio responsable de generar el archivo XML de salida con los resultados
/// de la optimización de los mensajes. Genera la estructura especificada en el enunciado
/// con tiempo óptimo, mensaje recibido e instrucciones detalladas.
/// </summary>
public class XmlOutputService
{
    /// <summary>
    /// Genera un documento XML con los resultados optimizados de los mensajes.
    /// </summary>
    public string GenerarXmlSalida(
        MensajeOptimizado[] mensajesOptimizados)
    {
        XmlDocument doc = new();

        // Crear declaración XML
        XmlDeclaration declaracion = doc.CreateXmlDeclaration("1.0", null, null);
        doc.AppendChild(declaracion);

        // Crear elemento raíz
        XmlElement raiz = doc.CreateElement("respuesta");
        doc.AppendChild(raiz);

        // Crear lista de mensajes
        XmlElement listaMensajes = doc.CreateElement("listaMensajes");
        raiz.AppendChild(listaMensajes);

        // Agregar cada mensaje optimizado
        foreach (var mensajeOpt in mensajesOptimizados)
        {
            XmlElement mensaje = CrearElementoMensaje(doc, mensajeOpt);
            listaMensajes.AppendChild(mensaje);
        }

        return doc.OuterXml;
    }

    /// <summary>
    /// Crear un elemento XML para un mensaje optimizado.
    /// </summary>
    private XmlElement CrearElementoMensaje(
        XmlDocument doc,
        MensajeOptimizado mensajeOpt)
    {
        XmlElement mensaje = doc.CreateElement("mensaje");
        mensaje.SetAttribute("nombre", mensajeOpt.Nombre);

        // Sistema de drones
        XmlElement sistemaDrones = doc.CreateElement("sistemaDrones");
        sistemaDrones.InnerText = mensajeOpt.NombreSistemaDrones;
        mensaje.AppendChild(sistemaDrones);

        // Tiempo óptimo
        XmlElement tiempoOptimo = doc.CreateElement("tiempoOptimo");
        tiempoOptimo.InnerText = mensajeOpt.TiempoOptimo.ToString();
        mensaje.AppendChild(tiempoOptimo);

        // Mensaje recibido (decodificado)
        XmlElement mensajeRecibido = doc.CreateElement("mensajeRecibido");
        mensajeRecibido.InnerText = mensajeOpt.MensajeRecibido;
        mensaje.AppendChild(mensajeRecibido);

        // Instrucciones
        XmlElement instrucciones = doc.CreateElement("instrucciones");
        foreach (var instruccionTiempo in mensajeOpt.InstruccionesPorTiempo)
        {
            XmlElement tiempo = CrearElementoTiempo(doc, instruccionTiempo);
            instrucciones.AppendChild(tiempo);
        }
        mensaje.AppendChild(instrucciones);

        return mensaje;
    }

    /// <summary>
    /// Crear un elemento XML para las acciones en un tiempo específico.
    /// </summary>
    private XmlElement CrearElementoTiempo(
        XmlDocument doc,
        InstruccionPorTiempo instruccionTiempo)
    {
        XmlElement tiempo = doc.CreateElement("tiempo");
        tiempo.SetAttribute("valor", instruccionTiempo.Tiempo.ToString());

        XmlElement acciones = doc.CreateElement("acciones");

        // Obtener todas las acciones para este tiempo
        var accionesArray = instruccionTiempo.Acciones.ObtenerTodas();
        foreach (var (nombreDron, accion) in accionesArray)
        {
            XmlElement dron = doc.CreateElement("dron");
            dron.SetAttribute("nombre", nombreDron);
            dron.InnerText = accion;
            acciones.AppendChild(dron);
        }

        tiempo.AppendChild(acciones);
        return tiempo;
    }

    /// <summary>
    /// Guarda el XML generado en un archivo.
    /// </summary>
    public bool GuardarXmlSalida(string rutaArchivo, string contenidoXml)
    {
        try
        {
            File.WriteAllText(rutaArchivo, contenidoXml);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Guarda el XML con formato legible (indentado).
    /// </summary>
    public bool GuardarXmlSalidaFormateado(string rutaArchivo, string contenidoXml)
    {
        try
        {
            XmlDocument doc = new();
            doc.LoadXml(contenidoXml);

            XmlWriterSettings settings = new()
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\n",
                Encoding = System.Text.Encoding.UTF8
            };

            using (XmlWriter writer = XmlWriter.Create(rutaArchivo, settings))
            {
                doc.WriteTo(writer);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}

/// <summary>
/// Representa un mensaje que ha sido optimizado con su tiempo óptimo e instrucciones.
/// </summary>
public class MensajeOptimizado
{
    public string Nombre { get; set; }

    public string NombreSistemaDrones { get; set; }

    public int TiempoOptimo { get; set; }

    public string MensajeRecibido { get; set; }

    public InstruccionPorTiempo[] InstruccionesPorTiempo { get; set; }

    public MensajeOptimizado()
    {
        Nombre = string.Empty;
        NombreSistemaDrones = string.Empty;
        TiempoOptimo = 0;
        MensajeRecibido = string.Empty;
        InstruccionesPorTiempo = [];
    }
}
