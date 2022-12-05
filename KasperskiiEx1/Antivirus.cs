using System.IO;
using System.Collections.Generic;
using System.Text;
using System;
using System.Diagnostics;

namespace KasperskiiEx1
{
    static class Antivirus
    {
        static int jsDetects;
        static int rmDetects;
        static int Rundll32;
        static int countErorrs;
        static int processedFiles;
        static double time;

        public static void Start(string [] args)
        {
            var findTime = new Stopwatch();
            findTime.Start();
            jsDetects = 0;
            rmDetects = 0;
            Rundll32 = 0;
            countErorrs = 0;
            
             
            var optionsForDirectory = new EnumerationOptions();

            string path = args.Length > 0 ? args[0] : @"C:\Users\user\Desktop\Test";
            var files = Directory.GetFiles(path);
            processedFiles = files.Length;
            foreach (string file in files)
            {
                bool isDetected = false;
                try
                {
                    using (var reader = new StreamReader(file))
                    {
                        while (!reader.EndOfStream || isDetected)
                        {
                            string str = reader.ReadLine();
                            
                            if (file.Substring(file.LastIndexOf('.')) == ".js" && str.Equals("<script>evil_script()</script>"))
                            {
                                jsDetects++;
                                isDetected = false;

                            }
                            else if (str.Equals("rm -rf %userprofile%\\Documents"))
                            {
                                rmDetects++;
                                isDetected = false;
                            }
                            else if (str.Equals("Rundll32 sus.dll SusEntry"))
                            {
                                Rundll32++;
                                isDetected = false;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    countErorrs++;
                    
                }
                
            }
            findTime.Stop();
            time = findTime.Elapsed.Seconds;
            GetInfo();
        }

        private static void GetInfo()
        {
            Console.WriteLine($@"===== Scan result =====
Processed files: {processedFiles}
JS detects: {jsDetects}
rm -rf detects: {rmDetects}
Rundll32 detects: {Rundll32}
Errors: {countErorrs}
Exection time: {time:00:00:00}
=======================");
        }
    }
}
