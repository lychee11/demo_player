using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.IO;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using TagLib;
using Microsoft.Win32;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace sharpdj11
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> Paths = new List<string>();
        public int CurrentIndex = 0;
        public bool Playing = false;
        private LoadMusic load_audios = new LoadMusic();
        public PlayList mainPL = new PlayList() { Name = "Main Playlist" };
        public List<Audio> CurrentList;
        private TimeSpan TotalTime;
        public MainWindow()
        {
            InitializeComponent();
            DeserializeData();
            CurrentList = mainPL.AudioList;
        }
        private void ms_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (RepeatButt.IsChecked == true)
            {
                ms.Position = TimeSpan.Zero;
                ms.Play();
            }
            else
            {
                if (CurrentIndex < CurrentList.Count - 1)
                    ChangeAudio(CurrentList[++CurrentIndex]);
            }
        }

        private void ChangeAudio(Audio audioContext)
        {
            ms.Source = new Uri(audioContext.DirectoryName + "\\" + audioContext.Name);

            Playing = true;
            ms.Play();

            CurrentIndex = CurrentList.IndexOf(audioContext);

            (PlayButton.Child as Image).Source = LoadImage(@"pack://application:,,,/Images/Pause.png", false);

            if (!System.IO.File.Exists(audioContext.DirectoryName + "\\" + audioContext.Name))
            {


                NewWindow dialog = new NewWindow();
                dialog.text.Text = "This file was deleted. Delete file information?";
                dialog.Owner = this;
                dialog.ShowDialog();
                if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
                {

                    mainPL.AudioList = mainPL.AudioList.Where(x => x.Name != audioContext.Name || x.DirectoryName != audioContext.DirectoryName).ToList();
                    CurrentList = CurrentList.Where(x => x.Name != audioContext.Name || x.DirectoryName != audioContext.DirectoryName).ToList();
                    Play.ItemsSource = CurrentList;
                    if (CurrentList.Count > 0)
                    {
                        ChangeAudio(CurrentList[--CurrentIndex]);
                    }
                    return;

                }
                else
                {
                    ChangeAudio(CurrentList[++CurrentIndex]);
                    return;
                }
            }

            TagLib.File Ds = TagLib.File.Create(audioContext.DirectoryName + "\\" + audioContext.Name);
            Info.DataContext = audioContext;

        }

        public BitmapImage LoadImage(string text, bool decode)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(text, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            if (decode)
            {
                bitmap.DecodePixelWidth = 200;
                bitmap.DecodePixelHeight = 200;
            }
            bitmap.EndInit();

            return bitmap;
        }
        private void Audio_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var audioContext = ((sender as Grid).DataContext as Audio);
            ChangeAudio(audioContext);
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            NewWindow dialog = new NewWindow();
            dialog.text.Text = "Would you like to save your main playlist?";
            dialog.Owner = this;
            dialog.ShowDialog();
            if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
            {
                SerializeData();
            }
        }

    }
}
