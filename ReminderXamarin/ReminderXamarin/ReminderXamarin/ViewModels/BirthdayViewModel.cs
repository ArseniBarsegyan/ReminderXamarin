using System;
using System.Windows.Input;
using ReminderXamarin.ViewModels.Base;
using ReminderXamarin.Extensions;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class BirthdayViewModel : BaseViewModel
    {
        public BirthdayViewModel()
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

        public ICommand CreateBirthdayCommand { get; set; }
        public ICommand UpdateBirthdayCommand { get; set; }
        public ICommand DeleteBirthdayCommand { get; set; }

        private void CreateBirthday()
        {
            App.BirthdaysRepository.Save(this.ToBirthdayModel());
        }

        private void UpdateBirthday()
        {
            App.BirthdaysRepository.Save(this.ToBirthdayModel());
        }

        private void DeleteBirthday()
        {
            App.BirthdaysRepository.DeleteBirthday(this.ToBirthdayModel());
        }
    }
}