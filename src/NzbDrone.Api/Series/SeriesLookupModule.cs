using System.Collections.Generic;
using Nancy;
using NzbDrone.Api.Extensions;
using NzbDrone.Core.MediaCover;
using NzbDrone.Core.MetadataSource;
using System.Linq;

namespace NzbDrone.Api.Series
{
    public class SeriesLookupModule : NzbDroneRestModule<BookResource>
    {
        private readonly ISearchForNewSeries _searchProxy;

        public SeriesLookupModule(ISearchForNewSeries searchProxy)
            : base("/book/lookup")
        {
            _searchProxy = searchProxy;
            Get["/"] = x => Search();
        }
        
        private Response Search()
        {
            var tvDbResults = _searchProxy.SearchForNewBook((string)Request.Query.term);
            return MapToResource(tvDbResults).AsResponse();
        }

        private static IEnumerable<BookResource> MapToResource(IEnumerable<Core.Models.Book> groups)
        {
            foreach(var group in groups)
                yield return group.ToResource();
        }
    }
}