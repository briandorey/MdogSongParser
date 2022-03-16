using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MdogSongParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string CurrentSongID = "";

            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(path).ToString() + "\\nowplaying.txt";
            Console.WriteLine(directory);
            while (true)
            {

                using (WebClient wc = new WebClient())
                {
                    var json = wc.DownloadString("https://radio.mdogradio.com/api/nowplaying_static/mdog_radio.json");
                    //JObject o = JObject.Parse(json);
                    var message = JsonConvert.DeserializeObject<Root>(json);

                    StringBuilder builder = new StringBuilder();


                    builder.Append("<audio ID=\"" + message.now_playing.song.id + "\">" + Environment.NewLine);
                    builder.Append("<type>Song</type>" + Environment.NewLine);
                    builder.Append("<status>Playing</status>" + Environment.NewLine);
                    builder.Append("<artist>" + message.now_playing.song.artist + "</artist>" + Environment.NewLine);
                    builder.Append("<title>" + message.now_playing.song.title + "</title>" + Environment.NewLine);
                    builder.Append("<duration>" + message.now_playing.duration + "</title>" + Environment.NewLine);
                    builder.Append("<text>" + message.now_playing.song.text + "</text>" + Environment.NewLine);
                    builder.Append("<genre>" + message.now_playing.song.genre + "</genre>" + Environment.NewLine);
                    builder.Append("<art>" + message.now_playing.song.art + "</art>" + Environment.NewLine);
                    builder.Append("</audio>" + Environment.NewLine);

                    Console.Write(builder.ToString());
                    if (CurrentSongID != message.now_playing.song.id.ToString())
                    {
                        // save to text file
                        try
                        {
                            System.IO.File.WriteAllText(directory, builder.ToString());
                        } catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }

                    CurrentSongID = message.now_playing.song.id.ToString();
                }

                System.Threading.Thread.Sleep(20 * 1000);
            }
            
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Listeners
    {
        public int total { get; set; }
        public int unique { get; set; }
        public int current { get; set; }
    }

    public class Remote
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public int bitrate { get; set; }
        public string format { get; set; }
        public Listeners listeners { get; set; }
    }

    public class Station
    {
        public int id { get; set; }
        public string name { get; set; }
        public string shortcode { get; set; }
        public string description { get; set; }
        public string frontend { get; set; }
        public string backend { get; set; }
        public string listen_url { get; set; }
        public string url { get; set; }
        public string public_player_url { get; set; }
        public string playlist_pls_url { get; set; }
        public string playlist_m3u_url { get; set; }
        public bool is_public { get; set; }
        public List<object> mounts { get; set; }
        public List<Remote> remotes { get; set; }
    }

    public class Live
    {
        public bool is_live { get; set; }
        public string streamer_name { get; set; }
        public object broadcast_start { get; set; }
    }

    public class Song
    {
        public string id { get; set; }
        public string text { get; set; }
        public string artist { get; set; }
        public string title { get; set; }
        public string album { get; set; }
        public string genre { get; set; }
        public string lyrics { get; set; }
        public string art { get; set; }
        public List<object> custom_fields { get; set; }
    }

    public class NowPlaying
    {
        public int sh_id { get; set; }
        public int played_at { get; set; }
        public int duration { get; set; }
        public string playlist { get; set; }
        public string streamer { get; set; }
        public bool is_request { get; set; }
        public Song song { get; set; }
        public int elapsed { get; set; }
        public int remaining { get; set; }
    }

    public class PlayingNext
    {
        public int cued_at { get; set; }
        public int played_at { get; set; }
        public int duration { get; set; }
        public string playlist { get; set; }
        public bool is_request { get; set; }
        public Song song { get; set; }
    }

    public class Root
    {
        public Station station { get; set; }
        public Listeners listeners { get; set; }
        public Live live { get; set; }
        public NowPlaying now_playing { get; set; }
        public PlayingNext playing_next { get; set; }
        public List<object> song_history { get; set; }
        public bool is_online { get; set; }
        public string cache { get; set; }
    }


}
