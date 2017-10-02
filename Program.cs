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
using System.Configuration;


namespace WordScraper
{
    public class Methods
    {
        public class Location
        {
            //
            // Set a list of Locations we will later use in our database Location column
            //            
            public List<string> Locations = new List<string>()
            {
              "Luzon","Ilocos,","Cagayan Valley","Central Luzon","Calabarzon","Mimaropa","Bicol","Cordillera Administrative Region (CAR)",
              "National Capital Region","Visayas","Westen Visayas","Central Visayas","Eastern Visayas","Mindanao","Zamboanga Penisula",
              "Northern Mindanao","Davao Region","Soccsksargen","Caraga","Autonomous Region of Muslim Mindanao","ARRM","SULU",
            };

            internal void ForEach(Func<object, StringBuilder> p)
            {
                throw new NotImplementedException();
            }
        }
       
        public class Crimes
        {
            //
            //Set the keywords we want to scrape from our website in a list called list
            //
            public List<string> list = new List<string>()
            {
                "corpse","kidnapping","ransom", "murder", "torture", "corruption", "KFR","assualt","fire","disaster",
                "PNP","hurt", "carnapping", "MILF", "ISIS", "BIFF", "pirate","bandits",
            };
        }
        public class Organization
        {
            //
            //Set a list of terrorist organizations in the Philippines
            //
            public List<string> Terrorist = new List<string>()
            {
                  "MILF","Moro Islamic Liberation Front","BIFF","Bangsamoro Islamic Freedom Fighters","JI", "Jemaah Islamiyah",
                  "ASG", "Abu Sayyaf Group","NPA", "New People’s Army","ISIS","IS","ISIL","Daesh",
            };
        }
        public class Keyword
        {
            //
            //Set the keywords we want to scrape from our website in a list called list
            //
            public List<string> list = new List<string>()
            {
             "corpse","kidnapping","ransom","murder","torture","corruption","KFR", "assualt", "fire","disaster","PNP", "hurt",
             "carnapping","MILF","ISIS", "BIFF", "pirate","bandits", "terrorists", "Terror",
            };
        }

        public class Website
        {
            //
            //Set our list of websites to crawl
            //

            public List<string> sites = new List<string>()
            {
                "http://www.inquirer.net//",
                "http://www.manilatimes.net//news//",
                "http://mb.com.ph//",
                "http://www.malaya.com.ph/news)",
                "http://www.daily-tribune.com//)",
                "http://www.sunstar.com.ph//",
                "http://manilastandard.net//)",
                "http://mindanaoexaminer.com//)",
                "http://www.journal.com.ph//)",
                "http://www.sunstar.com.ph//cebu//)",
                "https://thedailyguardian.net//)",
                "http://www.thenewstoday.info//)",
            };

        }


