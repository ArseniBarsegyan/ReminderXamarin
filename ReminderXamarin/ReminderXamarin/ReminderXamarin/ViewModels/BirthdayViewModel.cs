using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels.Base;
using RI.Data.Data.Entities;
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

        public string Id { get; set; }
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
            var model = new BirthdayModel
            {
                Id = Guid.NewGuid().ToString(),
                UserId = Settings.CurrentUserId,
                BirthDayDate = BirthDayDate,
                GiftDescription = GiftDescription,
                ImageContent = ImageContent,
                Name = Name
            };
            App.BirthdaysRepository.Create(model);
        }

        private async Task UpdateBirthdayCommandExecute()
        {
            var birthdayModel = App.BirthdaysRepository.GetById(Id);

            App.BirthdaysRepository.RealmInstance.Write(() =>
            {
                birthdayModel.UserId = Settings.CurrentUserId;
                birthdayModel.Name = Name;
                birthdayModel.ImageContent = ImageContent;
                birthdayModel.BirthDayDate = BirthDayDate;
                birthdayModel.GiftDescription = GiftDescription;
            });
        }

        private async Task DeleteBirthdayCommandExecute()
        {
            App.BirthdaysRepository.Delete(Id);
        }
    }
}