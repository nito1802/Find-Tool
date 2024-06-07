namespace Pull_Projects
{
    internal class Program
    {
        private static void Main()
        {
            string filePath = @"C:\Users\vvv\Desktop\Istotne\source\Visual Studio\Main\Find Tool\Find Tool\App.xaml.cs";
            string userName = GetUserNameFromPath(filePath);
            Console.WriteLine($"User name: {userName}");
        }

        private static string GetUserNameFromPath(string path)
        {
            // Użycie klasy Path do uzyskania segmentów ścieżki
            string[] segments = path.Split(Path.DirectorySeparatorChar);

            // Szukanie segmentu "Users" i pobieranie kolejnego segmentu jako nazwy użytkownika
            for (int i = 0; i < segments.Length - 1; i++)
            {
                if (segments[i].Equals("Users", StringComparison.OrdinalIgnoreCase))
                {
                    return segments[i + 1];
                }
            }

            return null; // Zwróć null, jeśli nie znaleziono segmentu "Users"
        }
    }
}