        class Program
        {
            static void Main(string[] args)
            {
                
                Methods method = new Methods();
                Website web = new Website();
                Keyword word = new Keyword();
                Organization organization = new Organization();
                Crimes crime = new Crimes();
                Location location = new Location();

                foreach (string s in web.sites)
                {
                    var client = new WebClient();
                    var url = s;
                    var pageContent = client.DownloadString(url);
                    foreach (string l in word.list)

                    {
                        var keywords = l;
                        var keywordLocation = pageContent.IndexOf(keywords, StringComparison.OrdinalIgnoreCase);

                        if (keywordLocation >= 0)
                        {
                            Console.WriteLine(url + " is talking about " + keywords + " today.");
                            Console.WriteLine("\nSnippet:" + pageContent.Substring(keywordLocation, 100));
                            string Results = pageContent.Substring(keywordLocation);
                            string resultswithHTML = Results;

                            //
                            //This will clean up the HTML before it is stored in the output file Results.txt and add a break to make the .txt readable
                            //

                            Results = Regex.Replace(Results, @"<[^>]+>|&nbsp;", "").Trim();
                            Results = Regex.Replace(Results, @"\s{2,}", " ");
                            System.IO.File.WriteAllText(@"C:\\Users\\wwstudent\\source\\repos\\ConsoleApp8\\ConsoleApp8\\Results.txt", Results + "\r\n");

                            ///
                            ///Test to see that the location of the crime is stated in the Results of our crawl and place it in a variable crimeloctaion
                            ///

                            var locationstringcollection = location.Locations;
                            var stringcollectionlocation = locationstringcollection.Aggregate((a, b) => a + ", " + b);

                            // StringBuilder locationcollection = new StringBuilder();
                            // location.ForEach(z => locationcollection.Append(z));
                            // Match locationsmatch = Regex.Match(Results, stringcollectionlocation);

                            MatchCollection locationsmatch = Regex.Matches(Results, stringcollectionlocation);
                            //foreach (Match matches in locationsmatch)
                            string loc = locationsmatch.ToString();
                            
                            ///
                            ///Add the local time of the crawl to the results.  This does not have to be specific per customer request so we will use the date of the pull.
                            ///
                            DateTime CurrentTime = DateTime.Now;
                            TimeZoneInfo.ConvertTimeBySystemTimeZoneId(CurrentTime, TimeZoneInfo.Local.Id);

                            ///
                            ///We will now try to pull organization names that committed crimes from our keywords
                            ///

                            var organizations = organization.Terrorist.Any(x => Results.Contains(x));
                            string terroristorganization = organizations.ToString();
                            ///
                            ///We will now write the results of the file Results to a .txt file named Results.txt
                            ///TODO clean up the results so there is a website url and page break added to make the .txt file more readable.
                            ///

                            List<string> resultslist = new List<string>();
                            using (StreamReader reader = new StreamReader(@"C:\\Users\\wwstudent\\source\\repos\\ConsoleApp8\\ConsoleApp8\\Results.txt"))

                            {
                                string line;
                                using (StreamReader file = new StreamReader(@"C:\\Users\\wwstudent\\source\\repos\\ConsoleApp8\\ConsoleApp8\\Results.txt"));

                                if ((line = reader.ReadLine()) != null)
                                {
                                    string[] fields = line.Split(',');
                                    using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\wwstudent\source\repos\ConsoleApp8\ConsoleApp8\ResultsData.mdf;Integrated Security=True"))

                                        while ((line = reader.ReadLine()) != null)
                                        {

                                            //string sqlquery = "INSERT INTO @C:\\Users\\wwstudent\\source\repos\\ConsoleApp8\\ConsoleApp8\\ResultsData.mdf(Crimelocation, Currenttime, location, organization) VALUES(@Crime, @Date, @Location, @Organization)";
                                            //SqlCommand cmd = con.CreateCommand();
                                            using (SqlCommand cmd = new SqlCommand("INSERT INTO @C:\\Users\\wwstudent\\source\repos\\ConsoleApp8\\ConsoleApp8\\ResultsData.mdf(Crimelocation, Currenttime, location, organization) VALUES(@Crime, @Date, @Location, @Organization)", con))
                                            {
                                                cmd.CommandText = "INSERT INTO C:\\Users\\wwstudent\\source\repos\\ConsoleApp8\\ConsoleApp8\\ResultsData.mdf(Crimelocation, Currenttime, location, organization) VALUES(@Crime, @Date, @Location, @Organization)";
                                                cmd.CommandTimeout = 15;
                                                cmd.CommandType = System.Data.CommandType.Text;
                                                cmd.Parameters.AddWithValue("@Crime", keywords);
                                                cmd.Parameters.AddWithValue("@Date", CurrentTime);
                                                cmd.Parameters.AddWithValue("@Location", loc);
                                                cmd.Parameters.AddWithValue("@Organization", terroristorganization);
                                                con.Open();
                                                int rowsAffected = cmd.ExecuteNonQuery();
                                                con.Close();

                                            }
                                        }

                                }
                                else
                                {

                                };
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

                        else
                        { };



                    }

                }


                /*    If you want confirmation that your keyword does not appear on the specific HTTP site, uncoment this code.
                else
                {
                       Console.WriteLine("{0} not found not found at {1}", (string)keywords, (string)url);
                }*/

                
                //
                //Create a list of string that will later be used to create a function that will assign region names from the string to a variable named Location which will be addded to the database
                //




            }

        }
    }
}
// Created by Thomas P. Kasulke.  Please use freely and openly.  Critiques are always welcome.
// https://github.com/tk421isasith
