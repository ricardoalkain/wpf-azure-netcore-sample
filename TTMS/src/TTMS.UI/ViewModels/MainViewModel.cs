using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using TTMS.Common.Enums;
using TTMS.Common.Models;
using TTMS.UI.Helpers;
using TTMS.UI.Services;
using Unity;

namespace TTMS.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ITravelerService travelerService;
        private readonly IMessageBoxService messageBox;
        private Traveler selectedTraveler;
        private IEnumerable<Traveler> travelersList;
        private EditViewModel editView;
        private bool isEnabled = true;
        private bool isListLoading = true;
        private bool isViewLoading = true;
        private TravelerType filterByType;

        public RelayCommand NewTravelerCmd { get; private set; }

        public RelayCommand EditTravelerCmd { get; private set; }

        public RelayCommand DeleteTravelerCmd { get; private set; }

        public MainViewModel()
        {
            messageBox = DependencyManager.Container.Resolve<IMessageBoxService>();
            travelerService = DependencyManager.Container.Resolve<ITravelerService>();
            editView = DependencyManager.Container.Resolve<EditViewModel>();

            NewTravelerCmd = new RelayCommand(NewTraveler);
            EditTravelerCmd = new RelayCommand(EditTraveler);
            DeleteTravelerCmd = new RelayCommand(DeleteTraveler);
            editView.OnCancel += EditCancelled;
            editView.OnSave += EditCommitted;
        }

        public EditViewModel DetailsViewModel
        {
            get => editView;
            set => SetProperty(ref editView, value);
        }

        public bool IsEnabled
        {
            get => isEnabled;
            set => SetProperty(ref isEnabled, value);
        }

        public bool IsListLoading
        {
            get => isListLoading;
            set => SetProperty(ref isListLoading, value);
        }

        public bool IsViewLoading
        {
            get => isViewLoading;
            set => SetProperty(ref isViewLoading, value);
        }

        public IEnumerable<Traveler> TravelersList
        {
            get => travelersList;
            set => SetProperty(ref travelersList, value);
        }

        public Traveler SelectedTraveler
        {
            get => selectedTraveler;
            set
            {
                SetProperty(ref selectedTraveler, value);
                LoadTraveler(value?.Id ?? default);
            }
        }

        public TravelerType FilterByType
        {
            get => filterByType;
            set
            {
                SetProperty(ref filterByType, value);
                if (value == TravelerType.None)
                {
                    RefreshData();
                }
                else
                {
                    RefreshData(value);
                }
            }
        }

        public void RefreshData()
        {
            var currentItemId = SelectedTraveler?.Id ?? default; // Tries to keep selection after refreshing
            RefreshData(currentItemId);
        }

        public async void RefreshData(Guid id)
        {
            Log.Logger.Information("Refreshing Travelers list");

            IsListLoading = true;
            var list = (await travelerService.GetAllAsync()).OrderByDescending(t => t.Type).ToList();

            var traveler = list.FirstOrDefault(t => t.Id.Equals(id));

            if (traveler == null)
            {
                traveler = list.FirstOrDefault();
            }

            SelectedTraveler = traveler;
            TravelersList = list;
            IsListLoading = false;
        }

        public async void RefreshData(TravelerType type)
        {
            Log.Logger.Information("Refreshing Travelers list");

            IsListLoading = true;
            var list = (await travelerService.GetByTypeAsync(type)).OrderByDescending(t => t.Type).ToList();

            var id = SelectedTraveler?.Id ?? default;
            var traveler = list.FirstOrDefault(t => t.Id.Equals(id));

            if (traveler == null)
            {
                traveler = list.FirstOrDefault();
            }

            SelectedTraveler = traveler;
            TravelersList = list;
            IsListLoading = false;
        }

        public async void LoadTraveler(Guid id)
        {
            Log.Logger.Information("Loading Traveler Details: ID {id}", id);

            IsViewLoading = true;
            var traveler = await travelerService.GetByIdAsync(id);
            editView.ShowTraveler(traveler);
            IsViewLoading = false;
        }

        private void EditTraveler()
        {
            Log.Logger.Debug("Starting edit mode...");
            IsEnabled = false;
            editView.EditTraverler();
        }

        private void NewTraveler()
        {
            Log.Logger.Debug("Creating new traveler...");
            IsEnabled = false;
            editView.NewTraverler();
        }
        private async void DeleteTraveler()
        {
            if (!messageBox.Confirm("Are you sure you want to delete this record?"))
            {
                return;
            }

            Log.Logger.Debug("Deleting traveler...");
            try
            {
                IsListLoading = true;

                await travelerService.DeleteAsync(SelectedTraveler.Id).ConfigureAwait(false);
                RefreshData();
            }
            finally
            {
                IsListLoading = false;
            }
        }

        private async void EditCommitted()
        {
            Log.Logger.Debug("Traveler editing committed");

            try
            {
                IsViewLoading = true;
                var traveler = editView.GetTraveler();

                if (traveler.Id == default)
                {
                    await travelerService.CreateAsync(traveler).ConfigureAwait(false);
                }
                else
                {
                    await travelerService.UpdateAsync(traveler).ConfigureAwait(false);
                }

                RefreshData(traveler.Id);
            }
            finally
            {
                IsEnabled = true;
                IsViewLoading = false;
            }
        }

        private void EditCancelled()
        {
            Log.Logger.Debug("Traveler editing cancelled");
            editView.ShowTraveler(SelectedTraveler);
            IsEnabled = true;
        }
    }
}

