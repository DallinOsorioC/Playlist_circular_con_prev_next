using Playlist_circular_con_prev_next;
using System;
using System.Collections.Generic;

internal class Program
{
    private static void Main(string[] args)
    {
        var playlist = new Playlist();

        playlist.AddLast(new Song("Safari", "J Balvin ft. Pharrell", 250));
        playlist.AddLast(new Song("La Bicicleta", "Carlos Vives & Shakira", 228));
        playlist.AddLast(new Song("Ginza", "J Balvin", 190));
        playlist.AddLast(new Song("Duele el Corazon", "Enrique Iglesias ft. Wisin", 215));
        playlist.AddLast(new Song("Safari", "J Balvin ft. Pharrell", 250));
        playlist.AddLast(new Song("Krippy Kush", "Farruko ft. Bad Bunny & Rvssian", 230));
        playlist.AddLast(new Song("Soy Peor", "Bad Bunny", 220));
        playlist.AddLast(new Song("Te Bote", "Nio Garcia, Casper Magico, Darell", 245));
        playlist.AddLast(new Song("Sensualidad", "Bad Bunny, Prince Royce & J Balvin", 210));
        playlist.AddLast(new Song("X", "Nicky Jam & J Balvin", 200));

        int option;
        do
        {
            Console.Clear();
            Console.WriteLine("===== PLAYLIST MENU =====");
            Console.WriteLine("1. Agregar canción");
            Console.WriteLine("2. Eliminar canción");
            Console.WriteLine("3. Ver playlist");
            Console.WriteLine("4. Next");
            Console.WriteLine("5. Prev");
            Console.WriteLine("6. Shuffle");
            Console.WriteLine("7. Exportar JSON");
            Console.WriteLine("8. Salir");
            Console.Write("Selecciona una opción: ");

            if (!int.TryParse(Console.ReadLine(), out option))
                option = 0;

            Console.Clear();

            switch (option)
            {
                case 1:
                    AddSongMenu(playlist);
                    break;

                case 2:
                    RemoveSongMenu(playlist);
                    break;

                case 3:
                    PrintTitles(playlist);
                    break;

                case 4:
                    NextSong(playlist);
                    break;

                case 5:
                    PrevSong(playlist);
                    break;

                case 6:
                    ShuffleMenu(playlist);
                    break;

                case 7:
                    ExportJson(playlist);
                    break;

                case 8:
                    Console.WriteLine("Saliendo...");
                    break;

                default:
                    Console.WriteLine("Opción inválida.");
                    break;
            }

            if (option != 8)
            {
                Console.WriteLine("\nPresiona Enter para continuar...");
                Console.ReadLine();
            }

        } while (option != 8);
    }


    static void AddSongMenu(Playlist pl)
    {
        Console.WriteLine("=== Agregar Canción ===");
        Console.Write("Título: ");
        string title = Console.ReadLine()!;

        Console.Write("Artista: ");
        string artist = Console.ReadLine()!;

        Console.Write("Duración (segundos): ");
        int duration = int.Parse(Console.ReadLine()!);

        var song = new Song(title, artist, duration);
        pl.AddLast(song);

        Console.WriteLine("\nCanción agregada con éxito.");
    }

    static void RemoveSongMenu(Playlist pl)
    {
        Console.WriteLine("=== Eliminar Canción ===");
        PrintTitles(pl);

        Console.Write("\nIngresa el ID de la canción a eliminar: ");
        string input = Console.ReadLine()!;

        if (!Guid.TryParse(input, out Guid id))
        {
            Console.WriteLine("Formato de ID inválido.");
            return;
        }

        bool removed = pl.RemoveById(id);

        Console.WriteLine(removed ? "Canción eliminada." : "No se encontró la canción.");
    }

    static void ShuffleMenu(Playlist pl)
    {
        Console.Write("Ingresa un numero: ");
        int seed = int.Parse(Console.ReadLine()!);

        pl.Shuffle(seed);
        Console.WriteLine("Playlist mezclada.");
    }

    static void ExportJson(Playlist pl)
    {
        string json = pl.ExportTitlesJson();
        Console.WriteLine("=== JSON ===");
        Console.WriteLine(json);
    }

    static void NextSong(Playlist pl)
    {
        try
        {
            var s = pl.Next();
            Console.WriteLine("Siguiente: " + s.Title);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    static void PrevSong(Playlist pl)
    {
        try
        {
            var s = pl.Prev();
            Console.WriteLine("Anterior: " + s.Title);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    static void PrintTitles(Playlist pl)
    {
        Console.WriteLine("=== PLAYLIST ===");

        if (pl.Count == 0)
        {
            Console.WriteLine("(Vacía)");
            return;
        }

        int i = 1;
        foreach (var s in pl)
        {
            Console.WriteLine($"{i++}. {s.Title} — {s.Artist}  (ID: {s.Id})");
        }

        Console.WriteLine("\nCursor actual:");
        Console.WriteLine(pl.Current().Title);
    }
}
