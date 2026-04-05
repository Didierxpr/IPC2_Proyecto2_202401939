# MANUAL DE USUARIO - SISTEMA DE CONTROL DE DRONES ENCRIPTADOS

## ÍNDICE
1. Introducción
2. Requisitos del Sistema
3. Instalación y Ejecución
4. Interfaz Principal
5. Procedimientos Paso a Paso
6. Resolución de Problemas

---

## 1. INTRODUCCIÓN

El **Sistema de Control de Drones Encriptados** es una aplicación web que permite gestionar, optimizar y coordinar la transmisión de mensajes encriptados mediante un conjunto de drones.

### Características Principales
- ✓ Gestión completa de drones y sistemas
- ✓ Carga incremental de configuraciones via XML
- ✓ Optimización automática del tiempo de transmisión
- ✓ Visualización gráfica de instrucciones
- ✓ Exportación de resultados en XML
- ✓ Interfaz web intuitiva y responsiva

---

## 2. REQUISITOS DEL SISTEMA

### Mínimos
- **OS:** Windows 10/11 o Linux/macOS
- **Runtime:** .NET 8.0 SDK instalado
- **Navegador:** Chrome, Firefox, Safari, Edge (versión reciente)
- **Espacio:** 500 MB
- **RAM:** 2 GB mínimo

### Recomendados
- **CPU:** Procesador moderno (2+ cores)
- **RAM:** 4 GB o superior
- **Conexión:** Internet (local en localhost)

---

## 3. INSTALACIÓN Y EJECUCIÓN

### 3.1 Descargar el Proyecto
```powershell
git clone https://github.com/Didierxpr/IPC2_Proyecto2_202401939.git
cd IPC2_Proyecto2_202401939/IPC2_Proyecto2_202401939
```

### 3.2 Ejecutar la Aplicación
```powershell
# Compilar y ejecutar
dotnet run

# Output esperado:
# Building...
# Build succeeded with 14 warning(s)
# Now listening on: http://localhost:5000
```

### 3.3 Acceder a la Interfaz
Abrir navegador e ir a: **http://localhost:5000**

---

## 4. INTERFAZ PRINCIPAL

### 4.1 Navegación
La aplicación ofrece 8 opciones principales via barra de navegación:

| Opción | Descripción |
|--------|------------|
| **Inicio** | Panel principal con opciones iniciales |
| **Cargar XML** | Cargador de archivos de configuración |
| **Drones** | Gestión y visualización de drones |
| **Sistemas** | Visualización de sistemas de drones |
| **Mensajes** | Listado y detalles de mensajes |
| **Descargar Salida** | Descarga resultados en XML |
| **Ayuda** | Información del proyecto |

### 4.2 Estructura de la Interfaz

```
┌─────────────────────────────────────────────┐
│  SISTEMA DE CONTROL DE DRONES ENCRIPTADOS  │
├─────────────────────────────────────────────┤
│ [Inicio] [Cargar] [Drones] [Sistemas] [Msg]│
│ [Descarga] [Ayuda]                          │
├─────────────────────────────────────────────┤
│                                             │
│              CONTENIDO PRINCIPAL            │
│                                             │
│                                             │
└─────────────────────────────────────────────┘
└ Pie: Año, Versión, Universidad
```

---

## 5. PROCEDIMIENTOS PASO A PASO

### Procedimiento 1: Inicializar Sistema

**Objetivo:** Limpiar todos los datos cargados y empezar de cero

**Pasos:**
1. En la página de **Inicio**, hacer clic en botón "Inicializar Sistema"
2. Sistema muestra confirmación
3. Todos los drones, sistemas y mensajes son eliminados
4. Sistema listo para nueva carga

**Resultado:** Aplicación en estado limpio, lista para nueva configuración

---

### Procedimiento 2: Cargar Archivo XML

**Objetivo:** Cargar configuración de drones, sistemas y mensajes

**Requisitos Previos:**
- Archivo XML con estructura válida
- Formato según especificación del proyecto

