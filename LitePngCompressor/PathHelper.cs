using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace LitePngCompressor
{
    internal static class PathHelper
    {
        internal static string UnifyPath(string Path)
        {
            Path = Path.Replace("\\", "/");
            // File
            if (Path.Contains("."))
            {
                return Path;
            }
            // Directory
            if (!Path.EndsWith("/"))
            {
                Path += "/";
            }
            return Path;
        }

        internal static string GetFilePath(string Path)
        {
            Path = UnifyPath(Path);
            
            if (Path.Contains("."))
            {
                Path = Path.Substring(0, Path.LastIndexOf("/"));
            }
            
            return UnifyPath(Path);
        }

        internal static string GetFileNameWithExt(string Path)
        {
            return System.IO.Path.GetFileName(Path);
        }

        internal static string GetFileExt(string Path)
        {
            return System.IO.Path.GetExtension(Path);
        }

        internal static string GetFileNameWithoutExt(string Path)
        {
            return System.IO.Path.GetFileNameWithoutExtension(Path);
        }

        internal static long GetFileSize(string Path)
        {
            return new FileInfo(Path).Length;
        }
        
        internal static string GetFilePathWithRootPath(string SourcePath, string SourceRootPath, string TargetRootPath)
        {
            SourcePath = UnifyPath(SourcePath);
            SourceRootPath = UnifyPath(SourceRootPath);
            TargetRootPath = UnifyPath(TargetRootPath);

            if (!SourcePath.Contains(SourceRootPath))
            {
                return string.Empty;
            }

            var SubPath = SourcePath.Substring(SourceRootPath.Length);
            return $"{TargetRootPath}{SubPath}";
        }

        internal static List<string> GetFileList(string DirectoryPath, Func<string, bool> Filter = null)
        {
            var FileList = new List<string>();
            DirectoryPath = UnifyPath(DirectoryPath);

            var DirectoryAttrs = File.GetAttributes(DirectoryPath);
            if (((DirectoryAttrs & FileAttributes.Hidden) != 0) || ((DirectoryAttrs & FileAttributes.System) != 0) || (((DirectoryAttrs & FileAttributes.Directory) == 0)))
            {
                return null;
            }

            var Files = Directory.GetFiles(DirectoryPath);
            var Directories = Directory.GetDirectories(DirectoryPath);

            if (Files != null)
            {
                foreach (var SubFile in Files)
                {
                    var Attrs = File.GetAttributes(SubFile);
                    if (((Attrs & FileAttributes.Hidden) != 0) || ((Attrs & FileAttributes.System) != 0))
                    {
                        continue;
                    }

                    if (Filter != null && !Filter.Invoke(SubFile))
                    {
                        continue;
                    }

                    FileList.Add(UnifyPath(SubFile));
                }
            }

            if (Directories != null)
            {
                foreach (var SubDirectoryPath in Directories)
                {
                    var SubFileList = GetFileList(SubDirectoryPath, Filter);
                    if (SubFileList != null)
                    {
                        FileList.AddRange(SubFileList);
                    }
                }
            }

            return FileList;
        }

        internal static bool CopyDirectory(string SourceDirectoryPath, string DestinationDirectoryPath, Func<string, bool> Filter = null)
        {
            SourceDirectoryPath = UnifyPath(SourceDirectoryPath);
            DestinationDirectoryPath = UnifyPath(DestinationDirectoryPath);

            var DirectoryAttrs = File.GetAttributes(SourceDirectoryPath);
            if (((DirectoryAttrs & FileAttributes.Hidden) != 0) || ((DirectoryAttrs & FileAttributes.System) != 0) || (((DirectoryAttrs & FileAttributes.Directory) == 0)))
            {
                return false;
            }
            
            if (!CreateDirectory(DestinationDirectoryPath))
            {
                return false;
            }

            var IsEmptyPath = true;
            var Files = Directory.GetFiles(SourceDirectoryPath);
            var Directories = Directory.GetDirectories(SourceDirectoryPath);

            if (Files != null)
            {
                foreach (var SubFile in Files)
                {
                    var Attrs = File.GetAttributes(SubFile);
                    if (((Attrs & FileAttributes.Hidden) != 0) || ((Attrs & FileAttributes.System) != 0))
                    {
                        continue;
                    }

                    if (Filter != null && !Filter.Invoke(SubFile))
                    {
                        continue;
                    }

                    File.Copy(SubFile, $"{DestinationDirectoryPath}{GetFileNameWithExt(SubFile)}", true);
                    IsEmptyPath = false;
                }
            }

            if (Directories != null)
            {
                foreach (var SubDirectoryPath in Directories)
                {
                    var DestDirectoryPath = GetFilePathWithRootPath(SubDirectoryPath, SourceDirectoryPath, DestinationDirectoryPath);
                    if (CopyDirectory(SubDirectoryPath, DestDirectoryPath, Filter))
                    {
                        IsEmptyPath = false;
                    }
                }
            }

            if (IsEmptyPath)
            {
                DeleteDirectory(DestinationDirectoryPath);
            }

            return !IsEmptyPath;
        }

        internal static bool CopyFile(string SourceFilePath, string DestinationFilePath)
        {
            var DestPath = GetFilePath(DestinationFilePath);
            if (!CreateDirectory(DestPath))
            {
                return false;
            }

            File.Copy(SourceFilePath, DestinationFilePath, true);
            return true;
        }

        internal static string FileMD5(string FilePath)
        {
            using (var InStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                System.Security.Cryptography.MD5 MD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                var RetVal = MD5.ComputeHash(InStream);
                InStream.Close();

                var Code = new StringBuilder();
                for (var Index = 0; Index < RetVal.Length; ++Index)
                {
                    Code.Append(RetVal[Index].ToString("x2"));
                }

                return Code.ToString();
            }
        }
        
        internal static bool CreateDirectory(string DirectoryPath)
        {
            if (!Directory.Exists(DirectoryPath))
            {
                if (!Directory.CreateDirectory(DirectoryPath).Exists)
                {
                    Console.WriteLine($"Can't create directory : {DirectoryPath}");
                    return false;
                }
            }

            return true;
        }

        internal static void DeleteDirectory(string DirectoryPath)
        {
            try
            {
                Directory.Delete(DirectoryPath, true);
            }
            catch
            {
            }
        }
    }
}