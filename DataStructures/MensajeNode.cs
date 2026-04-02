using IPC2_Proyecto2_202401939.Domain;

namespace IPC2_Proyecto2_202401939.DataStructures;

public class MensajeNode
{
    public MensajeNode(MensajeConfig valor)
    {
        Valor = valor;
        Siguiente = null;
    }

    public MensajeConfig Valor { get; set; }

    public MensajeNode? Siguiente { get; set; }
}
