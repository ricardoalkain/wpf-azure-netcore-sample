using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using Microsoft.Win32;
using TTMS.Common.Enums;
using TTMS.Common.Models;
using TTMS.UI.Helpers;
using TTMS.UI.Services;
using Unity;

namespace TTMS.UI.ViewModels
{
    public class EditViewModel : BaseViewModel, IDataErrorInfo
    {
        private readonly IMessageBoxService messageBox;

        private bool isEditing = false;
        private bool isValidating = false;
        private Traveler undoTraveler = null;

        public EditViewModel()
        {
            messageBox = DependencyManager.Container.Resolve<IMessageBoxService>();

            CancelCommand = new RelayCommand(Cancel);
            SaveCommand = new RelayCommand(SaveData);
            LoadPictureCommand = new RelayCommand(LoadPictureFromFile);
        }

        public event Action OnSave = delegate { };

        public event Action OnCancel = delegate { };

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

        private int? birthtimelineid;
        public int? BirthTimelineId
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

        private DateTime? lastdatetime;
        public DateTime? LastDateTime
        {
            get => lastdatetime;
            set => SetProperty(ref lastdatetime, value);
        }

        private int? lasttimelineid;
        public int? LastTimelineId
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

        public string Error => null;

        public bool HasErrors
        {
            get
            {
                foreach (var prop in GetType().GetProperties())
                {
                    if (!string.IsNullOrEmpty(this[prop.Name]))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public string this[string fieldName]
        {
            get
            {
                if (!isValidating)
                {
                    return null;
                }

                switch (fieldName)
                {
                    case nameof(Name):
                        if (string.IsNullOrEmpty(Name))
                        {
                            return "This field is required!";
                        }
                        break;

                    case nameof(Type):
                        if (Type == TravelerType.None)
                        {
                            return "Traveler's type of traveler is required!";
                        }
                        break;

                    case nameof(Status):
                        if (Status == TravelerStatus.None)
                        {
                            return "Traveler's status is required!";
                        }
                        break;

                    case nameof(BirthDate):
                        if (!BirthDate.HasValue)
                        {
                            return "Please inform traveler's birth date. If this information is unknown, input the first day of the presumed birth year.";
                        }
                        break;

                    case nameof(BirthTimelineId):
                        if (!BirthTimelineId.HasValue)
                        {
                            return "Please inform traveler's original timeline.";
                        }
                        break;

                    case nameof(LastTimelineId):
                        if (!LastTimelineId.HasValue)
                        {
                            LastTimelineId = 935;
                        }
                        break;

                    case nameof(LastDateTime):
                        if (!LastDateTime.HasValue)
                        {
                            LastDateTime = DateTime.Now;
                        }
                        break;

                    case nameof(LastLocation):
                        if (string.IsNullOrEmpty(LastLocation))
                        {
                            LastLocation = "London, UK (presumed)";
                        }
                        break;
                }

                return null;
            }
        }

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
            traveler.BirthTimelineId = this.BirthTimelineId.GetValueOrDefault();
            traveler.BirthLocation = this.BirthLocation;
            traveler.LastDateTime = this.LastDateTime.GetValueOrDefault(DateTime.Now);
            traveler.LastTimelineId = this.LastTimelineId.GetValueOrDefault();
            traveler.LastLocation = this.LastLocation;
            traveler.Type = this.Type;
            traveler.Status = this.Status;
            traveler.DeviceModel = this.DeviceModel;
        }

        private void SaveData()
        {
            isValidating = true;
            RaisePropertyChanged(null);

            if (HasErrors)
            {
                return;
            }

            if (Picture == null || Picture.Length == 0)
            {
                if (!messageBox.Confirm("Are you sure you want to create a traveler profile without a picture?\n\n" +
                    "Our agents will certainly thank you for a clear way to identify this traveler in the adevent of " +
                    "any future time crime investigation."))
                {
                    return;
                }
            }

            isValidating = false;
            RaisePropertyChanged(null);

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
            this.Id = default;
            this.Name = default;
            this.Alias = default;
            this.Skills = default;
            this.Picture = default;
            this.BirthDate = default;
            this.BirthTimelineId = default;
            this.BirthLocation = default;
            this.LastDateTime = default;
            this.LastTimelineId = default;
            this.LastLocation = default;
            this.Type = default;
            this.Status = TravelerStatus.Active;
            this.DeviceModel = DeviceModel.None;
        }
    }
}
