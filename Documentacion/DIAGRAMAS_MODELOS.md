# DIAGRAMAS Y MODELOS COMPLEMENTARIOS

## 1. DIAGRAMA DE ACTIVIDADES - FLUJO PRINCIPAL

```
┌─────────────────────────────────────────────────────────────────┐
│                  INICIO DE APLICACIÓN                           │
└─────────────────────────────────────────────────────────────────┘
                           ↓
┌─────────────────────────────────────────────────────────────────┐
│  Inicializar AppStateService (Singleton)                        │
│  - Crear LinkedLists vacías para drones, sistemas, mensajes    │
└─────────────────────────────────────────────────────────────────┘
                           ↓
┌─────────────────────────────────────────────────────────────────┐
│  Usuario accede a interfaz web                                  │
└─────────────────────────────────────────────────────────────────┘
                           ↓
                      ┌────┴────┐
                      │ Decisión│
                      └─────────┘
           ┌──────┬────────┼────────┬──────┐
           ↓      ↓        ↓        ↓      ↓
      Ver  Cargar Gestionar Gestionar Ver
      Ayuda XML   Drones   Sistemas Mensajes
           ↓
    [FLUJO: CARGAR XML]
           ↓
    ┌──────────────────────────────────────┐
    │ Usuario sube archivo entrada.xml     │
    └──────────────────────────────────────┘
           ↓
    ┌──────────────────────────────────────┐
    │ XmlInputService.CargarArchivoXml()   │
    │ - Validar formato XML                │
    │ - Parsear drones                     │
    │ - Parsear sistemas                   │
    │ - Parsear mensajes                   │
    └──────────────────────────────────────┘
           ↓
    ┌──────────────────────────────────────┐
    │ AppStateService actualiza TDAs       │
    │ - Inserta en DronLinkedList          │
    │ - Inserta en SistemaDronesLinkedList │
    │ - Inserta en MensajeLinkedList       │
    └──────────────────────────────────────┘
           ↓
    ⊕ ← Datos cargados exitosamente
           ↓
    [FLUJO: VER MENSAJE OPTIMIZADO]
           ↓
    ┌──────────────────────────────────────┐
    │ Usuario selecciona mensaje           │
    └──────────────────────────────────────┘
           ↓
    ┌──────────────────────────────────────┐
    │ AppStateService.ObtenerMensajes()    │
    │ - Filtrar por nombre                 │
    │ - Retornar lista ordenada            │
    └──────────────────────────────────────┘
           ↓
    ┌──────────────────────────────────────┐
    │ Usuario hace clic en mensaje         │
    └──────────────────────────────────────┘
           ↓
    ┌──────────────────────────────────────┐
    │ OptimizadorMensaje.Optimizar()       │
    │                                      │
    │ 1. Obtener SistemaDrones             │
    │ 2. Obtener instrucciones             │
    │ 3. Calcular distancias iniciales     │
    │ 4. Simular segundo a segundo:        │
    │    - Actualizar posición drones      │
    │    - Validar restricción luz (1 dron)│
    │    - Marcar acciones del segundo     │
    │ 5. Retornar tiempo óptimo            │
    └──────────────────────────────────────┘
           ↓
    ┌──────────────────────────────────────┐
    │ GeneradorGraphviz.GenerarSVG()       │
    │ - Crear tabla con acciones por seg.  │
    │ - Generar inline SVG                 │
    └──────────────────────────────────────┘
           ↓
    ┌──────────────────────────────────────┐
    │ Mostrar en página:                   │
    │ - Tiempo óptimo                      │
    │ - Mensaje decodificado               │
    │ - Tabla instrucciones                │
    │ - Gráfica SVG                        │
    └──────────────────────────────────────┘
           ↓
    [FLUJO: DESCARGAR SALIDA XML]
           ↓
    ┌──────────────────────────────────────┐
    │ XmlOutputService.GenerarArchivoSalida│
    │                                      │
    │ Crear estructura XML:                │
    │ <respuesta>                          │
    │   <listaMensajes>                    │
    │     <mensaje>                        │
    │       - nombre                       │
    │       - sistemaDrones                │
    │       - tiempoOptimo                 │
    │       - mensajeRecibido              │
    │       - instrucciones por tiempo     │
    │     </mensaje>                       │
    │   </listaMensajes>                   │
    │ </respuesta>                         │
    └──────────────────────────────────────┘
           ↓
    ┌──────────────────────────────────────┐
    │ Descargar archivo
    │ salida_[timestamp].xml               │
    └──────────────────────────────────────┘
           ↓
          FIN
```

---

## 2. DIAGRAMA DE SECUENCIA - OPTIMIZACIÓN DE MENSAJE

```
Usuario        Página              AppState        Optimizador       Gráficos
  │               │                  │                 │               │
  │─ Ver mensaje ─→│                  │                 │               │
  │               │─ ObtenerMensaje()→│                 │               │
  │               │←─ Mensaje ────────│                 │               │
  │               │                  │                 │               │
  │               │─ Optimizar() ────────────────────→│               │
  │               │                  │    Calcular distancias         │
  │               │                  │    Simular segundo a segundo   │
  │               │                  │    Validar restricciones       │
  │               │←─ Resultado ──────────────────────│               │
  │               │                  │                 │               │
  │               │─ GenerarSVG() ──────────────────────────────────→│
  │               │←─ Gráfica SVG ──────────────────────────────────│
  │               │                  │                 │               │
  │←─ Mostrar ────│                  │                 │               │
  │               │                  │                 │               │
  └───────────────────────────────────────────────────────────────────┘
```

---

## 3. MODELO CONCEPTUAL

### 3.1 Entidades Principales

