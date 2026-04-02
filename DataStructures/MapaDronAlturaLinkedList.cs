using IPC2_Proyecto2_202401939.Domain;

namespace IPC2_Proyecto2_202401939.DataStructures;

public class MapaDronAlturaLinkedList
{
    private MapaDronAlturaNode? _primero;
    private int _cantidad;

    public MapaDronAlturaLinkedList()
    {
        _primero = null;
        _cantidad = 0;
    }

    // Agrega un elemento al final manteniendo orden de carga.
    public void InsertarAlFinal(MapaDronAltura mapa)
    {
        MapaDronAlturaNode nuevo = new(mapa);
        if (_primero == null)
        {
            _primero = nuevo;
            _cantidad++;
            return;
        }

        MapaDronAlturaNode actual = _primero;
        while (actual.Siguiente != null)
        {
            actual = actual.Siguiente;
        }

        actual.Siguiente = nuevo;
        _cantidad++;
    }

    // Convierte los elementos a un arreglo para visualizacion en vistas.
    public MapaDronAltura[] ConvertirAArreglo()
    {
        MapaDronAltura[] arreglo = new MapaDronAltura[_cantidad];
        MapaDronAlturaNode? actual = _primero;
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
