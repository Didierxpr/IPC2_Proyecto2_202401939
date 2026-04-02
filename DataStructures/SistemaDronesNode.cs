using IPC2_Proyecto2_202401939.Domain;

namespace IPC2_Proyecto2_202401939.DataStructures;

public class SistemaDronesNode
{
    public SistemaDronesNode(SistemaDrones valor)
    {
        Valor = valor;
        Siguiente = null;
    }

    public SistemaDrones Valor { get; set; }

    public SistemaDronesNode? Siguiente { get; set; }
}
