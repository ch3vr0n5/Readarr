using NzbDrone.Common.Http;

namespace NzbDrone.Common.Cloud
{
    public interface ISonarrCloudRequestBuilder
    {
        IHttpRequestBuilderFactory Services { get; }
        IHttpRequestBuilderFactory Books { get; }
    }

    public class SonarrCloudRequestBuilder : ISonarrCloudRequestBuilder
    {
        public SonarrCloudRequestBuilder()
        {
            Services = new HttpRequestBuilder("http://services.sonarr.tv/v1/")
                .CreateFactory();

            Books = new HttpRequestBuilder("https://www.googleapis.com/books/v1/volumes")
                .CreateFactory();
        }

        public IHttpRequestBuilderFactory Services { get; }

        public IHttpRequestBuilderFactory Books { get; }
    }
}
