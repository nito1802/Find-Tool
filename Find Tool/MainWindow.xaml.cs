using ConvertUrls.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ConvertUrls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow : Window
    {
        public int MaxCountOfVisibleFiles { get; } = 30;
        public string SearchUrl { get; set; } = @"https://next-test.media-press.tv";
        public string SearchLocalhost { get; set; } = @"http://localhost:12626/api";
        public string CompareTextsOk { get; } = "Files are identically!";

        public List<string> FindPaths { get; set; } = new List<string>();
        public string[] StartFindPaths { get; set; }
        public DispatcherTimer FindTimer { get; set; } = new DispatcherTimer();
        public Stopwatch SwFind { get; set; } = new Stopwatch();
        public Task TaskFind { get; set; }
        private CancellationTokenSource Cts = new CancellationTokenSource();
        public Dictionary<string, string> UrlMappings { get; set; }

        //public string LeftPath { get; set; }
        //public string RightPath { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            FindTimer.Interval = TimeSpan.FromMilliseconds(1);
            FindTimer.Tick += FindTimer_Tick;

            UrlMappings = new Dictionary<string, string>
            {
                { "https://next-test-search-api.media-press.tv", "http://localhost:12626"},
                { "https://next-search-api.media-press.tv", "http://localhost:12626"},
                { "https://next-automats-search-api.media-press.tv", "http://localhost:12626"},
                { "https://next-import5-test-api.media-press.tv", "http://localhost:49186"},
                { "https://next-import5-api.media-press.tv", "http://localhost:49186"},
                { "https://next-api-test.media-press.tv", "http://localhost:54641"},
                { "https://next-api.media-press.tv", "http://localhost:54641"}
            };
        }

        private void FindTimer_Tick(object sender, EventArgs e)
        {
            tblFindsProcessTime.Text = $"{SwFind.ElapsedMilliseconds}ms";
        }

        //private void btnSearchUrlToLocalhost_Click(object sender, RoutedEventArgs e)
        //{
        //    string nextUrl = tbNextUrl.Text.Trim();

        //    var mappingUrl = UrlMappings.Keys.FirstOrDefault(a => nextUrl.Contains(a));

        //    if (mappingUrl == null)
        //    {
        //        tbLocalhostUrl.Text = string.Empty;
        //        return;
        //    }

        //    var dictItem = UrlMappings[mappingUrl];

        //    string localhostUrl = nextUrl.Replace(mappingUrl, dictItem);
        //    tbLocalhostUrl.Text = localhostUrl;
        //}

        private void tbSearchUrl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 3)
                (sender as TextBox).SelectAll();
        }

        //private void btnCopySearchUrl_Click(object sender, RoutedEventArgs e)
        //{
        //    Clipboard.SetDataObject(tbNextUrl.Text);
        //}

        //private void btnCopySearchLocalhost_Click(object sender, RoutedEventArgs e)
        //{
        //    Clipboard.SetDataObject(tbLocalhostUrl.Text);
        //}

        private void borderLeftFile_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (cbAllowMultipleFilesCompare.IsChecked == true)
                {
                    tblLeftFilePath.Text = string.Join("\n", files);
                }
                else
                {
                    tblLeftFilePath.Text = files[0];
                }
            }
        }

        private void borderRightFile_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (cbAllowMultipleFilesCompare.IsChecked == true)
                {
                    tblRightFilePath.Text = string.Join("\n", files);
                }
                else
                {
                    tblRightFilePath.Text = files[0];
                }
            }
        }

        private void btnCompare_Click(object sender, RoutedEventArgs e)
        {
            string compareResultContent = null;
            string compareProcessTime = null;

            if (cbAllowMultipleFilesCompare.IsChecked == true)
            {
                List<string> results = new List<string>();

                var leftPaths = tblLeftFilePath.Text.Split('\n').OrderBy(a => a).ToList();
                var rightPaths = tblRightFilePath.Text.Split('\n').OrderBy(a => a).ToList();

                if (leftPaths.Count != rightPaths.Count)
                {
                    MessageBox.Show($"Lengths right and left paths are not the same! LeftPathsCount: {leftPaths.Count} RightPathsCount: {rightPaths.Count}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                List<TimeSpan> compareElapsedTimes = new List<TimeSpan>();

                Stopwatch globalSw = new Stopwatch();
                globalSw.Start();

                for (int i = 0; i < leftPaths.Count; i++)
                {
                    TimeSpan compareElapsed = new TimeSpan();
                    string result = CompareFilesHelper.CompareTwoFiles(leftPaths[i], rightPaths[i], ref compareElapsed);
                    results.Add(result);
                    compareElapsedTimes.Add(compareElapsed);
                }
                globalSw.Stop();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Global: {globalSw.Elapsed}");
                //sb.AppendLine($"{string.Join(", ", compareElapsedTimes.Select(a => $"{a.Milliseconds}ms").ToArray())}");

                compareProcessTime = sb.ToString();

                var groupedResults = results.GroupBy(a => a).ToList();

                if (groupedResults.All(a => a.Key == CompareTextsOk))
                {
                    compareResultContent = CompareTextsOk;
                }
                else
                {
                    List<int> wrongIndexes = new List<int>();
                    for (int i = 0; i < results.Count; i++)
                    {
                        if (results[i] != CompareTextsOk)
                        {
                            wrongIndexes.Add(i);
                        }
                    }

                    compareResultContent = $"Error: wrong indexes: {string.Join(", ", wrongIndexes)}";
                }
            }
            else
            {
                TimeSpan compareElapsed = new TimeSpan();
                compareResultContent = CompareFilesHelper.CompareTwoFiles(tblLeftFilePath.Text, tblRightFilePath.Text, ref compareElapsed);
                compareProcessTime = $"{compareElapsed.Milliseconds}ms";
            }

            tblCompareProcessTime.Text = compareProcessTime;
            MessageBox.Show(compareResultContent);
        }

        private void UpdateFilesToFindTextbox()
        {
            bool topDirectoryOnly = cbOnlyTopDirectory.IsChecked.Value;

            if (StartFindPaths == null || !StartFindPaths.Any())
            {
                MessageBox.Show("No Directory or Files selected!");
                return;
            }

            FindPaths.Clear();

            foreach (var item in StartFindPaths)
            {
                if (File.Exists(item))
                {
                    FindPaths.Add(item);
                }
                else if (Directory.Exists(item))
                {
                    var filesFromDir = Directory.GetFiles(item, "*.cs", topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
                    //var filesFromDir = Directory.GetFiles(item, "*", topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
                    FindPaths.AddRange(filesFromDir);
                }
                else
                {
                }
            }

            int countFiles = FindPaths.Count;

            tbFindFilesCount.Text = $"{countFiles} files";

            if (countFiles < MaxCountOfVisibleFiles)
            {
                tblFindPaths.Text = string.Join(Environment.NewLine, FindPaths);
            }
            else
            {
                tblFindPaths.Text = string.Join(Environment.NewLine, FindPaths.Take(MaxCountOfVisibleFiles));
                tblFindPaths.Text += "\n...";
            }
        }

        private void borderFindPaths_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                StartFindPaths = (string[])e.Data.GetData(DataFormats.FileDrop);

                UpdateFilesToFindTextbox();
            }
        }

        private List<string> GetOccurencesFromFile(string name, string content, string toFind)
        {
            int occurs = 0;
            List<string> res = new List<string>();

            int idx = 0;
            while (true)
            {
                var foundIdx = content.IndexOf(toFind, idx);

                if (foundIdx == -1) break;

                string txtFragment = GetTextFragment(content, foundIdx);
                //res.Add($"{foundIdx} on {System.IO.Path.GetFileName(name)}\t\t{txtFragment}");
                res.Add($"{foundIdx} on {name}\t\t{txtFragment}");
                idx = foundIdx + 1;
                occurs++;
            }

            return res;
        }

        private string GetTextFragment(string inputText, int idx, int range = 10)
        {
            int startIdx = idx - 10;
            if (startIdx < 0) startIdx = 0;

            int endIdx = idx + 10;
            if (endIdx > (inputText.Length - 1)) endIdx = inputText.Length - 1;

            string res = inputText.Substring(startIdx, endIdx - startIdx);
            return res;
        }

        private void FindWordsInFiles()
        {
            if (string.IsNullOrEmpty(tbFind.Text))
            {
                MessageBox.Show("Textbox \"Find Text\" is empty!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string toFind = tbFind.Text;

            int occurs = 0;
            StringBuilder sb = new StringBuilder();
            List<string> filtered = new List<string>();
            List<string> resList = new List<string>();
            int counter = 0;

            if (TaskFind != null && !TaskFind.IsCompleted)
            {
                Cts.Cancel();
            }

            Cts.Dispose();
            Cts = new CancellationTokenSource();

            TaskFind = Task.Run(() =>
            {
                FindTimer.Stop();
                FindTimer.Start();
                SwFind.Restart();

                try
                {
                    Parallel.ForEach(FindPaths, (item, state) =>
                    {
                        string content = File.ReadAllText(item);

                        if (content.Contains(toFind))
                        {
                            var res = GetOccurencesFromFile(item, content, toFind);
                            Interlocked.Add(ref occurs, res.Count);
                            lock (resList)
                            {
                                resList.AddRange(res);
                            }

                            //occurs += res.Count;
                            //resList.AddRange(res);
                            //sb.AppendLine(string.Join(Environment.NewLine, res));
                        }

                        int currentCounter = Interlocked.Increment(ref counter);

                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            tblProcessedFiles.Text = $"{currentCounter}/{FindPaths.Count}";
                        }));

                        if (counter % 500 == 0)
                        {
                            GC.Collect();
                        }

                        try
                        {
                            if (Cts.Token.IsCancellationRequested)
                                Cts.Token.ThrowIfCancellationRequested();
                        }
                        catch (Exception ex)
                        {
                            state.Break();
                        }
                    });
                }
                catch (OperationCanceledException cancelEx)
                {
                }
                catch (Exception ex)
                {
                }

                SwFind.Stop();
                FindTimer.Stop();
                Dispatcher.Invoke(() => tblFindsProcessTime.Text = $"{SwFind.ElapsedMilliseconds}ms");

                try
                {
                    if (Cts.Token.IsCancellationRequested)
                        Cts.Token.ThrowIfCancellationRequested();
                }
                catch (Exception ex)
                {
                    return;
                }

                if (resList.Any())
                {
                    //StringBuilder globalBuilder = new StringBuilder();
                    //globalBuilder.AppendLine($"Looked at {FindPaths.Length} files, Occurences: {occurs}");

                    string header = $"Looked at {FindPaths.Count} files, Occurences: {occurs}\n";
                    string sbContent = string.Join(Environment.NewLine, resList);
                    string fullContent = $"Looked at {FindPaths.Count} files, Occurences: {occurs}\n{sbContent}";

                    File.WriteAllText("LastResults.txt", fullContent);

                    Dispatcher.Invoke(() =>
                    {
                        ResultWindow resultWindow = new ResultWindow(fullContent);
                        resultWindow.Owner = System.Windows.Application.Current.MainWindow;
                        resultWindow.ShowDialog();
                    });

                    //MessageBox.Show($"Looked at {FindPaths.Length} files, Occurences: {occurs}\n" + sb.ToString());
                }
                else
                {
                    MessageBox.Show($"No founds! Looked at {FindPaths.Count} files");
                }
            }, Cts.Token);
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            FindWordsInFiles();
        }

        private void btnConvertToBranchName_Click(object sender, RoutedEventArgs e)
        {
            string from = tbFromBranchName.Text;
            tbToBranchName.Text = ToBranchName(from);
        }

        private string ToBranchName(string text)
        {
            char sign = '_';
            string result = text.Replace(',', sign)
                                .Replace(':', sign)
                                .Replace('.', sign)
                                .Replace(' ', sign);

            result = result.Replace("-", string.Empty);

            return result;
        }

        private void btnCopyToBranchName_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(tbToBranchName.Text);
        }

        private void btnCopyFromBranchName_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(tbFromBranchName.Text);
        }

        private void tbFind_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FindWordsInFiles();
            }
        }

        private void cbOnlyTopDirectory_Checked(object sender, RoutedEventArgs e)
        {
            UpdateFilesToFindTextbox();
        }

        private void cbOnlyTopDirectory_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateFilesToFindTextbox();
        }
    }
}