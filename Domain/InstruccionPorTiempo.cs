using IPC2_Proyecto2_202401939.DataStructures;

namespace IPC2_Proyecto2_202401939.Domain;

/// <summary>
/// Representa las acciones que todos los drones deben ejecutar en un tiempo específico.
/// Para un tiempo dado, almacena qué acción realiza cada dron (subir, bajar, esperar, emitir luz).
/// </summary>
public class InstruccionPorTiempo
{
    public InstruccionPorTiempo(int tiempo)
    {
        Tiempo = tiempo;
        Acciones = new DictAccionDron();
    }

    /// <summary>
    /// El segundo en el que ocurren estas acciones.
    /// </summary>
    public int Tiempo { get; }

    /// <summary>
    /// Diccionario que almacena la acción de cada dron en este tiempo.
    /// Clave: nombre del dron, Valor: descripción de la acción.
    /// </summary>
    public DictAccionDron Acciones { get; }

    /// <summary>
    /// Agrega una acción para un dron específico en este tiempo.
    /// </summary>
    public void AgregarAccion(string nombreDron, string accion)
    {
        Acciones.Agregar(nombreDron, accion);
    }

    /// <summary>
    /// Obtiene la acción de un dron en este tiempo.
    /// </summary>
    public string ObtenerAccion(string nombreDron)
    {
        return Acciones.Obtener(nombreDron) ?? "Esperar";
    }
}

/// <summary>
/// Estructura simple para almacenar acciones por dron en un tiempo.
/// Implementado sin usar Dictionary de C# para mantener TDA propio.
/// </summary>
public class DictAccionDron
{
    private class NodoAccion
    {
        public string NombreDron { get; set; }
        public string Accion { get; set; }
        public NodoAccion Siguiente { get; set; }

        public NodoAccion(string nombreDron, string accion)
        {
            NombreDron = nombreDron;
            Accion = accion;
            Siguiente = null;
        }
    }

    private NodoAccion _inicio;

    public DictAccionDron()
    {
        _inicio = null;
    }

    /// <summary>
    /// Agrega o actualiza una acción para un dron.
    /// </summary>
    public void Agregar(string nombreDron, string accion)
    {
        if (string.IsNullOrWhiteSpace(nombreDron))
        {
            return;
        }

        // Si el dron ya existe, actualizar su acción
        NodoAccion actual = _inicio;
        while (actual != null)
        {
            if (actual.NombreDron == nombreDron)
            {
                actual.Accion = accion;
                return;
            }
            actual = actual.Siguiente;
        }

        // Si no existe, crear nuevo nodo al inicio
        NodoAccion nodoNuevo = new(nombreDron, accion);
        nodoNuevo.Siguiente = _inicio;
        _inicio = nodoNuevo;
    }

    /// <summary>
    /// Obtiene la acción de un dron; retorna null si no existe.
    /// </summary>
    public string Obtener(string nombreDron)
    {
        NodoAccion actual = _inicio;
        while (actual != null)
        {
            if (actual.NombreDron == nombreDron)
            {
                return actual.Accion;
            }
            actual = actual.Siguiente;
        }
        return null;
    }

    /// <summary>
    /// Obtiene todas las acciones como arreglo de tuplas (nombreDron, accion).
    /// </summary>
    public (string nombreDron, string accion)[] ObtenerTodas()
    {
        int cantidad = 0;
        NodoAccion actual = _inicio;
        while (actual != null)
        {
            cantidad++;
            actual = actual.Siguiente;
        }

        (string, string)[] resultado = new (string, string)[cantidad];
        actual = _inicio;
        for (int i = 0; i < cantidad; i++)
        {
            if (actual != null)
            {
                resultado[i] = (actual.NombreDron, actual.Accion);
                actual = actual.Siguiente;
            }
        }

        return resultado;
    }
}
