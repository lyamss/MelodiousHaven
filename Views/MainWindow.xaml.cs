using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MelodiousHaven
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonRecoverFolderMusic(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selectedPath = dialog.SelectedPath;
                FindMusicFiles(selectedPath);
            }
        }

        private void FindMusicFiles(string path)
        {
            foreach (var file in Directory.GetFiles(path))
            {
                string extension = Path.GetExtension(file).ToLower();
                if (extension == ".mp3" || extension == ".wav" || extension == ".flac")
                {
                    AddToPlaylist(file);
                }
            }

            foreach (var directory in Directory.GetDirectories(path))
            {
                FindMusicFiles(directory); // recursively for the subfolder
            }
        }

        private List<string> musicFiles = new List<string>();
        private List<string> filteredMusicFiles = new List<string>();


        private void AddToPlaylist(string filePath)
        {
            string fileName = Path.GetFileName(filePath);

            myListMusic.Items.Add(fileName);
            musicFiles.Add(filePath);
            filteredMusicFiles = new List<string>(musicFiles);
        }

        private void ListMusic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = myListMusic.SelectedIndex;

            if (selectedIndex >= 0 && selectedIndex < filteredMusicFiles.Count)
            {
                string selectedFile = filteredMusicFiles[selectedIndex];

                mediaPlayer.Open(new Uri(selectedFile));

                mediaPlayer.Play();
            }
        }

        private void SearchMusicInList(object sender, TextChangedEventArgs e)
        {
            string searchText = searchTextBox.Text.ToLower();

            filteredMusicFiles.Clear();
            myListMusic.Items.Clear();

            foreach (string musicFile in musicFiles)
            {
                if (Path.GetFileName(musicFile).ToLower().Contains(searchText))
                {
                    myListMusic.Items.Add(Path.GetFileName(musicFile));
                    filteredMusicFiles.Add(musicFile);
                }
            }
        }

    }
}