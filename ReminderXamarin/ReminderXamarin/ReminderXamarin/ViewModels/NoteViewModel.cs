using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using ReminderXamarin.Helpers;
using ReminderXamarin.Models;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class NoteViewModel : BaseViewModel
    {
        private readonly MediaHelper _mediaHelper;

        public NoteViewModel()
        {
            _mediaHelper = new MediaHelper();
            Photos = new ObservableCollection<PhotoViewModel>();
            Videos = new ObservableCollection<VideoModel>();

            TakePhotoCommand = new Command(async () => await TakePhotoCommandExecute());
            TakeVideoCommand = new Command(async () => await TakeVideoCommandExecute());
            CreateNoteCommand = new Command<NoteViewModel>(CreateNoteCommandExecute);
            UpdateNoteCommand = new Command<NoteViewModel>(UpdateNoteCommandExecute);
            DeleteNoteCommand = new Command<NoteViewModel>(note => DeleteNoteCommandExecute(note));
        }

        public PhotoViewModel SelectedPhoto { get; set; }
        public ObservableCollection<PhotoViewModel> Photos { get; set; }
        public ObservableCollection<VideoModel> Videos { get; set; }

        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
        public string FullDescription { get; set; }
        
        public ICommand TakePhotoCommand { get; set; }
        public ICommand TakeVideoCommand { get; set; }
        public ICommand CreateNoteCommand { get; set; }
        public ICommand UpdateNoteCommand { get; set; }
        public ICommand DeleteNoteCommand { get; set; }

        /// <summary>
        /// Invokes when photo added to Photos.
        /// </summary>
        public event EventHandler PhotoAdded;

        private async Task TakePhotoCommandExecute()
        {
            var photoModel = await _mediaHelper.TakePhotoAsync();
            if (photoModel != null)
            {
                Photos.Add(photoModel.ToPhotoViewModel());
                PhotoAdded?.Invoke(this, EventArgs.Empty);
            }
        }

        //TODO: implement video player
        private async Task TakeVideoCommandExecute()
        {
            var videoModel = await _mediaHelper.TakeVideoAsync();
            if (videoModel != null)
            {
                Videos.Add(videoModel);
            }
        }

        private void CreateNoteCommandExecute(NoteViewModel viewModel)
        {
            App.NoteRepository.Save(viewModel.ToNoteModel());
        }

        private void UpdateNoteCommandExecute(NoteViewModel viewModel)
        {
            // Update edit date since user pressed confirm
            viewModel.EditDate = DateTime.Now;
            App.NoteRepository.Save(viewModel.ToNoteModel());
        }

        private int DeleteNoteCommandExecute(NoteViewModel viewModel)
        {
            return App.NoteRepository.DeleteNote(viewModel.ToNoteModel());
        }
    }
}