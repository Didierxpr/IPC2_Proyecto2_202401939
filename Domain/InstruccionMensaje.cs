namespace IPC2_Proyecto2_202401939.Domain;

public class InstruccionMensaje
{
    public InstruccionMensaje(string nombreDron, int altura)
    {
        NombreDron = nombreDron;
        Altura = altura;
    }

    public string NombreDron { get; }

    public int Altura { get; }
}
