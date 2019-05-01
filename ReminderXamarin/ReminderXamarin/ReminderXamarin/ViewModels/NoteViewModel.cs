using System;
using System.Collections.ObjectModel;
using ReminderXamarin.ViewModels.Base;

namespace ReminderXamarin.ViewModels
{
    public class NoteViewModel : BaseViewModel
    {
        public NoteViewModel()
        {
            Photos = new ObservableCollection<PhotoViewModel>();
            Videos = new ObservableCollection<VideoViewModel>();
        }

        public PhotoViewModel SelectedPhoto { get; set; }
        public ObservableCollection<PhotoViewModel> Photos { get; set; }
        public ObservableCollection<VideoViewModel> Videos { get; set; }

        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
        public string FullDescription { get; set; }
        public bool IsLoading { get; set; }
    }
}