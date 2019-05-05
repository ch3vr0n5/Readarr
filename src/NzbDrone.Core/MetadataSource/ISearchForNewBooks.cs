using System.Collections.Generic;
using NzbDrone.Core.Books;

namespace NzbDrone.Core.MetadataSource
{
    public interface ISearchForNewBooks
    {
        List<Book> SearchForNewBooks(string title);
    }
}