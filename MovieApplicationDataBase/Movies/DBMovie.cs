using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApplicationDataBase.Movies
{
    public class DBMovie
    {
        public int? ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Image Image { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public List<DBJanres> Janres { get; set; } = new List<DBJanres>();

        public string StrJanres => string.Join(", ", Janres.Select(x => x.Name));

    }
}
