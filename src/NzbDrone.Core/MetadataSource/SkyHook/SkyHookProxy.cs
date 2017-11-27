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

            var episodes = httpResponse.Resource.Episodes.Select(MapEpisode);
            //var series = MapSeries(httpResponse.Resource);

            return null;//new Tuple<Series, List<Episode>>(series, episodes.ToList());
        }

        public List<Series> SearchForNewSeries(string title)
        {
            try
            {
                var httpRequest = _requestBuilder.Create()
                                                 .AddQueryParam("q", "inauthor:" + title.ToLower().Trim())
                                                 .Build();

                var httpResponse = _httpClient.Get<List<VolumeResource>>(httpRequest);

                return httpResponse.Resource.SelectList(MapVolumes);
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

        private static Series MapVolumes(VolumeResource volume)
        {
            var series = new Series();

            series.Title = volume.items[0].volumeInfo.title;

            return series;
        }

        private static Actor MapActors(ActorResource arg)
        {
            var newActor = new Actor
            {
                Name = arg.Name,
                Character = arg.Character
            };

            if (arg.Image != null)
            {
                newActor.Images = new List<MediaCover.MediaCover>
                {
                    new MediaCover.MediaCover(MediaCoverTypes.Headshot, arg.Image)
                };
            }

            return newActor;
        }

        private static Episode MapEpisode(EpisodeResource oracleEpisode)
        {
            var episode = new Episode();
            episode.Overview = oracleEpisode.Overview;
            episode.SeasonNumber = oracleEpisode.SeasonNumber;
            episode.EpisodeNumber = oracleEpisode.EpisodeNumber;
            episode.AbsoluteEpisodeNumber = oracleEpisode.AbsoluteEpisodeNumber;
            episode.Title = oracleEpisode.Title;

            episode.AirDate = oracleEpisode.AirDate;
            episode.AirDateUtc = oracleEpisode.AirDateUtc;

            episode.Ratings = MapRatings(oracleEpisode.Rating);

            //Don't include series fanart images as episode screenshot
            if (oracleEpisode.Image != null)
            {
                episode.Images.Add(new MediaCover.MediaCover(MediaCoverTypes.Screenshot, oracleEpisode.Image));
            }

            return episode;
        }

        private static Season MapSeason(SeasonResource seasonResource)
        {
            return new Season
            {
                SeasonNumber = seasonResource.SeasonNumber,
                Images = seasonResource.Images.Select(MapImage).ToList(),
                Monitored = seasonResource.SeasonNumber > 0
            };
        }

        private static SeriesStatusType MapSeriesStatus(string status)
        {
            if (status.Equals("ended", StringComparison.InvariantCultureIgnoreCase))
            {
                return SeriesStatusType.Ended;
            }

            return SeriesStatusType.Continuing;
        }

        private static Ratings MapRatings(RatingResource rating)
        {
            if (rating == null)
            {
                return new Ratings();
            }

            return new Ratings
            {
                Votes = rating.Count,
                Value = rating.Value
            };
        }

        private static MediaCover.MediaCover MapImage(ImageResource arg)
        {
            return new MediaCover.MediaCover
            {
                Url = arg.Url,
                CoverType = MapCoverType(arg.CoverType)
            };
        }

        private static MediaCoverTypes MapCoverType(string coverType)
        {
            switch (coverType.ToLower())
            {
                case "poster":
                    return MediaCoverTypes.Poster;
                case "banner":
                    return MediaCoverTypes.Banner;
                case "fanart":
                    return MediaCoverTypes.Fanart;
                default:
                    return MediaCoverTypes.Unknown;
            }
        }
    }
}
