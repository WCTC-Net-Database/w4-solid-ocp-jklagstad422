using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using W4_assignment_template.Interfaces;
using W4_assignment_template.Models;
using Microsoft.VisualBasic.FileIO;

namespace W4_assignment_template.Services
{
    public class CsvFileHandler : IFileHandler
    {
        public List<Character> ReadCharacters(string filePath)
        {
            var characters = new List<Character>();
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"CSV file not found: {filePath}");

            using (var parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = true;

                string[] headers = parser.ReadFields(); // read header and discard

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    if (fields.Length < 5) continue;

                    var character = new Character
                    {
                        Name = fields[0],
                        Class = fields[1],
                        Level = int.TryParse(fields[2], out int level) && level > 0 ? level : 1,
                        Hp = int.TryParse(fields[3], out int hp) && hp > 0 ? hp : 1,
                        Equipment = fields[4].Split('|', StringSplitOptions.RemoveEmptyEntries).ToList()
                    };
                    characters.Add(character);
                }
            }
            return characters;
        }

        public void WriteCharacters(string filePath, List<Character> characters)
        {
            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (var writer = new StreamWriter(filePath, false))
            {
                // Write header
                writer.WriteLine("Name,Class,Level,HP,Equipment");
                foreach (var c in characters)
                {
                    string equipment = c.Equipment != null ? string.Join("|", c.Equipment) : "";
                    // Quote name if contains comma
                    string name = c.Name.Contains(",") ? $"\"{c.Name}\"" : c.Name;
                    writer.WriteLine($"{name},{c.Class},{c.Level},{c.Hp},{equipment}");
                }
            }
        }
    }
}

