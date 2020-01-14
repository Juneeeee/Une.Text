using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace Une.Text.Combiner
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonAddFiles_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Filter = "文本文件|*.txt",
                Multiselect = true
            };
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var fileName in openFileDialog.FileNames)
                {
                    ListFiles.Items.Add(fileName);
                }
            }
        }

        private void ButtonRemoveFile_Click(object sender, RoutedEventArgs e)
        {
            if (ListFiles.SelectedIndex > -1)
            {
                ListFiles.Items.RemoveAt(ListFiles.SelectedIndex);
            }
        }

        private void ButtonClearFiles_Click(object sender, RoutedEventArgs e)
        {
            ListFiles.Items.Clear();
        }

        private void ButtonUpFileIndex_Click(object sender, RoutedEventArgs e)
        {
            if (ListFiles.SelectedIndex > 0)
            {
                var idx = ListFiles.SelectedIndex;
                var item = ListFiles.SelectedItem;
                ListFiles.Items.Insert(ListFiles.SelectedIndex - 1, item);
                ListFiles.Items.RemoveAt(idx + 1);
            }
        }

        private void ButtonDownFileIndex_Click(object sender, RoutedEventArgs e)
        {
            if (ListFiles.SelectedIndex < ListFiles.Items.Count - 1)
            {
                var idx = ListFiles.SelectedIndex;
                var item = ListFiles.SelectedItem;
                ListFiles.Items.Insert(ListFiles.SelectedIndex + 2, item);
                ListFiles.Items.RemoveAt(idx);
                ListFiles.SelectedIndex = idx + 1;
            }
        }

        private void ButtonSelectNewFilePath_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Filter = "文本文件|*.txt",
                CheckFileExists = false
            };
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TxtNewFilePath.Text = openFileDialog.FileName;
            }
        }

        private void ButtonCombineFiles_Click(object sender, RoutedEventArgs e)
        {
          
            if (ListFiles.Items.Count < 0)
            {
                System.Windows.Forms.MessageBox.Show("当前没有可合并的文本文件，请添加文件后重试。");
                return;
            }
            var newFilePath = TxtNewFilePath.Text;
            if (string.IsNullOrEmpty(newFilePath))
            {
                System.Windows.Forms.MessageBox.Show("请选择新文件路径后重试。");
                return;
            }
            ProgressCombine.Maximum = ListFiles.Items.Count;
            
            using (var writer = new StreamWriter(newFilePath, true))
            { 
                foreach (var item in ListFiles.Items)
                {
                    var filePath = item.ToString();
                    using (var reader = new StreamReader(filePath))
                    {
                        var line = reader.ReadLine();
                        writer.WriteLine(line);
                    }
                    ListProcess.Items.Add($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}]:{filePath} >> {newFilePath}");
                    ProgressCombine.Value += 1;
                    LabPercentage.Content = $"进度状态：{(int)(ProgressCombine.Value / ProgressCombine.Maximum * 100)}%";
                }
                ListProcess.Items.Add($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}]:处理完成"); 
            }


        }

        private void Label_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start("https://baddy.cn");
        }
    }
}
