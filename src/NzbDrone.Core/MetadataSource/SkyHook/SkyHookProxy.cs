using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NLog;
using NzbDrone.Common.Cloud;
using NzbDrone.Common.Extensions;
using NzbDrone.Common.Http;
using NzbDrone.Core.Exceptions;
using NzbDrone.Core.MediaCover;
using NzbDrone.Core.MetadataSource.SkyHook.Resource;
using NzbDrone.Core.MetadataSource.SkyHook.New_Resources;
using NzbDrone.Core.Tv;
using NzbDrone.Core.Models;

namespace NzbDrone.Core.MetadataSource.SkyHook
{
    public class SkyHookProxy : IProvideSeriesInfo, ISearchForNewSeries
    {
        private readonly IHttpClient _httpClient;
        private readonly Logger _logger;
        private readonly IHttpRequestBuilderFactory _requestBuilder;

        public SkyHookProxy(IHttpClient httpClient, ISonarrCloudRequestBuilder requestBuilder, Logger logger)
        {
            _httpClient = httpClient;
            _requestBuilder = requestBuilder.Books;
            _logger = logger;
        }

        public Tuple<Series, List<Episode>> GetSeriesInfo(int tvdbSeriesId)
        {
            var httpRequest = _requestBuilder.Create()
                                             .SetSegment("route", "shows")
                                             .Resource(tvdbSeriesId.ToString())
                                             .Build();

            httpRequest.AllowAutoRedirect = true;
            httpRequest.SuppressHttpError = true;

            var httpResponse = _httpClient.Get<ShowResource>(httpRequest);

            if (httpResponse.HasHttpError)
            {
                if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new SeriesNotFoundException(tvdbSeriesId);
                }
                else
                {
                    throw new HttpException(httpRequest, httpResponse);
                }
            }

            //var episodes = httpResponse.Resource.Episodes.Select(MapEpisode);
            //var series = MapSeries(httpResponse.Resource);

            return null;//new Tuple<Series, List<Episode>>(series, episodes.ToList());
        }

        public List<BookGroup> SearchForNewBook(string title)
        {
            try
            {
                var httpRequest = _requestBuilder.Create()
                                                 .AddQueryParam("q", "inauthor:" + title.ToLower().Trim())
                                                 .Build();

                var httpResponse = _httpClient.Get<VolumeResource>(httpRequest);
                return httpResponse.Resource.items.SelectList(MapVolumes);
            }
            catch (HttpException)
            {
                throw new SkyHookException("Search for '{0}' failed. Unable to communicate with SkyHook.", title);
            }
            catch (Exception ex)
            {
                _logger.Warn(ex, ex.Message);
                throw new SkyHookException("Search for '{0}' failed. Invalid response received from SkyHook.", title);
            }
        }

        private static BookGroup MapVolumes(VolumeItem volume)
        {
            var book = new Book();
            book.Title = volume.volumeInfo.title;
            book.SubTitle = volume.volumeInfo.subtitle;
            book.Description = volume.volumeInfo.description;
            book.Language = volume.volumeInfo.language;

            book.PageCount = volume.volumeInfo.pageCount;
            book.Rating = volume.volumeInfo.averageRating;
            book.RatingCount = volume.volumeInfo.ratingsCount;

            book.Authors = volume.volumeInfo.authors;
            book.Categories = volume.volumeInfo.categories;

            book.Publisher = volume.volumeInfo.publisher;
            book.PublishedDate = volume.volumeInfo.publishedDate;

            book.ISBN10 = volume.volumeInfo.industryIdentifiers.Where(a => a.type == "ISBN_10").Select(a => a.identifier).SingleOrDefault();
            book.ISBN13 = volume.volumeInfo.industryIdentifiers.Where(a => a.type == "ISBN_13").Select(a => a.identifier).SingleOrDefault();

            if (volume.volumeInfo.imageLinks != null)
            {
                book.RemoteImage = volume.volumeInfo.imageLinks.thumbnail;
                book.RemoteImageSmall = volume.volumeInfo.imageLinks.smallThumbnail;
            }

            return new BookGroup() { books = { book } };
        }
    }
}
