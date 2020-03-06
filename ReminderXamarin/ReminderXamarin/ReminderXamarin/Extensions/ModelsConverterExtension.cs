using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using ReminderXamarin.Core.Interfaces;
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
        public static GalleryItemViewModel ToViewModel(this GalleryItemModel model, 
            INavigationService navigationService,
            ICommandResolver commandResolver)
        {
            var viewModel = new GalleryItemViewModel(navigationService, commandResolver)
            {
                Id = model.Id,
                ImagePath = model.ImagePath,
                IsVideo = model.IsVideo,
                NoteId = model.NoteId,
                Thumbnail = model.Thumbnail,
                VideoPath = model.VideoPath
            };
            return viewModel;
        }

        public static GalleryItemModel ToModel(this GalleryItemViewModel viewModel)
        {
            var model = new GalleryItemModel
            {
                Id = viewModel.Id,
                ImagePath = viewModel.ImagePath,
                IsVideo = viewModel.IsVideo,
                NoteId = viewModel.NoteId,
                Thumbnail = viewModel.Thumbnail,
                VideoPath = viewModel.VideoPath
            };
            return model;
        }

        public static IEnumerable<GalleryItemViewModel> ToViewModels(this IEnumerable<GalleryItemModel> models,
            INavigationService navigationService,
            ICommandResolver commandResolver)
        {
            return models.Select(model => model.ToViewModel(navigationService, commandResolver));
        }

        public static List<GalleryItemModel> ToModels(this IEnumerable<GalleryItemViewModel> viewModels)
        {
            return viewModels.Select(vm => vm.ToModel()).ToList();
        }

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

        public static AchievementStep ToModel(this AchievementStepViewModel viewModel)
        {
            return new AchievementStep
            {
                Id = viewModel.Id,
                AchievementId = viewModel.AchievementId,
                ImageContent = viewModel.ImageContent,
                Title = viewModel.Title,
                Description = viewModel.Description,
                TimeSpent = viewModel.TimeSpent,
                TimeEstimation = viewModel.TimeEstimation
            };
        }

        public static AchievementStepViewModel ToViewModel(this AchievementStep model,
            INavigationService navigationService,
            IFileSystem fileService,
            IMediaService mediaService,
            ICommandResolver commandResolver)
        {
            return new AchievementStepViewModel(navigationService, 
                fileService, 
                mediaService, 
                commandResolver)
            {
                Id = model.Id,
                AchievementId = model.AchievementId,
                ImageContent = model.ImageContent,
                Title = model.Title,
                Description = model.Description,
                TimeSpent = model.TimeSpent,
                TimeEstimation = model.TimeEstimation
            };
        }

        public static ObservableCollection<AchievementStepViewModel> ToViewModels(
            this IEnumerable<AchievementStep> models, 
            INavigationService navigationService,
            IFileSystem fileService,
            IMediaService mediaService,
            ICommandResolver commandResolver)
        {
            return models.Select(model => model.ToViewModel(navigationService, 
                fileService, 
                mediaService,
                commandResolver))
                .ToObservableCollection();
        }

        public static List<AchievementStep> ToModels(this IEnumerable<AchievementStepViewModel> viewModels)
        {
            return viewModels.Select(model => model.ToModel()).ToList();
        }

        public static AchievementViewModel ToAchievementViewModel(this AchievementModel model, INavigationService navigationService)
        {
            return new AchievementViewModel(navigationService)
            {
                Id = model.Id,
                Title = model.Title,
                GeneralDescription = model.GeneralDescription,
                GeneralTimeSpent = model.GeneralTimeSpent,
                ImageContent = model.ImageContent
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