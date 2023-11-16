using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

class GuestBookEntry
{
    public string Owner { get; set; }
    public string Message { get; set; }
}

class GuestBook
{
    private List<GuestBookEntry> entries;

    public GuestBook()
    {
        entries = new List<GuestBookEntry>();
        LoadEntries(); // Ladda tidigare sparade poster vid start
    }

    public void AddEntry()
    {
        Console.Write("Ange ägaren av inlägget: ");
        string owner = Console.ReadLine();

        Console.Write("Skriv ditt inlägg: ");
        string message = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(owner) || string.IsNullOrWhiteSpace(message))
        {
            Console.WriteLine("Fel: Ägare eller inläggstext får inte vara tomma.");
            return;
        }

        entries.Add(new GuestBookEntry { Owner = owner, Message = message });
        SaveEntries();
    }

    public void RemoveEntry()
    {
        Console.WriteLine("Välj index för inlägg att ta bort:");
        ShowEntries();

        if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < entries.Count)
        {
            entries.RemoveAt(index);
            SaveEntries();
        }
        else
        {
            Console.WriteLine("Ogiltigt index.");
        }
    }

    public void ShowEntries()
    {
        for (int i = 0; i < entries.Count; i++)
        {
            Console.WriteLine($"[{i}] Ägare: {entries[i].Owner}, Inlägg: {entries[i].Message}");
        }
    }

    private void SaveEntries()
    {
        string json = JsonConvert.SerializeObject(entries);
        File.WriteAllText("guestbook.json", json);
    }

    private void LoadEntries()
    {
        if (File.Exists("guestbook.json"))
        {
            string json = File.ReadAllText("guestbook.json");
            entries = JsonConvert.DeserializeObject<List<GuestBookEntry>>(json);
        }
    }
}

class Program
{
    static void Main()
    {
        GuestBook guestBook = new GuestBook();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Välkommen till Dominikas gästbok!");
            Console.WriteLine("1. Lägg till inlägg");
            Console.WriteLine("2. Ta bort inlägg");
            Console.WriteLine("3. Visa alla inlägg");
            Console.WriteLine("4. Avsluta");
            Console.Write("Välj ett alternativ: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        guestBook.AddEntry();
                        break;
                    case 2:
                        guestBook.RemoveEntry();
                        break;
                    case 3:
                        guestBook.ShowEntries();
                        break;
                    case 4:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Ogiltigt val. Försök igen.");
            }

            Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
            Console.ReadKey();
        }
    }
}

