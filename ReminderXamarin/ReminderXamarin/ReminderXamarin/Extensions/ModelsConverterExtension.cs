using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Enums;
using ReminderXamarin.ViewModels;

using Rm.Data.Data.Entities;
using Rm.Helpers;

namespace ReminderXamarin.Extensions
{
    public static class ModelsConverterExtension
    {
        public static ToDoModel ToToDoModel(this ToDoViewModel viewModel)
        {
            return new ToDoModel
            {
                Id = viewModel.Id,
                UserId = Settings.CurrentUserId,
                Status = viewModel.Status.ToString(),
                WhenHappens = viewModel.WhenHappens,
                Description = viewModel.Description
            };
        }

        public static ToDoViewModel ToToDoViewModel(this ToDoModel model,
            ICommandResolver commandResolver)
        {
            var viewModel = new ToDoViewModel(commandResolver)
            {
                Id = model.Id,
                WhenHappens = model.WhenHappens,
                Description = model.Description
            };

            switch (model.Status)
            {
                case ConstantsHelper.Active:
                    viewModel.Status = ToDoStatus.Active;
                    break;
                case ConstantsHelper.Completed:
                    viewModel.Status = ToDoStatus.Completed;
                    break;
                default:
                    viewModel.Status = ToDoStatus.Active;
                    break;
            }
            return viewModel;
        }

        public static ObservableCollection<ToDoViewModel> ToToDoViewModels(this IEnumerable<ToDoModel> models,
            ICommandResolver commandResolver)
        {
            var viewModels = new ObservableCollection<ToDoViewModel>();

            foreach (var model in models)
            {
                var viewModel = new ToDoViewModel(commandResolver)
                {
                    Id = model.Id,
                    WhenHappens = model.WhenHappens,
                    Description = model.Description
                };
                switch (model.Status)
                {
                    case ConstantsHelper.Active:
                        viewModel.Status = ToDoStatus.Active;
                        break;
                    case ConstantsHelper.Completed:
                        viewModel.Status = ToDoStatus.Completed;
                        break;
                    default:
                        viewModel.Status = ToDoStatus.Active;
                        break;
                }
                viewModels.Add(viewModel);
            }
            return viewModels;
        }

        public static AchievementViewModel ToAchievementViewModel(this AchievementModel model)
        {
            return new AchievementViewModel
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                GeneralTimeSpent = model.GeneralTimeSpent
            };
        }

        public static ObservableCollection<AchievementViewModel> ToAchievementViewModels(
            this IEnumerable<AchievementModel> models)
        {
            return models.Select(model => model.ToAchievementViewModel()).ToObservableCollection();
        }

        public static AchievementStepViewModel ToAchievementStepViewModel(this AchievementStep model)
        {
            return new AchievementStepViewModel
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                AchievedDate = model.AchievedDate,
                AchievementId = model.AchievementId,
                TimeSpent = model.TimeSpent
            };
        }

        public static ObservableCollection<AchievementStepViewModel> ToAchievementStepViewModels(
            this IEnumerable<AchievementStep> models)
        {
            return models.Select(model => model.ToAchievementStepViewModel())
                .ToObservableCollection();
        }

        public static AchievementStep ToAchievementModel(this AchievementStepViewModel viewModel)
        {
            return new AchievementStep
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                Description = viewModel.Description,
                AchievedDate = viewModel.AchievedDate,
                AchievementId = viewModel.AchievementId,
                TimeSpent = viewModel.TimeSpent
            };
        }
        
        public static List<AchievementStep> ToAchievementStepViewModels(
            this ObservableCollection<AchievementStepViewModel> models)
        {
            return models.Select(model => model.ToAchievementModel())
                .ToList();
        }

        public static BirthdayModel ToBirthdayModel(this BirthdayViewModel viewModel)
        {
            return new BirthdayModel
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                GiftDescription = viewModel.GiftDescription,
                ImageContent = viewModel.ImageContent,
                UserId = viewModel.UserId,
                BirthDayDate = viewModel.BirthDayDate
            };
        }
        
        public static BirthdayViewModel ToBirthdaysViewModel(this BirthdayModel model)
        {
            return new BirthdayViewModel
            {
                Id = model.Id,
                Name = model.Name,
                GiftDescription = model.GiftDescription,
                ImageContent = model.ImageContent,
                UserId = model.UserId,
                BirthDayDate = model.BirthDayDate
            };
        }
        
        public static List<BirthdayModel> ToAchievementStepViewModels(
            this ObservableCollection<BirthdayViewModel> models)
        {
            return models.Select(model => model.ToBirthdayModel())
                .ToList();
        }
        
        public static ObservableCollection<BirthdayViewModel> ToAchievementStepViewModels(
            this IEnumerable<BirthdayModel> models)
        {
            return models.Select(model => model.ToBirthdaysViewModel())
                .ToObservableCollection();
        }
    }
}