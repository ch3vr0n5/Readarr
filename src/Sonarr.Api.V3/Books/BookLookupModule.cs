using System.Collections.Generic;
using System.Linq;
using Nancy;
using NzbDrone.Core.MediaCover;
using NzbDrone.Core.MetadataSource;
using NzbDrone.Core.SeriesStats;
using Sonarr.Api.V3.Books;
using Sonarr.Http;
using Sonarr.Http.Extensions;

namespace Sonarr.Api.V3.Series
{
    public class SeriesLookupModule : SonarrRestModule<BookResource>
    {
        private readonly ISearchForNewBooks _searchProxy;

        public SeriesLookupModule(ISearchForNewBooks searchProxy)
            : base("/books/lookup")
        {
            _searchProxy = searchProxy;
            Get["/"] = x => Search();
        }

        private Response Search()
        {
            var tvDbResults = _searchProxy.SearchForNewBooks((string)Request.Query.term);
            return MapToResource(tvDbResults).AsResponse();
        }

        private static IEnumerable<BookResource> MapToResource(IEnumerable<NzbDrone.Core.Books.Book> books)
        {
            foreach (var currentBook in books)
            {
                var resource = currentBook.ToResource();

                //var poster = currentBook.Images.FirstOrDefault(c => c.CoverType == MediaCoverTypes.Poster);
                //if (poster != null)
                //   resource.RemotePoster = poster.Url;

                resource.Statistics = null; //new SeriesStatistics().ToResource(resource.Seasons);

                yield return resource;
            }
        }
    }
}
