using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LazyBook.Models
{
    enum Cat { Business, Cooking, History, Computers, Detective, Thriller, Kids, Politics, Law, Religion, Romance, Science_Fiction, Health} 
    class Book
    {
        [Newtonsoft.Json.JsonProperty("Id")]
        public string Id { get; set; }

        [Newtonsoft.Json.JsonProperty("Name")]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("Author")]
        public string Author { get; set; }

        [Newtonsoft.Json.JsonProperty("Publisher")]
        public string Publisher { get; set; }

        [Newtonsoft.Json.JsonProperty("Year")]
        public int Year { get; set; }

        [Newtonsoft.Json.JsonProperty("Category")]
        public Cat Category { get; set; }

        [Newtonsoft.Json.JsonProperty("Summary")]
        public string Summary { get; set; }

        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }

        public Book(string name, string author, string publisher, int year, Cat category, string summary)
        {
            this.Name = name;
            this.Author = author;
            this.Publisher = publisher;
            this.Year = year;
            this.Category = category;
            this.Summary = summary;
        }
    }
}
