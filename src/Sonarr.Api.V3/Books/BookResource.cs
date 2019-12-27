using NzbDrone.Core.Books;
using NzbDrone.Core.MediaCover;
using NzbDrone.Core.Tv;
using Sonarr.Api.V3.Series;
using Sonarr.Http.REST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonarr.Api.V3.Books
{
    public class BookResource : RestResource
    {
        public long ISBN { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Overview { get; set; }
        public string Publisher { get; set; }

        public bool Monitored { get; set; }
        public List<MediaCover> Images { get; set; }
        public string TitleSlug { get; set; }
        public string Path { get; set; }
        public Ratings Ratings { get; set; }
        public List<Author> Authors { get; set; }
        public string RootFolderPath { get; set; }
        public DateTime Added { get; set; }
        public DateTime PublishDate { get; set; }
        public SeriesStatisticsResource Statistics { get; set; }
    }

    public static class BookResourceMapper
    {
        public static BookResource ToResource(this Book model)
        {
            if (model == null) return null;

            return new BookResource
            {
                Id = model.Id,
                ISBN = model.ISBN,
                Title = model.Title,
                SubTitle = model.SubTitle,
                Overview = model.Overview,
                Publisher = model.Publisher,
                Monitored = model.Monitored,
                Images = model.Images,
                TitleSlug = model.TitleSlug,
                Path = model.Path,
                Ratings = model.Ratings,
                Authors = model.Authors,
                Added = model.Added,
                PublishDate = model.PublishDate
            };
        }

        public static Book ToModel(this BookResource resource)
        {
            if (resource == null) return null;

            return new Book
            {
                Id = resource.Id,
                ISBN = resource.ISBN,
                Title = resource.Title,
                SubTitle = resource.SubTitle,
                Overview = resource.Overview,
                Publisher = resource.Publisher,
                Monitored = resource.Monitored,
                Images = resource.Images,
                TitleSlug = resource.TitleSlug,
                Path = resource.Path,
                Ratings = resource.Ratings,
                Authors = resource.Authors,
                Added = resource.Added,
                PublishDate = resource.PublishDate
            };
        }

        public static Book ToModel(this BookResource resource, Book series)
        {
            var updatedSeries = resource.ToModel();

            series.ApplyChanges(updatedSeries);

            return series;
        }

        public static List<BookResource> ToResource(this IEnumerable<Book> series)
        {
            return series.Select(s => ToResource(s)).ToList();
        }

        public static List<Book> ToModel(this IEnumerable<BookResource> resources)
        {
            return resources.Select(ToModel).ToList();
        }
    }
}
