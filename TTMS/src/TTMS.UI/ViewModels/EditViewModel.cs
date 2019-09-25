using System;
using System.Drawing;
using System.IO;
using Microsoft.Win32;
using TTMS.Common.Enums;
using TTMS.Common.Models;
using TTMS.UI.Helpers;

namespace TTMS.UI.ViewModels
{
    public class EditViewModel : BaseViewModel
    {
        private bool isEditing;
        private Traveler undoTraveler = null;

        public event Action OnSave = delegate { };
        public event Action OnCancel = delegate { };

        public EditViewModel()
        {
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

        private DateTime? birthdate;
        public DateTime? BirthDate
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

        public void NewTraverler()
        {
            SaveState();
            ClearForm();
            IsEditing = true;
        }

        public void EditTraverler()
        {
            SaveState();
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
            dlg.CheckFileExists = true;

            if (dlg.ShowDialog() == true)
            {
                // Check for Azure Table row size limit (max 1MB)
                var fileInfo = new FileInfo(dlg.FileName);
                if (fileInfo.Length > 524288)
                {
                    throw new ArgumentOutOfRangeException("Picture is too large: File larger than 500KB are not allowed.\nPlease specify another file.");
                }

                var img = Image.FromFile(dlg.FileName);
                var converter = new ImageConverter();
                Picture = (byte[])converter.ConvertTo(img, typeof(byte[]));
            }
        }

        public void ShowTraveler(Traveler traveler)
        {
            if (traveler == null)
            {
                ClearForm();
            }
            else
            {
                this.Id = traveler.Id;
                this.Name = traveler.Name;
                this.Alias = traveler.Alias;
                this.Skills = traveler.Skills;
                this.Picture = traveler.Picture;
                this.BirthDate = traveler.BirthDate;
                this.BirthTimelineId = traveler.BirthTimelineId;
                this.BirthLocation = traveler.BirthLocation;
                this.LastDateTime = traveler.LastDateTime;
                this.LastTimelineId = traveler.LastTimelineId;
                this.LastLocation = traveler.LastLocation;
                this.Type = traveler.Type;
                this.Status = traveler.Status;
                this.DeviceModel = traveler.DeviceModel;
            }
        }

        public Traveler GetTraveler()
        {
            var traveler = new Traveler();
            UpdateTraveler(traveler);
            return traveler;
        }

        public void UpdateTraveler(Traveler traveler)
        {
            traveler.Id = this.Id;
            traveler.Name = this.Name;
            traveler.Alias = this.Alias;
            traveler.Skills = this.Skills;
            traveler.Picture = this.Picture;
            traveler.BirthDate = this.BirthDate.GetValueOrDefault();
            traveler.BirthTimelineId = this.BirthTimelineId;
            traveler.BirthLocation = this.BirthLocation;
            traveler.LastDateTime = this.LastDateTime;
            traveler.LastTimelineId = this.LastTimelineId;
            traveler.LastLocation = this.LastLocation;
            traveler.Type = this.Type;
            traveler.Status = this.Status;
            traveler.DeviceModel = this.DeviceModel;
        }

        private void SaveData()
        {
            IsEditing = false;
            OnSave();
        }

        private void Cancel()
        {
            UndoChanges();
            IsEditing = false;
            OnCancel();
        }

        private void SaveState()
        {
            if (undoTraveler == null)
            {
                undoTraveler = new Traveler();
            }

            UpdateTraveler(undoTraveler);
        }

        private void UndoChanges()
        {
            ShowTraveler(undoTraveler);
        }

        private void ClearForm()
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
        }
    }
}
