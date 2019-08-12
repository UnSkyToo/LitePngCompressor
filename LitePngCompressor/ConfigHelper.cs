using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LitePngCompressor
{
    internal static class ConfigHelper
    {
        private static Dictionary<string, string> Values_ = new Dictionary<string, string>();

        internal static bool LoadConfig()
        {
            var FilePath = $"{PathHelper.UnifyPath(AppDomain.CurrentDomain.BaseDirectory)}cfg.dat";

            try
            {
                using (var InStream = new StreamReader(FilePath, Encoding.ASCII))
                {
                    while (!InStream.EndOfStream)
                    {
                        var Line = InStream.ReadLine();
                        var Params = Line.Split('&');
                        if (Params.Length == 2 && !Values_.ContainsKey(Params[0]))
                        {
                            Values_.Add(Params[0], Params[1]);
                        }
                        else
                        {
                            Console.WriteLine($"Error Config : {Line}");
                        }
                    }

                    InStream.Close();
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return false;
            }

            return true;
        }

        internal static void SaveConfig()
        {
            var FilePath = $"{PathHelper.UnifyPath(AppDomain.CurrentDomain.BaseDirectory)}cfg.dat";

            try
            {
                using (var OutStream = new StreamWriter(FilePath, false, Encoding.ASCII))
                {
                    foreach (var Line in Values_)
                    {
                        OutStream.WriteLine($"{Line.Key}&{Line.Value}");
                    }

                    OutStream.Close();
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
        }

        internal static void Clear()
        {
            Values_.Clear();
        }

        internal static string GetValue(string Key, string Default = "")
        {
            if (Values_.ContainsKey(Key))
            {
                return Values_[Key];
            }

            return Default;
        }

        internal static void SetValue(string Key, string Value)
        {
            if (Values_.ContainsKey(Key))
            {
                Values_[Key] = Value;
            }
            else
            {
                Values_.Add(Key, Value);
            }
        }
    }
}