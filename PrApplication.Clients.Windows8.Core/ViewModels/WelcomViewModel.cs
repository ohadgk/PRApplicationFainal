using Cirrious.MvvmCross.ViewModels;
using PrApplication.Clients.Windows8.Core.PrServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PrApplication.Clients.Windows8.Core.ViewModels
{
    public class WelcomViewModel : MvxViewModel
    {
        PrServiceClient wcfService;

        #region Initialize
        //login as admin is allways possible,
        //on the initializiig of the welcome screen the app checks if there are any online events at the moment,
        // if there is, the possebility to log as host opens, any host can login to one event only.
        public WelcomViewModel()
        {
            IsBusyLoadingEvents = true;
            NoEventsMassegeOn = false;
            wcfService = new PrServiceClient();
            Initialize();
        }

        public void Initialize()
        {
            wcfService.GetEventsAsync();
            wcfService.GetEventsCompleted += wcfService_GetEventsCompleted;
        }

        void wcfService_GetEventsCompleted(object sender, GetEventsCompletedEventArgs e)
        {

            IfHasEventOnline(e.Result);
            IsBusyLoadingEvents = false;
        }

        private void IfHasEventOnline(ICollection<Event> Events)
        {
            ObservableCollection<Event> EventsToShowOnWelcome = new ObservableCollection<Event>();
            foreach (var Event in Events)
            {
                if (Event.StartDate.Day == DateTime.Now.Day)
                    EventsToShowOnWelcome.Add(Event);
            }

            if (EventsToShowOnWelcome.Count == 0)
            {
                NoEventsMassegeOn = true;
            }
            WelcomeEvents = null;
            WelcomeEvents = EventsToShowOnWelcome;

        }
        #endregion

        #region Page Props
        //binds to the events lst on the UI after filtering only the relevant events
        private ObservableCollection<Event> _welcomeEvents;

        public ObservableCollection<Event> WelcomeEvents
        {
            get { return _welcomeEvents; }
            set
            {
                _welcomeEvents = value;
                RaisePropertyChanged(() => WelcomeEvents);
            }
        }
        //binds to the visibility of the loading bar on the UI
        private bool _isBusyLoadingEvents;

        public bool IsBusyLoadingEvents
        {
            get { return _isBusyLoadingEvents; }
            set { _isBusyLoadingEvents = value; RaisePropertyChanged(() => IsBusyLoadingEvents); }
        }
        //binds to the visibility of the "no events tody" mssage bar on the UI
        private bool _noEventsMassegeOn;

        public bool NoEventsMassegeOn
        {
            get { return _noEventsMassegeOn; }
            set { _noEventsMassegeOn = value; RaisePropertyChanged(() => NoEventsMassegeOn); }
        }
        #endregion

        #region Navigation
        //navigated to the login view without evenet id passed, that means you can login only with admin users.
        public ICommand LoginAsAdminCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ShowViewModel<LoginViewModel>();
                });
            }
        }

        //if en evens was chosen it means that the user is a host, we navigates to the login view WITH event id,
        //that lets the login view know it needs to allow host users as well.
        public ICommand EventChooseCommand
        {
            get
            {
                return new MvxCommand<int>((SelectedEventID) =>
                 {
                     ShowViewModel<LoginViewModel>(new { SelectedEventId = SelectedEventID });

                 });
            }
        }

        //refresh to load all event from the beginning
        public ICommand RefreshCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Initialize();
                });
            }
        }
        #endregion
    }
}
