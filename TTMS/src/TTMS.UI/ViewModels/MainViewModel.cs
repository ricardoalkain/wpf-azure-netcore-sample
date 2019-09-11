using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using TTMS.Data.Models;
using TTMS.Data.Services;
using TTMS.UI.Helpers;
using Unity;

namespace TTMS.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ITravelerService travelerService;
        private Traveler selectedTraveler;
        private IEnumerable<Traveler> travelersList;
        private EditViewModel editView;
        private bool isEnabled = true;

        public RelayCommand NewTravelerCmd { get; private set; }
        public RelayCommand EditTravelerCmd { get; private set; }

        public MainViewModel()
        {
            travelerService = DependencyManager.Container.Resolve<ITravelerService>();
            editView = DependencyManager.Container.Resolve<EditViewModel>();

            NewTravelerCmd = new RelayCommand(NewTraveler);
            EditTravelerCmd = new RelayCommand(EditTraveler);
            editView.OnCancel += EditCancelled;
            editView.OnSave += EditCommitted;

            // Work around bug in WPF where SizeToContent somehow disable Interation.EventTriggers
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                RefreshData();
            }
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
                editView.ShowTraveler(value?.Id ?? default);
            }
        }

        public async void RefreshData()
        {
            var currentItemId = SelectedTraveler?.Id ?? default; // Tries to keep selection after refreshing
            RefreshData(currentItemId);
        }

        public async void RefreshData(Guid id)
        {
            TravelersList = await travelerService.GetAllAsync();

            SelectedTraveler = TravelersList.FirstOrDefault(t => t.Id.Equals(id));

            if (SelectedTraveler == null)
            {
                SelectedTraveler = TravelersList.FirstOrDefault();
            }
        }

        private void EditTraveler()
        {
            IsEnabled = false;
            editView.EditTraverler();
        }

        private void NewTraveler()
        {
            IsEnabled = false;
            editView.NewTraverler();
        }

        private void EditCommitted()
        {
            IsEnabled = true;
            RefreshData(editView.Id);
        }

        private void EditCancelled()
        {
            IsEnabled = true;
            editView.ShowTraveler(SelectedTraveler.Id);
        }
    }
}
