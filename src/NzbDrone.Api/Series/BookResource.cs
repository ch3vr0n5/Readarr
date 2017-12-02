using NzbDrone.Api.REST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NzbDrone.Api.Series
{
    public class BookGroupResource : RestResource
    {
        public List<BookResource> books;
        
        public class BookResource
        {
            public BookResource()
            {

            }

            public string GoogleID { get; set; }

            public string Title { get; set; }
            public string Description { get; set; }

            public List<string> Authors { get; set; }

            public string RemoteImageSmall { get; set; }
            public string RemoteImage { get; set; }
        }
    }

    public static class BookResourceMapper
    {
        public static BookGroupResource ToResource(this Core.Models.BookGroup group)
        {
            if (group == null) return null;

            return new BookGroupResource() { books =
                group.books.Select(a => new BookGroupResource.BookResource() {
                    Title = a.Title,
                    Description = a.Description,
                    RemoteImage = a.RemoteImage,
                    RemoteImageSmall = a.RemoteImage,
                    Authors = a.Authors,
                    GoogleID = a.GoogleID
                }).ToList()
            };
        }

        public static Core.Models.BookGroup ToModel(this BookGroupResource resource)
        {
            if (resource == null) return null;

            return new Core.Models.BookGroup
            {
                
            };
        }

        public static Core.Models.BookGroup ToModel(this BookGroupResource resource, Core.Models.BookGroup group)
        {
            var updatedSeries = resource.ToModel();

            group.ApplyChanges(updatedSeries);

            return group;
        }

        public static List<BookGroupResource> ToResource(this IEnumerable<Core.Models.BookGroup> groups)
        {
            return groups.Select(ToResource).ToList();
        }
    }
}
