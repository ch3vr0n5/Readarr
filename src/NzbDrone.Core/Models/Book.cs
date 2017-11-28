using NzbDrone.Core.Datastore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NzbDrone.Core.Models
{
    public class Book : ModelBase
    {
        public Book()
        {
            Authors = new List<string>();
            Categories = new List<string>();
        }

        public string GoogleID { get; set; }

        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }

        public int PageCount { get; set; }
        public double Rating { get; set; }
        public int RatingCount { get; set; }

        public List<string> Authors;
        public List<string> Categories;

        public string Publisher { get; set; }
        public string PublishedDate { get; set; }

        //NOTE: Need both?
        public string ISBN10 { get; set; }
        public string ISBN13 { get; set; }

        public string RemoteImageSmall { get; set; }
        public string RemoteImage { get; set; }
        
        public bool Monitored { get; set; }
        public string Path { get; set; }
        public string RootFolderPath { get; set; }

        public void ApplyChanges(Book otherBook)
        {
            GoogleID = otherBook.GoogleID;
            Monitored = otherBook.Monitored;
            Path = otherBook.Path;
            RootFolderPath = otherBook.RootFolderPath;

            //TvdbId = otherSeries.TvdbId;

            //Seasons = otherSeries.Seasons;
            //Path = otherSeries.Path;
            //ProfileId = otherSeries.ProfileId;

            //SeasonFolder = otherSeries.SeasonFolder;
            //Monitored = otherSeries.Monitored;

            //SeriesType = otherSeries.SeriesType;
            //RootFolderPath = otherSeries.RootFolderPath;
            //Tags = otherSeries.Tags;
            //AddOptions = otherSeries.AddOptions;
        }
    }
}
