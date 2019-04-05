using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ReminderXamarin.Extensions;
using ReminderXamarin.Helpers;
using ReminderXamarin.Services;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class AchievementViewModel : BaseViewModel
    {
        private static readonly IFileSystem FileSystem = DependencyService.Get<IFileSystem>();
        private static readonly IMediaService MediaService = DependencyService.Get<IMediaService>();

        public AchievementViewModel()
        {
            AchievementNotes = new ObservableCollection<AchievementNoteViewModel>();

            NavigateToEditAchievementCommand = new Command(async () => await NavigateToEditAchievement());
            SetImageCommand = new Command<PlatformDocument>(SetImageCommandExecute);
            RefreshListCommand = new Command(async () => await RefreshCommandExecute());
            CreateAchievementCommand = new Command(async () => await CreateAchievementCommandExecute());
            CreateAchievementNoteCommand = new Command<AchievementNoteViewModel>(async (viewModel) => await CreateAchievementNoteCommandExecute(viewModel));
            UpdateAchievementCommand = new Command(async () => await UpdateAchievementCommandExecute());
            UpdateAchievementNoteCommand = new Command<AchievementNoteViewModel>(async (viewModel) => await UpdateAchievementNoteCommandExecute(viewModel));
            DeleteAchievementCommand = new Command(async viewModel => await DeleteAchievementCommandExecute());
            DeleteAchievementNoteCommand = new Command<AchievementNoteViewModel>(async(viewModel) => await DeleteAchievementNoteCommandExecute(viewModel));
        }

        public string FileName { get; set; }
        public bool IsFileNameLabelVisible { get; set; }
        public bool IsImageVisible { get; set; }
        public bool IsRefreshing { get; set; }
        public int Id { get; set; }
        public byte[] ImageContent { get; set; }
        public string Title { get; set; }
        public string GeneralDescription { get; set; }
        public int GeneralTimeSpent { get; set; }
        public ObservableCollection<AchievementNoteViewModel> AchievementNotes { get; set; }

        public ICommand NavigateToEditAchievementCommand { get; set; }
        public ICommand SetImageCommand { get; set; }
        public ICommand RefreshListCommand { get; set; }
        public ICommand CreateAchievementCommand { get; set; }
        public ICommand CreateAchievementNoteCommand { get; set; }
        public ICommand UpdateAchievementCommand { get; set; }
        public ICommand UpdateAchievementNoteCommand { get; set; }
        public ICommand DeleteAchievementCommand { get; set; }
        public ICommand DeleteAchievementNoteCommand { get; set; }

        private async Task NavigateToEditAchievement()
        {
            await NavigationService.NavigateToAsync<AchievementDetailsViewModel>();
        }

        private async void SetImageCommandExecute(PlatformDocument document)
        {
            // Retrieve file content through IFileService implementation.
            FileName = document.Name;
            IsFileNameLabelVisible = true;
            try
            {
                var imageContent = FileSystem.ReadAllBytes(document.Path);
                ImageContent = MediaService.ResizeImage(imageContent, ConstantsHelper.AchievementImageWidth,
                    ConstantsHelper.AchievementImageHeight);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message);
            }
            IsImageVisible = true;
        }

        public async Task OnAppearing()
        {
            await LoadAchievementNotesFromDataBase();
        }

        private async Task RefreshCommandExecute()
        {
            IsRefreshing = true;
            await LoadAchievementNotesFromDataBase();
            IsRefreshing = false;
        }

        private async Task CreateAchievementCommandExecute()
        {
            App.AchievementRepository.Save(this.ToAchievementModel());
        }

        private async Task CreateAchievementNoteCommandExecute(AchievementNoteViewModel achievementNoteViewModel)
        {
            if (!AchievementNotes.Contains(achievementNoteViewModel))
            {
                AchievementNotes.Add(achievementNoteViewModel);
            }
            await UpdateAchievementCommandExecute();
        }

        private async Task UpdateAchievementCommandExecute()
        {
            GeneralTimeSpent = AchievementNotes.Sum(x => x.HoursSpent);
            App.AchievementRepository.Save(this.ToAchievementModel());
        }

        // Insert updated achievement note instead of old.
        private async Task UpdateAchievementNoteCommandExecute(AchievementNoteViewModel achievementNoteViewModel)
        {
            var oldNote = AchievementNotes.FirstOrDefault(x => x.Id == achievementNoteViewModel.Id);
            if (oldNote != null)
            {
                oldNote.AchievementId = Id;
                oldNote.Date = achievementNoteViewModel.Date;
                oldNote.Description = achievementNoteViewModel.Description;
                oldNote.HoursSpent = achievementNoteViewModel.HoursSpent;
            }

            await UpdateAchievementCommandExecute();
        }

        private async Task DeleteAchievementCommandExecute()
        {
            App.AchievementRepository.DeleteAchievement(this.ToAchievementModel());
        }

        private async Task DeleteAchievementNoteCommandExecute(AchievementNoteViewModel noteViewModel)
        {
            if (AchievementNotes.Contains(noteViewModel))
            {
                AchievementNotes.Remove(noteViewModel);
            }
            await UpdateAchievementCommandExecute();
        }

        private async Task LoadAchievementNotesFromDataBase()
        {
            // Fetch all note models from database, order by recent date, then by recent upload.
            AchievementNotes = App.AchievementRepository.GetAchievementAsync(Id)
                .AchievementNotes
                .OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.Id)
                .ToAchievementNoteViewModels()
                .ToObservableCollection();
        }
    }
}