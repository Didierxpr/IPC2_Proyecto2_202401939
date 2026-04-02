namespace IPC2_Proyecto2_202401939.Domain;

public class MapaDronAltura
{
    public MapaDronAltura(string nombreDron, int altura, string simbolo)
    {
        NombreDron = nombreDron;
        Altura = altura;
        Simbolo = simbolo;
    }

    public string NombreDron { get; }

    public int Altura { get; }

    public string Simbolo { get; }
}
