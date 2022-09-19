using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StringCounterAndOccurenceLineChecker
{
    class Program
    {
        public static string workingDirectory;
        public static string projectDirectory;
        public static string rootInput=@"/input/";
        public static string rootOutput=@"/output/";
        public static string projectDirectoryRootInput;
        public static string projectDirectoryRootOutput;


        static void Main(string[] args)
        {
            // This will get the current WORKING directory (i.e. \bin\Debug)
            workingDirectory = Environment.CurrentDirectory;

            //// or: Directory.GetCurrentDirectory() gives the same result

            //// This will get the current PROJECT bin directory (ie ../bin/)
            projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

            projectDirectoryRootInput = projectDirectory + rootInput;

            projectDirectoryRootOutput = projectDirectory + rootOutput;

            string fileName1 = projectDirectoryRootInput + "Article.txt";
            string fileName2 = projectDirectoryRootInput+"Words.txt";
            string fileName3 = projectDirectoryRootOutput + "Output.txt";
        
            string text = File.ReadAllText(fileName1);
            //Console.WriteLine(text);

        
            string[] source = text.Split(new char[] { '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);

            string replacedText = text.Replace("Mr.", "Mr*").Replace("Mrs.","Mrs*").Replace("e.g.","e*g*");


            FileStream fileOutput = new FileStream(fileName3, FileMode.OpenOrCreate);
            Console.WriteLine(fileName3);
            using (StreamWriter fileWrite = new StreamWriter(fileOutput))
            {
                ///Read file using StreamReader. Reads file line by line    
                using (StreamReader file = new StreamReader(fileName2))
                {
                    int counter = 0;
                    string ln;
                    string character = "abcdefghijklmnopqrstuvwxyz";
                    char[] ch = character.ToCharArray();
                    int length = character.Length;
                    int multiplier = 1;
                    int i = 0;
                    string parent = string.Empty;
                    while ((ln = file.ReadLine()) != null)
                    {
                        int breaker = length * multiplier;

                        if (i == breaker)
                            multiplier++;

                        if (i >= length)
                        {
                            var values = (length - (breaker - i));
                            if (values == length)
                                values = 0;

                            string child = string.Empty;
                            for (int j = 0; j < multiplier; j++)
                            {
                                //child = ch[values].ToString();
                                child = child + ch[values].ToString();
                            }

                            parent = child + ". ";
                        }
                        else
                        {
                            //Console.WriteLine("false " + i);
                            //Console.WriteLine(ch[i]);
                            parent = ch[i].ToString() + ". ";
                        }

                        var matchQuery = from word in source
                                         where word.Equals(ln, StringComparison.InvariantCultureIgnoreCase)
                                         select word;
                        int wordCount = matchQuery.Count();

                        int lineCount = 0;
                        List<string> lineNumber = new List<string>();
                        foreach (var sentence in replacedText.TrimEnd('.').Split(new[] { ". " }, StringSplitOptions.None))
                        {
                            lineCount++;
                            var lineSentence = sentence.ToLower().Trim().Replace("*", ".").ToString();
                            var lineCounterChecker = lineSentence.Split(new char[] { '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
                            var matchFound = from word in lineCounterChecker
                                             where word.Equals(ln)
                                             select word;

                            int lineWordCount = matchFound.Count();

                            if (lineWordCount > 0)
                            {
                                lineNumber.Add(lineCount.ToString());
                                if (lineWordCount > 1)
                                {
                                    lineNumber.Add(lineWordCount.ToString());
                                }


                            }

                        }

                        string keyValue = parent + ln + " {" + wordCount + ":" + String.Join(",", lineNumber) + "}";
                        Console.WriteLine(keyValue);
                      
                        counter++;
                        i++;

                        fileWrite.WriteLine(keyValue);


                    }
                    file.Close();
                    fileWrite.Close();
                    fileOutput.Close();
                    Console.WriteLine($"Successfully added record in Output.txt with {counter} lines");
                }
            }

           


            Console.ReadLine();

        }
       
    }
}
