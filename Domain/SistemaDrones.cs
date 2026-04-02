using IPC2_Proyecto2_202401939.DataStructures;

namespace IPC2_Proyecto2_202401939.Domain;

public class SistemaDrones
{
    public SistemaDrones(string nombre, int alturaMaxima, int cantidadDrones)
    {
        Nombre = nombre;
        AlturaMaxima = alturaMaxima;
        CantidadDrones = cantidadDrones;
        Mapas = new MapaDronAlturaLinkedList();
    }

    public string Nombre { get; }

    public int AlturaMaxima { get; }

    public int CantidadDrones { get; }

    public MapaDronAlturaLinkedList Mapas { get; }
}