```
DRON
├─ Identificación única (nombre)
├─ Altura actual (0-100 metros)
├─ Estado (en movimiento, esperando, emitiendo)
└─ Posición en tabla de mapeo

SISTEMA DE DRONES
├─ Nombre único
├─ Altura máxima permitida
├─ Drones asignados (1 a n)
└─ Mapeo Altura → Letra para cada dron

MENSAJE
├─ Nombre único
├─ Sistema de drones asociado
├─ Instrucciones (secuencia de Dron + Altura)
└─ Letra resultante (decodificada)

MAPEO DRON-ALTURA
├─ Dron: referencia al dron
├─ Altura: valor 1-100
└─ Letra: carácter del alfabeto

INSTRUCCIÓN
├─ Dron destino: nombre del dron
└─ Altura destino: valor 1-100
```

### 3.2 Relaciones

```
SISTEMA tiene n DRONES
SISTEMA tiene n MAPEOS (Dron-Altura-Letra)
MENSAJE pertenece a 1 SISTEMA
MENSAJE tiene n INSTRUCCIONES
INSTRUCCIÓN referencia 1 DRON
```

---

## 4. DISTRIBUCIÓN DE CLASES POR CAPA

### Domain Layer
```
├─ Dron.cs
├─ SistemaDrones.cs
├─ Mensaje.cs
├─ InstruccionMensaje.cs
├─ MapaDronAltura.cs
└─ MensajeConfig.cs
```

### DataStructures Layer
```
├─ DronLinkedList.cs
├─ DronNode.cs
├─ SistemaDronesLinkedList.cs
├─ SistemaDronesNode.cs
├─ MensajeLinkedList.cs
├─ MensajeNode.cs
├─ InstruccionMensajeLinkedList.cs
├─ InstruccionMensajeNode.cs
├─ MapaDronAlturaLinkedList.cs
└─ MapaDronAlturaNode.cs
```

### Services Layer
```
├─ AppStateService.cs (Singleton)
├─ OptimizadorMensaje.cs
├─ XmlInputService.cs
├─ XmlOutputService.cs
└─ GeneradorGraphviz.cs
```

### Presentation Layer (Pages)
```
├─ Pages/Index.cshtml.cs (Inicio)
├─ Pages/Drones/Index.cshtml.cs (Gestión)
├─ Pages/Sistemas/Index.cshtml.cs (Visualización)
├─ Pages/Mensajes/Index.cshtml.cs (Listado)
├─ Pages/MensajeDetalle/Index.cshtml.cs (Detalles & Optimización)
├─ Pages/Xml/Cargar.cshtml.cs (Carga XML)
├─ Pages/DescargarSalida/Index.cshtml.cs (Descarga)
└─ Pages/Ayuda/Index.cshtml.cs (Información)
```

---

## 5. CASOS DE USO PRINCIPALES

### Caso de Uso 1: Cargar Configuración
```
ACTOR: Usuario
PRECONDICIÓN: Archivo XML válido disponible
FLUJO:
  1. Usuario accede a "Cargar XML"
  2. Usuario selecciona archivo entrada.xml
  3. Sistema valida estructura XML
  4. Sistema parsea drones, sistemas, mensajes
  5. Sistema inserta en TDAs
  6. Sistema muestra confirmación
POSTCONDICIÓN: Datos cargados en memoria
```

### Caso de Uso 2: Optimizar Mensaje
```
ACTOR: Usuario
PRECONDICIÓN: Mensaje cargado, Sistema disponible
FLUJO:
  1. Usuario selecciona mensaje
  2. Sistema obtiene instrucciones
  3. Sistema ejecuta OptimizadorMensaje
  4. Sistema calcula tiempo óptimo
  5. Sistema genera gráfica SVG
  6. Sistema muestra resultados
POSTCONDICIÓN: Tiempo óptimo y tabla de acciones mostrados
```

### Caso de Uso 3: Descargar Resultados
```
ACTOR: Usuario
PRECONDICIÓN: Mensaje optimizado disponible
FLUJO:
  1. Usuario hace clic en "Descargar XML"
  2. Sistema genera estructura XML
  3. Sistema popula datos de mensajes
  4. Sistema crea archivo descargable
  5. Sistema inicia descarga
POSTCONDICIÓN: Archivo salida_[timestamp].xml descargado
```

---

## 6. RESTRICCIONES Y VALIDACIONES

### En Carga de Datos
```
✓ Validar XML bien formado
✓ Verificar dron no exista duplicado
✓ Verificar altura 1-100
✓ Verificar cantidadDrones ≥ 1
✓ Verificar alturaMaxima reasonable
```

### En Optimización
```
✓ Solo 1 dron emite luz por segundo
✓ Drones no pueden ir bajo 0 o sobre alturaMaxima
✓ Cada movimiento es 1 metro
✓ Cada acción es 1 segundo
```

### En Salida
```
✓ XML válido bien formado
✓ Estructura especificada en enunciado
✓ Datos completos y consistentes
```

---

## 7. COMPLEJIDAD ALGORÍTMICA

| Operación | Complejidad | Notas |
|-----------|------------|-------|
| Insertar Dron | O(n) | Recorre lista until final |
| Buscar Dron | O(n) | Búsqueda lineal en LinkedList |
| Optimizar Mensaje | O(m × n) | m instrucciones, n segundo × drones |
| Generar XML | O(n) | Recorrer todas listas |
| Generar SVG | O(t) | t = cantidad de segundos |

Donde:
- n = cantidad de drones
- m = cantidad de instrucciones
- t = tiempo óptimo resultante

---

**Documento complementario** | IPC2 Proyecto 2 | 2026
