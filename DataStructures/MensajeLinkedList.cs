using IPC2_Proyecto2_202401939.Domain;

namespace IPC2_Proyecto2_202401939.DataStructures;

public class MensajeLinkedList
{
    private MensajeNode? _primero;
    private int _cantidad;

    public MensajeLinkedList()
    {
        _primero = null;
        _cantidad = 0;
    }

    // Limpia la lista completa de mensajes.
    public void Limpiar()
    {
        _primero = null;
        _cantidad = 0;
    }

    // Valida si ya existe un mensaje por nombre.
    public bool ExisteNombre(string nombre)
    {
        MensajeNode? actual = _primero;
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

    // Inserta un mensaje en orden alfabetico.
    public void InsertarOrdenado(MensajeConfig mensaje)
    {
        MensajeNode nuevo = new(mensaje);
        if (_primero == null ||
            string.Compare(mensaje.Nombre, _primero.Valor.Nombre, StringComparison.OrdinalIgnoreCase) < 0)
        {
            nuevo.Siguiente = _primero;
            _primero = nuevo;
            _cantidad++;
            return;
        }

        MensajeNode actual = _primero;
        while (actual.Siguiente != null &&
               string.Compare(actual.Siguiente.Valor.Nombre, mensaje.Nombre, StringComparison.OrdinalIgnoreCase) <= 0)
        {
            actual = actual.Siguiente;
        }

        nuevo.Siguiente = actual.Siguiente;
        actual.Siguiente = nuevo;
        _cantidad++;
    }

    // Convierte la lista a arreglo para uso en vistas.
    public MensajeConfig[] ConvertirAArreglo()
    {
        MensajeConfig[] arreglo = new MensajeConfig[_cantidad];
        MensajeNode? actual = _primero;
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
