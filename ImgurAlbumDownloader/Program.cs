using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ini;
using System.Windows.Forms;

namespace ImgurAlbumDownloader
{
    class Program
    {
        private static int toDownload;
        private static int downloaded;

        [STAThread]
        static void Main(string[] args)
        {
            ServicePointManager.DefaultConnectionLimit = 10000;

            string clientID = ReadClientID();
            Console.Write(">album id:  ");
            string albumID = Console.ReadLine();
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK) return;
            string downloadFolder = fbd.SelectedPath;
            Console.WriteLine(">download folder: " + downloadFolder + "\n");
            var stuff = GetImages(string.Format("https://api.imgur.com/3/album/{0}/images", albumID), clientID);
            toDownload = stuff.Count;
            Console.WriteLine(">images to download: " + toDownload);
            Console.WriteLine(">downloading...");

            Parallel.ForEach(stuff, new ParallelOptions { MaxDegreeOfParallelism = 50 }, item =>
                {
                    string fileName = item.link.Substring(item.link.LastIndexOf('/') + 1);
                    WebClient client = new WebClient();
                    client.DownloadFile(item.link, Path.Combine(downloadFolder, fileName));
                    downloaded++;
                    Console.Clear();
                    Console.WriteLine(string.Format(">current progress: {0} / {1} ({2})", downloaded, toDownload, ((downloaded * 100) / toDownload) + "%"));
                });
        }

        private static List<ImgurImage> GetImages(string url, string apiKey)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization", string.Format("Client-ID {0}", apiKey));
            Stream stream = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            JToken token = JToken.Parse(reader.ReadToEnd());
            return token["data"].Children().Select(o => o.ToObject<ImgurImage>()).ToList();
        }

        private static string ReadClientID()
        {

            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.ini")))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("[Settings]" + Environment.NewLine);
                sb.Append("ClientID=" + Environment.NewLine);

                using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.ini")))
                {
                    sw.WriteLine(sb.ToString());
                    sw.Close();
                }
                Console.WriteLine("configure ClientID in settings.ini inside application startup folder");
                Console.WriteLine("press any key to exit...");
                Console.Read();
                Environment.Exit(0);
            }

            else
            {
                IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.ini"));
                string clientID = ini.IniReadValue("Settings", "ClientID");
                if (string.IsNullOrEmpty(clientID))
                {
                    Console.WriteLine("values are incorrect. configure settings.ini");
                    Console.WriteLine("press any key to exit...");
                    Console.Read();
                    Environment.Exit(0);
                }

                else
                {
                    return clientID;
                }

            }

            return string.Empty;
        }
    }
}
