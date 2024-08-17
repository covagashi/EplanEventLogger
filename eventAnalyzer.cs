using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class EventAnalyzer
{
    private const string LOG_FILE_PATH = @"C:\Test\Events.csv";

    public void AnalyzeEvents()
    {
        try
        {
            var lines = File.ReadAllLines(LOG_FILE_PATH).Skip(1); // Saltamos la línea de encabezado
            var events = lines.Select(line => line.Split(',')).ToList();

            Console.WriteLine($"Total de eventos registrados: {events.Count}");

            // Análisis por usuario
            var eventsByUser = events.GroupBy(e => e[3])
                                     .Select(g => new { User = g.Key, Count = g.Count() })
                                     .OrderByDescending(x => x.Count);

            Console.WriteLine("\nEventos por usuario:");
            foreach (var user in eventsByUser)
            {
                Console.WriteLine($"{user.User}: {user.Count}");
            }

            // Análisis por acción
            var topActions = events.GroupBy(e => e[2])
                                   .Select(g => new { Action = g.Key, Count = g.Count() })
                                   .OrderByDescending(x => x.Count)
                                   .Take(10);

            Console.WriteLine("\nTop 10 acciones más frecuentes:");
            foreach (var action in topActions)
            {
                Console.WriteLine($"{action.Action}: {action.Count}");
            }

            // Análisis por proyecto
            var projectActivity = events.GroupBy(e => e[4])
                                        .Select(g => new { Project = g.Key, Count = g.Count() })
                                        .OrderByDescending(x => x.Count);

            Console.WriteLine("\nActividad por proyecto:");
            foreach (var project in projectActivity)
            {
                Console.WriteLine($"{project.Project}: {project.Count}");
            }

            // Puedes añadir más análisis aquí según tus necesidades
        }
        catch (Exception exc)
        {
            Console.WriteLine($"Error al analizar los eventos: {exc.Message}");
        }
    }
}
