using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using TagLib;

namespace sharpdj11
{
    class LoadMusic
    {
        string[] Formats = new string[] { ".mp3", ".wav", ".flac", ".aac", ".m4p",".m4a" };

        public List<Audio> GetMusic(string Path)
        {
            List<Audio> AudioPathList = new List<Audio>();
            try
            {
                DirectoryInfo dir = new DirectoryInfo(Path);

                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo f in files)
                {
                    if (Formats.Contains(f.Extension))
                    {
                        TagLib.File FD = TagLib.File.Create(f.FullName);
                        AudioPathList.Add(new Audio(f.Name, FD.Tag.Title, f.DirectoryName, FD.Tag.FirstArtist, FD.Tag.Album, FD.Properties.Duration));
                    }
                }


                return AudioPathList;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return AudioPathList;
            }
        }
    }
}