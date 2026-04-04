namespace IPC2_Proyecto2_202401939.Domain;

/// <summary>
/// Representa el estado actual de un dron durante la simulación de envío de un mensaje.
/// Rastrea la altura actual, si está emitiendo luz, y su última altura.
/// </summary>
public class EstadoDron
{
    public EstadoDron(string nombre)
    {
        Nombre = nombre;
        AlturaActual = 0;
        EstaEmitiendo = false;
        UltimaAltura = 0;
    }

    /// <summary>
    /// Nombre identificador del dron.
    /// </summary>
    public string Nombre { get; }

    /// <summary>
    /// Altura actual en metros donde se encuentra el dron.
    /// </summary>
    public int AlturaActual { get; set; }

    /// <summary>
    /// Indica si el dron está emitiendo luz en este momento.
    /// </summary>
    public bool EstaEmitiendo { get; set; }

    /// <summary>
    /// Última altura a la que se movió; usado para optimizar cálculos.
    /// </summary>
    public int UltimaAltura { get; set; }

    /// <summary>
    /// Calcula la distancia que debe recorrer el dron para alcanzar una altura objetivo.
    /// </summary>
    public int ObtenerDistanciaA(int alturaObjetivo)
    {
        return Math.Abs(AlturaActual - alturaObjetivo);
    }
}
