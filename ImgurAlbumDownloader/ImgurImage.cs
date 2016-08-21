using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgurAlbumDownloader
{
    public class ImgurImage
    {
        public string id { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public int datetime { get; set; }

        public string type { get; set; }

        public bool animated { get; set; }

        public int width { get; set; }

        public int height { get; set; }

        public int size { get; set; }

        public int views { get; set; }

        public long bandwidth { get; set; }

        public string vote { get; set; }

        public string favorite { get; set; }

        public string nsfw { get; set; }

        public string section { get; set; }

        public string account_url { get; set; }

        public string account_id { get; set; }

        public string comment_preview { get; set; }

        public string link { get; set; }

        public string comment_count { get; set; }

        public string ups { get; set; }

        public string downs { get; set; }

        public string points { get; set; }

        public int score { get; set; }

        public bool is_album { get; set; }
    }
}
