using NzbDrone.Core.Datastore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NzbDrone.Core.Models
{
    public class BookGroup : ModelBase
    {
        public BookGroup()
        {
            books = new List<Book>();
        }

        public List<Book> books;
        
        public void ApplyChanges(BookGroup otherBook)
        {
            for(int i = 0; i < books.Count; i++)
                books[i].ApplyChanges(otherBook.books[i]);
        }
    }
}
