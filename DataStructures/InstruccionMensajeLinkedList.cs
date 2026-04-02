using IPC2_Proyecto2_202401939.Domain;

namespace IPC2_Proyecto2_202401939.DataStructures;

public class InstruccionMensajeLinkedList
{
    private InstruccionMensajeNode? _primero;
    private int _cantidad;

    public InstruccionMensajeLinkedList()
    {
        _primero = null;
        _cantidad = 0;
    }

    // Agrega una instruccion al final para mantener el orden de entrada.
    public void InsertarAlFinal(InstruccionMensaje instruccion)
    {
        InstruccionMensajeNode nuevo = new(instruccion);
        if (_primero == null)
        {
            _primero = nuevo;
            _cantidad++;
            return;
        }

        InstruccionMensajeNode actual = _primero;
        while (actual.Siguiente != null)
        {
            actual = actual.Siguiente;
        }

        actual.Siguiente = nuevo;
        _cantidad++;
    }

    // Convierte la estructura a arreglo para mostrarla en pantalla.
    public InstruccionMensaje[] ConvertirAArreglo()
    {
        InstruccionMensaje[] arreglo = new InstruccionMensaje[_cantidad];
        InstruccionMensajeNode? actual = _primero;
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
