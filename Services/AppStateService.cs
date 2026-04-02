using IPC2_Proyecto2_202401939.DataStructures;
using IPC2_Proyecto2_202401939.Domain;

namespace IPC2_Proyecto2_202401939.Services;

public class AppStateService
{
    private readonly DronLinkedList _drones;
    private readonly SistemaDronesLinkedList _sistemas;
    private readonly MensajeLinkedList _mensajes;

    public AppStateService()
    {
        _drones = new DronLinkedList();
        _sistemas = new SistemaDronesLinkedList();
        _mensajes = new MensajeLinkedList();
    }

    // Inicializa la aplicacion y elimina la informacion cargada en memoria.
    public void Inicializar()
    {
        _drones.Limpiar();
        _sistemas.Limpiar();
        _mensajes.Limpiar();
    }

    // Registra un dron si el nombre no existe y cumple reglas basicas.
    public bool AgregarDron(string nombre, out string mensaje)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            mensaje = "El nombre del dron no puede estar vacio.";
            return false;
        }

        if (_drones.ExisteNombre(nombre))
        {
            mensaje = "El dron ya existe en el sistema.";
            return false;
        }

        _drones.InsertarOrdenado(new Dron(nombre.Trim()));
        mensaje = "Dron agregado correctamente.";
        return true;
    }

    // Obtiene el listado de drones en orden alfabetico.
    public Dron[] ObtenerDrones()
    {
        return _drones.ConvertirAArreglo();
    }

    // Registra un sistema de drones validando nombre unico y rangos minimos.
    public bool AgregarSistema(SistemaDrones sistema, out string mensaje)
    {
        if (string.IsNullOrWhiteSpace(sistema.Nombre))
        {
            mensaje = "El sistema de drones debe tener nombre.";
            return false;
        }

        if (sistema.AlturaMaxima < 1 || sistema.AlturaMaxima > 100)
        {
            mensaje = "La altura maxima del sistema debe estar entre 1 y 100.";
            return false;
        }

        if (sistema.CantidadDrones < 1 || sistema.CantidadDrones > 200)
        {
            mensaje = "La cantidad de drones del sistema debe estar entre 1 y 200.";
            return false;
        }

        if (_sistemas.ExisteNombre(sistema.Nombre))
        {
            mensaje = "El sistema de drones ya existe.";
            return false;
        }

        _sistemas.InsertarOrdenado(sistema);
        mensaje = "Sistema agregado correctamente.";
        return true;
    }

    // Agrega un mapeo letra-altura-dron dentro de un sistema existente.
    public bool AgregarMapaASistema(string nombreSistema, MapaDronAltura mapa, out string mensaje)
    {
        SistemaDrones? sistema = _sistemas.BuscarPorNombre(nombreSistema);
        if (sistema == null)
        {
            mensaje = "El sistema de drones indicado no existe.";
            return false;
        }

        if (!_drones.ExisteNombre(mapa.NombreDron))
        {
            mensaje = "El dron del mapeo no existe en la lista global de drones.";
            return false;
        }

        if (mapa.Altura < 1 || mapa.Altura > sistema.AlturaMaxima)
        {
            mensaje = "La altura del mapeo no es valida para el sistema.";
            return false;
        }

        sistema.Mapas.InsertarAlFinal(mapa);
        mensaje = "Mapeo agregado correctamente.";
        return true;
    }

    // Registra un mensaje validando nombre unico y existencia de su sistema de drones.
    public bool AgregarMensaje(MensajeConfig mensajeConfig, out string mensaje)
    {
        if (string.IsNullOrWhiteSpace(mensajeConfig.Nombre))
        {
            mensaje = "El mensaje debe tener nombre.";
            return false;
        }

        if (_mensajes.ExisteNombre(mensajeConfig.Nombre))
        {
            mensaje = "El mensaje ya existe.";
            return false;
        }

        if (!_sistemas.ExisteNombre(mensajeConfig.SistemaDrones))
        {
            mensaje = "El sistema de drones del mensaje no existe.";
            return false;
        }

        _mensajes.InsertarOrdenado(mensajeConfig);
        mensaje = "Mensaje agregado correctamente.";
        return true;
    }

    // Obtiene todos los sistemas de drones ordenados alfabeticamente.
    public SistemaDrones[] ObtenerSistemas()
    {
        return _sistemas.ConvertirAArreglo();
    }

    // Obtiene todos los mensajes ordenados alfabeticamente.
    public MensajeConfig[] ObtenerMensajes()
    {
        return _mensajes.ConvertirAArreglo();
    }
}
