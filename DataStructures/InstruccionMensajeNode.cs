using IPC2_Proyecto2_202401939.Domain;

namespace IPC2_Proyecto2_202401939.DataStructures;

public class InstruccionMensajeNode
{
    public InstruccionMensajeNode(InstruccionMensaje valor)
    {
        Valor = valor;
        Siguiente = null;
    }

    public InstruccionMensaje Valor { get; set; }

    public InstruccionMensajeNode? Siguiente { get; set; }
}
