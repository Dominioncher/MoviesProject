using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApplication.DTO
{
    public class Photo
    {
        public Guid ID { get; set; }

        public Image Image { get; set; }
    }
}
