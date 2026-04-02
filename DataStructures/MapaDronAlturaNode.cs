using IPC2_Proyecto2_202401939.Domain;

namespace IPC2_Proyecto2_202401939.DataStructures;

public class MapaDronAlturaNode
{
    public MapaDronAlturaNode(MapaDronAltura valor)
    {
        Valor = valor;
        Siguiente = null;
    }

    public MapaDronAltura Valor { get; set; }

    public MapaDronAlturaNode? Siguiente { get; set; }
}
