using DocumentFormat.OpenXml.Packaging;
using System.Collections.Concurrent;

namespace FindInDocx
{
    internal class Program
    {
        private static void Main()
        {
            string folderPath = "path/to/your/folder"; // Ścieżka do folderu, w którym szukasz plików
            string searchText = "szukana fraza"; // Fraza, której szukasz

            // Pobierz wszystkie pliki .docx z folderu i zagnieżdżonych folderów
            List<string> docxFiles = Directory.GetFiles(folderPath, "*.docx", SearchOption.AllDirectories).ToList();

            // Kolekcja do przechowywania ścieżek plików, w których znaleziono szukany tekst
            ConcurrentBag<string> foundFiles = new ConcurrentBag<string>();

            // Szukaj frazy we wszystkich plikach równolegle
            Parallel.ForEach(docxFiles, filePath =>
            {
                try
                {
                    if (ContainsText(filePath, searchText))
                    {
                        foundFiles.Add(filePath); // Dodaj ścieżkę do listy, jeśli znaleziono frazę
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing file {filePath}: {ex.Message}");
                }
            });

            // Konwertuj ConcurrentBag na listę
            List<string> result = foundFiles.ToList();

            // Wyświetl wyniki
            if (result.Any())
            {
                Console.WriteLine("Files containing the searched text:");
                foreach (var file in result)
                {
                    Console.WriteLine(file);
                }
            }
            else
            {
                Console.WriteLine($"No files found containing the text: {searchText}");
            }
        }

        private static bool ContainsText(string filePath, string searchText)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
            {
                // Sprawdź, czy treść dokumentu zawiera szukaną frazę
                return wordDoc.MainDocumentPart.Document.Body.InnerText.Contains(searchText, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}