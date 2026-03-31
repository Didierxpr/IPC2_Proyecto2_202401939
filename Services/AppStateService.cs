using IPC2_Proyecto2_202401939.DataStructures;
using IPC2_Proyecto2_202401939.Domain;

namespace IPC2_Proyecto2_202401939.Services;

public class AppStateService
{
    private readonly DronLinkedList _drones;

    public AppStateService()
    {
        _drones = new DronLinkedList();
    }

    // Inicializa la aplicacion y elimina la informacion cargada en memoria.
    public void Inicializar()
    {
        _drones.Limpiar();
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
}
