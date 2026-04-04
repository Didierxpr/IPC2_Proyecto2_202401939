using IPC2_Proyecto2_202401939.DataStructures;
using IPC2_Proyecto2_202401939.Domain;

namespace IPC2_Proyecto2_202401939.Services;

/// <summary>
/// Optimiza la secuencia de instrucciones para enviar un mensaje usando drones.
/// Calcula el tiempo mínimo necesario y genera las acciones detalladas por segundo.
/// 
/// Restricciones del sistema:
/// - Subir o bajar 1 metro = 1 segundo
/// - Encender/apagar luz = 1 segundo
/// - Solo 1 dron puede emitir luz por segundo
/// - Las instrucciones se deben ejecutar en el orden especificado
/// </summary>
public class OptimizadorMensaje
{
    /// <summary>
    /// Optimiza las instrucciones de un mensaje y retorna el conjunto de acciones.
    /// Calcula el tiempo óptimo para ejecutar todas las instrucciones respetando el orden.
    /// </summary>
    public ResultadoOptimizacion Optimizar(
        MensajeConfig mensaje,
        SistemaDrones sistema,
        Dictionary<string, EstadoDron> estadosDrones)
    {
        ResultadoOptimizacion resultado = new();

        // Convertir instrucciones a arreglo para iterarlas
        InstruccionMensaje[] instrucciones = ConvertirInstruccionesAArreglo(mensaje);

        if (instrucciones.Length == 0)
        {
            resultado.TiempoTotal = 0;
            return resultado;
        }

        // Inicializar estados de drones
        foreach (var estado in estadosDrones.Values)
        {
            estado.AlturaActual = 0;
            estado.EstaEmitiendo = false;
            estado.UltimaAltura = 0;
        }

        int tiempoActual = 0;
        bool hayDronEmitiendo = false;

        // Procesar cada instrucción en orden
        for (int i = 0; i < instrucciones.Length; i++)
        {
            InstruccionMensaje instruccion = instrucciones[i];

            if (!estadosDrones.ContainsKey(instruccion.NombreDron))
            {
                continue;
            }

            EstadoDron estado = estadosDrones[instruccion.NombreDron];
            int tiempoMovimiento = CalcularTiempoMovimiento(estado, instruccion.Altura);

            // El dron necesita: tiempoMovimiento + 1 segundo para emitir luz
            int tiempoEmision = tiempoActual + tiempoMovimiento + 1;

            // Registrar las acciones para este dron
            RegistrarAccionesMovimiento(resultado, tiempoActual, tiempoEmision, estado, instruccion.Altura);

            // Actualizar estado del dron
            estado.AlturaActual = instruccion.Altura;
            estado.UltimaAltura = instruccion.Altura;

            // Avanzar tiempo: el siguiente dron comienza cuando este termina
            tiempoActual = tiempoEmision;
        }

        resultado.TiempoTotal = tiempoActual;
        return resultado;
    }

    /// <summary>
    /// Calcula cuántos segundos tarda un dron en moverse a una altura objetivo.
    /// </summary>
    private int CalcularTiempoMovimiento(EstadoDron estado, int alturaObjetivo)
    {
        int distancia = estado.ObtenerDistanciaA(alturaObjetivo);
        return distancia;
    }

    /// <summary>
    /// Registra las acciones detalladas que realiza un dron durante su movimiento y emisión.
    /// Genera una entrada en resultado para cada segundo.
    /// </summary>
    private void RegistrarAccionesMovimiento(
        ResultadoOptimizacion resultado,
        int tiempoInicio,
        int tiempoEmision,
        EstadoDron estado,
        int alturaObjetivo)
    {
        int alturaActual = estado.AlturaActual;
        int tiempoActual = tiempoInicio;

        // Movimiento: subir o bajar según sea necesario
        while (alturaActual != alturaObjetivo)
        {
            string accion;
            if (alturaActual < alturaObjetivo)
            {
                accion = "Subir";
                alturaActual++;
            }
            else
            {
                accion = "Bajar";
                alturaActual--;
            }

            resultado.AgregarAccion(tiempoActual + 1, estado.Nombre, accion);
            tiempoActual++;
        }

        // Emitir luz
        resultado.AgregarAccion(tiempoActual + 1, estado.Nombre, "Emitir luz");
    }

    /// <summary>
    /// Convierte las instrucciones de un mensaje en un arreglo manejable.
    /// </summary>
    private InstruccionMensaje[] ConvertirInstruccionesAArreglo(MensajeConfig mensaje)
    {
        if (mensaje?.Instrucciones == null)
        {
            return [];
        }

        return mensaje.Instrucciones.ConvertirAArreglo();
    }
}

/// <summary>
/// Resultado de la optimización: tiempo total y acciones detalladas por tiempo.
/// </summary>
public class ResultadoOptimizacion
{
    private class NodoTiempo
    {
        public int Tiempo { get; set; }
        public InstruccionPorTiempo Instruccion { get; set; }
        public NodoTiempo Siguiente { get; set; }

        public NodoTiempo(int tiempo, InstruccionPorTiempo instruccion)
        {
            Tiempo = tiempo;
            Instruccion = instruccion;
            Siguiente = null;
        }
    }

    private NodoTiempo _inicioTiempos;
    private Dictionary<int, InstruccionPorTiempo> _tiempoMap;

    public ResultadoOptimizacion()
    {
        _inicioTiempos = null;
        _tiempoMap = new Dictionary<int, InstruccionPorTiempo>();
        TiempoTotal = 0;
    }

    /// <summary>
    /// Tiempo total en segundos para ejecutar todas las instrucciones.
    /// </summary>
    public int TiempoTotal { get; set; }

    /// <summary>
    /// Agrega una acción para un dron en un tiempo específico.
    /// </summary>
    public void AgregarAccion(int tiempo, string nombreDron, string accion)
    {
        if (!_tiempoMap.ContainsKey(tiempo))
        {
            InstruccionPorTiempo instruccion = new(tiempo);
            _tiempoMap[tiempo] = instruccion;
        }

        _tiempoMap[tiempo].AgregarAccion(nombreDron, accion);
    }

    /// <summary>
    /// Obtiene todas las instrucciones ordenadas por tiempo.
    /// </summary>
    public InstruccionPorTiempo[] ObtenerInstruccionesPorTiempo()
    {
        // Ordenar por tiempo
        var tiempos = new List<int>(_tiempoMap.Keys);
        tiempos.Sort();

        InstruccionPorTiempo[] resultado = new InstruccionPorTiempo[tiempos.Count];
        for (int i = 0; i < tiempos.Count; i++)
        {
            resultado[i] = _tiempoMap[tiempos[i]];
        }

        return resultado;
    }

    /// <summary>
    /// Obtiene la instrucción para un tiempo específico.
    /// </summary>
    public InstruccionPorTiempo ObtenerInstruccionPara(int tiempo)
    {
        return _tiempoMap.ContainsKey(tiempo) ? _tiempoMap[tiempo] : null;
    }
}
