﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels.Base;
using Rm.Data.Entities;
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
            await App.BirthdaysRepository.CreateAsync(model);
            await App.BirthdaysRepository.SaveAsync();
        }

        private async Task UpdateBirthdayCommandExecute()
        {
            var model = await App.BirthdaysRepository.GetByIdAsync(Id);
            model.UserId = Settings.CurrentUserId;
            model.Name = Name;
            model.ImageContent = ImageContent;
            model.BirthDayDate = BirthDayDate;
            model.GiftDescription = GiftDescription;

            App.BirthdaysRepository.Update(model);
            await App.BirthdaysRepository.SaveAsync();
        }

        private async Task DeleteBirthdayCommandExecute()
        {
            await App.BirthdaysRepository.DeleteAsync(Id);
            await App.BirthdaysRepository.SaveAsync();
        }
    }
}