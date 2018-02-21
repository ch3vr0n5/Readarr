using System.Collections.Generic;
using NzbDrone.Core.Models;

namespace NzbDrone.Core.MetadataSource
{
    public interface ISearchForNewSeries
    {
        List<Book> SearchForNewBook(string title);
    }
}