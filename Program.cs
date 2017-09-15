using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using HtmlAgilityPack;


namespace WordScraper
{

    class Program
    {
        

        
        static void Main(string[] args)
        {
            {
                //Set the keywords we want to scrape from our website in a list
                List<string> list = new List<string>();
                list.Add("corpse");
                list.Add("kidnapping");
                list.Add("ransom");
                list.Add("murder");
                list.Add("torture");
                list.Add("corruption");
                list.Add("KFR");
                list.Add("assualt");
                list.Add("fire");
                list.Add("disaster");
                list.Add("PNP");
                list.Add("hurt");
                list.Add("carnapping");

                //Set our list of websites to crawl
                List<string> sites = new List<string>();
                sites.Add("http://www.inquirer.net//");
                sites.Add("http://www.manilatimes.net//news//");
                sites.Add("http://mb.com.ph//");
                sites.Add("http://www.malaya.com.ph/news");
                sites.Add("http://www.daily-tribune.com//");
                sites.Add("http://www.sunstar.com.ph//");
                sites.Add("http://manilastandard.net//");
                sites.Add("http://mindanaoexaminer.com//");
                sites.Add("http://www.journal.com.ph//");
                sites.Add("http://www.sunstar.com.ph//cebu//");
                sites.Add("https://thedailyguardian.net//");
                sites.Add("http://www.thenewstoday.info//");


                //foreach loop needed for html websites to run the keywords that nests the foreach loop for the keywords

                foreach (string s in sites)
                {
                    
                    var client = new WebClient();

                    //TODO make a way to input the website in case user has a custom site they want to add.
                                        
                    var url = s;
                    var pageContent = client.DownloadString(url);

                    foreach (string l in list)

                    {
                        var keywords = l;
                        var keywordLocation = pageContent.IndexOf(keywords, StringComparison.OrdinalIgnoreCase);

                        if (keywordLocation >= 0)
                        {
                            Console.WriteLine(url + " is talking about " + keywords + " today.");
                            Console.WriteLine("\nSnippet:" + pageContent.Substring(keywordLocation, 100));
                            string Results = pageContent.Substring(keywordLocation);

                            //Will clean up the HTML before it is stored in the output file

                            Results = Regex.Replace(Results, @"<[^>]+>|&nbsp;", "").Trim();
                            Results = Regex.Replace(Results, @"\s{2,}", " ");
                           
                            //We will now write the results of the file Results to a .txt file named Results.txt
                            System.IO.File.WriteAllText(@"C:\\Users\\wwstudent\\Desktop\\Results.txt", Results);
                            {

                                // Read the file line-by-line, and store it all in a list named resultslist.
                                
                                List<string> resultslist = new List<string>();
                                using (StreamReader reader = new StreamReader(@"C:\\Users\\wwstudent\\Desktop\\Results.txt"))
                                {
                                    string line;
                                    while ((line = reader.ReadLine()) != null)
                                    {

                                        resultslist.Add(line); // Add to list.
                                        // Console.WriteLine(line); // Write to console.
                                    }
                                }
                            }
                                                    }
                                                
                        /*    If you want confirmation that your keyword does not appear on the specific HTTP site, uncoment this code.
                        else
                        {
                               Console.WriteLine("{0} not found not found at {1}", (string)keywords, (string)url);
                        }*/


                    }

                    //TODO save the outcome the database
                    //TODO Figure out data redundancy and isolate it from the database
                }
                Console.ReadLine();
            }


        }
            
        
    }
    
}
