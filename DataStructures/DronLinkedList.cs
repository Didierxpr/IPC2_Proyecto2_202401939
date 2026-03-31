using IPC2_Proyecto2_202401939.Domain;

namespace IPC2_Proyecto2_202401939.DataStructures;

public class DronLinkedList
{
    private DronNode? _primero;
    private int _cantidad;

    public DronLinkedList()
    {
        _primero = null;
        _cantidad = 0;
    }

    // Reinicia la estructura para dejarla vacia.
    public void Limpiar()
    {
        _primero = null;
        _cantidad = 0;
    }

    // Indica cuantos drones contiene actualmente la lista.
    public int ObtenerCantidad()
    {
        return _cantidad;
    }

    // Busca si ya existe un dron por nombre ignorando mayusculas/minusculas.
    public bool ExisteNombre(string nombre)
    {
        DronNode? actual = _primero;
        while (actual != null)
        {
            if (string.Equals(actual.Valor.Nombre, nombre, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            actual = actual.Siguiente;
        }

        return false;
    }

    // Inserta el dron en orden alfabetico ascendente.
    public void InsertarOrdenado(Dron dron)
    {
        DronNode nuevo = new(dron);

        if (_primero == null || string.Compare(dron.Nombre, _primero.Valor.Nombre, StringComparison.OrdinalIgnoreCase) < 0)
        {
            nuevo.Siguiente = _primero;
            _primero = nuevo;
            _cantidad++;
            return;
        }

        DronNode actual = _primero;
        while (actual.Siguiente != null &&
               string.Compare(actual.Siguiente.Valor.Nombre, dron.Nombre, StringComparison.OrdinalIgnoreCase) <= 0)
        {
            actual = actual.Siguiente;
        }

        nuevo.Siguiente = actual.Siguiente;
        actual.Siguiente = nuevo;
        _cantidad++;
    }

    // Convierte la lista enlazada en arreglo para facilitar su despliegue en Razor.
    public Dron[] ConvertirAArreglo()
    {
        Dron[] arreglo = new Dron[_cantidad];
        int indice = 0;
        DronNode? actual = _primero;

        while (actual != null)
        {
            arreglo[indice] = actual.Valor;
            indice++;
            actual = actual.Siguiente;
        }

        return arreglo;
    }
}
