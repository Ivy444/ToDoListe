using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    // Dateipfad
    static string dateiPfad = "todos.txt";
    // Liste für alle Todos
    static List<string> beschreibungen = new List<string>();
    static List<string> fälligkeitsDaten = new List<string>();
    static List<bool> erledigt = new List<bool>();

    static void Main(string[] args)
    {
        // Todos aus Datei laden
        TodosLaden();

        bool programmLäuft = true;

        // Hauptschleife des Programms
        while (programmLäuft)
        {
            // Menü anzeigen
            Console.WriteLine("\n=== ToDo Liste ===");
            Console.WriteLine("1. Aufgaben anzeigen");
            Console.WriteLine("2. Neue Aufgabe hinzufügen");
            Console.WriteLine("3. Aufgabe bearbeiten");
            Console.WriteLine("4. Aufgabe löschen");
            Console.WriteLine("5. Speichern und beenden");
            Console.Write("\nWähle eine Option (1-5): ");
            
            // Benutzereingabe lesen
            string auswahl = Console.ReadLine();
            Console.Clear();
            // Aktionen basierend auf Auswahl
            switch (auswahl)
            {
                case "1":
                    AufgabenAnzeigen();
                    break;
                case "2":
                    NeueAufgabeHinzufügen();
                    break;
                case "3":
                    AufgabeBearbeiten();
                    break;
                case "4":
                    AufgabeLöschen();
                    break;
                case "5":
                    TodosSpeichern();
                    Console.WriteLine("\nÄnderungen wurden gespeichert. Programm wird beendet...");
                    programmLäuft = false;
                    return;
                default:
                    Console.WriteLine("\nUngültige Eingabe. Bitte erneut versuchen.");;
                    break;
            }
        }
    }

    // Zeigt alle Aufgaben an
    static void AufgabenAnzeigen()
    {
        Console.WriteLine("\n=== Aktuelle Aufgaben ===");
        
        // Prüfen ob Aufgaben vorhanden sind
        if (beschreibungen.Count == 0)
        {
            Console.WriteLine("Keine Aufgaben vorhanden.");
            return;
        }

        // Alle Aufgaben durchgehen und anzeigen
        for (int i = 0; i < beschreibungen.Count; i++)
        {
            string erledigtZeichen;
            if (erledigt[i] == true)
            {
                erledigtZeichen = "X";
            }
            else
            {
                erledigtZeichen = " ";
            }
            Console.WriteLine($"{i + 1}. [{erledigtZeichen}] {beschreibungen[i]} (Fällig: {fälligkeitsDaten[i]})");
        }
        
        Console.ReadKey();
        Console.Clear();
    }

    // Neue Aufgabe hinzufügen
    static void NeueAufgabeHinzufügen()
    {
        Console.Write("\nBeschreibung der Aufgabe: ");
        string beschreibung = Console.ReadLine();

        string datum = "";
        bool datumGültig = false;

        // Datum abfragen bis ein gültiges Format eingegeben wird
        while (!datumGültig)
        {
            Console.Write("Fälligkeitsdatum (TT.MM.YYYY): ");
            datum = Console.ReadLine();
            
            // Einfache Überprüfung ob das Datum etwa richtig aussieht
            if (datum.Length == 10 && datum[2] == '.' && datum[5] == '.')
            {
                datumGültig = true;
            }
            else
            {
                Console.WriteLine("Ungültiges Datumsformat. Bitte erneut versuchen.");
            }
        }

        // Aufgabe zu den Listen hinzufügen
        beschreibungen.Add(beschreibung);
        fälligkeitsDaten.Add(datum);
        erledigt.Add(false);

        TodosSpeichern();
        Console.WriteLine("Aufgabe wurde hinzugefügt!");
        Console.ReadKey();
        Console.Clear();
    }

    // Bestehende Aufgabe bearbeiten
    static void AufgabeBearbeiten()
    {
        AufgabenAnzeigen();
        if (beschreibungen.Count == 0) return;

        Console.Write("\nWelche Aufgabe möchtest du bearbeiten? (Nummer): ");
        string eingabe = Console.ReadLine();
        
        // Versuchen die Eingabe in eine Zahl umzuwandeln
        int nummer;
        bool istZahl = int.TryParse(eingabe, out nummer);
        
        // Prüfen ob die Nummer gültig ist
        if (istZahl && nummer > 0 && nummer <= beschreibungen.Count)
        {
            int index = nummer - 1;

            Console.Write($"Neue Beschreibung ({beschreibungen[index]}): ");
            string neueBeschreibung = Console.ReadLine();
            if (neueBeschreibung != "")
            {
                beschreibungen[index] = neueBeschreibung;
            }

            Console.Write($"Neues Fälligkeitsdatum ({fälligkeitsDaten[index]}): ");
            string neuesDatum = Console.ReadLine();
            if (neuesDatum != "")
            {
                fälligkeitsDaten[index] = neuesDatum;
            }

            Console.Write($"Aufgabe abgeschlossen? (j/n): ");
            string fertig = Console.ReadLine().ToLower();
            if (fertig == "j")
            {
                erledigt[index] = true;
            }
            else if (fertig == "n")
            {
                erledigt[index] = false;
            }

            TodosSpeichern();
            Console.WriteLine("Aufgabe wurde aktualisiert!");
            Console.ReadKey();
            Console.Clear();
        }
        else
        {
            Console.WriteLine("Ungültige Nummer.");
            Console.ReadKey();
            Console.Clear();
        }
    }

    // Aufgabe löschen
    static void AufgabeLöschen()
    {
        AufgabenAnzeigen();
        if (beschreibungen.Count == 0) return;

        Console.Write("\nWelche Aufgabe möchtest du löschen? (Nummer): ");
        string eingabe = Console.ReadLine();
        
        int nummer;
        bool istZahl = int.TryParse(eingabe, out nummer);
        
        if (istZahl && nummer > 0 && nummer <= beschreibungen.Count)
        {
            int index = nummer - 1;
            
            // Aus allen Listen entfernen
            beschreibungen.RemoveAt(index);
            fälligkeitsDaten.RemoveAt(index);
            erledigt.RemoveAt(index);
            
            TodosSpeichern();
            Console.WriteLine("Aufgabe wurde gelöscht!");
            Console.ReadKey();
            Console.Clear();
        }
        else
        {
            Console.WriteLine("Ungültige Nummer.");
            Console.ReadKey();
            Console.Clear();
        }
    }

    // Todos aus Datei laden
    static void TodosLaden()
    {
        if (File.Exists(dateiPfad))
        {
            string[] zeilen = File.ReadAllLines(dateiPfad);
            
            foreach (string zeile in zeilen)
            {
                string[] teile = zeile.Split('|');
                
                beschreibungen.Add(teile[0]);
                fälligkeitsDaten.Add(teile[1]);
                erledigt.Add(teile[2] == "True");
            }
        }
    }

    // Todos in Datei speichern
    static void TodosSpeichern()
    {
        List<string> zeilen = new List<string>();
        
        for (int i = 0; i < beschreibungen.Count; i++)
        {
            zeilen.Add(beschreibungen[i] + "|" + fälligkeitsDaten[i] + "|" + erledigt[i]);
        }
        
        File.WriteAllLines(dateiPfad, zeilen);
    }
}




