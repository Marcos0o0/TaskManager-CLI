using System.Text.Json;

namespace TaskManager
{
    public class Task
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // Pending, In Progress, Completed
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            List<Task> tasks = LoadTasks();

            Console.WriteLine("Bienvenido al Task Tracker CLI!");
            Console.WriteLine("Comandos disponibles: add, list, delete, update, mark-done, mark-in-progress, help, exit");

            while (true)
            {
                Console.Write("task-cli > ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                string[] inputArgs = input.Split(' ', 2); // primer elemento comando, segundo argumento opcional
                string comando = inputArgs[0].ToLower();
                string argumento = inputArgs.Length > 1 ? inputArgs[1] : null;

                switch (comando)
                {
                    case "add":
                        if (string.IsNullOrWhiteSpace(argumento))
                        {
                            WriteError("Debes ingresar la descripción de la tarea.");
                            break;
                        }

                        int nuevoId = tasks.Count == 0 ? 1 : tasks.Max(t => t.Id) + 1;
                        Task nuevaTarea = new Task
                        {
                            Id = nuevoId,
                            Description = argumento,
                            Status = "Pending",
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };

                        tasks.Add(nuevaTarea);
                        SaveTasks(tasks);
                        WriteSuccess($"Tarea agregada, ID: {nuevoId}");
                        break;

                    case "list":
                        ListTasks(tasks, argumento);
                        break;

                    case "delete":
                        ChangeTask(tasks, "delete", argumento);
                        break;

                    case "update":
                        ChangeTask(tasks, "update", argumento);
                        break;

                    case "mark-done":
                        ChangeTask(tasks, "Completed", argumento);
                        break;

                    case "mark-in-progress":
                        ChangeTask(tasks, "In Progress", argumento);
                        break;

                    case "help":
                        ShowHelp();
                        break;

                    case "exit":
                        return;

                    default:
                        WriteError("Comando no reconocido. Usa 'help' para ver los comandos.");
                        break;
                }
            }
        }

        static void ListTasks(List<Task> tasks, string filter = null)
        {
            IEnumerable<Task> tareasFiltradas = tasks;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                filter = filter.ToLower();
                tareasFiltradas = filter switch
                {
                    "todo" => tasks.Where(t => t.Status == "Pending"),
                    "in-progress" => tasks.Where(t => t.Status == "In Progress"),
                    "done" => tasks.Where(t => t.Status == "Completed"),
                    _ => tasks.Where(t => t.Description.ToLower().Contains(filter))
                };
            }

            if (!tareasFiltradas.Any())
            {
                WriteInfo("No hay tareas para mostrar.");
                return;
            }

            Console.WriteLine("\nID | STATUS       | DESCRIPTION                 | CREATED AT           | UPDATED AT");
            Console.WriteLine("-------------------------------------------------------------------------------------");

            foreach (var t in tareasFiltradas)
            {
                // Truncar la descripción para que no se desborde
                string descripcionTruncada = t.Description.Length > 27
                    ? t.Description.Substring(0, 24) + "..."
                    : t.Description;

                // Formatear las fechas para que sean más compactas
                string fechaCreacion = t.CreatedAt.ToString("dd-MM-yyyy HH:mm");
                string fechaActualizacion = t.UpdatedAt.ToString("dd-MM-yyyy HH:mm");

                // Crear la línea completa primero
                string linea = $"{t.Id,-3}| {t.Status,-12}| {descripcionTruncada,-27}| {fechaCreacion,-16}| {fechaActualizacion,-16}";

                // Aplicar color solo al status para indicar el estado
                Console.Write($"{t.Id,-3}| ");

                // Colorear solo el status
                Console.ForegroundColor = t.Status switch
                {
                    "Pending" => ConsoleColor.Red,
                    "In Progress" => ConsoleColor.Yellow,
                    "Completed" => ConsoleColor.Green,
                    _ => ConsoleColor.White
                };
                Console.Write($"{t.Status,-12}");
                Console.ResetColor();

                // Resto de la línea en color normal
                Console.WriteLine($"| {descripcionTruncada,-27}| {fechaCreacion,-16}| {fechaActualizacion,-16}");
            }

