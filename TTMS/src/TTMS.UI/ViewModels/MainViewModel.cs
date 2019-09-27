using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

            TravelersList = list;
            SelectedTraveler = traveler;
            IsListLoading = false;
        }

        public async void LoadTraveler(Guid id)
        {
            Log.Logger.Information("Loading Traveler Details: ID {id}", id);

            IsViewLoading = true;

            try
            {
                var traveler = await travelerService.GetByIdAsync(id);
                editView.ShowTraveler(traveler);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Fail to load traveler {id}", id);
                selectedTraveler = null;
                RefreshData();
            }
            finally
            {
                IsViewLoading = false;
            }
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
                IsEnabled = false;
                IsListLoading = true;
                IsViewLoading = true;

                await travelerService.DeleteAsync(SelectedTraveler.Id).ConfigureAwait(false);
                Thread.Sleep(1000);
                RefreshData();
            }
            finally
            {
                IsEnabled = true;
                IsListLoading = false;
                IsViewLoading = false;
            }
        }

        private async void EditCommitted()
        {
            Log.Logger.Debug("Traveler editing committed");

            try
            {
                IsEnabled = false;
                IsViewLoading = true;
                var traveler = editView.GetTraveler();

                if (traveler.Id == default)
                {
                    // UI creates a key to be able to properly show the new record. It's a simple way to "hide"
                    // temporary inconsistence while message is still to be processed.
                    traveler.Id = Guid.NewGuid();
                    traveler = await travelerService.CreateAsync(traveler).ConfigureAwait(false);
                    if (FilterByType != traveler.Type)
                    {
                        FilterByType = TravelerType.None;
                    }
                }
                else
                {
                    await travelerService.UpdateAsync(traveler).ConfigureAwait(false);
                }

                Thread.Sleep(1000);

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

