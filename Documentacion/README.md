# DOCUMENTACIÓN: PROYECTO 2 - SISTEMA DE CONTROL DE DRONES ENCRIPTADOS

---

## INFORMACIÓN DEL PROYECTO

**Universidad:** Universidad de San Carlos de Guatemala  
**Facultad:** Facultad de Ingeniería  
**Escuela:** Escuela de Ciencias y Sistemas  
**Curso:** Introducción a la Programación y Computación 2 (IPC2)  
**Instructor:** Aux. Carlos Manuel Lima y Lima  
**Sección:** P  
**Ciclo:** Primer Semestre 2026  

**Estudiante:** Carlos Didiere Cabrera Rodriguez  
**Carnet:** 202401939  
**Repositorio:** https://github.com/Didierxpr/IPC2_Proyecto2_202401939  
**Fecha de Entrega:** 05 de abril de 2026  

---

## 1. INTRODUCCIÓN

El presente proyecto aborda el desarrollo de un sistema de control de drones encriptados para el Ministerio de la Defensa de Guatemala. Este sistema implementa una solución innovadora para la transmisión segura de mensajes mediante la emisión coordinada de luz LED desde múltiples drones a diferentes alturas.

La Facultad de Ingeniería de la Universidad de San Carlos propone un diseño que utiliza n drones capaces de subir y bajar metros, emitiendo luces LED de alta energía. Dependiendo del dron que emita luz y la altura a la que lo haga, se representa una letra del alfabeto. De esta forma, el componente receptor puede medir alturas, determinar qué dron emitió el haz de luz y decodificar el mensaje original.

El software desarrollado controla el sistema emisor de drones y, a partir de un conjunto de instrucciones recibidas, genera el mensaje en el menor tiempo posible mediante algoritmos de optimización.

---

## 2. OBJETIVOS

### 2.1 Objetivo General
Modelar, documentar e implementar una solución al problema de transmisión de mensajes encriptados mediante un sistema de drones utilizando herramientas de desarrollo modernas.

### 2.2 Objetivos Específicos
- Implementar una solución utilizando lenguaje de programación C# con paradigma orientado a objetos
- Utilizar estructuras de programación secuenciales, cíclicas y condicionales
- Implementar Tipos de Dato Abstracto (TDA) propios sin utilizar colecciones estándar de C#
- Manipular archivos XML para entrada y salida de datos
- Generar reportes visuales con gráficas SVG
- Optimizar el tiempo de transmisión de mensajes mediante algoritmos propios
- Desarrollar una interfaz de usuario web intuitiva y funcional
- Aplicar correctamente patrones de diseño orientado a objetos

---

## 3. DESCRIPCIÓN DEL PROBLEMA

### 3.1 Contexto
El Ministerio de la Defensa de Guatemala requiere un sistema para transmitir mensajes encriptados que no puedan ser interceptados por personal no autorizado. La solución propuesta utiliza drones como emisores de mensajes codificados.

### 3.2 Características del Sistema
- **Drones:** Pueden realizar 4 acciones: subir 1 metro, bajar 1 metro, esperar, emitir luz LED
- **Restricciones Principales:**
  - Solo 1 dron puede emitir luz en un tiempo t dado
  - Cada acción tarda 1 segundo (subir/bajar 1 metro o encender/apagar luz)
  - Máximo 200 drones
  - Altura máxima: 100 metros
  - Una misma letra puede representarse en diferentes alturas para aumentar seguridad

### 3.3 Mapeo de Alturas
Cada día se genera una tabla que mapea la combinación (altura, dron) a una letra del alfabeto. Por ejemplo:

| Altura | Dron01 | Dron02 | Dron03 | Dron04 |
|--------|---------|---------|---------|---------|
| 8      | (espacio) | X | Y | Z |
| 7      | T | U | V | W |
| ... | ... | ... | ... | ... |

---

## 4. ANÁLISIS DE LA SOLUCIÓN

### 4.1 Enfoque Arquitectónico
La solución adopta una arquitectura orientada a servicios con separación clara de responsabilidades:

