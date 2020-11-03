using System;
using System.Collections.Generic;
using System.IO;

namespace GradeSplitter
{
    class Program
    {
        static void Main(string[] args)
        {
            List<String> ecList = new List<String>(); // (-3)-0
            List<String> elementaryList = new List<String>();// 1-5
            List<String> middleList = new List<String>();// 6-8
            List<String> highList = new List<String>();// 9-12

            try
            {
                using (StreamReader sr = new StreamReader("C:/Users/Enzoc/Desktop/test/documenti/test.csv")) {
                    string line;
                    while ((line = sr.ReadLine()) != null){

                        String[] tokens = line.Split("\t");

                        if ( Int32.Parse(tokens[0]) >= 9 && Int32.Parse(tokens[0]) <= 12)
                        {
                            tokens[1] = tokens[1].Replace(";", ",");
                            tokens[1] = tokens[1].Replace(" ", "");

                            if (tokens[1].Contains(","))
                            {
                                
                                String[] moreEmail = tokens[1].Split(",");

                                if (!highList.Contains(moreEmail[0]) && moreEmail[0].Length > 1)
                                {
                                    highList.Add(moreEmail[0]);
                                }
                                if (!highList.Contains(moreEmail[1]) && moreEmail[1].Length > 1)
                                {
                                    highList.Add(moreEmail[1]);
                                }
                            }else
                            {
                                if (!highList.Contains(tokens[1]) && tokens[1].Length > 1)
                                {
                                    highList.Add(tokens[1]);
                                }
                            }

                        }else if ( Int32.Parse(tokens[0]) >= 6 && Int32.Parse(tokens[0]) <= 8)
                        {
                            tokens[1] = tokens[1].Replace(";", ",");
                            tokens[1] = tokens[1].Replace(" ", "");

                            if (tokens[1].Contains(","))
                            {
                                
                                String[] moreEmail = tokens[1].Split(",");

                                if (!middleList.Contains(moreEmail[0]) && moreEmail[0].Length > 1)
                                {
                                    middleList.Add(moreEmail[0]);
                                }
                                if (!middleList.Contains(moreEmail[1]) && moreEmail[1].Length > 1)
                                {
                                    middleList.Add(moreEmail[1]);
                                }
                            }
                            else
                            {
                                if (!middleList.Contains(tokens[1]) && tokens[1].Length > 1)
                                {
                                    middleList.Add(tokens[1]);
                                }
                            }
                        }
                        else if ( Int32.Parse(tokens[0]) >= 0 && Int32.Parse(tokens[0]) <= 5)
                        {
                            tokens[1] = tokens[1].Replace(";", ",");
                            tokens[1] = tokens[1].Replace(" ", "");

                            if (tokens[1].Contains(","))
                            {
                                
                                String[] moreEmail = tokens[1].Split(",");

                                if (!elementaryList.Contains(moreEmail[0]) && moreEmail[0].Length > 1)
                                {
                                    elementaryList.Add(moreEmail[0]);
                                }
                                if (!elementaryList.Contains(moreEmail[1]) && moreEmail[1].Length > 1)
                                {
                                    elementaryList.Add(moreEmail[1]);
                                }
                            }
                            else
                            {
                                if (!elementaryList.Contains(tokens[1]) && tokens[1].Length > 1)
                                {
                                    elementaryList.Add(tokens[1]);
                                }
                            }

                        }
                        else
                        {
                            tokens[1] = tokens[1].Replace(";", ",");
                            tokens[1] = tokens[1].Replace(" ", "");

                            if (tokens[1].Contains(","))
                            {

                                String[] moreEmail = tokens[1].Split(",");

                                if (!ecList.Contains(moreEmail[0]) && moreEmail[0].Length > 1)
                                {
                                    ecList.Add(moreEmail[0]);
                                }
                                if (!ecList.Contains(moreEmail[1]) && moreEmail[1].Length > 1)
                                {
                                    ecList.Add(moreEmail[1]);
                                }
                            }
                            else
                            {
                                if (!ecList.Contains(tokens[1]) && tokens[1].Length > 1)
                                {
                                    ecList.Add(tokens[1]);
                                }
                            }
                        }
                    }
                }               
            }catch(Exception e){
                throw new Exception("Errore file lettura");
            }

            try
            {
                if (File.Exists("C:/Users/Enzoc/Desktop/test/documenti/convertitoHigh.csv")) File.Delete(("C:/Users/Enzoc/Desktop/test/documenti/convertitoHigh.csv"));
                using (StreamWriter sw = new StreamWriter(File.Create("C:/Users/Enzoc/Desktop/test/documenti/convertitoHigh.csv")))
                {
                    sw.WriteLine("email");
                    foreach (string x in highList)
                    {
                        sw.WriteLine(x);
                    }

                }

                if (File.Exists("C:/Users/Enzoc/Desktop/test/documenti/convertitoMiddle.csv")) File.Delete(("C:/Users/Enzoc/Desktop/test/documenti/convertitoMiddle.csv"));
                using (StreamWriter sw = new StreamWriter(File.Create("C:/Users/Enzoc/Desktop/test/documenti/convertitomiddle.csv")))
                {
                    sw.WriteLine("email");
                    foreach (string x in middleList)
                    {
                        sw.WriteLine(x);
                    }

                }

                if (File.Exists("C:/Users/Enzoc/Desktop/test/documenti/convertitoElementary.csv")) File.Delete(("C:/Users/Enzoc/Desktop/test/documenti/convertitoElementary.csv"));
                using (StreamWriter sw = new StreamWriter(File.Create("C:/Users/Enzoc/Desktop/test/documenti/convertitoelEmentary.csv")))
                {
                    sw.WriteLine("email");
                    foreach (string x in elementaryList)
                    {
                        sw.WriteLine(x);
                    }

                }

                if (File.Exists("C:/Users/Enzoc/Desktop/test/documenti/convertitoEc.csv")) File.Delete(("C:/Users/Enzoc/Desktop/test/documenti/convertitoEc.csv"));
                using (StreamWriter sw = new StreamWriter(File.Create("C:/Users/Enzoc/Desktop/test/documenti/convertitoEc.csv")))
                {
                    sw.WriteLine("email");
                    foreach (string x in ecList)
                    {
                        sw.WriteLine(x);
                    }

                }
            }
            catch (Exception e){
                throw new Exception("Errore file scrittura");
            }

        }
    }
}
