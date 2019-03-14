using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels.Base;
using ReminderXamarin.Data.Entities;
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

        public Guid Id { get; set; }
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
                UserId = Settings.CurrentUserId,
                BirthDayDate = BirthDayDate,
                GiftDescription = GiftDescription,
                ImageContent = ImageContent,
                Name = Name
            };
            await BirthdaysRepository.Value.CreateAsync(model);
            await BirthdaysRepository.Value.SaveAsync();
        }

        private async Task UpdateBirthdayCommandExecute()
        {
            var model = await BirthdaysRepository.Value.GetByIdAsync(Id);
            model.UserId = Settings.CurrentUserId;
            model.Name = Name;
            model.ImageContent = ImageContent;
            model.BirthDayDate = BirthDayDate;
            model.GiftDescription = GiftDescription;

            BirthdaysRepository.Value.Update(model);
            await BirthdaysRepository.Value.SaveAsync();
        }

        private async Task DeleteBirthdayCommandExecute()
        {
            await BirthdaysRepository.Value.DeleteAsync(Id);
            await BirthdaysRepository.Value.SaveAsync();
        }
    }
}