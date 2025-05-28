using System.Diagnostics;

namespace Pull_Projects_2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string baseDir = @"C:\Users\Jarek\Desktop\Istotne\source\Visual Studio\Main";
            var directories = Directory.GetDirectories(baseDir);

            foreach (var dir in directories)
            {
                if (Directory.Exists(Path.Combine(dir, ".git")))
                {
                    Console.WriteLine($"[{Path.GetFileName(dir)}]");

                    // Sprawdź aktualną gałąź
                    string branch = RunGitCommand("rev-parse --abbrev-ref HEAD", dir);
                    Console.WriteLine($"  Branch: {branch}");

                    // Pull
                    string pullOutput = RunGitCommand("pull", dir);
                    Console.WriteLine($"  Pull Output:\n{pullOutput}");

                    // Lista zmian (HEAD@{1} to commit sprzed pulla)
                    string changedFiles = RunGitCommand("diff --name-only HEAD@{1} HEAD", dir);
                    if (!string.IsNullOrWhiteSpace(changedFiles))
                    {
                        Console.WriteLine("  Changed Files:");
                        foreach (var line in changedFiles.Split('\n'))
                            Console.WriteLine($"    {line.Trim()}");
                    }

                    Console.WriteLine();
                }
            }

            Console.WriteLine("Hello, World!");
        }

        private static string RunGitCommand(string arguments, string workingDir)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = arguments,
                WorkingDirectory = workingDir,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            return string.IsNullOrWhiteSpace(error) ? output : $"ERROR: {error}";
        }
    }
}