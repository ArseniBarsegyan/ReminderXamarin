using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ReminderXamarin.Models;
using ReminderXamarin.ViewModels;

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
                Thumbnail = model.Thumbnail
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
                Thumbnail = viewModel.Thumbnail
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
                    NoteId = model.NoteId
                }).ToObservableCollection();
        }

        public static IEnumerable<PhotoModel> ToPhotoModels(this IEnumerable<PhotoViewModel> viewModels)
        {
            return viewModels.Select(viewModel => new PhotoModel
            {
                Id = viewModel.Id,
                Landscape = viewModel.Landscape,
                ResizedPath = viewModel.ResizedPath,
                Thumbnail = viewModel.Thumbnail,
                NoteId = viewModel.NoteId
            });
        }

        public static NoteViewModel ToNoteViewModel(this Note note)
        {
            var viewModel = new NoteViewModel
            {
                Id = note.Id,
                CreationDate = note.CreationDate,
                EditDate = note.EditDate,
                Description = note.Description,
                FullDescription = note.EditDate.ToString("dd.MM.yy") + " "+ note.Description,
                Photos = note.Photos.ToPhotoViewModels()
            };
            return viewModel;
        }

        public static Note ToNoteModel(this NoteViewModel note)
        {
            var model = new Note
            {
                Id = note.Id,
                CreationDate = note.CreationDate,
                EditDate = note.EditDate,
                Description = note.Description,
                Photos = note.Photos.ToPhotoModels().ToList()
            };
            return model;
        }

        public static IEnumerable<NoteViewModel> ToNoteViewModels(this IEnumerable<Note> models)
        {
            return models.Select(model => new NoteViewModel
                {
                    Id = model.Id,
                    CreationDate = model.CreationDate,
                    EditDate = model.EditDate,
                    Description = model.Description,
                    FullDescription = model.EditDate.ToString("dd.MM.yy") + " " + model.Description,
                    Photos = model.Photos.ToPhotoViewModels()
                })
                .ToList();
        }

        public static IEnumerable<Note> ToNoteViewModels(this IEnumerable<NoteViewModel> viewModels)
        {
            return viewModels.Select(viewModel => new Note
                {
                    Id = viewModel.Id,
                    CreationDate = viewModel.CreationDate,
                    EditDate = viewModel.EditDate,
                    Description = viewModel.Description,
                    Photos = viewModel.Photos.ToPhotoModels().ToList()
                })
                .ToList();
        }
    }
}