using System;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.Win32;
using TTMS.Common.Abstractions;
using TTMS.Common.Enums;
using TTMS.Common.Entities;
using TTMS.UI.Helpers;
using TTMS.UI.Services;
using Unity;

namespace TTMS.UI.ViewModels
{
    public class EditViewModel : BaseViewModel
    {
        private readonly ITravelerService travelerService;

        private bool isEditing;

        public event Action OnSave = delegate { };
        public event Action OnCancel = delegate { };

        public EditViewModel()
        {
            travelerService = DependencyManager.Container.Resolve<ITravelerService>();

            CancelCommand = new RelayCommand(Cancel);
            SaveCommand = new RelayCommand(SaveData);
            LoadPictureCommand = new RelayCommand(LoadPictureFromFile);
        }

        public RelayCommand SaveCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand LoadPictureCommand { get; private set; }


        public bool IsEditing
        {
            get => isEditing;
            set => SetProperty(ref isEditing, value);
        }

        #region Traveler props

        private Guid id;
        public Guid Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string alias;
        public string Alias
        {
            get => alias;
            set => SetProperty(ref alias, value);
        }

        private string skills;
        public string Skills
        {
            get => skills;
            set => SetProperty(ref skills, value);
        }

        private byte[] picture;
        public byte[] Picture
        {
            get => picture;
            set => SetProperty(ref picture, value);
        }

        private DateTime birthdate;
        public DateTime BirthDate
        {
            get => birthdate;
            set => SetProperty(ref birthdate, value);
        }

        private int birthtimelineid;
        public int BirthTimelineId
        {
            get => birthtimelineid;
            set => SetProperty(ref birthtimelineid, value);
        }

        private string birthlocation;
        public string BirthLocation
        {
            get => birthlocation;
            set => SetProperty(ref birthlocation, value);
        }

        private DateTime lastdatetime;
        public DateTime LastDateTime
        {
            get => lastdatetime;
            set => SetProperty(ref lastdatetime, value);
        }

        private int lasttimelineid;
        public int LastTimelineId
        {
            get => lasttimelineid;
            set => SetProperty(ref lasttimelineid, value);
        }

        private string lastlocation;
        public string LastLocation
        {
            get => lastlocation;
            set => SetProperty(ref lastlocation, value);
        }

        private TravelerType type;
        public TravelerType Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }

        private TravelerStatus status;
        public TravelerStatus Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }

        private DeviceModel deviceModel;
        public DeviceModel DeviceModel
        {
            get => deviceModel;
            set => SetProperty(ref deviceModel, value);
        }

        #endregion

        public async void ShowTraveler(Guid id)
        {
            await LoadData(id);
        }

        public void NewTraverler()
        {
            this.Id = Guid.NewGuid();
            this.Name = default;
            this.Alias = default;
            this.Skills = default;
            this.Picture = default;
            this.BirthDate = default;
            this.BirthTimelineId = default;
            this.BirthLocation = default;
            this.LastDateTime = DateTime.Now;
            this.LastTimelineId = 1;
            this.LastLocation = default;
            this.Type = default;
            this.Status = TravelerStatus.Active;
            this.DeviceModel = DeviceModel.Unknown;

            IsEditing = true;
        }

        public void EditTraverler()
        {
            IsEditing = true;
        }

        public void LoadPictureFromFile()
        {
            var dlg = new OpenFileDialog();
            dlg.Title = "Select traveler's picture";
            dlg.Filter = "All supported graphics|*.jpg;*.jpeg;*.png;*.bmp|" +
                         "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                         "Portable Network Graphic (*.png)|*.png|" +
                         "Bitmap (*.bmp)|*.bmp";

            if (dlg.ShowDialog() == true)
            {
                var img = Image.FromFile(dlg.FileName);
                var converter = new ImageConverter();
                Picture = (byte[])converter.ConvertTo(img, typeof(byte[]));
            }
        }

        private async Task LoadData(Guid id)
        {
            var traveler = await travelerService.GetByIdAsync(id).ConfigureAwait(false);

            this.Id = traveler?.Id ?? id;
            this.Name = traveler?.Name;
            this.Alias = traveler?.Alias;
            this.Skills = traveler?.Skills;
            this.Picture = traveler?.Picture;
            this.BirthDate = traveler?.BirthDate ?? default;
            this.BirthTimelineId = traveler?.BirthTimelineId ?? 1;
            this.BirthLocation = traveler?.BirthLocation;
            this.LastDateTime = traveler?.LastDateTime ?? default;
            this.LastTimelineId = traveler?.LastTimelineId ?? 1;
            this.LastLocation = traveler?.LastLocation;
            this.Type = traveler?.Type ?? default;
            this.Status = traveler?.Status ?? default;
            this.DeviceModel = traveler?.DeviceModel ?? default;
        }

        private async void SaveData()
        {
            var traveler = new Traveler
            {
                Id = this.Id,
                Name = this.Name,
                Alias = this.Alias,
                Skills = this.Skills,
                Picture = this.Picture,
                BirthDate = this.BirthDate,
                BirthTimelineId = this.BirthTimelineId,
                BirthLocation = this.BirthLocation,
                LastDateTime = this.LastDateTime,
                LastTimelineId = this.LastTimelineId,
                LastLocation = this.LastLocation,
                Type = this.Type,
                Status = this.Status,
                DeviceModel = this.DeviceModel
            };

            if (traveler.Id == default)
            {
                await travelerService.CreateAsync(traveler).ConfigureAwait(false);
            }
            else
            {
                await travelerService.UpdateAsync(traveler).ConfigureAwait(false);
            }

            IsEditing = false;
            OnSave();
        }

        private async void Cancel()
        {
            await LoadData(Id);
            IsEditing = false;
            OnCancel();
        }
    }
}
