using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using PropertyChanged;
using ReminderXamarin.Collections;
using ReminderXamarin.Core.Interfaces;
using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;
using Rm.Data.Data.Entities;
using Rm.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.ViewModels
{
    [Preserve(AllMembers = true)]
    public class BirthdayEditViewModel : BaseNavigableViewModel
    {
        private readonly IFileSystem _fileService;
        private readonly IMediaService _mediaService;
        private readonly IPlatformDocumentPicker _documentPicker;
        private BirthdayModel _model;
        private int _birthdayId;

        public BirthdayEditViewModel(
            ICommandResolver commandResolver,
            INavigationService navigationService,
            IFileSystem fileService,
            IMediaService mediaService,
            IPlatformDocumentPicker documentPicker)
            : base(navigationService)
        {
            _fileService = fileService;
            _mediaService = mediaService;
            SaveBirthdayCommand = commandResolver.AsyncCommand(SaveBirthday);
            ChangePersonImageCommand = commandResolver.AsyncCommand(ChangePersonImage);
            SelectMonthCommand = commandResolver.Command<BirthDateViewModel>(SelectMonth);
            SelectDayCommand = commandResolver.Command<BirthdayDayViewModel>(SelectDay);
            _documentPicker = documentPicker;

            BirthDateViewModels = new RangeObservableCollection<BirthDateViewModel>();
            InitializeDays();
            SelectedBirthDateViewModel = BirthDateViewModels.ElementAt(0);
        }

        public RangeObservableCollection<BirthDateViewModel> BirthDateViewModels { get; }

        [AlsoNotifyFor(nameof(IsSaveEnabled))] 
        public BirthDateViewModel SelectedBirthDateViewModel { get; set; }
        public string Title { get; private set; }

        [AlsoNotifyFor(nameof(IsSaveEnabled))] 
        public string Name { get; set; }

        public bool IsSaveEnabled
        {
            get
            {
                if (_model == null)
                {
                    return !string.IsNullOrEmpty(Name)
                           && !string.IsNullOrEmpty(AdditionalInfo);
                }

                return (Name != _model.Name && !string.IsNullOrEmpty(Name))
                       || (AdditionalInfo != _model.GiftDescription && !string.IsNullOrEmpty(AdditionalInfo))
                       || ImageContent != _model.ImageContent
                       || (SelectedBirthDateViewModel != null &&
                           SelectedBirthDateViewModel.MonthNumber != _model.BirthDayDate.Month)
                       || (SelectedBirthDateViewModel != null &&
                           SelectedBirthDateViewModel.SelectedDay.Number != _model.BirthDayDate.Day);
            }
        }

        [AlsoNotifyFor(nameof(PersonImageSource), nameof(IsSaveEnabled))]
        public byte[] ImageContent { get; private set; } = new byte[0];

        public ImageSource PersonImageSource
        {
            get
            {
                if (ImageContent == null || ImageContent.Length == 0)
                {
                    return ImageSource.FromResource(
                        ConstantsHelper.NoPhotoImage);
                }

                return ImageSource.FromStream(
                    () => new MemoryStream(ImageContent));
            }
        }

        [AlsoNotifyFor(nameof(IsSaveEnabled))] public string AdditionalInfo { get; set; }
        public bool ViewModelChanged { get; private set; }

        public IAsyncCommand SaveBirthdayCommand { get; }
        public IAsyncCommand ChangePersonImageCommand { get; }
        public ICommand SelectMonthCommand { get; }
        public ICommand SelectDayCommand { get; }

        public override Task InitializeAsync(object navigationData)
        {
            _birthdayId = (int) navigationData;

            if (_birthdayId == 0)
            {
                Title = Resmgr.Value.GetString(ConstantsHelper.CreateBirthdayTitle, CultureInfo.CurrentCulture);
                SelectedBirthDateViewModel = BirthDateViewModels.FirstOrDefault();
            }
            else
            {
                _model = App.BirthdaysRepository.Value.GetBirthdayAsync(_birthdayId);
                Title = _model.Name;
                Name = _model.Name;
                ImageContent = _model.ImageContent;
                AdditionalInfo = _model.GiftDescription;
                SelectedBirthDateViewModel = BirthDateViewModels
                    .FirstOrDefault(x => x.MonthNumber == _model.BirthDayDate.Month);

                OnAppearing();
                SelectedBirthDateViewModel.SelectDay(_model.BirthDayDate.Day);
            }

            return base.InitializeAsync(navigationData);
        }

        public void OnAppearing()
        {
            SelectedBirthDateViewModel.PropertyChanged -= SelectedBirthDateViewModelOnPropertyChanged;
            SelectedBirthDateViewModel.PropertyChanged += SelectedBirthDateViewModelOnPropertyChanged;
        }

        public void OnDisappearing()
        {
            SelectedBirthDateViewModel.PropertyChanged -= SelectedBirthDateViewModelOnPropertyChanged;
        }

        private async Task ChangePersonImage()
        {
            var document = await _documentPicker.DisplayImportAsync();
             
            if (document == null)
            {
                return;
            }
            
            // Ensure that user downloads .png or .jpg file as profile icon.
            if (document.Name.EndsWith(".png") 
                || document.Name.EndsWith(".jpg") 
                || document.Name.EndsWith(".jpeg"))
            {
                try
                {
                    ViewModelChanged = true;
                    await Task.Run(() =>
                    {
                        var imageContent = _fileService.ReadAllBytes(document.Path);
                        var resizedImage = _mediaService.ResizeImage(
                            imageContent,
                            ConstantsHelper.ResizedImageWidth,
                            ConstantsHelper.ResizedImageHeight);

                        ImageContent = resizedImage;
                    }).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(ex.Message);
                }
            }
        }

        private async Task SaveBirthday()
        {
            if (_model == null)
                _model = new BirthdayModel();

            // We need to find latest leap year in order to have possibility to
            // pick 29 of Feb as someone's Birthday
            const int latestLeapYear = 2020;
            var dateTime = new DateTime(
                latestLeapYear,
                SelectedBirthDateViewModel.MonthNumber,
                SelectedBirthDateViewModel.SelectedDay.Number);
            
            _model.Name = Name;
            _model.GiftDescription = AdditionalInfo;
            _model.ImageContent = ImageContent;
            _model.BirthDayDate = dateTime;
            _model.UserId = Settings.CurrentUserId;
            
            App.BirthdaysRepository?.Value.Save(_model);
            await NavigationService.NavigateBackAsync();
        }

        private void InitializeDays()
        {
            var monthsDays = new List<BirthDateViewModel>
            {
                new BirthDateViewModel { MonthName = "January", MonthNumber = 1, DaysInCurrentMonth = 31 },
                new BirthDateViewModel { MonthName = "February", MonthNumber = 2, DaysInCurrentMonth = 29 },
                new BirthDateViewModel { MonthName = "March", MonthNumber = 3, DaysInCurrentMonth = 31 },
                new BirthDateViewModel { MonthName = "April", MonthNumber = 4, DaysInCurrentMonth = 30 },
                new BirthDateViewModel { MonthName = "May", MonthNumber = 5, DaysInCurrentMonth = 31 },
                new BirthDateViewModel { MonthName = "June", MonthNumber = 6, DaysInCurrentMonth = 30 },
                new BirthDateViewModel { MonthName = "July", MonthNumber = 7, DaysInCurrentMonth = 31 },
                new BirthDateViewModel { MonthName = "August", MonthNumber = 8, DaysInCurrentMonth = 31 },
                new BirthDateViewModel { MonthName = "September", MonthNumber = 9, DaysInCurrentMonth = 30 },
                new BirthDateViewModel { MonthName = "October", MonthNumber = 10, DaysInCurrentMonth = 31 },
                new BirthDateViewModel { MonthName = "November", MonthNumber = 11, DaysInCurrentMonth = 30 },
                new BirthDateViewModel { MonthName = "December", MonthNumber = 12, DaysInCurrentMonth = 31 }
            };

            BirthDateViewModels.ReplaceRangeWithoutUpdating(monthsDays);
            BirthDateViewModels.RaiseCollectionChanged();
        }

        private void SelectMonth(BirthDateViewModel viewModel)
        {
            SelectedBirthDateViewModel.PropertyChanged -= SelectedBirthDateViewModelOnPropertyChanged;
            SelectedBirthDateViewModel = viewModel;
            SelectedBirthDateViewModel.PropertyChanged += SelectedBirthDateViewModelOnPropertyChanged;
        }

        private void SelectDay(BirthdayDayViewModel dayViewModel)
        {
            if (dayViewModel != null)
            {
                SelectedBirthDateViewModel.SelectDay(dayViewModel.Number);
            }
        }
        
        private void SelectedBirthDateViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BirthDateViewModel.SelectedDay))
            {
                OnPropertyChanged(nameof(IsSaveEnabled));
            }
        }
    }
}