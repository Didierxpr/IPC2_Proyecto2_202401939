using IPC2_Proyecto2_202401939.Domain;

namespace IPC2_Proyecto2_202401939.DataStructures;

public class SistemaDronesLinkedList
{
    private SistemaDronesNode? _primero;
    private int _cantidad;

    public SistemaDronesLinkedList()
    {
        _primero = null;
        _cantidad = 0;
    }

    // Limpia la lista completa de sistemas.
    public void Limpiar()
    {
        _primero = null;
        _cantidad = 0;
    }

    // Verifica existencia por nombre ignorando mayusculas y minusculas.
    public bool ExisteNombre(string nombre)
    {
        return BuscarPorNombre(nombre) != null;
    }

    // Retorna un sistema por nombre o null si no existe.
    public SistemaDrones? BuscarPorNombre(string nombre)
    {
        SistemaDronesNode? actual = _primero;
        while (actual != null)
        {
            if (string.Equals(actual.Valor.Nombre, nombre, StringComparison.OrdinalIgnoreCase))
            {
                return actual.Valor;
            }

            actual = actual.Siguiente;
        }

        return null;
    }

    // Inserta un sistema en orden alfabetico.
    public void InsertarOrdenado(SistemaDrones sistema)
    {
        SistemaDronesNode nuevo = new(sistema);
        if (_primero == null ||
            string.Compare(sistema.Nombre, _primero.Valor.Nombre, StringComparison.OrdinalIgnoreCase) < 0)
        {
            nuevo.Siguiente = _primero;
            _primero = nuevo;
            _cantidad++;
            return;
        }

        SistemaDronesNode actual = _primero;
        while (actual.Siguiente != null &&
               string.Compare(actual.Siguiente.Valor.Nombre, sistema.Nombre, StringComparison.OrdinalIgnoreCase) <= 0)
        {
            actual = actual.Siguiente;
        }

        nuevo.Siguiente = actual.Siguiente;
        actual.Siguiente = nuevo;
        _cantidad++;
    }

    // Convierte la estructura a arreglo para desplegar en interfaz.
    public SistemaDrones[] ConvertirAArreglo()
    {
        SistemaDrones[] arreglo = new SistemaDrones[_cantidad];
        SistemaDronesNode? actual = _primero;
        int indice = 0;
        while (actual != null)
        {
            arreglo[indice] = actual.Valor;
            indice++;
            actual = actual.Siguiente;
        }

        return arreglo;
    }
}
