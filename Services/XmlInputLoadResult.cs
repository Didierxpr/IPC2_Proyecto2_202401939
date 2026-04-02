namespace IPC2_Proyecto2_202401939.Services;

public class XmlInputLoadResult
{
    public XmlInputLoadResult()
    {
        Exito = true;
        Resumen = string.Empty;
        Errores = string.Empty;
    }

    public bool Exito { get; set; }

    public string Resumen { get; set; }

    public string Errores { get; set; }
}
