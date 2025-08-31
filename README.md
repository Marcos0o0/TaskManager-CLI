# Task Manager CLI

Un administrador de tareas simple y elegante desarrollado en C# para la línea de comandos. Permite gestionar tareas de forma eficiente con persistencia de datos en JSON.

## ✨ Características

- **Interfaz CLI intuitiva** con colores para mejor experiencia visual
- **Persistencia de datos** mediante archivos JSON
- **Estados de tareas**: Pending, In Progress, Completed
- **Filtrado avanzado** por estado o palabra clave
- **Timestamps automáticos** de creación y actualización
- **Comandos simples** y fáciles de recordar

## 🚀 Instalación

### Requisitos previos
- .NET 6.0 o superior
- Sistema operativo compatible con .NET (Windows, macOS, Linux)

### Pasos de instalación

1. **Clonar el repositorio**
```bash
git clone https://github.com/Marcos0o0/TaskManager-CLI.git
cd task-manager-cli
```

2. **Compilar el proyecto**
```bash
dotnet build
```

3. **Ejecutar la aplicación**
```bash
dotnet run
```

## 📖 Uso

Al ejecutar la aplicación, verás el prompt interactivo:

```
Bienvenido al Task Tracker CLI!
Comandos disponibles: add, list, delete, update, mark-done, mark-in-progress, help, exit
task-cli >
```

## 🛠️ Comandos

### Agregar una tarea
```bash
add <descripción>
```
**Ejemplo:**
```bash
add Comprar leche en el supermercado
```

### Listar tareas
```bash
list [filtro]
```

**Opciones de filtro:**
- `todo` - Solo tareas pendientes
- `in-progress` - Solo tareas en progreso  
- `done` - Solo tareas completadas
- `palabra` - Buscar por palabra clave en la descripción

**Ejemplos:**
```bash
list                    # Listar todas las tareas
list todo              # Solo pendientes
list done              # Solo completadas
list comprar           # Buscar tareas que contengan "comprar"
```

### Actualizar una tarea
```bash
update <id> <nueva descripción>
```
**Ejemplo:**
```bash
update 1 Comprar pan integral y leche
```

### Cambiar estado de una tarea
```bash
mark-done <id>         # Marcar como completada
mark-in-progress <id>  # Marcar como en progreso
```
**Ejemplos:**
```bash
mark-done 1
mark-in-progress 2
```

### Eliminar una tarea
```bash
delete <id>
```
**Ejemplo:**
```bash
delete 3
```

### Otros comandos
```bash
help    # Mostrar ayuda
exit    # Salir de la aplicación
```

## 🎨 Códigos de colores

El CLI utiliza colores para facilitar la identificación del estado de las tareas:

- 🔴 **Rojo**: Tareas pendientes (Pending)
- 🟡 **Amarillo**: Tareas en progreso (In Progress) 
- 🟢 **Verde**: Tareas completadas (Completed)

## 📁 Estructura de datos

Las tareas se almacenan en un archivo `tasks.json` en el directorio de ejecución con la siguiente estructura:

```json
[
  {
    "Id": 1,
    "Description": "Comprar leche",
    "Status": "Pending",
    "CreatedAt": "2025-08-31T10:30:00",
    "UpdatedAt": "2025-08-31T10:30:00"
  }
]
```

## 📊 Ejemplo de uso

```bash
task-cli > add Revisar emails
Tarea agregada, ID: 1

task-cli > add Preparar presentación
Tarea agregada, ID: 2

task-cli > list
ID | STATUS       | DESCRIPTION           | CREATED AT       | UPDATED AT
-------------------------------------------------------------------------------
1  | Pending      | Revisar emails        | 31-08-2025 10:30 | 31-08-2025 10:30
2  | Pending      | Preparar presentación | 31-08-2025 10:31 | 31-08-2025 10:31

task-cli > mark-in-progress 1
Tarea con ID 1 marcada como In Progress.

task-cli > mark-done 2
Tarea con ID 2 marcada como Completed.

task-cli > list
ID | STATUS       | DESCRIPTION           | CREATED AT       | UPDATED AT
-------------------------------------------------------------------------------
1  | In Progress  | Revisar emails        | 31-08-2025 10:30 | 31-08-2025 10:32
2  | Completed    | Preparar presentación | 31-08-2025 10:31 | 31-08-2025 10:33
```

## 🔧 Características técnicas

- **Lenguaje**: C# (.NET 6+)
- **Persistencia**: System.Text.Json para serialización
- **Arquitectura**: CLI interactivo con loop principal
- **Manejo de errores**: Validación de entrada y mensajes informativos
- **Colores**: Soporte para terminales con color

## 🤝 Contribuir

1. Fork del proyecto
2. Crear una rama para tu feature (`git checkout -b feature/nueva-funcionalidad`)
3. Commit de tus cambios (`git commit -am 'Agregar nueva funcionalidad'`)
4. Push a la rama (`git push origin feature/nueva-funcionalidad`)
5. Crear un Pull Request
---

⭐ Si te gusta este proyecto, ¡dame una estrella!