using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ReminderXamarin.Enums;
using Rm.Helpers;
using ReminderXamarin.ViewModels;
using Rm.Data.Data.Entities;
using Xamarin.Forms;

namespace ReminderXamarin.Extensions
{
    public static class ModelsConverterExtension
    {
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

        public static ObservableCollection<GalleryItemViewModel> ToViewModels(this IEnumerable<GalleryItemModel> models)
        {
            return models.Select(model => new GalleryItemViewModel
            {
                Id = model.Id,
                ImagePath = model.ImagePath,
                IsVideo = model.IsVideo,
                NoteId = model.NoteId,
                Thumbnail = model.Thumbnail,
                VideoPath = model.VideoPath
            }).ToObservableCollection();
        }

        public static List<GalleryItemModel> ToModels(this IEnumerable<GalleryItemViewModel> viewModels)
        {
            return viewModels.Select(viewModel => new GalleryItemModel
            {
                Id = viewModel.Id,
                ImagePath = viewModel.ImagePath,
                IsVideo = viewModel.IsVideo,
                NoteId = viewModel.NoteId,
                Thumbnail = viewModel.Thumbnail,
                VideoPath = viewModel.VideoPath
            }).ToList();
        }

        public static PhotoViewModel ToPhotoViewModel(this PhotoModel model)
        {
            var viewModel = new PhotoViewModel
            {
                Id = model.Id,
                NoteId = model.NoteId,
                Landscape = model.Landscape,
                ResizedPath = model.ResizedPath,
                Thumbnail = model.Thumbnail,
                IsVideo = model.IsVideo
            };
            return viewModel;
        }

        public static PhotoModel ToPhotoModel(this PhotoViewModel viewModel)
        {
            var model = new PhotoModel
            {
                Id = viewModel.Id,
                NoteId = viewModel.NoteId,
                Landscape = viewModel.Landscape,
                ResizedPath = viewModel.ResizedPath,
                Thumbnail = viewModel.Thumbnail,
                IsVideo = viewModel.IsVideo
            };
            return model;
        }

        public static ObservableCollection<PhotoViewModel> ToPhotoViewModels(this IEnumerable<PhotoModel> models)
        {
            return models.Select(model => new PhotoViewModel
            {
                Id = model.Id,
                Landscape = model.Landscape,
                ResizedPath = model.ResizedPath,
                Thumbnail = model.Thumbnail,
                NoteId = model.NoteId,
                IsVideo = model.IsVideo
            }).ToObservableCollection();
        }

        public static List<PhotoModel> ToPhotoModels(this IEnumerable<PhotoViewModel> viewModels)
        {
            return viewModels.Select(viewModel => new PhotoModel
            {
                Id = viewModel.Id,
                Landscape = viewModel.Landscape,
                ResizedPath = viewModel.ResizedPath,
                Thumbnail = viewModel.Thumbnail,
                NoteId = viewModel.NoteId,
                IsVideo = viewModel.IsVideo
            }).ToList();
        }

        public static NoteViewModel ToNoteViewModel(this Note note)
        {
            var viewModel = new NoteViewModel
            {
                Id = note.Id,
                CreationDate = note.CreationDate,
                EditDate = note.EditDate,
                Description = note.Description,
                FullDescription = note.EditDate.ToString("dd.MM.yy") + " " + note.Description,
                Photos = note.Photos.ToPhotoViewModels(),
                Videos = note.Videos.ToVideoViewModels(),
                GalleryItemsViewModels = note.GalleryItems.ToViewModels()
            };
            return viewModel;
        }

        public static Note ToNoteModel(this NoteViewModel note)
        {
            var model = new Note
            {
                Id = note.Id,
                UserId = Settings.CurrentUserId,
                CreationDate = note.CreationDate,
                EditDate = note.EditDate,
                Description = note.Description,
                Photos = note.Photos.ToPhotoModels().ToList(),
                Videos = note.Videos.ToVideoModels().ToList(),
                GalleryItems = note.GalleryItemsViewModels.ToModels().ToList()
            };
            return model;
        }

        public static ObservableCollection<NoteViewModel> ToNoteViewModels(this IEnumerable<Note> models)
        {
            return models.Select(model => new NoteViewModel
            {
                Id = model.Id,
                CreationDate = model.CreationDate,
                EditDate = model.EditDate,
                Description = model.Description,
                FullDescription = model.EditDate.ToString("dd.MM.yy") + " " + model.Description,
                Photos = model.Photos.ToPhotoViewModels(),
                Videos = model.Videos.ToVideoViewModels(),
                GalleryItemsViewModels = model.GalleryItems.ToViewModels()
            }).ToObservableCollection();
        }

