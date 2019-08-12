using System;
using System.IO;
using System.Diagnostics;

namespace LitePngCompressor
{
    internal static class PngQuant
    {
        internal static string PngQuantExeFilePath = $"{PathHelper.UnifyPath(AppDomain.CurrentDomain.BaseDirectory)}pngquant/pngquant.exe";

        internal static bool Compress(string InputFilePath, string OutputFilePath, int Timeout = 20 * 1000)
        {
            if (!File.Exists(InputFilePath))
            {
                Console.WriteLine($"Can't find file : {InputFilePath}");
                return false;
            }

            var OutputDirectoryPath = PathHelper.GetFilePath(OutputFilePath);
            if (!PathHelper.CreateDirectory(OutputDirectoryPath))
            {
                return false;
            }
            
            var InputTempFilePath = $"{PathHelper.GetFilePath(InputFilePath)}{PathHelper.GetFileNameWithoutExt(InputFilePath)}_l_i_t_e.png";

            var CompressProcess = new Process();
            CompressProcess.StartInfo = new ProcessStartInfo
            {
                FileName = PngQuantExeFilePath,
                //Arguments = $"--force --verbose --ext _l_i_t_e.png --speed 3 {InputFilePath}",
                Arguments = $"--force --ext _l_i_t_e.png --speed 3 {InputFilePath}",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            CompressProcess.OutputDataReceived += (Sender, Args) => Console.WriteLine(Args.Data);
            CompressProcess.ErrorDataReceived += (Sender, Args) => Console.WriteLine(Args.Data);
            CompressProcess.EnableRaisingEvents = true;
            CompressProcess.Start();
            CompressProcess.BeginOutputReadLine();
            CompressProcess.BeginErrorReadLine();
            
            if (!CompressProcess.WaitForExit(Timeout))
            {
                return false;
            }

            if (!File.Exists(InputTempFilePath))
            {
                return false;
            }

            if (File.Exists(OutputFilePath))
            {
                File.Delete(OutputFilePath);
            }

            File.Move(InputTempFilePath, OutputFilePath);

            return true;
        }
    }
}