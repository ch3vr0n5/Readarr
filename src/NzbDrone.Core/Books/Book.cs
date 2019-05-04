using NzbDrone.Core.Datastore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NzbDrone.Core.Books
{
    public class Book : ModelBase
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }


    }
}