            Console.WriteLine();
        }

        static void ChangeTask(List<Task> tasks, string action, string argumento)
        {
            if (string.IsNullOrWhiteSpace(argumento))
            {
                WriteError("Debes ingresar el ID de la tarea.");
                return;
            }

            if (!int.TryParse(argumento.Split(' ')[0], out int id))
            {
                WriteError("ID inválido.");
                return;
            }

            Task tarea = tasks.FirstOrDefault(t => t.Id == id);
            if (tarea == null)
            {
                WriteError("No se encontró ninguna tarea con ese ID.");
                return;
            }

            if (action == "delete")
            {
                tasks.Remove(tarea);
                SaveTasks(tasks);
                WriteSuccess($"Tarea con ID {id} eliminada.");
            }
            else if (action == "update")
            {
                string nuevaDesc = string.Join(" ", argumento.Split(' ').Skip(1));
                if (string.IsNullOrWhiteSpace(nuevaDesc))
                {
                    WriteError("Debes ingresar la nueva descripción.");
                    return;
                }

                tarea.Description = nuevaDesc;
                tarea.UpdatedAt = DateTime.Now;
                SaveTasks(tasks);
                WriteSuccess($"Tarea con ID {id} actualizada.");
            }
            else // mark-done / mark-in-progress
            {
                tarea.Status = action;
                tarea.UpdatedAt = DateTime.Now;
                SaveTasks(tasks);
                WriteSuccess($"Tarea con ID {id} marcada como {action}.");
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine();
            WriteInfo("=== Task Tracker CLI - Ayuda ===\n");

            WriteInfo("Comandos principales:");

            WriteCommand("add", "<descripcion>");
            WriteInfo("  Agrega una nueva tarea con la descripción indicada.");

            WriteCommand("list", "[todo|in-progress|done|palabraClave]");
            WriteInfo("  Lista todas las tareas. Opcional: filtra por estado o busca por palabra clave.");

            WriteCommand("update", "<id> <nueva descripcion>");
            WriteInfo("  Actualiza la descripción de la tarea con el ID especificado.");

            WriteCommand("delete", "<id>");
            WriteInfo("  Elimina la tarea con el ID indicado.");

            WriteCommand("mark-done", "<id>");
            WriteInfo("  Marca la tarea con el ID indicada como completada.");

            WriteCommand("mark-in-progress", "<id>");
            WriteInfo("  Marca la tarea con el ID indicado como en progreso.");

            WriteCommand("help", "");
            WriteInfo("  Muestra esta guía de ayuda.");

            WriteCommand("exit", "");
            WriteInfo("  Cierra la aplicación.\n");

            WriteInfo("Ejemplos de uso:");
            WriteInfo("  add Comprar leche");
            WriteInfo("  list todo");
            WriteInfo("  update 1 Comprar pan integral");
            WriteInfo("  mark-done 1");
            WriteInfo("  delete 2");
            Console.WriteLine();
        }

        // Helpers para colorear
        static void WriteCommand(string command, string args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(command);
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (!string.IsNullOrEmpty(args))
                Console.Write(" " + args);
            Console.ResetColor();
            Console.WriteLine();
        }

        static void WriteInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        static void WriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        static List<Task> LoadTasks()
        {
            var tasks = new List<Task>();
            try
            {
                if (File.Exists("tasks.json"))
                {
                    string json = File.ReadAllText("tasks.json");
                    tasks.AddRange(JsonSerializer.Deserialize<List<Task>>(json) ?? new List<Task>());
                }
            }
            catch
            {
                WriteError("Error leyendo tasks.json. Se iniciará una lista vacía.");
            }
            return tasks;
        }

        static void SaveTasks(List<Task> tasks)
        {
            string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("tasks.json", json);
        }
    }
}