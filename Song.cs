using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playlist_circular_con_prev_next
{
    public class Song
    {
        public Guid Id { get; }
        public string Title { get; }
        public string Artist { get; }
        public int DurationInSeconds { get; }

        public Song(Guid id, string title, string artist, int durationInSeconds)
        {
            Id = id;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Artist = artist ?? throw new ArgumentNullException(nameof(artist));
            DurationInSeconds = durationInSeconds;
        }

        public Song(string title, string artist, int durationInSeconds)
            : this(Guid.NewGuid(), title, artist, durationInSeconds)
        { }

        public override string ToString()
            => $"{Title} — {Artist} ({DurationInSeconds}s) [{Id}]";
    }
}