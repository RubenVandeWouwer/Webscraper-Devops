using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Webscraper_Devops
{
    class Program
    {
        static void Main(string[] args)
        {
            bool running = true;
            while (running) {
                Console.WriteLine("----------");
                Console.WriteLine("Welcome to the webscraping tool");
                Console.WriteLine("Options:");
                Console.WriteLine("Youtube: 1");
                Console.WriteLine("Indeed: 2");
                Console.WriteLine("Immoweb: 3");
                Console.WriteLine("Exit scraper: 4");
                var option = Console.ReadLine();
                if (option == "1")
                {
                    Scrapeyt();
                }
                else if (option == "2")
                {
                    scrapeindeed();
                }
                else if (option == "3")
                {
                    scrapeimmoweb();
                }
                else if (option == "4")
                {
                    Console.WriteLine("Ending program...");
                    Console.WriteLine("You can close this window.");
                    running = false;
                }
                else
                {
                    Console.WriteLine("This is not a valid option");
                    Console.WriteLine("----------");
                }
            }
            
        }
        public static void Scrapeyt()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://youtube.com");
            var acceptcookies = driver.FindElement(By.XPath("/html/body/ytd-app/ytd-consent-bump-v2-lightbox/tp-yt-paper-dialog/div[4]/div[2]/div[5]/div[2]/ytd-button-renderer[2]/a/tp-yt-paper-button"));
            acceptcookies.Click();
            var element = driver.FindElement(By.XPath("//input[@id=\"search\"]"));
            Console.WriteLine("Vul een zoekterm in:");
            var searchedstr = Console.ReadLine();
            element.SendKeys(searchedstr);
            element.Submit();
            Console.WriteLine("Opzoeken...");


            var sortby = driver.FindElement(By.XPath("/html/body/ytd-app/div/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div[1]/div[2]/ytd-search-sub-menu-renderer/div[1]/div/ytd-toggle-button-renderer/a/tp-yt-paper-button"));
            sortby.Click();
            var uploadtimefilter = driver.FindElement(By.XPath("/html/body/ytd-app/div/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div[1]/div[2]/ytd-search-sub-menu-renderer/div[1]/iron-collapse/div/ytd-search-filter-group-renderer[5]/ytd-search-filter-renderer[2]/a/div/yt-formatted-string"));
            uploadtimefilter.Click();

            System.Threading.Thread.Sleep(2000);
            var videos = driver.FindElements(By.XPath("//*[@id=\"dismissible\"]"));

            Console.WriteLine("Naar welk bestand wil je de data wegschrijven?");
            var bestandsnaam = Console.ReadLine();
            bestandsnaam += ".csv";

            foreach (var video in videos)
            {
                var videotitle = video.FindElement(By.Id("title-wrapper")).Text;
                var videouploader = video.FindElement(By.Id("channel-name")).Text;
                var videoviews = video.FindElement(By.XPath("//*[@id=\"metadata-line\"]/span[1]")).Text;
                var videolink = video.FindElement(By.Id("thumbnail")).GetAttribute("href");

                Console.WriteLine("Title: " + videotitle);
                Console.WriteLine("Views: " + videoviews);
                Console.WriteLine("Uploader: " + videouploader);
                Console.WriteLine("Link: " + videolink);

                Writeyttocsv(videotitle, videouploader, videoviews, videolink, bestandsnaam);
            }
            driver.Close();
          
        }
        public static void scrapeindeed()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://be.indeed.com/");
            Console.WriteLine("Naar welke functie ben je op zoek?");
            var jobfunctie = Console.ReadLine();
            Console.WriteLine("In de buurt van welke stad/gemeente wil je werken?");
            var locatie = Console.ReadLine();
            Console.WriteLine("Zoeken naar de beste jobs...");
            var functieveld = driver.FindElement(By.XPath("//input[@id=\"text-input-what\"]"));
            functieveld.SendKeys(jobfunctie);
            var locatieveld = driver.FindElement(By.XPath("//input[@id=\"text-input-where\"]"));
            locatieveld.SendKeys(locatie);
            locatieveld.Submit();
            var datefilter = driver.FindElement(By.XPath("/html/body/table[1]/tbody/tr/td/div/div[2]/div/div[1]/button"));
            datefilter.Click();
            var days = driver.FindElement(By.XPath("html/body/table[1]/tbody/tr/td/div/div[2]/div/div[1]/ul/li[2]"));
            days.Click();
            System.Threading.Thread.Sleep(2000);
            var delpopup = driver.FindElement(By.XPath("html/body/div[5]/div[1]/button"));
            delpopup.Click();
            
            var jobs = driver.FindElements(By.CssSelector("a.result"));

            Console.WriteLine("Naar welk bestand wil je de data wegschrijven?");
            var bestandsnaam = Console.ReadLine();
            bestandsnaam += ".csv";

            foreach (var job in jobs)
            {
                var title = job.FindElement(By.CssSelector(".jobTitle > span")).Text;
                var company = job.FindElement(By.CssSelector(".companyName")).Text;
                var place = job.FindElement(By.CssSelector(".companyLocation")).Text;
                var link = job.FindElement(By.CssSelector("a")).GetAttribute("href");

                Console.WriteLine("----------");
                Console.WriteLine("Title: " + title);
                Console.WriteLine("Company: " + company);
                Console.WriteLine("Place: " + place);
                Console.WriteLine("Link: " + link);

                Writeindeedtocsv(title, company, place, link, bestandsnaam);

            }
            driver.Close();
        }

        public static void scrapeimmoweb()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.immoweb.be/nl");
            System.Threading.Thread.Sleep(1000);
            var acceptcoockies = driver.FindElement(By.XPath("/html/body/div[3]/div[4]/div[2]/div/div[2]/div[1]/button"));
            acceptcoockies.Click();
            Console.WriteLine("In welke buurt ben je op zoek naar een woning?");
            var locatie = Console.ReadLine();
            var locatieveld = driver.FindElement(By.XPath("/html/body/div[1]/div[1]/main/div/div[2]/div/div[1]/form/div[2]/div/div/form/div/div[1]/div/input"));
            locatieveld.SendKeys(locatie);
            locatieveld.Submit();
            var cards = driver.FindElements(By.CssSelector(".card--result"));

            Console.WriteLine("Naar welk bestand wil je de data wegschrijven?");
            var bestandsnaam = Console.ReadLine();
            bestandsnaam += ".csv";

            foreach (var card in cards)
            {

                var soort = card.FindElement(By.CssSelector(".card__title-link")).Text;
                var prijs = card.FindElement(By.CssSelector(".card--result__price > span > span")).Text;
                var beschrijving = card.FindElement(By.CssSelector(".card--result__description")).Text;
                var link = card.FindElement(By.CssSelector(".card__title-link")).GetAttribute("href");

                Console.WriteLine("Soort: " + soort);
                Console.WriteLine("Prijs: " + prijs);
                Console.WriteLine("beschrijving: " + beschrijving);
                Console.WriteLine("Link: " + link);
                Console.WriteLine("------");

                               

                Writeimmotocsv(soort, prijs, beschrijving, link, bestandsnaam);

            }

            driver.Close();

        }
        public static void Writeimmotocsv(string soort, string prijs, string beschrijving, string link, string bestandsnaam)
        {
            Console.WriteLine("Writing data to file...");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@bestandsnaam, true))
            {
                file.WriteLine(soort + ";" + prijs + ";" + beschrijving + ";" + link);
            }
        }

        public static void Writeyttocsv(string videotitle, string videouploader, string videoviews, string videolink, string bestandsnaam)
        {
            Console.WriteLine("Writing data to file...");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@bestandsnaam, true))
            {
                file.WriteLine(videotitle + ";" + videouploader + ";" + videoviews + ";" + videolink);
            }

        }
        public static void Writeindeedtocsv(string title, string company, string place, string link, string bestandsnaam)
        {
            Console.WriteLine("Writing data to file...");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@bestandsnaam, true))
            {
                file.WriteLine(title + ";" + company + ";" + place + ";" + link);
            }

        }

    }
}
