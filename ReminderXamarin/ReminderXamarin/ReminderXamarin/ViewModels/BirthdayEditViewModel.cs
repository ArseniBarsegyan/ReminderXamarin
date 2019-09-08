using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.ViewModels.Base;
using Rm.Data.Data.Entities;
using Rm.Helpers;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class BirthdayEditViewModel : BaseViewModel
    {
        private BirthdayModel _birthdayModel;

        public BirthdayEditViewModel()
        {
            ImageContent = new byte[0];
            Name = "New birthday";
            BirthDayDate = new DateTime();
            SaveBirthdayCommand = new Command(async () => await SaveBirthday());
            DeleteBirthdayCommand = new Command(async () => await DeleteBirthday());
        }

        public override Task InitializeAsync(object navigationData)
        {
            Id = (int) navigationData;

            if (Id != 0)
            {
                _birthdayModel = App.BirthdaysRepository.Value.GetBirthdayAsync(Id);
                ImageContent = _birthdayModel.ImageContent;
                Name = _birthdayModel.Name;
                BirthDayDate = _birthdayModel.BirthDayDate;
                GiftDescription = _birthdayModel.GiftDescription;
            }

            return base.InitializeAsync(navigationData);
        }

        public bool IsNewBirthday => Id == 0;

        public int Id { get; private set; }
        public byte[] ImageContent { get; set; }
        public string Name { get; set; }
        public string Title => Name + ", " + BirthDayDate.ToString("M");
        public DateTime BirthDayDate { get; set; }
        public string GiftDescription { get; set; }

        public ICommand SaveBirthdayCommand { get; set; }
        public ICommand DeleteBirthdayCommand { get; set; }

        private async Task SaveBirthday()
        {
            if (Id == 0)
            {
                var birthdayModel = new BirthdayModel
                {
                    ImageContent = ImageContent,
                    Name = Name,
                    BirthDayDate = BirthDayDate,
                    GiftDescription = GiftDescription,
                    UserId = Settings.CurrentUserId
                };
                App.BirthdaysRepository.Value.Save(birthdayModel);
            }
            else
            {
                _birthdayModel.BirthDayDate = BirthDayDate;
                _birthdayModel.GiftDescription = GiftDescription;
                _birthdayModel.ImageContent = ImageContent;
                _birthdayModel.Name = Name;
                _birthdayModel.UserId = Settings.CurrentUserId;
                App.BirthdaysRepository.Value.Save(_birthdayModel);
            }

            await NavigationService.NavigateBackAsync();
        }

        private async Task DeleteBirthday()
        {
            bool result = await Acr.UserDialogs.UserDialogs.Instance.ConfirmAsync(
                ConstantsHelper.BirthdaysDeleteMessage,
                ConstantsHelper.Warning, ConstantsHelper.Ok, ConstantsHelper.Cancel);

            if (result)
            {
                if (_birthdayModel != null)
                {
                    App.BirthdaysRepository.Value.DeleteBirthday(_birthdayModel);
                    await NavigationService.NavigateBackAsync();
                }
            }
        }
    }
}