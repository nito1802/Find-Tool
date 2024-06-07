using System.Diagnostics;

namespace Pull_Projects
{
    internal class Program
    {
        private static void Main()
        {
            string baseDir = @"C:\Users\dante\Desktop\Istotne\source\Visual Studio\Main";
            ExecutePowerShellScript(baseDir);
        }

        private static void ExecutePowerShellScript(string baseDir)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"pull_repos.ps1\" -baseDir \"{baseDir}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                process.BeginOutputReadLine();
                process.ErrorDataReceived += (sender, e) => Console.WriteLine($"ERROR: {e.Data}");
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
        }
    }
}