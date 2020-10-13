using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SHA2.Utils
{
    public static class DataLoader
    {
        public static List<TestData> LoadData(string Filename, bool CheckComment = false)
        {
            List<TestData> TestDataList = new List<TestData>();
            string LocalFilename = Filename;

            if (!File.Exists(LocalFilename))
            {
                LocalFilename = @$"{Directory.GetCurrentDirectory()}\{Filename}";
                if (!File.Exists(LocalFilename))
                {
                    LocalFilename = Path.Combine("" + Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent, "ressources", Filename);
                    if (!File.Exists(LocalFilename))
                    {
                        throw new FileNotFoundException($"File not found exception. Please check your path {LocalFilename} of the provided file.");
                    }
                }
            }

            // Test data
            string[] LineParts = null;
            string Title = null;
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            // Read the file and display it line by line. 
            string line;
            int counter = 0;
            StreamReader file = new StreamReader(LocalFilename);
            while ((line = file.ReadLine()) != null)
            {
                if ((line.StartsWith("#****") || line.StartsWith("# begin") || line.StartsWith("# sha256")) && CheckComment)
                    continue;
                if (!line.StartsWith("#"))
                {
                    if (line.StartsWith("t"))
                    {
                        line.Trim();
                        if (line.Contains("="))
                        {
                            LineParts = line.Split('=');
                            Title = (LineParts != null && LineParts.Length == 2) ? LineParts[1] : "";
                            LineParts = null;
                            counter++;
                        }
                    }
                    else
                    {
                        line.Trim();
                        if (line.Contains("="))
                        {
                            LineParts = line.Split('=');
                            if (LineParts.Length == 2)
                            {
                                if (Parameters.ContainsKey(LineParts[0].Trim()))
                                {
                                    string LinePartName = LineParts[0].Trim() + Parameters.Count(p => p.Key.Contains(LineParts[0].Trim()));
                                    Parameters.Add(LinePartName, LineParts[1]);
                                }
                                else
                                {
                                    Parameters.Add(LineParts[0].Trim(), LineParts[1]);
                                }
                            }
                            LineParts = null;
                            counter++;
                        }
                    }
                }
                else if (counter > 0)
                {
                    counter = -1;
                }
                if (Title != null && Parameters.Count > 0 && counter == -1)
                {
                    TestDataList.Add(new TestData(Title, new Dictionary<string, string>(Parameters)));
                    Title = null;
                    Parameters.Clear();
                    counter = 0;
                }

            }

            file.Close();

            return TestDataList;
        }
    }

    public class TestData
    {
        public string Title { get; set; }       // Title: title of the current operation
        public Dictionary<string, string> Parameters { get; set; } // Operants and operators

        public TestData()
        {
            this.Title = "None";
            this.Parameters = new Dictionary<string, string>();
        }

        public TestData(string Operation, Dictionary<string, string> Parameters)
        {
            this.Title = Operation;
            this.Parameters = Parameters;
        }
    }
}
