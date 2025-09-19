using System;
using System.Collections.Generic;
using W4_assignment_template.Interfaces;
using W4_assignment_template.Models;
using W4_assignment_template.Services;

namespace W4_assignment_template
{
    class Program
    {
        static IFileHandler fileHandler;
        static List<Character> characters;
        static string filePath;

        static void Main()
        {
            ChooseFileFormat();

            try
            {
                characters = fileHandler.ReadCharacters(filePath);
                Console.WriteLine($"Loaded {characters.Count} characters from file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
                characters = new List<Character>();
            }

            MenuLoop();
        }

        static void ChooseFileFormat()
        {
            Console.WriteLine("Select file format:");
            Console.WriteLine("1. CSV");
            Console.WriteLine("2. JSON");
            Console.Write("Choice: ");
            string choice = Console.ReadLine();

            if (choice == "2")
            {
                fileHandler = new JsonFileHandler();
                filePath = @"C:\Users\jklag\OneDrive\Desktop\mod4dotnetassigment\mod4dotnetassignment2.0\Files\input.json";
            }
            else
            {
                fileHandler = new CsvFileHandler();
                filePath = @"C:\Users\jklag\OneDrive\Desktop\mod4dotnetassigment\mod4dotnetassignment2.0\Files\input.csv";
            }
        }

        static void MenuLoop()
        {
            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Display Characters");
                Console.WriteLine("2. Add Character");
                Console.WriteLine("3. Level Up Character");
                Console.WriteLine("4. Save and Exit");
                Console.Write("Enter your choice: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        DisplayAllCharacters();
                        break;
                    case "2":
                        AddCharacter();
                        break;
                    case "3":
                        LevelUpCharacter();
                        break;
                    case "4":
                        fileHandler.WriteCharacters(filePath, characters);
                        Console.WriteLine("Characters saved. Exiting.");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static void DisplayAllCharacters()
        {
            if (characters.Count == 0)
            {
                Console.WriteLine("No characters found.");
                return;
            }

            foreach (var c in characters)
            {
                Console.WriteLine($"Name: {c.Name}, Class: {c.Class}, Level: {c.Level}, HP: {c.Hp}, Equipment: {string.Join(", ", c.Equipment ?? new List<string>())}");
            }
        }

        static void AddCharacter()
        {
            Console.Write("Enter name: ");
            string name = Console.ReadLine();

            Console.Write("Enter class: ");
            string charClass = Console.ReadLine();

            Console.Write("Enter level: ");
            int.TryParse(Console.ReadLine(), out int level);
            if (level <= 0) level = 1;

            Console.Write("Enter HP: ");
            int.TryParse(Console.ReadLine(), out int hp);
            if (hp <= 0) hp = 1;

            Console.Write("Enter equipment (comma separated): ");
            string equipStr = Console.ReadLine();
            var equipment = (equipStr ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var equipmentList = new List<string>();
            foreach (var e in equipment)
            {
                equipmentList.Add(e.Trim());
            }

            characters.Add(new Character
            {
                Name = name,
                Class = charClass,
                Level = level,
                Hp = hp,
                Equipment = equipmentList
            });

            Console.WriteLine("Character added.");
        }

        static void LevelUpCharacter()
        {
            Console.Write("Enter character name to level up: ");
            string name = Console.ReadLine();

            var character = characters.Find(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));

            if (character == null)
            {
                Console.WriteLine("Character not found.");
                return;
            }

            character.Level++;
            Console.WriteLine($"Level increased. {character.Name} is now level {character.Level}.");
        }
    }
}