**Pasos:**
1. Ir a sección **Cargar XML**
2. Hacer clic en "Seleccionar archivo"
3. Navegar a archivo `entrada.xml`
4. Hacer clic en "Cargar"
5. Sistema valida y carga datos
6. Mostrar confirmación: "Datos cargados exitosamente"

**Formato XML Esperado:**
```xml
<?xml version="1.0"?>
<config>
  <listaDrones>
    <dron>Dron01</dron>
    <dron>Dron02</dron>
  </listaDrones>
  <listaSistemasDrones>
    <sistemaDrones nombre="SistemaA">
      <alturaMaxima>100</alturaMaxima>
      <cantidadDrones>2</cantidadDrones>
      <contenido>
        <dron>Dron01</dron>
        <alturas>
          <altura valor="1">A</altura>
          <altura valor="2">B</altura>
        </alturas>
      </contenido>
    </sistemaDrones>
  </listaSistemasDrones>
  <listaMensajes>
    <Mensaje nombre="Msg1">
      <sistemaDrones>SistemaA</sistemaDrones>
      <instrucciones>
        <instruccion dron="Dron01">1</instruccion>
        <instruccion dron="Dron02">2</instruccion>
      </instrucciones>
    </Mensaje>
  </listaMensajes>
</config>
```

**Errores Comunes:**
- "XML malformado" → Verificar sintaxis
- "Dron no existe" → Verificar nombres de drones
- "Altura fuera de rango" → Verificar 1-100 metros

---

### Procedimiento 3: Ver Drones

**Objetivo:** Visualizar listado de drones disponibles

**Pasos:**
1. Ir a sección **Drones**
2. Sistema muestra tabla con columnas:
   - Nombre del dron
   - Estado actual
   - Altura actual
3. Drones mostrados en orden alfabético
4. Hacer clic en un dron para ver detalles

**Tabla de Ejemplo:**
```
Nombre    | Estado      | Altura
----------|-------------|-------
Dron01    | Esperando   | 0 m
Dron02    | Esperando   | 0 m
Dron03    | En movimiento | 5 m
```

---

### Procedimiento 4: Ver Sistemas

**Objetivo:** Visualizar todos los sistemas de drones con sus mapeos

**Pasos:**
1. Ir a sección **Sistemas**
2. Para cada sistema se muestra:
   - Nombre
   - Altura máxima
   - Cantidad de drones
   - Tabla de mapeos (Altura → Letra por dron)

**Tabla de Mapeo Ejemplo:**
```
Altura | Dron01 | Dron02 | Dron03 | Dron04
-------|--------|--------|--------|--------
8      | (espacio) | X | Y | Z
7      | T | U | V | W
6      | M | Q | R | S
...
```

---

### Procedimiento 5: Ver Mensajes

**Objetivo:** Listar todos los mensajes disponibles

**Pasos:**
1. Ir a sección **Mensajes**
2. Sistema muestra tabla con:
   - Nombre del mensaje
   - Sistema de drones usado
   - Estado (Pendiente/Optimizado)
3. Mensajes en orden alfabético
4. Hacer clic en mensaje para ver detalles

---

### Procedimiento 6: Optimizar Mensaje (PRINCIPAL)

**Objetivo:** Calcular tiempo óptimo y obtener instrucciones detalladas

**Pasos:**
1. En sección **Mensajes**, seleccionar un mensaje
2. Hacer clic en "Ver Detalles"
3. Hacer clic en "Optimizar" o "Ver Instrucciones Optimizadas"
4. Sistema calcula:
   - Tiempo óptimo requerido
   - Mensaje decodificado
   - Acciones por segundo
5. Resultados mostrados en tres área:
   - **Resumen:** Tiempo óptimo, Mensaje, Sistema
   - **Tabla Detallada:** Acciones por segundo
   - **Gráfica:** Visualización SVG

**Tabla de Acciones Ejemplo:**

```
Segundo | Dron01  | Dron02  | Dron03  | Dron04
--------|---------|---------|---------|--------
1       | Subir   | Subir   | Subir   | Esperar
2       | Subir   | Subir   | Esperar | Esperar
3       | Emitir  | Subir   | Esperar | Esperar
4       | Esperar | Emitir  | Esperar | Esperar
5       | Esperar | Bajar   | Emitir  | Esperar
```

