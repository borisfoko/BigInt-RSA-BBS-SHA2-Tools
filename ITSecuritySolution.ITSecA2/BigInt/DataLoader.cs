using System.Collections.Generic;
using System.IO;
using System.Linq;

/**
*
* @author Boris Foko Kouti
*/
namespace BigInt
{
    public static class DataLoader
    {
        public static List<TestData> LoadData(string Filename)
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
            short Size = 0;
            string Comment = null;
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            // Read the file and display it line by line. 
            string line;
            int counter = 0;
            StreamReader file = new StreamReader(LocalFilename);
            while ((line = file.ReadLine()) != null)
            {
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
                    else if (line.StartsWith("s"))
                    {
                        line.Trim();
                        if (line.Contains("="))
                        {
                            LineParts = line.Split('=');
                            Size = short.Parse((LineParts != null && LineParts.Length == 2) ? LineParts[1] : "");
                            LineParts = null;
                            counter++;
                        }
                    }
                    else if (line.StartsWith("c") && line.Contains("pseudo"))
                    {
                        line.Trim();
                        if (line.Contains("="))
                        {
                            LineParts = line.Split('=');
                            Comment = (LineParts != null && LineParts.Length == 2) ? LineParts[1] : null;
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
                                if (Parameters.ContainsKey(LineParts[0]))
                                {
                                    string LinePartName = LineParts[0] + Parameters.Count(p => p.Key.Contains(LineParts[0]));
                                    Parameters.Add(LinePartName, LineParts[1]);
                                }
                                else
                                {
                                    Parameters.Add(LineParts[0], LineParts[1]);
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
                if (Title != null && Size != 0 && Parameters.Count > 0 && counter == -1)
                {
                    if (Comment != null)
                    {
                        TestDataList.Add(new TestData(Title, Size, Comment, new Dictionary<string, string>(Parameters)));
                    }
                    else
                    {
                        TestDataList.Add(new TestData(Title, Size, new Dictionary<string, string>(Parameters)));
                    }
                    
                    Title = null;
                    Size = 0;
                    Comment = null;
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
        public string Name { get; set; }        // Name: the name of the operation
        public short Size { get; set; }         // S: the maximal size of the provider hexadecimal number 
        public string Comment { get; set; }     // C: comment         
        public Dictionary<string, string> Parameters { get; set; } // Operants and operators

        public TestData()
        {
            this.Title = "None";
            this.Name = BigIntOperation.None.AsString(); // By default set to none
            this.Size = 1536;                                 // Set default size to 1536
            this.Parameters = new Dictionary<string, string>();
        }

        public TestData(string Operation, short Size, Dictionary<string, string>  Parameters)
        {
            this.Title = Operation;
            this.Name = BigIntOperationExtensions.GetOperation(Operation); // By default set to none
            this.Size = Size;                                 // Set default size to 1536
            this.Parameters = Parameters;
        }

        public TestData(string Operation, short Size, string Comment, Dictionary<string, string> Parameters)
        {
            this.Title = Operation;
            this.Name = BigIntOperationExtensions.GetOperation(Operation); // By default set to none
            this.Size = Size;                              // Set default size to 1536
            this.Comment = Comment;
            this.Parameters = Parameters;
        }
    }
}
