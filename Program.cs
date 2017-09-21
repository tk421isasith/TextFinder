using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using System.Data.SqlClient;


namespace WordScraper
{

    class Program
    {



        static void Main(string[] args)
        {

            //
            // Set a list of Locations we will later use in our database Location column
            //


            List<string> Location = new List<string>()
                {
                    "Luzon",
                    "Ilocos,",
                    "Cagayan Valley",
                    "Central Luzon",
                    "Calabarzon",
                    "Mimaropa",
                    "Bicol",
                    "Cordillera Administrative Region (CAR)",
                    "National Capital Region",
                    "Visayas",
                    "Westen Visayas",
                    "Central Visayas",
                    "Eastern Visayas",
                    "Mindanao",
                    "Zamboanga Penisula",
                    "Northern Mindanao",
                    "Davao Region",
                    "Soccsksargen",
                    "Caraga",
                    "Autonomous Region of Muslim Mindanao",
                    "ARRM",
                    "SULU",
                };
                //
                //Set the keywords we want to scrape from our website in a list called list
                //

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
                list.Add("MILF");
                list.Add("ISIS");
                list.Add("BIFF");
                list.Add("pirate");
                list.Add("bandits");
                

                //
                //Set a list of terrorist organizations in the Philippines
                //

                List<string> Terrorist = new List<string>()
                {
                    "MILF",
                    "Moro Islamic Liberation Front",
                    "BIFF",
                    "Bangsamoro Islamic Freedom Fighters",
                    "JI",
                    "Jemaah Islamiyah",
                    "ASG",
                    "Abu Sayyaf Group",
                    "NPA",
                    "New People’s Army",
                    "ISIS",
                    "IS",
                    "ISIL",
                    "Daesh",
                };



                //
                //Set our list of websites to crawl
                //

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

                //
                //
                //

                foreach (string s in sites)
                {

                    var client = new WebClient();
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

                        //
                        //This will clean up the HTML before it is stored in the output file Results.txt and add a break to make the .txt readable
                        //

                        Results = Regex.Replace(Results, @"<[^>]+>|&nbsp;", "").Trim();
                        Results = Regex.Replace(Results, @"\s{2,}", " ");

                        System.IO.File.WriteAllText(@"C:\Users\wwstudent\source\repos\ConsoleApp8\ConsoleApp8\Results.txt", Results + "\r\n");


                        {   ///
                            ///Test to see that the location of the crime is stated in the Results of our crawl and place it in a variable crimeloctaion
                            ///
                            var Crimelocation = Location.Any(x => Results.Contains(x));

                            ///
                            ///Add the local time of the crawl to the results.  This does not have to be specific per customer request so we will use the date of the pull.
                            ///
                            DateTime CurrentTime = DateTime.Now;
                            TimeZoneInfo.ConvertTimeBySystemTimeZoneId(CurrentTime, TimeZoneInfo.Local.Id);

                            ///
                            ///We will now try to pull organization names that committed crimes from our keywords
                            ///

                            var Organizations = Terrorist.Any(x => Results.Contains(x));


                            /// Results.(Results + "\r\n");
                            ///We will now write the results of the file Results to a .txt file named Results.txt
                            ///TODO clean up the results so there is a website url and page break added to make the .txt file more readable.
                            ///TODO scrub the Results.txt and add the keywords to the database.
                            ///TODO add the keywords string to the database
                            ///TODO add the date variable to the database
                            ///TODO save the outcome the database
                            ///

                            List<string> resultslist = new List<string>();
                            using (StreamReader reader = new StreamReader(@"C:\Users\wwstudent\source\repos\ConsoleApp8\ConsoleApp8\Results.txt"))

                            {
                                string line;
                                StreamReader file = new StreamReader(@"C:\Users\wwstudent\source\repos\ConsoleApp8\ConsoleApp8\Results.txt");
                                if ((line = file.ReadLine()) != null)
                                {
                                    string[] fields = line.Split(',');
                                    using (SqlConnection con = new SqlConnection(@"C:\Users\wwstudent\source\repos\ConsoleApp8\ConsoleApp8\ResultsDatabase.mdf=NT;Initial Catalog=ResultsDatabase;Integrated Security=True"))
                                    {
                                        
                                        con.Open();
                                        while ((line = file.ReadLine()) != null)
                                        {
                                            
                                            SqlCommand cmd = new SqlCommand("INSERT INTO C:\\Users\\wwstudent\\source\\repos\\ConsoleApp8\\ConsoleApp8\\ResultsDatabase1.md (Crimelocation, Currenttime, Location, Organization) VALUES (@Crime, @Date, @Location, @Organization)", con);
                                            cmd.Parameters.AddWithValue("@Crime", fields[0].ToString());
                                            cmd.Parameters.AddWithValue("@Date", fields[1].ToString());
                                            cmd.Parameters.AddWithValue("@Location", fields[2].ToString());
                                            cmd.Parameters.AddWithValue("@Organization", fields[3].ToString());
                                            cmd.ExecuteNonQuery();
                                        }
                                    }

                                }
                                /*    
                                *    
                                *    Uncomment if you want another .txt file with more information from the websites
                                *    Make sure your file 
                                *    
                                *    //
                                *    // Read the file line-by-line, and store it all in a list named resultslist that we can later call to the database.
                                *    //
                                */

                                /*  while ((line = reader.ReadLine()) != null)
                                    {
                                        resultslist.Add(line);
                                       
                                    //fix this so it writes the file
                                    using (StreamWriter Resultslist = new StreamWriter(@"C:\\USers\wwstudent\\Desktop\\Resultslist.txt", true))
                                    {
                                        Resultslist.Write(Results);
                                    }
                                        // Add to list.
                                        //Console.WriteLine(line); // Write to console.
                                    }*/
                            }
                        
                            
                        }
                    }

                        /*    If you want confirmation that your keyword does not appear on the specific HTTP site, uncoment this code.
                        else
                        {
                               Console.WriteLine("{0} not found not found at {1}", (string)keywords, (string)url);
                        }*/


                    }


            }
               
            
                
            //
            //Create a list of string that will later be used to create a function that will assign region names from the string to a variable named Location which will be addded to the database
            //


                      

        }

    }


}
    
