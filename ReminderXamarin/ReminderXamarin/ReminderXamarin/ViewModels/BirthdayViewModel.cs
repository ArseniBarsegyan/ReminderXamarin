using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Extensions;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using System;
using System.Windows.Input;

namespace ReminderXamarin.ViewModels
{
    public class BirthdayViewModel : BaseViewModel
    {
        public BirthdayViewModel(INavigationService navigationService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            CreateBirthdayCommand = commandResolver.Command(CreateBirthday);
            UpdateBirthdayCommand = commandResolver.Command(UpdateBirthday);
            DeleteBirthdayCommand = commandResolver.Command(DeleteBirthday);
        }

        public int Id { get; set; }
        public byte[] ImageContent { get; set; }
        public string Name { get; set; }
        public DateTime BirthDayDate { get; set; }
        public string Title => Name + ", " + BirthDayDate.ToString("dd.MM.yy");
        public string GiftDescription { get; set; }

        public ICommand CreateBirthdayCommand { get; }
        public ICommand UpdateBirthdayCommand { get; }
        public ICommand DeleteBirthdayCommand { get; }

        private void CreateBirthday()
        {
            App.BirthdaysRepository.Value.Save(this.ToBirthdayModel());
        }

        private void UpdateBirthday()
        {
            App.BirthdaysRepository.Value.Save(this.ToBirthdayModel());
        }

        private void DeleteBirthday()
        {
            App.BirthdaysRepository.Value.DeleteBirthday(this.ToBirthdayModel());
        }
    }
}