        public static List<Note> ToNoteModels(this IEnumerable<NoteViewModel> viewModels)
        {
            return viewModels.Select(viewModel => new Note
            {
                Id = viewModel.Id,
                CreationDate = viewModel.CreationDate,
                EditDate = viewModel.EditDate,
                Description = viewModel.Description,
                Photos = viewModel.Photos.ToPhotoModels().ToList(),
                Videos = viewModel.Videos.ToVideoModels().ToList(),
                GalleryItems = viewModel.GalleryItemsViewModels.ToModels().ToList()
            }).ToList();
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

        public static List<ToDoModel> ToToDoModels(this IEnumerable<ToDoViewModel> viewModels)
        {
            return viewModels.Select(viewModel => new ToDoModel
            {
                Id = viewModel.Id,
                Status = viewModel.Status.ToString(),
                WhenHappens = viewModel.WhenHappens,
                Description = viewModel.Description
            })
            .ToList();
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

        public static List<BirthdayModel> ToBirthdayModels(this IEnumerable<BirthdayViewModel> viewModels)
        {
            return viewModels.Select(viewModel => viewModel.ToBirthdayModel()).ToList();
        }

        public static ObservableCollection<BirthdayViewModel> ToFriendViewModels(this IEnumerable<BirthdayModel> models)
        {
            return models.Select(model => model.ToBirthdayViewModel()).ToObservableCollection();
        }

        public static ObservableCollection<Image> ToImages(this IEnumerable<PhotoViewModel> viewModels)
        {
            var images = new ObservableCollection<Image>();
            foreach (var viewModel in viewModels)
            {
                images.Add(viewModel.ToImage());
            }
            return images;
        }

        public static Image ToImage(this PhotoViewModel viewModel)
        {
            return new Image { Source = viewModel.ResizedPath };
        }

        public static AchievementNote ToAchievementNote(this AchievementNoteViewModel viewModel)
        {
            return new AchievementNote
            {
                Id = viewModel.Id,
                Description = viewModel.Description,
                Date = viewModel.Date,
                HoursSpent = viewModel.HoursSpent,
                AchievementId = viewModel.AchievementId
            };
        }

        public static AchievementStep ToModel(this AchievementStepViewModel viewModel)
        {
            return new AchievementStep
            {
                Id = viewModel.Id,
                AchievementId = viewModel.AchievementId,
                Image = viewModel.Image,
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
                Image = model.Image,
                Title = model.Title,
                Description = model.Description,
                TimeSpent = model.TimeSpent,
                TimeEstimation = model.TimeEstimation
            };
        }

        public static AchievementNoteViewModel ToAchievementNoteViewModel(this AchievementNote model)
        {
            return new AchievementNoteViewModel
            {
                Id = model.Id,
                Description = model.Description,
                Date = model.Date,
                HoursSpent = model.HoursSpent,
                AchievementId = model.AchievementId
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

        public static ObservableCollection<AchievementNoteViewModel> ToAchievementNoteViewModels(
            this IEnumerable<AchievementNote> models)
        {
            return models.Select(model => model.ToAchievementNoteViewModel()).ToObservableCollection();
        }

        public static List<AchievementNote> ToAchievementNoteViewModels(
            this IEnumerable<AchievementNoteViewModel> viewModels)
        {
            return viewModels.Select(viewModel => viewModel.ToAchievementNote()).ToList();
        }

        public static AchievementModel ToAchievementModel(this AchievementViewModel viewModel)
        {
            return new AchievementModel
            {
                Id = viewModel.Id,
                UserId = Settings.CurrentUserId,
                AchievementNotes = viewModel.AchievementNotes.ToAchievementNoteViewModels(),
                AchievementSteps = viewModel.AchievementStepViewModels.ToModels(),
                Title = viewModel.Title,
                GeneralDescription = viewModel.GeneralDescription,
                GeneralTimeSpent = viewModel.GeneralTimeSpent,
                ImageContent = viewModel.ImageContent
            };
        }

        public static AchievementViewModel ToAchievementViewModel(this AchievementModel model)
        {
            return new AchievementViewModel
            {
                Id = model.Id,
                AchievementNotes = model.AchievementNotes.ToAchievementNoteViewModels(),
                AchievementStepViewModels = model.AchievementSteps.ToViewModels(),
                Title = model.Title,
                GeneralDescription = model.GeneralDescription,
                GeneralTimeSpent = model.GeneralTimeSpent,
                ImageContent = model.ImageContent
            };
        }

        public static List<AchievementModel> ToAchievementModels(
            this IEnumerable<AchievementViewModel> viewModels)
        {
            return viewModels.Select(viewModel => viewModel.ToAchievementModel()).ToList();
        }

        public static ObservableCollection<AchievementViewModel> ToAchievementViewModels(
            this IEnumerable<AchievementModel> models)
        {
            return models.Select(model => model.ToAchievementViewModel()).ToObservableCollection();
        }

        public static UserModel ToUserModel(this UserProfileViewModel viewModel)
        {
            return new UserModel
            {
                Id = viewModel.Id,
                ImageContent = viewModel.ImageContent,
                UserName = viewModel.UserName
            };
        }

        public static UserProfileViewModel ToUserProfileViewModel(this UserModel model)
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

        public static VideoViewModel ToVideoViewModel(this VideoModel model)
        {
            return new VideoViewModel
            {
                Id = model.Id,
                NoteId = model.NoteId,
                Path = model.Path
            };
        }

        public static VideoModel ToVideoModel(this VideoViewModel viewModel)
        {
            return new VideoModel
            {
                Id = viewModel.Id,
                Path = viewModel.Path,
                NoteId = viewModel.NoteId
            };
        }

        public static List<VideoModel> ToVideoModels(
            this IEnumerable<VideoViewModel> viewModels)
        {
            return viewModels.Select(viewModel => viewModel.ToVideoModel()).ToList();
        }

        public static ObservableCollection<VideoViewModel> ToVideoViewModels(
            this IEnumerable<VideoModel> models)
        {
            return models.Select(model => model.ToVideoViewModel()).ToObservableCollection();
        }
    }
}