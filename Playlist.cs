using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Playlist_circular_con_prev_next
{
    public class Playlist : IEnumerable<Song>
    {
        private class Node
        {
            public Song Song { get; }
            public Node? Next { get; set; }
            public Node? Prev { get; set; }

            public Node(Song song) => Song = song ?? throw new ArgumentNullException(nameof(song));
        }

        private Node? head; 
        private Node? cursor;    
        private int count;

        public int Count => count;

        public bool IsEmpty => count == 0;

        public void AddLast(Song s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));

            var node = new Node(s);

            if (head == null)
            {
                node.Next = node;
                node.Prev = node;
                head = node;
                cursor = node;
            }
            else
            {
                var tail = head.Prev!; 

                tail.Next = node;
                node.Prev = tail;
                node.Next = head;
                head.Prev = node;
            }

            count++;
        }

        public bool RemoveById(Guid id)
        {
            if (head == null) return false;

            var curr = head;
            for (int i = 0; i < count; i++)
            {
                if (curr!.Song.Id == id)
                {
                    
                    if (count == 1)
                    {
                        head = null;
                        cursor = null;
                        count = 0;
                        return true;
                    }

                    
                    curr.Prev!.Next = curr.Next;
                    curr.Next!.Prev = curr.Prev;

                    
                    if (head == curr) head = curr.Next;

                    
                    if (cursor == curr) cursor = curr.Next;

                    count--;
                    return true;
                }

                curr = curr.Next;
            }

            return false;
        }

        public Song Next()
        {
            if (cursor == null) throw new InvalidOperationException("Playlist vacía.");
            cursor = cursor.Next!;
            return cursor.Song;
        }

        public Song Prev()
        {
            if (cursor == null) throw new InvalidOperationException("Playlist vacía.");
            cursor = cursor.Prev!;
            return cursor.Song;
        }


        public void Shuffle(int seed)
        {
            if (count < 2 || head == null) return;

            var nodes = new List<Node>(count);
            var n = head;
            for (int i = 0; i < count; i++)
            {
                nodes.Add(n!);
                n = n!.Next!;
            }

            var rng = new Random(seed);
            for (int i = nodes.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                var tmp = nodes[i];
                nodes[i] = nodes[j];
                nodes[j] = tmp;
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                var cur = nodes[i];
                var next = nodes[(i + 1) % nodes.Count];
                var prev = nodes[(i - 1 + nodes.Count) % nodes.Count];

                cur.Next = next;
                cur.Prev = prev;
            }

            head = nodes[0];
        }

        public string ExportTitlesJson(bool indented = true)
        {
            var titles = GetTitles();
            var opts = new JsonSerializerOptions { WriteIndented = indented };
            return JsonSerializer.Serialize(titles, opts);
        }

        public List<string> GetTitles()
        {
            var result = new List<string>(count);
            if (head == null) return result;

            var curr = head;
            for (int i = 0; i < count; i++)
            {
                result.Add(curr!.Song.Title);
                curr = curr.Next!;
            }

            return result;
        }

        public Song Current()
        {
            if (cursor == null) throw new InvalidOperationException("Playlist vacía.");
            return cursor.Song;
        }

        public IEnumerator<Song> GetEnumerator()
        {
            if (head == null) yield break;
            var n = head;
            for (int i = 0; i < count; i++)
            {
                yield return n!.Song;
                n = n!.Next!;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}