using DocumentFormat.OpenXml.Packaging;

namespace FindInDocx
{
    internal class Program
    {
        private static void Main()
        {
            // Ścieżka do folderu "Template" w projekcie WebApi
            string sourceFolder = @"C:\Users\dante\Desktop\Istotne\source\Visual Studio\Test\FindInDocx\WebApplication1\Templates\"; // Zmień ścieżkę na właściwą do folderu Template w projekcie WebApi

            // Ścieżka do folderu "TxtTemplates" w projekcie konsolowym
            string destinationFolder = @"C:\Users\dante\Desktop\Istotne\source\Visual Studio\Test\FindInDocx\FindInDocx\TxtTemplates\"; // Zmień ścieżkę na właściwą do folderu TxtTemplates w projekcie konsolowym

            // Pobierz wszystkie pliki .docx, w tym zagnieżdżone
            List<string> docxFiles = Directory.GetFiles(sourceFolder, "*.docx", SearchOption.AllDirectories).ToList();

            // Użycie równoległego przetwarzania do konwersji plików
            Parallel.ForEach(docxFiles, docxFilePath =>
            {
                try
                {
                    // Wczytaj zawartość dokumentu Word jako plain text
                    string plainText = ExtractPlainTextFromWord(docxFilePath);

                    // Zmodyfikuj ścieżkę docelową, aby odwzorować strukturę źródłową
                    string relativePath = System.IO.Path.GetRelativePath(sourceFolder, docxFilePath);
                    string txtFilePath = System.IO.Path.Combine(destinationFolder, System.IO.Path.ChangeExtension(relativePath, ".txt"));

                    // Upewnij się, że katalog docelowy istnieje
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(txtFilePath));

                    // Zapisz plain text do pliku .txt
                    File.WriteAllText(txtFilePath, plainText);

                    Console.WriteLine($"Successfully converted: {docxFilePath} to {txtFilePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing {docxFilePath}: {ex.Message}");
                }
            });

            Console.WriteLine("Conversion completed.");

            /*
            string input = "<<doc [DocumentTitleTemplate]>> Dane zlecenia <<doc [OrderDataTemplate] -build>> Dane uczestnika";

            // Wyrażenie regularne do znalezienia wzorców <<cośtam>>
            string pattern = @"(<<.*?>>)";

            // Podział tekstu na nowe linie
            string result = Regex.Replace(input, pattern, Environment.NewLine + "$1" + Environment.NewLine);

            // Uporządkowanie tekstu - usunięcie podwójnych nowych linii
            result = Regex.Replace(result, @"\n\s*\n", Environment.NewLine);

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
            }*/
        }

        // Funkcja do odczytywania plain textu z dokumentu Word
        private static string ExtractPlainTextFromWord(string filePath)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
            {
                // Odczytaj zawartość dokumentu jako plain text
                return wordDoc.MainDocumentPart.Document.Body.InnerText;
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