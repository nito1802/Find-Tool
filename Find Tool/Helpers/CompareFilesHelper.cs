using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace ConvertUrls.Helpers
{
    public static class CompareFilesHelper
    {
        public static string CompareTextsOk { get; } = "Files are identically!";

        public static string CompareTwoFiles(string leftPath, string rightPath, ref TimeSpan elapsed)
        {
            string[] leftLines = null;
            string[] rightLines = null;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                //tblLeftFilePath.Text = @"C:\Users\jaroslaws\Desktop\MojeDane\2019\styczeń\24_01_2019\Inne\BatchModeModifsyBTV.txt";
                //leftLines = File.ReadLines(tblLeftFilePath.Text).OrderBy(a => a).ToArray();
                leftLines = File.ReadAllText(leftPath).Split(',').OrderBy(a => a).ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show("First path is not found! " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            try
            {
                //tblRightFilePath.Text = @"C:\Users\jaroslaws\Desktop\MojeDane\2019\styczeń\24_01_2019\Inne\NOTBatchModeModifsy.txt";
                //rightLines = File.ReadLines(tblRightFilePath.Text).OrderBy(a => a).ToArray();
                rightLines = File.ReadAllText(rightPath).Split(',').OrderBy(a => a).ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Second path is not found! " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            string compareResultContent = "";

            var leftExcRight = leftLines.Except(rightLines).ToList();
            var rightExcLeft = rightLines.Except(leftLines).ToList();

            var leftExcRightNotArrivedHeader = leftExcRight.Select(a => Regex.Match(a, @"^(.*?):.*").Groups[1].ToString()).ToList();
            var rightExcLeftNotArrivedHeader = rightExcLeft.Select(a => Regex.Match(a, @"^(.*?):.*").Groups[1].ToString()).ToList();

            var groupLeft = leftExcRightNotArrivedHeader.GroupBy(a => a).Select(b => new { Key = b.Key, Ilosc = b.Count() }).OrderBy(c => c.Ilosc).ToList();
            var groupRight = rightExcLeftNotArrivedHeader.GroupBy(a => a).Select(b => new { Key = b.Key, Ilosc = b.Count() }).OrderBy(c => c.Ilosc).ToList();

            var leftExcRightNotArrived = leftExcRight.Where(a => !a.Contains("ArrivedAt")).ToList();
            var rightExcLeftNotArrived = rightExcLeft.Where(a => !a.Contains("ArrivedAt")).ToList();

            if (leftLines.Length != rightLines.Length)
            {
                compareResultContent = string.Join(Environment.NewLine, $"Files length are different! First have {leftLines.Length} lines and second have {rightLines.Length} lines.",
                                                                        $"Left except Right {leftExcRight.Count} lines and Right except left {rightExcLeft.Count} lines.");
            }
            else if (!(leftExcRight.Count == 0 && rightExcLeft.Count == 0))
            {
                compareResultContent = $"Files are different! Left except Right {leftExcRight.Count} lines and Right except left {rightExcLeft.Count} lines.";
            }
            else
            {
                compareResultContent = CompareTextsOk;
            }
            sw.Stop();
            elapsed = sw.Elapsed;

            return compareResultContent;
        }
    }
}