using NzbDrone.Core.Books;
using NzbDrone.Core.MediaCover;
using NzbDrone.Core.Tv;
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

        public bool Monitored { get; set; }
        public List<MediaCover> Images { get; set; }
        public string TitleSlug { get; set; }
        public string Path { get; set; }
        public Ratings Ratings { get; set; }
        public List<Author> Authors { get; set; }
        public string RootFolderPath { get; set; }
        public DateTime Added { get; set; }
        public SeriesStatisticsResource Statistics { get; set; }
    }

    public static class BookResourceMapper
    {
        public static BookResource ToResource(this NzbDrone.Core.Books.Book model)
        {
            if (model == null) return null;

            return new BookResource
            {
                Id = model.Id,
                ISBN = model.ISBN,
                Title = model.Title
            };
        }

        public static NzbDrone.Core.Books.Book ToModel(this BookResource resource)
        {
            if (resource == null) return null;

            return new NzbDrone.Core.Books.Book
            {
                Id = resource.Id,
                ISBN = resource.ISBN,
                Title = resource.Title,
            };
        }

        public static NzbDrone.Core.Books.Book ToModel(this BookResource resource, NzbDrone.Core.Books.Book series)
        {
            var updatedSeries = resource.ToModel();

            series.ApplyChanges(updatedSeries);

            return series;
        }

        public static List<BookResource> ToResource(this IEnumerable<NzbDrone.Core.Books.Book> series)
        {
            return series.Select(s => ToResource(s)).ToList();
        }

        public static List<NzbDrone.Core.Books.Book> ToModel(this IEnumerable<BookResource> resources)
        {
            return resources.Select(ToModel).ToList();
        }
    }
}
