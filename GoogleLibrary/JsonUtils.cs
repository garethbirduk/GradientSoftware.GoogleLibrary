using Newtonsoft.Json;

namespace GoogleLibrary
{
    public class JsonUtils
    {
        public static List<T> LoadFromFile<T>(string filePath)
            where T : class
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file {filePath} does not exist.");
            }

            var json = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<T>(); // Ensure we don't return null
            }

            var result = JsonConvert.DeserializeObject<List<T>>(json);
            return result ?? new List<T>(); // Safely handle null deserialization
        }

        public static void SaveToFile<T>(List<T> list, string filePath)
            where T : class
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list), "Data to save cannot be null.");
            }

            var json = JsonConvert.SerializeObject(list, Formatting.Indented);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                File.WriteAllText(filePath, json);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error writing to file {filePath}: {ex.Message}");
                throw;
            }
        }
    }
}