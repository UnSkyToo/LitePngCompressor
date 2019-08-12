using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace LitePngCompressor
{
    public partial class MainForm : Form
    {
        private string InputDirectoryPath_ = string.Empty;
        private string OutputDirectoryPath_ = string.Empty;
        private bool IsRunning_ = false;

        private long TotalFileSize_ = 0;
        private long CompressedFileSize_ = 0;
        private long TotalFileCount_ = 0;
        private long CompressedFileCount_ = 0;

        public MainForm()
        {
            InitializeComponent();
        }
        
        private void AddItemToListBox(ListBox List, string Item)
        {
            if (List.InvokeRequired)
            {
                Action<string> AddDelegate = (Msg) => { List.Items.Add(Msg); List.TopIndex = List.Items.Count - 1; };
                List.Invoke(AddDelegate, Item);
            }
            else
            {
                List.Items.Add(Item);
                List.TopIndex = List.Items.Count - 1;
            }
        }

        private void AddValueToProgressBar(ProgressBar Bar, int Value)
        {
            if (Bar.InvokeRequired)
            {
                Action<int> AddDelegate = (Num) => { Bar.Value += Num; };
                Bar.Invoke(AddDelegate, Value);
            }
            else
            {
                Bar.Value += Value;
            }
        }

        private void SetValueToProgressBar(ProgressBar Bar, int MinValue, int MaxValue, int Value)
        {
            if (Bar.InvokeRequired)
            {
                Action<int, int, int> AddDelegate = (Min, Max, Num) =>
                {
                    Bar.Minimum = Min;
                    Bar.Maximum = Max;
                    Bar.Value = Value;
                };
                Bar.Invoke(AddDelegate, MinValue, MaxValue, Value);
            }
            else
            {
                Bar.Minimum = MinValue;
                Bar.Maximum = MaxValue;
                Bar.Value = Value;
            }
        }

        private void SetLabel(Label Lab, string Msg)
        {
            if (Lab.InvokeRequired)
            {
                Action<string> AddDelegate = (Txt) => { Lab.Text = Txt; };
                Lab.Invoke(AddDelegate, Msg);
            }
            else
            {
                Lab.Text = Msg;
            }
        }

        private void UpdateInfo()
        {
            SetLabel(LabelInfo, $"Handle Percent : {((double)CompressedFileCount_ / (double)TotalFileCount_ * 100.0d):00.00}% | Compressed Info : {CompressedFileSize_}/{TotalFileSize_} - {((double)CompressedFileSize_ / (double)TotalFileSize_ * 100.0d):00.00}%");
        }

        private void Log(string Msg)
        {
            AddItemToListBox(ListFiles, $"{DateTime.Now.ToString()} - {Msg}");
            Console.WriteLine($"{DateTime.Now.ToString()} - {Msg}");
        }

        private List<CopyFileItemEntity> GenerateCopyItemEntities(string SourceRootPath, string DestRootPath, List<string> FileList)
        {
            var Entites = new List<CopyFileItemEntity>();

            foreach (var FilePath in FileList)
            {
                Entites.Add(new CopyFileItemEntity
                {
                    InputFilePath = FilePath,
                    OutputFilePath = PathHelper.GetFilePathWithRootPath(FilePath, SourceRootPath, DestRootPath)
                });
            }

            return Entites;
        }

        private void StartCopyFileList(string SourceRootPath, string DestRootPath, List<string> FileList, Action Callback)
        {
            Log("Copy File List Begin");

            var Entites = GenerateCopyItemEntities(SourceRootPath, DestRootPath, FileList);
            SetValueToProgressBar(ProgressBarCompress, 0, Entites.Count, 0);
            
            var Job = new CopyFileJobSystem(Entites.ToArray(), Environment.ProcessorCount);
            Job.OnCompleted += () =>
            {
                Log("Copy File List End");
                Callback?.Invoke();
            };
            Job.OnExecuted += (Entity, Code) =>
            {
                Log($"{Entity.InputFilePath} -> {Entity.OutputFilePath}");
                AddValueToProgressBar(ProgressBarCompress, 1);
            };
            Job.Execute();
        }

        private List<CompressItemEntity> GenerateCompressItemEntities(string SourceRootPath, string DestRootPath, List<string> FileList)
        {
            var Entites = new List<CompressItemEntity>();

            foreach (var FilePath in FileList)
            {
                Entites.Add(new CompressItemEntity
                {
                    InputFilePath = FilePath,
                    OutputFilePath = PathHelper.GetFilePathWithRootPath(FilePath, SourceRootPath, DestRootPath)
                });
            }

            return Entites;
        }

        private void StartCompressed(string SourceRootPath, string DestRootPath, List<string> FileList, Action Callback)
        {
            Log("Compressed Begin");
            
            var Entites = GenerateCompressItemEntities(SourceRootPath, DestRootPath, FileList);
            SetValueToProgressBar(ProgressBarCompress, 0, Entites.Count, 0);
            TotalFileCount_ = Entites.Count;

            var Job = new CompressJobSystem(Entites.ToArray(), Environment.ProcessorCount);
            Job.OnCompleted += () =>
            {
                Log("Compressed End");
                Callback?.Invoke();
            };
            Job.OnExecuted += (Entity, Code) =>
            {
                var FileOriginSize = PathHelper.GetFileSize(Entity.InputFilePath);
                TotalFileSize_ += FileOriginSize;

                if (Code)
                {
                    var FileCompressedSize = PathHelper.GetFileSize(Entity.OutputFilePath);
                    CompressedFileCount_++;
                    CompressedFileSize_ += FileCompressedSize;

                    Log($"{Entity.InputFilePath} Compressed : {FileOriginSize / 1024}Kb -> {FileCompressedSize / 1024}Kb!");
                }
                else
                {
                    Log($"{Entity.InputFilePath} Compress Error!");
                }

                AddValueToProgressBar(ProgressBarCompress, 1);
                SetLabel(LabelInfo, $"Handle Percent : {((double)CompressedFileCount_ / (double)TotalFileCount_ * 100.0d):00.00}% | Compressed Info : {CompressedFileSize_}/{TotalFileSize_} - {((double)CompressedFileSize_ / (double)TotalFileSize_ * 100.0d):00.00}%");
            };
            Job.Execute();
        }

        private void StartStatisticsFile(string SourceRootPath, List<string> OutputFileList, Action Callback)
        {
            Log("Statistics File List Begin");

            var Entites = PathHelper.GetFileList(SourceRootPath, (FilePath) => PathHelper.GetFileExt(FilePath) == ".png");
            SetValueToProgressBar(ProgressBarCompress, 0, Entites.Count, 0);

            var Job = new StatisticsFileJobSystem(Entites.ToArray(), Environment.ProcessorCount);
            Job.OnCompleted += () =>
            {
                Log("Statistics File List End");
                Log($"Total File Count : {OutputFileList.Count}");
                Callback?.Invoke();
            };
            Job.OnExecuted += (Entity, Code) =>
            {
                if (Code)
                {
                    OutputFileList.Add(Entity);
                    Log(Entity);
                }

                AddValueToProgressBar(ProgressBarCompress, 1);
            };
            Job.Execute();
        }

        private void Start()
        {
            if (IsRunning_)
            {
                MessageBox.Show("Please Wait Compress Process...", "System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(InputDirectoryPath_))
            {
                MessageBox.Show("Please Select Input Directory Path", "System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(OutputDirectoryPath_))
            {
                MessageBox.Show("Please Select Output Directory Path", "System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            IsRunning_ = true;
            TotalFileCount_ = 1;
            CompressedFileCount_ = 1;
            TotalFileSize_ = 1;
            CompressedFileSize_ = 1;
            SetLabel(LabelInfo, "Waiting...");

            ListFiles.Items.Clear();
            Log("Begin");
            
            var InputFileList = new List<string>();
            StartStatisticsFile(InputDirectoryPath_, InputFileList, () =>
            {
                if (InputFileList.Count == 0)
                {
                    Log("End");
                    IsRunning_ = false;
                    return;
                }
                
                var TempDirectoryPath = $"{OutputDirectoryPath_}temp/";
                Log($"Copy {InputDirectoryPath_} -> {TempDirectoryPath}");

                StartCopyFileList(InputDirectoryPath_, TempDirectoryPath, InputFileList, () =>
                {
                    var TempFileList = new List<string>();
                    foreach (var FilePath in InputFileList)
                    {
                        TempFileList.Add(PathHelper.GetFilePathWithRootPath(FilePath, InputDirectoryPath_, TempDirectoryPath));
                    }

                    StartCompressed(TempDirectoryPath, OutputDirectoryPath_, TempFileList, () =>
                    {
                        PathHelper.DeleteDirectory(TempDirectoryPath);
                        Log("End");
                        SetLabel(LabelInfo, "Done");
                        IsRunning_ = false;
                        ConfigHelper.SaveConfig();
                        MessageBox.Show($"Compressed Succeed\nCompressed File Count: {CompressedFileCount_}/{TotalFileCount_}\nCompressed File Size : {CompressedFileSize_}/{TotalFileSize_} - {((double)CompressedFileSize_ / (double)TotalFileSize_ * 100.0d):00.00}%");
                    });
                });
            });
        }

        private void MainForm_Load(object Sender, EventArgs Args)
        {
            ConfigHelper.LoadConfig();

            InputDirectoryPath_ = ConfigHelper.GetValue("InputDir");
            OutputDirectoryPath_ = ConfigHelper.GetValue("OutputDir");

            LabelInput.Text = $"Input  Directory : {InputDirectoryPath_}";
            LabelOutput.Text = $"Output Directory : {OutputDirectoryPath_}";

            AddItemToListBox(ListFiles, $"Core Num : {Environment.ProcessorCount}");
        }

        private void MainForm_FormClosing(object Sender, FormClosingEventArgs Args)
        {
            if (IsRunning_)
            {
                if (MessageBox.Show("Is Compressing, Quit Now?", "System", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                {
                    Args.Cancel = true;
                }
            }
        }

        private void MainForm_FormClosed(object Sender, FormClosedEventArgs Args)
        {
            ConfigHelper.SaveConfig();
        }

        private void BtnGo_Click(object Sender, EventArgs Args)
        {
            Start();
        }

        private void BtnInput_Click(object Sender, EventArgs Args)
        {
            var Dialog = new FolderBrowserDialog();
            Dialog.SelectedPath = InputDirectoryPath_.Replace("/", "\\");
            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                InputDirectoryPath_ = PathHelper.UnifyPath(Dialog.SelectedPath);
                LabelInput.Text = $"Input  Directory : {InputDirectoryPath_}";
                ConfigHelper.SetValue("InputDir", InputDirectoryPath_);
            }
        }

        private void BtnOutput_Click(object Sender, EventArgs Args)
        {
            var Dialog = new FolderBrowserDialog();
            Dialog.SelectedPath = OutputDirectoryPath_.Replace("/", "\\");
            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                OutputDirectoryPath_ = PathHelper.UnifyPath(Dialog.SelectedPath);
                LabelOutput.Text = $"Output Directory : {OutputDirectoryPath_}";
                ConfigHelper.SetValue("OutputDir", OutputDirectoryPath_);
            }
        }

        private void MainForm_KeyDown(object Sender, KeyEventArgs Args)
        {
            switch (Args.KeyCode)
            {
                case Keys.F1:
                    InputDirectoryPath_ = ConfigHelper.GetValue("InputDir");
                    OutputDirectoryPath_ = ConfigHelper.GetValue("OutputDir");
                    ConfigHelper.Clear();
                    ConfigHelper.SetValue("InputDir", InputDirectoryPath_);
                    ConfigHelper.SetValue("OutputDir", OutputDirectoryPath_);
                    ConfigHelper.SaveConfig();

                    Log("Clear MD5 Cache");
                    break;
                default:
                    break;
            }
        }
    }
}