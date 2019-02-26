using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using ReminderXamarin.Helpers;
using ReminderXamarin.Interfaces;
using ReminderXamarin.Interfaces.FilePickerService;
using Rm.Data.Entities;
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
        public Guid Id { get; set; }
        public byte[] ImageContent { get; set; }
        public string Title { get; set; }
        public string GeneralDescription { get; set; }
        public int GeneralTimeSpent { get; set; }
        public ObservableCollection<AchievementNoteViewModel> AchievementNotes { get; set; }

        public ICommand SetImageCommand { get; set; }
        public ICommand RefreshListCommand { get; set; }
        public ICommand CreateAchievementCommand { get; set; }
        public ICommand CreateAchievementNoteCommand { get; set; }
        public ICommand UpdateAchievementCommand { get; set; }
        public ICommand UpdateAchievementNoteCommand { get; set; }
        public ICommand DeleteAchievementCommand { get; set; }
        public ICommand DeleteAchievementNoteCommand { get; set; }

        private void SetImageCommandExecute(PlatformDocument document)
        {
            // Retrieve file content through IFileService implementation.
            FileName = document.Name;
            IsFileNameLabelVisible = true;

            var imageContent = FileSystem.ReadAllBytes(document.Path);
            ImageContent = MediaService.ResizeImage(imageContent, ConstantsHelper.AchievementImageWidth, ConstantsHelper.AchievementImageHeight);
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

        // TODO: refactor this
        private async Task CreateAchievementCommandExecute()
        {
            var achievement = new AchievementModel
            {
                GeneralDescription = GeneralDescription,
                GeneralTimeSpent = GeneralTimeSpent,
                ImageContent = ImageContent,
                Title = Title,
                AchievementNotes = new List<AchievementNote>(),
                UserId = Settings.CurrentUserId
            };

            foreach (var achievementNoteViewModel in AchievementNotes)
            {
                var model = new AchievementNote
                {
                    AchievementId = achievementNoteViewModel.AchievementId,
                    Date = achievementNoteViewModel.Date,
                    Description = achievementNoteViewModel.Description,
                    HoursSpent = achievementNoteViewModel.HoursSpent
                };
                achievement.AchievementNotes.Add(model);
            }
            await App.AchievementRepository.CreateAsync(achievement);
            await App.AchievementRepository.SaveAsync();
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
            var entity = await App.AchievementRepository.GetByIdAsync(Id);

            entity.UserId = Settings.CurrentUserId;
            entity.Title = Title;
            entity.GeneralDescription = GeneralDescription;
            entity.GeneralTimeSpent = GeneralTimeSpent;
            entity.ImageContent = ImageContent;
            entity.AchievementNotes = new List<AchievementNote>();

            foreach (var viewModel in AchievementNotes)
            {
                var model = new AchievementNote
                {
                    AchievementId = viewModel.AchievementId,
                    Date = viewModel.Date,
                    Description = viewModel.Description,
                    HoursSpent = viewModel.HoursSpent
                };
                entity.AchievementNotes.Add(model);
            }

            App.AchievementRepository.Update(entity);
            await App.AchievementRepository.SaveAsync();
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
            await App.AchievementRepository.DeleteAsync(Id);
            await App.AchievementRepository.SaveAsync();
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
            AchievementNotes = (await App.AchievementRepository.GetByIdAsync(Id))
                .AchievementNotes
                .OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.Id)
                .ToAchievementNoteViewModels()
                .ToObservableCollection();
        }
    }
}