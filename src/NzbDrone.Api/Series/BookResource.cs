using NzbDrone.Api.REST;
using NzbDrone.Core.Tv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NzbDrone.Api.Series
{
    public class BookResource : RestResource
    {
        public BookResource()
        {

        }

        public string GoogleID { get; set; }

        public string Title { get; set; }
        public string TitleSlug { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string RootFolderPath { get; set; }

        public List<string> Authors { get; set; }
        public DateTime Added { get; set; }
        public AddSeriesOptions AddOptions { get; set; }

        public string RemoteImageSmall { get; set; }
        public string RemoteImage { get; set; }
    }

    public static class BookResourceMapper
    {
        public static BookResource ToResource(this Core.Models.Book book)
        {
            if (book == null) return null;

            return new BookResource() {
                Title = book.Title,
                Description = book.Description,
                Path = book.Path,
                RemoteImage = book.RemoteImage,
                RemoteImageSmall = book.RemoteImage,
                Authors = book.Authors,
                GoogleID = book.GoogleID,
                RootFolderPath = book.RootFolderPath,
                AddOptions = book.AddOptions,
                Added = book.Added,
                TitleSlug = book.TitleSlug
            };
        }

        public static Core.Models.Book ToModel(this BookResource book)
        {
            if (book == null) return null;

            return new Core.Models.Book()
            {
                Title = book.Title,
                Description = book.Description,
                Path = book.Path,
                RemoteImage = book.RemoteImage,
                RemoteImageSmall = book.RemoteImageSmall,
                Authors = book.Authors,
                GoogleID = book.GoogleID,
                RootFolderPath = book.RootFolderPath,
                AddOptions = book.AddOptions,
                Added = book.Added,
                TitleSlug = book.TitleSlug
            };
        }

        public static Core.Models.Book ToModel(this BookResource resource, Core.Models.Book group)
        {
            var updatedSeries = resource.ToModel();

            group.ApplyChanges(updatedSeries);

            return group;
        }

        public static List<BookResource> ToResource(this IEnumerable<Core.Models.Book> groups)
        {
            return groups.Select(ToResource).ToList();
        }
    }
}
