using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
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

            foreach (var directory in Directory.GetDirectories(path)) // recursively for the subfolder
            {
                FindMusicFiles(directory);
            }
        }

        private List<string> musicFiles = new List<string>();


        private void AddToPlaylist(string filePath)
        {
            string fileName = Path.GetFileName(filePath);

            myListMusic.Items.Add(fileName);
            musicFiles.Add(filePath);
        }

        private void ListMusic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = myListMusic.SelectedIndex;

            string selectedFile = musicFiles[selectedIndex];

            mediaPlayer.Open(new Uri(selectedFile));

            mediaPlayer.Play();
        }

    }
}