using System;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class BirthdayViewModel : BaseViewModel
    {
        public BirthdayViewModel()
        {
            CreateBirthdayCommand = new Command(CreateBirthdayCommandExecute);
            UpdateBirthdayCommand = new Command(UpdateBirthdayCommandExecute);
            DeleteBirthdayCommand = new Command(DeleteBirthdayCommandExecute);
        }

        public int Id { get; set; }
        public byte[] ImageContent { get; set; }
        public string Name { get; set; }
        public DateTime BirthDayDate { get; set; }
        public string Title => Name + ", " + BirthDayDate.ToString("dd.MM.yy");
        public string GiftDescription { get; set; }

        public ICommand CreateBirthdayCommand { get; set; }
        public ICommand UpdateBirthdayCommand { get; set; }
        public ICommand DeleteBirthdayCommand { get; set; }

        private async void CreateBirthdayCommandExecute()
        {
            await App.BirthdaysRepository.CreateAsync(this.ToBirthdayModel());
            await App.AchievementRepository.SaveAsync();
        }

        private async void UpdateBirthdayCommandExecute()
        {
            App.BirthdaysRepository.Update(this.ToBirthdayModel());
            await App.AchievementRepository.SaveAsync();
        }

        private async void DeleteBirthdayCommandExecute()
        {
            await App.BirthdaysRepository.DeleteAsync(Id);
            await App.AchievementRepository.SaveAsync();
        }
    }
}