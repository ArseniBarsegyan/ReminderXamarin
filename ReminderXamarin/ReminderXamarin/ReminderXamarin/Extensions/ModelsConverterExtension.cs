using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ReminderXamarin.Enums;
using Rm.Helpers;
using ReminderXamarin.ViewModels;
using Rm.Data.Data.Entities;

namespace ReminderXamarin.Extensions
{
    public static class ModelsConverterExtension
    {
        public static GalleryItemViewModel ToViewModel(this GalleryItemModel model)
        {
            var viewModel = new GalleryItemViewModel
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

        public static Note ToModel(this NoteViewModel viewModel)
        {
            return new Note
            {
                Id = viewModel.Id,
                Description = viewModel.Description,
                CreationDate = viewModel.CreationDate,
                EditDate = viewModel.EditDate,
                UserId = Settings.CurrentUserId,
                GalleryItems = viewModel.GalleryItemsViewModels.ToModels()
            };
        }

        public static NoteViewModel ToViewModel(this Note model)
        {
            return new NoteViewModel
            {
                Id = model.Id,
                CreationDate = model.CreationDate,
                EditDate = model.EditDate,
                Description = model.Description,
                FullDescription = model.EditDate.ToString("dd.MM.yy") + " " + model.Description,
                GalleryItemsViewModels = model.GalleryItems.ToViewModels()
            };
        }

        public static ObservableCollection<GalleryItemViewModel> ToViewModels(this IEnumerable<GalleryItemModel> models)
        {
            return models.Select(model => model.ToViewModel()).ToObservableCollection();
        }

        public static List<GalleryItemModel> ToModels(this IEnumerable<GalleryItemViewModel> viewModels)
        {
            return viewModels.Select(vm => vm.ToModel()).ToList();
        }

        public static ObservableCollection<NoteViewModel> ToNoteViewModels(this IEnumerable<Note> models)
        {
            return models.Select(model => model.ToViewModel()).ToObservableCollection();
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

        public static ToDoViewModel ToToDoViewModel(this ToDoModel model)
        {
            var viewModel = new ToDoViewModel
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

        public static ObservableCollection<ToDoViewModel> ToToDoViewModels(this IEnumerable<ToDoModel> models)
        {
            var viewModels = new ObservableCollection<ToDoViewModel>();

            foreach (var model in models)
            {
                var viewModel = new ToDoViewModel
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

        public static BirthdayModel ToBirthdayModel(this BirthdayViewModel viewModel)
        {
            return new BirthdayModel
            {
                Id = viewModel.Id,
                UserId = Settings.CurrentUserId,
                Name = viewModel.Name,
                ImageContent = viewModel.ImageContent,
                BirthDayDate = viewModel.BirthDayDate,
                GiftDescription = viewModel.GiftDescription
            };
        }

        public static BirthdayViewModel ToBirthdayViewModel(this BirthdayModel model)
        {
            return new BirthdayViewModel
            {
                Id = model.Id,
                Name = model.Name,
                ImageContent = model.ImageContent,
                BirthDayDate = model.BirthDayDate,
                GiftDescription = model.GiftDescription
            };
        }

        public static ObservableCollection<BirthdayViewModel> ToFriendViewModels(this IEnumerable<BirthdayModel> models)
        {
            return models.Select(model => model.ToBirthdayViewModel()).ToObservableCollection();
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

        public static AchievementStepViewModel ToViewModel(this AchievementStep model)
        {
            return new AchievementStepViewModel
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
            this IEnumerable<AchievementStep> models)
        {
            return models.Select(model => model.ToViewModel()).ToObservableCollection();
        }

        public static List<AchievementStep> ToModels(this IEnumerable<AchievementStepViewModel> viewModels)
        {
            return viewModels.Select(model => model.ToModel()).ToList();
        }

        public static AchievementViewModel ToAchievementViewModel(this AchievementModel model)
        {
            return new AchievementViewModel
            {
                Id = model.Id,
                Title = model.Title,
                GeneralDescription = model.GeneralDescription,
                GeneralTimeSpent = model.GeneralTimeSpent,
                ImageContent = model.ImageContent
            };
        }

        public static ObservableCollection<AchievementViewModel> ToAchievementViewModels(
            this IEnumerable<AchievementModel> models)
        {
            return models.Select(model => model.ToAchievementViewModel()).ToObservableCollection();
        }
    }
}