- **Domain:** Clases de negocio (Dron, SistemaDrones, Mensaje, etc.)
- **DataStructures:** Implementación de TDAs (LinkedLists personalizadas)
- **Services:** Lógica de negocio (AppStateService, OptimizadorMensaje, XmlInputService, XmlOutputService)
- **Pages:** Interfaz web (Razor Pages con C#)

### 4.2 Algoritmo de Optimización
El núcleo del sistema es el **OptimizadorMensaje**, que implementa un algoritmo greedy que:

1. Recibe una secuencia de instrucciones (Dron, Altura)
2. Calcula la distancia que cada dron debe recorrer
3. Simula el movimiento paralelo de drones segundo a segundo
4. Respeta la restricción: solo 1 dron emite luz por segundo
5. Retorna el tiempo mínimo requerido y las acciones por segundo

### 4.3 Flujo de Datos

```
Archivo XML Entrada
        ↓
XmlInputService (parsea)
        ↓
AppStateService (almacena en TDAs)
        ↓
OptimizadorMensaje (calcula tiempo óptimo)
        ↓
XmlOutputService (genera salida)
        ↓
Interfaz Web (visualiza resultados)
```

---

## 5. DISEÑO DE LA SOLUCIÓN

### 5.1 Diagrama de Clases

```
┌─────────────────────────────────────────────────────────────────┐
│                        DOMAIN CLASSES                            │
├─────────────────────────────────────────────────────────────────┤

┌─────────────────┐
│     Dron        │
├─────────────────┤
│ - nombre: string│
└─────────────────┘

┌─────────────────────────────────┐
│     SistemaDrones               │
├─────────────────────────────────┤
│ - nombre: string                │
│ - alturaMaxima: int             │
│ - drones: LinkedList<Dron>      │
│ - mapeos: LinkedList<MapaDron>  │
└─────────────────────────────────┘

┌──────────────────────────┐
│   MapaDronAltura         │
├──────────────────────────┤
│ - dron: string           │
│ - altura: int            │
│ - letra: string          │
└──────────────────────────┘

┌───────────────────────────────────┐
│   InstruccionMensaje              │
├───────────────────────────────────┤
│ - dron: string                    │
│ - altura: int                     │
│ - letra: string (decodificada)    │
└───────────────────────────────────┘

┌──────────────────────────────────────┐
│   Mensaje                            │
├──────────────────────────────────────┤
│ - nombre: string                     │
│ - sistemaDrones: string              │
│ - instrucciones: LinkedList<Instr.> │
└──────────────────────────────────────┘

├─────────────────────────────────────────────────────────────────┤
│                   DATA STRUCTURES (TDAs)                        │
├─────────────────────────────────────────────────────────────────┤

┌──────────────────┐     ┌──────────────────┐
│ DronNode         │────→│ DronNode         │
├──────────────────┤     ├──────────────────┤
│ - dato: Dron     │     │ - dato: Dron     │
│ - siguiente: Link│     │ - siguiente: Link│
└──────────────────┘     └──────────────────┘

┌─────────────────────────┐
│ DronLinkedList          │
├─────────────────────────┤
│ - cabeza: DronNode      │
├─────────────────────────┤
│ + Insertar()            │
│ + Eliminar()            │
│ + Obtener()             │
│ + Recorrer()            │
└─────────────────────────┘

(Similar para: SistemaDronesLinkedList, MensajeLinkedList, 
 InstruccionMensajeLinkedList, MapaDronAlturaLinkedList)

├─────────────────────────────────────────────────────────────────┤
│                       SERVICES                                  │
├─────────────────────────────────────────────────────────────────┤

┌──────────────────────────────────────┐
│   AppStateService (Singleton)        │
├──────────────────────────────────────┤
│ - drones: DronLinkedList             │
│ - sistemas: SistemaDronesLinkedList  │
│ - mensajes: MensajeLinkedList        │
├──────────────────────────────────────┤
│ + AgregarDron()                      │
│ + AgregarSistema()                   │
│ + AgregarMensaje()                   │
│ + ObtenerMensajePorNombre()          │
│ + OptimizarMensaje()                 │
└──────────────────────────────────────┘

┌──────────────────────────────┐
│   OptimizadorMensaje         │
├──────────────────────────────┤
│ - mensaje: Mensaje           │
│ - sistemaDrones: SistemaDr.  │
├──────────────────────────────┤
│ + Optimizar()                │
│ + SimularPorSegundo()        │
│ + CalcularDistancias()       │
└──────────────────────────────┘

┌──────────────────────────────────────┐
│   XmlInputService                    │
├──────────────────────────────────────┤
│ + CargarArchivoXml()                 │
│ + ParsearDrones()                    │
│ + ParsearSistemas()                  │
│ + ParsearMensajes()                  │
└──────────────────────────────────────┘

┌──────────────────────────────────────┐
│   XmlOutputService                   │
├──────────────────────────────────────┤
│ + GenerarArchivoSalida()             │
│ + CrearXmlMensajes()                 │
│ + CrearXmlInstrucciones()            │
└──────────────────────────────────────┘

┌──────────────────────────────────────┐
│   GeneradorGraphviz                  │
├──────────────────────────────────────┤
│ + GenerarSVGSistema()                │
│ + GenerarSVGInstrucciones()          │
│ + CrearTablaHTML()                   │
└──────────────────────────────────────┘
```

### 5.2 Estructuras de Datos Principais

**LinkedList Personalizada (TDA):**
```
Cabeza → [Nodo 1|Siguiente] → [Nodo 2|Siguiente] → ... → NULL
         ^                     ^
         dato                  dato
```

Cada nodo contiene:
- `dato:` El objeto almacenado
- `siguiente:` Referencia al siguiente nodo

**Ventajas de esta implementación:**
- Gestión manual de memoria (sin GC descontrolado)
- Implementación completa desde cero
- Cumple requisito de TDA propio

---

## 6. ALGORITMO DE OPTIMIZACIÓN

### 6.1 Pseudocódigo Principal

```
FUNCIÓN Optimizar(mensajeInstrucciones, sistemaDrones):
    1. Calcular distancia inicial que cada dron debe recorrer
    2. tiempoOptimo = 0
    3. 
    4. MIENTRAS haya instrucciones pendientes O drones moviéndose:
    5.     tiempoOptimo += 1
    6.     
    7.     PARA cada dron:
    8.         SI el dron está en movimiento:
    9.             Continuar movimiento (subir/bajar)
    10.        SINO SI quedan instrucciones para este dron:
    11.            Iniciar movimiento hacia altura destino
    12.        FIN SI
    13.    FIN PARA
    14.    
    15.    Verificar si hay un dron que debe emitir luz en este tiempo
    16.    Respetar restricción: solo 1 dron emite por segundo
    17.    
    18. FIN MIENTRAS
    19. 
    20. RETORNAR (tiempoOptimo, acciones_por_segundo, mensaje_decodificado)
```

### 6.2 Lógica de Restricción de Luz
- Se mantiene un registro de cuál dron debe emitir luz en cada segundo
- Se valida que solo 1 dron por segundo puede emitir
- Si múltiples drones deben emitir en el mismo segundo, se reordena o se espera

---

## 7. CONSIDERACIONES DE IMPLEMENTACIÓN

### 7.1 Decisiones de Diseño

**1. Arquitectura Orientada a Servicios**
- `AppStateService` como singleton centraliza el estado de la aplicación
- Facilita el acceso desde any lugar de la aplicación
- Cumple patrón Repository

**2. TDAs Personalizados**
- No se utilizaron `List<T>`, `Queue<T>` ni otras colecciones de C#
- Se implementaron LinkedLists totalmente propias
- Cumple restricción de no usar estructuras estándar

**3. XML Incremental**
- La aplicación permite cargar múltiples archivos XML
- Cada carga actualiza/agrega datos sin limpiar anteriores
- Permite datos del Ministerio sobre múltiples días

**4. Optimización de Tiempo**
- El algoritmo greedy elige el camino más corto (primer dron disponible)
- Simula movimiento segundo a segundo garantizando exactitud
- Valida restricción: solo 1 luz por segundo

### 7.2 Tecnologías Utilizadas

| Tecnología | Propósito |
|-----------|---------|
| C# 8.0 | Lenguaje principal |
| ASP.NET Core Razor Pages | Interfaz web |
| System.Xml | Parseo XML |
| System.Text.StringBuilder | Generación SVG |
| LINQ | Consultas en memoria |

### 7.3 Manejo de Errores

```csharp
- Validación de archivo XML malformado
- Detección de dron/sistema no existe
- Identificación de durones con mismo nombre
- Verificación de alturas fuera de rango
- Control de instrucciones inválidas
```

---

## 8. CARACTERÍSTICAS IMPLEMENTADAS

### Release 1: Backend & Algoritmo ✓
- ✓ Clase OptimizadorMensaje con algoritmo greedy
- ✓ Gestión completa de TDAs LinkedList
- ✓ Parseo y carga de XML incremental
- ✓ Generación de XML de salida
- ✓ AppStateService con métodos de consulta
- ✓ Visualización de mensajes optimizados

### Release 2: Frontend & Visualización ✓
- ✓ Interfaz web completa (7 páginas principales)
- ✓ Gestión CRUD de drones
- ✓ Visualización de sistemas con gráficas SVG
- ✓ Página de Ayuda con información del estudiante
- ✓ Descarga de archivos XML de salida
- ✓ Diseño minimalista con CSS personalizado
- ✓ Navegación intuitiva y responsiva

---

## 9. CONCLUSIONES

El proyecto **Sistema de Control de Drones Encriptados (IPC2 Proyecto 2)** ha sido implementado satisfactoriamente con una arquitectura robusta, escalable y mainteniable.

### Logros Principales:
1. **Algoritmo de Optimización Funcional:** Calcula correctamente el tiempo mínimo para transmitir mensajes respetando todas las restricciones del sistema
2. **TDAs Propios:** Implementación completa de LinkedLists sin depender de colecciones estándar
3. **Interfaz Completa:** Aplicación web intuitiva que facilita la gestión de drones, sistemas y mensajes
4. **Cumplimiento de Requisitos:** Todo requerimiento del enunciado fue cubierto
5. **Código Limpio:** Aplicación consistente de POO con patrones de diseño

### Oportunidades de Mejora Futuro:
- Implementar algoritmos más sofisticados para optimización adicional
- Agregar persistencia en base de datos
- Implementar autenticación y autorización
- Crear API REST para integración con sistemas externos
- Generar reportes más complejos con Machine Learning

### Reflexión Final:
Este proyecto consolidó los conceptos de Programación Orientada a Objetos, Estructuras de Datos Abstractas y Desarrollo Web. La solución demuestra capacidad de análisis, diseño e implementación de sistemas complejos bajo restricciones específicas.

---

**Fecha de elaboración:** 05 de abril de 2026  
**Versión:** 2.0 (Release Final)  
**Estado:** Completo y funcional

