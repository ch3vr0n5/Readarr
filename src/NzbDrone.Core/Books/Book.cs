using NzbDrone.Core.Datastore;
using NzbDrone.Core.Tv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NzbDrone.Core.Books
{
    public class Book : ModelBase
    {
        public Book()
        {
            Images = new List<MediaCover.MediaCover>();
            Authors = new List<Author>();
        }

        public long ISBN { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Overview { get; set; }
        public string Publisher { get; set; }
        public DateTime PublishDate { get; set; }

        public bool Monitored { get; set; }
        public List<MediaCover.MediaCover> Images { get; set; }
        public string TitleSlug { get; set; }
        public string Path { get; set; }
        public Ratings Ratings { get; set; }
        public List<Author> Authors { get; set; }
        public string RootFolderPath { get; set; }
        public DateTime Added { get; set; }

        public void ApplyChanges(Book otherBook)
        {
            ISBN = otherBook.ISBN;
            Path = otherBook.Path;
            Monitored = otherBook.Monitored;
            RootFolderPath = otherBook.RootFolderPath;
        }
    }
}