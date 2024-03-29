﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpdj11
{
    [Serializable]
    public class Audio
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string DirectoryName { get; set; }
        public string Singer { get; set; }
        public string Album { get; set; }
        public TimeSpan Duration { get; set; }

        public Audio(string Name, string Title, string DirectoryName, string Singer, string Album, TimeSpan Duration)
        {
            this.Name = Name;
            this.Title = Title;
            this.DirectoryName = DirectoryName;
            this.Singer = Singer;
            this.Album = Album;
            this.Duration = Duration;

            if (String.IsNullOrEmpty(this.Album))
            {
                this.Album = "Unknown album detected";
            }

            if (String.IsNullOrEmpty(this.Singer))
            {
                this.Singer = "Unknown artist detected";
            }
            if (String.IsNullOrEmpty(this.Title))
            {
                this.Title = "Unknown title detected";
            }
        }
    }
}