using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class BirthdayViewModel : BaseViewModel
    {
        public BirthdayViewModel()
        {
            CreateBirthdayCommand = new Command(async() => await CreateBirthdayCommandExecute());
            UpdateBirthdayCommand = new Command(async() => await UpdateBirthdayCommandExecute());
            DeleteBirthdayCommand = new Command(async() => await DeleteBirthdayCommandExecute());
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

        private async Task CreateBirthdayCommandExecute()
        {
            await App.BirthdaysRepository.CreateAsync(this.ToBirthdayModel());
            await App.BirthdaysRepository.SaveAsync();
        }

        private async Task UpdateBirthdayCommandExecute()
        {
            App.BirthdaysRepository.Update(this.ToBirthdayModel());
            await App.BirthdaysRepository.SaveAsync();
        }

        private async Task DeleteBirthdayCommandExecute()
        {
            await App.BirthdaysRepository.DeleteAsync(this.Id);
            await App.BirthdaysRepository.SaveAsync();
            // App.BirthdaysRepository.DeleteBirthday(this.ToBirthdayModel());
        }
    }
}