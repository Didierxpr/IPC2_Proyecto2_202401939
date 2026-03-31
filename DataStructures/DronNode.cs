using IPC2_Proyecto2_202401939.Domain;

namespace IPC2_Proyecto2_202401939.DataStructures;

public class DronNode
{
    public DronNode(Dron valor)
    {
        Valor = valor;
        Siguiente = null;
    }

    public Dron Valor { get; set; }

    public DronNode? Siguiente { get; set; }
}