---

### Procedimiento 7: Descargar Resultados

**Objetivo:** Obtener archivo XML con resultados optimizados

**Pasos:**
1. Ir a sección **Descargar Salida**
2. Verificar que haya mensajes optimizados
3. Hacer clic en "Descargar XML"
4. Archivo `salida_TIMESTAMP.xml` se descarga
5. Abrir con editor de texto o navegador

**Archivo Generado:**
```xml
<?xml version="1.0"?>
<respuesta>
  <listaMensajes>
    <mensaje nombre="HelloWorld">
      <sistemaDrones>SistemaA</sistemaDrones>
      <tiempoOptimo>7</tiempoOptimo>
      <mensajeRecibido>HELLO WORLD</mensajeRecibido>
      <instrucciones>
        <tiempo valor="1">
          <acciones>
            <dron nombre="Dron01">Subir</dron>
            <dron nombre="Dron02">Subir</dron>
          </acciones>
        </tiempo>
        ...
      </instrucciones>
    </mensaje>
  </listaMensajes>
</respuesta>
```

---

### Procedimiento 8: Acceder a Información del Proyecto

**Objetivo:** Ver información del estudiante y enlace a documentación

**Pasos:**
1. Ir a sección **Ayuda**
2. Se muestran secciones:
   - **Información del Proyecto** - Descripción general
   - **Información del Estudiante** - Datos académicos
   - **Características del Sistema** - Listado de funciones
   - **Cómo Usar** - Guía paso a paso
   - **Restricciones** - Limitaciones del sistema
   - **Formato XML** - Link a documentación
   - **Soporte Técnico** - Contacto

---

## 6. RESOLUCIÓN DE PROBLEMAS

### Problema: "Error al cargar XML"

**Causas Posibles:**
1. Archivo XML no bien formado
2. Etiquetas mal cerradas
3. Caracteres especiales sin escape

**Solución:**
- Validar XML en herramienta como XMLValidator
- Verificar balanceo de etiquetas
- Escapar caracteres especiales (&, <, >, ", ')

### Problema: "Dron no encontrado"

**Causa:** Instrucción referencia dron no existente

**Solución:**
- Verificar que dron existe en listaDrones
- Verificar ortografía exacta
- Cargar XML nuevamente si modificó manualmente

### Problema: "Altura fuera de rango"

**Causa:** Altura especificada < 1 o > alturaMaxima

**Solución:**
- Verificar alturaMaxima del sistema
- Asegurar altura 1-100
- Revisar tabla de mapeos

### Problema: "Aplicación no inicia"

**Solución:**
```powershell
# Limpiar y recompilar
dotnet clean
dotnet build
dotnet run
```

### Problema: "Puerto 5000 ya en uso"

**Solución:**
```powershell
# Cambiar puerto en Program.cs
# Buscar: .UseUrls("http://localhost:5000")
# Cambiar a: .UseUrls("http://localhost:5001")
```

### Problema: "Caché de navegador no actualiza"

**Solución:**
```
- Presionar Ctrl+Shift+Delete
- Seleccionar "Todo el tiempo"
- Limpiar caché e cookies
- Presionar Ctrl+Shift+R para recarga forzada
```

---

## 7. CONSEJOS Y BUENAS PRÁCTICAS

### ✓ Hacer
- Inicializar sistema antes de nueva carga
- Validar XML antes de cargar
- Usar nombres descriptivos para drones/sistemas
- Hacer backup de archivos XML importantes
- Revisar tabla de mapeos antes de optimizar

### ✗ Evitar
- Cargar XML sin validar
- Usar caracteres especiales en nombres
- Cargar archivos XML muy grandes (>10MB)
- Dejar navegador abierto por largo tiempo sin uso
- Intentar cargar mismo archivo múltiples veces

---

**Manual de Usuario** | Sistema de Control de Drones Encriptados | 2026
