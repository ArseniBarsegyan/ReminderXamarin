using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels;
using RI.Data.Data.Entities;
using ToDoPriority = ReminderXamarin.Models.ToDoPriority;

namespace ReminderXamarin.Extensions
{
    public static class ModelsConverterExtension
    {
        public static PhotoViewModel ToPhotoViewModel(this PhotoModel model)
        {
            var viewModel = new PhotoViewModel
            {
                Id = model.Id,
                NoteId = model.NoteId,
                Landscape = model.Landscape,
                ResizedPath = model.ResizedPath,
                IsVideo = model.IsVideo,
                Thumbnail = model.Thumbnail
            };
            return viewModel;
        }

        public static VideoViewModel ToVideoViewModel(this VideoModel model)
        {
            var viewModel = new VideoViewModel
            {
                Id = model.Id,
                NoteId = model.NoteId,
                Path = model.Path
            };
            return viewModel;
        }

        public static ObservableCollection<PhotoViewModel> ToPhotoViewModels(this IEnumerable<PhotoModel> models)
        {
            return models.Select(model => new PhotoViewModel
                {
                    Id = model.Id,
                    Landscape = model.Landscape,
                    ResizedPath = model.ResizedPath,
                    Thumbnail = model.Thumbnail,
                    IsVideo = model.IsVideo,
                    NoteId = model.NoteId
                }).ToObservableCollection();
        }

        public static IEnumerable<PhotoModel> ToPhotoModels(this IEnumerable<PhotoViewModel> viewModels)
        {
            return viewModels.Select(viewModel => new PhotoModel
            {
                Id = viewModel.Id.ToString(),
                Landscape = viewModel.Landscape,
                ResizedPath = viewModel.ResizedPath,
                Thumbnail = viewModel.Thumbnail,
                IsVideo = viewModel.IsVideo,
                NoteId = viewModel.NoteId.ToString()
            });
        }

        public static ObservableCollection<VideoViewModel> ToVideoViewModels(this IEnumerable<VideoModel> models)
        {
            return models.Select(model => new VideoViewModel
            {
                Id = model.Id,
                NoteId = model.NoteId,
                Path = model.Path
            }).ToObservableCollection();
        }

        public static IEnumerable<VideoModel> ToVideoModels(this IEnumerable<VideoViewModel> viewModels)
        {
            return viewModels.Select(model => new VideoModel
            {
                Id = model.Id.ToString(),
                NoteId = model.NoteId.ToString(),
                Path = model.Path
            });
        }

        public static NoteViewModel ToNoteViewModel(this Note model)
        {
            var viewModel = new NoteViewModel
            {
                Id = model.Id,
                CreationDate = model.CreationDate.DateTime,
                EditDate = model.EditDate.DateTime,
                Description = model.Description,
                FullDescription = model.EditDate.ToString("dd.MM.yy") + " "+ model.Description,
                Photos = model.Photos.ToPhotoViewModels(),
                Videos = new ObservableCollection<VideoViewModel>(model.Videos.ToVideoViewModels())
            };
            return viewModel;
        }

        public static IEnumerable<NoteViewModel> ToNoteViewModels(this IEnumerable<Note> models)
        {
            return models.Select(model => new NoteViewModel
                {
                    Id = model.Id,
                    CreationDate = model.CreationDate.DateTime,
                    EditDate = model.EditDate.DateTime,
                    Description = model.Description,
                    FullDescription = model.EditDate.ToString("dd.MM.yy") + " " + model.Description,
                    Photos = model.Photos.ToPhotoViewModels(),
                    Videos = model.Videos.ToVideoViewModels()
                })
                .ToList();
        }

        public static ToDoViewModel ToToDoViewModel(this ToDoModel model)
        {
            var viewModel = new ToDoViewModel
            {
                Id = model.Id,
                WhenHappens = model.WhenHappens.DateTime,
                Description = model.Description
            };

            switch (model.Priority)
            {
                case ConstantsHelper.High:
                    viewModel.Priority = ToDoPriority.High;
                    break;
                case ConstantsHelper.Medium:
                    viewModel.Priority = ToDoPriority.Medium;
                    break;
                default:
                    viewModel.Priority = ToDoPriority.Low;
                    break;
            }
            return viewModel;
        }

        public static IEnumerable<ToDoViewModel> ToToDoViewModels(this IEnumerable<ToDoModel> models)
        {
            var viewModels = new List<ToDoViewModel>();

            foreach (var model in models)
            {
                var viewModel = new ToDoViewModel
                {
                    Id = model.Id,
                    WhenHappens = model.WhenHappens.DateTime,
                    Description = model.Description
                };
                switch (model.Priority)
                {
                    case ConstantsHelper.High:
                        viewModel.Priority = ToDoPriority.High;
                        break;
                    case ConstantsHelper.Medium:
                        viewModel.Priority = ToDoPriority.Medium;
                        break;
                    default:
                        viewModel.Priority = ToDoPriority.Low;
                        break;
                }
                viewModels.Add(viewModel);
            }
            return viewModels;
        }

        public static BirthdayViewModel ToBirthdayViewModel(this BirthdayModel model)
        {
            return new BirthdayViewModel
            {
                Id = model.Id,
                Name = model.Name,
                ImageContent = model.ImageContent,
                BirthDayDate = model.BirthDayDate.DateTime,
                GiftDescription = model.GiftDescription
            };
        }

        public static IEnumerable<BirthdayViewModel> ToFriendViewModels(this IEnumerable<BirthdayModel> models)
        {
            return models.Select(model => model.ToBirthdayViewModel()).ToObservableCollection();
        }

        public static AchievementNoteViewModel ToAchievementNoteViewModel(this AchievementNote model)
        {
            return new AchievementNoteViewModel
            {
                Id = model.Id,
                Description = model.Description,
                Date = model.Date.DateTime,
                HoursSpent = model.HoursSpent,
                AchievementId = model.Achievement.Id
            };
        }

        public static ObservableCollection<AchievementNoteViewModel> ToAchievementNoteViewModels(
            this IEnumerable<AchievementNote> models)
        {
            return models.Select(model => model.ToAchievementNoteViewModel()).ToObservableCollection();
        }

        public static AchievementViewModel ToAchievementViewModel(this AchievementModel model)
        {
            return new AchievementViewModel
            {
                Id = model.Id,
                AchievementNotes = model.AchievementNotes.ToAchievementNoteViewModels(),
                Title = model.Title,
                GeneralDescription = model.GeneralDescription,
                GeneralTimeSpent = model.GeneralTimeSpent,
                ImageContent = model.ImageContent
            };
        }

        public static IEnumerable<AchievementViewModel> ToAchievementViewModels(
            this IEnumerable<AchievementModel> models)
        {
            return models.Select(model => model.ToAchievementViewModel()).ToList();
        }

        public static UserProfileViewModel ToUserProfileViewModel(this AppUser model)
        {
            return new UserProfileViewModel
            {
                Id = model.Id,
                ImageContent = model.ImageContent,
                UserName = model.UserName,
                NotesCount = model.Notes.Count,
                AchievementsCount = model.Achievements.Count,
                FriendBirthdaysCount = model.Birthdays.Count
            };
        }
    }
}