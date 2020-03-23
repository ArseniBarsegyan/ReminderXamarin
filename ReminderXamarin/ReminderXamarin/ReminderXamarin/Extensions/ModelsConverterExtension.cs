using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Enums;
using ReminderXamarin.Services.Navigation;
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
            INavigationService navigationService,
            ICommandResolver commandResolver)
        {
            var viewModel = new ToDoViewModel(navigationService, commandResolver)
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
            INavigationService navigationService,
            ICommandResolver commandResolver)
        {
            var viewModels = new ObservableCollection<ToDoViewModel>();

            foreach (var model in models)
            {
                var viewModel = new ToDoViewModel(navigationService, commandResolver)
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

        public static AchievementViewModel ToAchievementViewModel(this AchievementModel model, 
            INavigationService navigationService)
        {
            return new AchievementViewModel(navigationService)
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                GeneralTimeSpent = model.GeneralTimeSpent
            };
        }

        public static ObservableCollection<AchievementViewModel> ToAchievementViewModels(
            this IEnumerable<AchievementModel> models,
            INavigationService navigationService)
        {
            return models.Select(model => model.ToAchievementViewModel(navigationService)).ToObservableCollection();
        }
    }
}