using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using W4_assignment_template.Interfaces;
using W4_assignment_template.Models;

namespace W4_assignment_template.Services
{
    public class JsonFileHandler : IFileHandler
    {
        public List<Character> ReadCharacters(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"JSON file not found: {filePath}");

            try
            {
                var json = File.ReadAllText(filePath);
                var characters = JsonConvert.DeserializeObject<List<Character>>(json);
                return characters ?? new List<Character>();
            }
            catch (JsonException ex)
            {
                throw new InvalidDataException($"Error parsing JSON file {filePath}: {ex.Message}");
            }
        }

        public void WriteCharacters(string filePath, List<Character> characters)
        {
            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            try
            {
                var json = JsonConvert.SerializeObject(characters, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to write JSON file {filePath}: {ex.Message}");
            }
        }
    }
}
