using NzbDrone.Core.Datastore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NzbDrone.Core.Books
{
    public class Author : IEmbeddedDocument
    {
        public Author()
        {
            Images = new List<MediaCover.MediaCover>();
        }

        public string Name { get; set; }
        public List<MediaCover.MediaCover> Images { get; set; }
    }
}
