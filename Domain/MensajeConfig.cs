using IPC2_Proyecto2_202401939.DataStructures;

namespace IPC2_Proyecto2_202401939.Domain;

public class MensajeConfig
{
    public MensajeConfig(string nombre, string sistemaDrones)
    {
        Nombre = nombre;
        SistemaDrones = sistemaDrones;
        Instrucciones = new InstruccionMensajeLinkedList();
    }

    public string Nombre { get; }

    public string SistemaDrones { get; }

    public InstruccionMensajeLinkedList Instrucciones { get; }
}
