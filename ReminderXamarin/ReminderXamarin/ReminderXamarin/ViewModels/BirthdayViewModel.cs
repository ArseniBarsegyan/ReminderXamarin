using System;
using System.Windows.Input;

using ReminderXamarin.Extensions;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class BirthdayViewModel : BaseViewModel
    {
        public BirthdayViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            CreateBirthdayCommand = new Command(CreateBirthday);
            UpdateBirthdayCommand = new Command(UpdateBirthday);
            DeleteBirthdayCommand = new Command(DeleteBirthday);
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