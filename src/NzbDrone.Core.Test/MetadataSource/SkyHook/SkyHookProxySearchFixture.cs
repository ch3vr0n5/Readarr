using FluentAssertions;
using Moq;
using NUnit.Framework;
using NzbDrone.Core.MetadataSource.SkyHook;
using NzbDrone.Core.Test.Framework;
using NzbDrone.Core.Tv;
using NzbDrone.Core.Books;
using NzbDrone.Test.Common;
using NzbDrone.Test.Common.Categories;

namespace NzbDrone.Core.Test.MetadataSource.SkyHook
{
    [TestFixture]
    [IntegrationTest]
    public class SkyHookProxySearchFixture : CoreTest<SkyHookProxy>
    {
        [SetUp]
        public void Setup()
        {
            UseRealHttp();
        }

        [TestCase("A Feast for Crows", "A Feast for Crows")]
        [TestCase("isbn:9780007378425", "A Game of Thrones (A Song of Ice and Fire, Book 1)")]
        [TestCase("isbn: 9780007378425 ", "A Game of Thrones (A Song of Ice and Fire, Book 1)")]
        public void successful_search(string title, string expected)
        {
            var result = Subject.SearchForNewBooks(title);

            result.Should().NotBeEmpty();

            result[0].Title.Should().Be(expected);

            ExceptionVerification.IgnoreWarns();
        }

        [TestCase("isbn:")]
        [TestCase("isbn: 99999999999999999999")]
        [TestCase("isbn: 0")]
        [TestCase("isbn: -12")]
        [TestCase("isbn:289578")]
        [TestCase("adjalkwdjkalwdjklawjdlKAJD")]
        public void no_search_result(string term)
        {
            var result = Subject.SearchForNewBooks(term);
            result.Should().BeEmpty();

            ExceptionVerification.IgnoreWarns();
        }

        [TestCase("isbn:9780007378425")]
        [TestCase("A Game of Thrones")]
        public void should_return_existing_series_if_found(string term)
        {
            const long isbn = 9780007378425;
            var existingSeries = new Book
            {
                ISBN = isbn
            };
            
            Mocker.GetMock<ISeriesService>().Setup(c => c.FindByTvdbId(isbn)).Returns(existingSeries);

            var result = Subject.SearchForNewBooks("isbn: " + isbn);

            result.Should().Contain(existingSeries);
            result.Should().ContainSingle(c => c.ISBN == isbn);

        }
    }
}
