using MelodiousHaven.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;

namespace MelodiousHaven.ViewsModels
{
    public class MainWindow_VM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private MediaPlayer _mediaPlayer = new MediaPlayer();

        public ICommand AddFolderMusicCommand { get; }
        public ICommand PlayCommand { get; }
        public ICommand PauseCommand { get; }
        public ObservableCollection<MusicFile> MusicName { get; } = new ObservableCollection<MusicFile>();
        public ObservableCollection<MusicFile> FilteredMusicFiles { get; } = new ObservableCollection<MusicFile>();

        public MainWindow_VM()
        {
            AddFolderMusicCommand = new RelayCommand(AddFolderMusic);
            PlayCommand = new RelayCommand(Play);
            PauseCommand = new RelayCommand(Pause);
            FilterMusicFiles();
        }

        private void AddFolderMusic()
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

        private void AddToPlaylist(string filePath)
        {
            string fileName = Path.GetFileName(filePath);

            MusicFile musicFile = new MusicFile { Name = fileName, Path = filePath };
            MusicName.Add(musicFile);
            FilteredMusicFiles.Add(musicFile);
            OnPropertyChanged(nameof(FilteredMusicFiles)); // updated objects
        }

        private void Play()
        {
            if (_mediaPlayer.Source != null)
            {
                _mediaPlayer.Play();
            }
        }

        private void Pause()
        {
            if (_mediaPlayer.Source != null)
            {
                _mediaPlayer.Pause();
            }
        }

        private MusicFile _selectedMusicFile;
        public MusicFile SelectedMusicFile
        {
            get { return _selectedMusicFile; }
            set
            {
                _selectedMusicFile = value;
                OnPropertyChanged(nameof(SelectedMusicFile)); // updated objects
                if (value != null)
                {
                    _mediaPlayer.Open(new Uri(value.Path));
                    _mediaPlayer.Play();
                }
            }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText)); // updated objects
                FilterMusicFiles();
            }
        }

        private void FilterMusicFiles()
        {
            FilteredMusicFiles.Clear();

            if (string.IsNullOrEmpty(SearchText))
            {
                foreach (var musicFile in MusicName)
                {
                    FilteredMusicFiles.Add(musicFile);
                }
            }
            else
            {
                foreach (var musicFile in MusicName)
                {
                    if (musicFile.Name.ToLower().Contains(SearchText.ToLower()))
                    {
                        FilteredMusicFiles.Add(musicFile);
                    }
                }
            }
        }

    }
}