using NzbDrone.Common.Http;

namespace NzbDrone.Common.Cloud
{
    public interface ISonarrCloudRequestBuilder
    {
        IHttpRequestBuilderFactory Services { get; }
        IHttpRequestBuilderFactory SkyHookTvdb { get; }
        IHttpRequestBuilderFactory Books { get; }
    }

    public class SonarrCloudRequestBuilder : ISonarrCloudRequestBuilder
    {
        public SonarrCloudRequestBuilder()
        {
            Services = new HttpRequestBuilder("http://services.sonarr.tv/v1/")
                .CreateFactory();

            SkyHookTvdb = new HttpRequestBuilder("http://skyhook.sonarr.tv/v1/tvdb/{route}/{language}/")
                .SetSegment("language", "en")
                .CreateFactory();

            Books = new HttpRequestBuilder("https://www.googleapis.com/books/v1/volumes")
                .CreateFactory();
        }

        public IHttpRequestBuilderFactory Services { get; }

        public IHttpRequestBuilderFactory SkyHookTvdb { get; }

        public IHttpRequestBuilderFactory Books { get; }
    }
}
