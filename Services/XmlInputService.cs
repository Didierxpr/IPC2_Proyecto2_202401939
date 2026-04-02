using System.Xml;
using IPC2_Proyecto2_202401939.Domain;

namespace IPC2_Proyecto2_202401939.Services;

public class XmlInputService
{
    private readonly AppStateService _appState;

    public XmlInputService(AppStateService appState)
    {
        _appState = appState;
    }

    // Carga un XML de entrada incremental y retorna el resultado del proceso.
    public XmlInputLoadResult CargarXml(Stream xmlStream)
    {
        XmlInputLoadResult resultado = new();
        int agregadosDrones = 0;
        int agregadosSistemas = 0;
        int agregadosMapeos = 0;
        int agregadosMensajes = 0;
        string errores = string.Empty;

        XmlDocument doc = new();
        doc.Load(xmlStream);

        XmlNode? raiz = doc.SelectSingleNode("/config");
        if (raiz == null)
        {
            resultado.Exito = false;
            resultado.Resumen = "No se pudo cargar el archivo XML.";
            resultado.Errores = "El nodo raiz <config> es obligatorio.";
            return resultado;
        }

        XmlNodeList? dronesNodos = raiz.SelectNodes("listaDrones/dron");
        if (dronesNodos != null)
        {
            for (int i = 0; i < dronesNodos.Count; i++)
            {
                XmlNode? nodo = dronesNodos[i];
                if (nodo == null)
                {
                    continue;
                }

                string nombre = (nodo.InnerText ?? string.Empty).Trim();
                if (_appState.AgregarDron(nombre, out string mensaje))
                {
                    agregadosDrones++;
                }
                else
                {
                    errores += "Dron: " + mensaje + Environment.NewLine;
                }
            }
        }

        XmlNodeList? sistemasNodos = raiz.SelectNodes("listaSistemasDrones/sistemaDrones");
        if (sistemasNodos != null)
        {
            for (int i = 0; i < sistemasNodos.Count; i++)
            {
                XmlNode? nodoSistema = sistemasNodos[i];
                if (nodoSistema == null)
                {
                    continue;
                }

                string nombreSistema = ObtenerAtributo(nodoSistema, "nombre");
                int alturaMaxima = ConvertirEntero(nodoSistema.SelectSingleNode("alturaMaxima")?.InnerText);
                int cantidadDrones = ConvertirEntero(nodoSistema.SelectSingleNode("cantidadDrones")?.InnerText);

                SistemaDrones sistema = new(nombreSistema, alturaMaxima, cantidadDrones);
                bool sistemaCreado = _appState.AgregarSistema(sistema, out string mensajeSistema);
                if (sistemaCreado)
                {
                    agregadosSistemas++;
                }
                else
                {
                    errores += "Sistema: " + mensajeSistema + Environment.NewLine;
                }

                XmlNodeList? contenidos = nodoSistema.SelectNodes("contenido");
                if (contenidos != null)
                {
                    for (int j = 0; j < contenidos.Count; j++)
                    {
                        XmlNode? contenido = contenidos[j];
                        if (contenido == null)
                        {
                            continue;
                        }

                        string nombreDron = (contenido.SelectSingleNode("dron")?.InnerText ?? string.Empty).Trim();
                        XmlNodeList? alturas = contenido.SelectNodes("alturas/altura");
                        if (alturas == null)
                        {
                            continue;
                        }

                        for (int k = 0; k < alturas.Count; k++)
                        {
                            XmlNode? alturaNodo = alturas[k];
                            if (alturaNodo == null)
                            {
                                continue;
                            }

                            int altura = ConvertirEntero(ObtenerAtributo(alturaNodo, "valor"));
                            string simbolo = (alturaNodo.InnerText ?? string.Empty).Trim();
                            MapaDronAltura mapa = new(nombreDron, altura, simbolo);
                            if (_appState.AgregarMapaASistema(nombreSistema, mapa, out string mensajeMapa))
                            {
                                agregadosMapeos++;
                            }
                            else
                            {
                                errores += "Mapeo: " + mensajeMapa + Environment.NewLine;
                            }
                        }
                    }
                }
            }
        }

        XmlNodeList? mensajesNodos = raiz.SelectNodes("listaMensajes/Mensaje");
        if (mensajesNodos != null)
        {
            for (int i = 0; i < mensajesNodos.Count; i++)
            {
                XmlNode? nodoMensaje = mensajesNodos[i];
                if (nodoMensaje == null)
                {
                    continue;
                }

                string nombreMensaje = ObtenerAtributo(nodoMensaje, "nombre");
                string sistemaMensaje = (nodoMensaje.SelectSingleNode("sistemaDrones")?.InnerText ?? string.Empty).Trim();
                MensajeConfig mensajeConfig = new(nombreMensaje, sistemaMensaje);

                XmlNodeList? instruccionesNodos = nodoMensaje.SelectNodes("instrucciones/instruccion");
                if (instruccionesNodos != null)
                {
                    for (int j = 0; j < instruccionesNodos.Count; j++)
                    {
                        XmlNode? instruccionNodo = instruccionesNodos[j];
                        if (instruccionNodo == null)
                        {
                            continue;
                        }

                        string nombreDron = ObtenerAtributo(instruccionNodo, "dron");
                        int altura = ConvertirEntero(instruccionNodo.InnerText);
                        mensajeConfig.Instrucciones.InsertarAlFinal(new InstruccionMensaje(nombreDron, altura));
                    }
                }

                if (_appState.AgregarMensaje(mensajeConfig, out string mensaje))
                {
                    agregadosMensajes++;
                }
                else
                {
                    errores += "Mensaje: " + mensaje + Environment.NewLine;
                }
            }
        }

        resultado.Exito = string.IsNullOrWhiteSpace(errores);
        resultado.Resumen =
            "Carga completada. Drones: " + agregadosDrones +
            ", Sistemas: " + agregadosSistemas +
            ", Mapeos: " + agregadosMapeos +
            ", Mensajes: " + agregadosMensajes + ".";
        resultado.Errores = errores.Trim();
        return resultado;
    }

    // Obtiene un atributo de nodo XML; si no existe retorna cadena vacia.
    private static string ObtenerAtributo(XmlNode nodo, string nombreAtributo)
    {
        if (nodo.Attributes == null)
        {
            return string.Empty;
        }

        XmlAttribute? atributo = nodo.Attributes[nombreAtributo];
        if (atributo == null)
        {
            return string.Empty;
        }

        return (atributo.Value ?? string.Empty).Trim();
    }

    // Convierte texto a entero; si falla retorna cero.
    private static int ConvertirEntero(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            return 0;
        }

        bool exito = int.TryParse(valor.Trim(), out int numero);
        return exito ? numero : 0;
    }
}
