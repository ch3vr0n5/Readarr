using System.Collections.Generic;
using Nancy;
using NzbDrone.Api.Extensions;
using NzbDrone.Core.MediaCover;
using NzbDrone.Core.MetadataSource;
using System.Linq;

namespace NzbDrone.Api.Series
{
    public class SeriesLookupModule : NzbDroneRestModule<BookGroupResource>
    {
        private readonly ISearchForNewSeries _searchProxy;

        public SeriesLookupModule(ISearchForNewSeries searchProxy)
            : base("/author/lookup")
        {
            _searchProxy = searchProxy;
            Get["/"] = x => Search();
        }
        
        private Response Search()
        {
            var tvDbResults = _searchProxy.SearchForNewBook((string)Request.Query.term);
            return MapToResource(tvDbResults).AsResponse();
        }

        private static IEnumerable<BookGroupResource> MapToResource(IEnumerable<Core.Models.BookGroup> groups)
        {
            foreach(var group in groups)
                yield return group.ToResource();
        }
    }
}