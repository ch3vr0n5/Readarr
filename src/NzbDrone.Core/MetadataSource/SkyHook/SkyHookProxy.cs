using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using NLog;
using NzbDrone.Common.Cloud;
using NzbDrone.Common.Extensions;
using NzbDrone.Common.Http;
using NzbDrone.Core.Books;
using NzbDrone.Core.Exceptions;
using NzbDrone.Core.MediaCover;
using NzbDrone.Core.MetadataSource.SkyHook.Resource;
using NzbDrone.Core.Tv;

namespace NzbDrone.Core.MetadataSource.SkyHook
{
    public class SkyHookProxy : IProvideSeriesInfo, ISearchForNewBooks
    {
        private readonly IHttpClient _httpClient;
        private readonly Logger _logger;
        private readonly ISeriesService _seriesService;
        private readonly IHttpRequestBuilderFactory _requestBuilder;

        public SkyHookProxy(IHttpClient httpClient, ISonarrCloudRequestBuilder requestBuilder, ISeriesService seriesService, Logger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _seriesService = seriesService;
            _requestBuilder = requestBuilder.GoogleBooksAPI;
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

        public List<Book> SearchForNewBooks(string title)
        {
            try
            {
                var lowerTitle = title.ToLowerInvariant();

                //Google search options
                var filters = new[] { "isbn:", "inauthor:", "inpublisher:" };

                //If a filter was used but it's not in the list then error out
                if(!filters.Where(a => lowerTitle.Contains(a)).Any() && lowerTitle.Contains(":"))
                    throw new SkyHookException("Invalid filter used when searching.");

                var httpRequest = _requestBuilder.Create()
                                                 .AddQueryParam("q", lowerTitle.Trim())
                                                 .Build();

                var httpResponse = _httpClient.Get<VolumeResource>(httpRequest);

                return MapSearchResult(httpResponse.Resource);
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

        private List<Book> MapSearchResult(VolumeResource volume)
        {
            /*var series = _seriesService.FindByTvdbId(show.TvdbId);

            if (series == null)
            {
                series = MapSeries(show);
            }

            return series;*/

            return volume.items.SelectList(MapBook);
        }
        
        private Book MapBook(VolumeItem volume)
        {
            var book = new Book();
            book.ISBN = volume.volumeInfo.industryIdentifiers.Where(a => a.type == "ISBN_13").Select(a => Convert.ToInt64(a.identifier)).FirstOrDefault();
            book.Title = volume.volumeInfo.title;
            book.SubTitle = volume.volumeInfo.subtitle;
            book.Overview = volume.volumeInfo.description;
            book.Publisher = volume.volumeInfo.publisher;

            if (volume.volumeInfo.publishedDate != null)
            {
                //Date seems to be missing day or month sometimes
                var parts = volume.volumeInfo.publishedDate.Split('-').Select(a => Convert.ToInt32(a));
                var dateParts = Enumerable.Repeat(1, 3).Select((a, i) => parts.Count() > i ? parts.ElementAt(i) : 1).ToList();

                book.PublishDate = new DateTime(dateParts[0], dateParts[1], dateParts[2]);
            }

            book.Monitored = true;

            //Some books just don't have images
            if(volume.volumeInfo.imageLinks != null)
                book.Images = MapImages(volume.volumeInfo.imageLinks);

            book.Ratings = MapRatings(volume.volumeInfo);
            book.Authors = volume.volumeInfo.authors.SelectList(MapAuthor);
            
            return book;
        }

        private static Ratings MapRatings(VolumeInfo info)
        {
            if (info.ratingsCount == null || info.averageRating == null)
            {
                return new Ratings();
            }

            return new Ratings
            {
                Votes = (int)info.ratingsCount,
                Value = (int)info.averageRating
            };
        }

        private static List<MediaCover.MediaCover> MapImages(ImageLinks links)
        {
            return new List<MediaCover.MediaCover> {
                new MediaCover.MediaCover
                {
                    Url = links.thumbnail,
                    CoverType = MediaCoverTypes.Poster
                }
            };
        }

        private static Author MapAuthor(string author)
        {
            return new Author()
            {
                Name = author
            };
        }
    }
}
