using Cirrious.MvvmCross.ViewModels;
using PrApplication.Clients.Windows8.Core.PrServiceReference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PrApplication.Clients.Windows8.Core.ViewModels
{
    public class HostViewModel : MvxViewModel
    {
        // Data Members
        PrServiceClient wcfService;
        private Event SelectedEvent;
        int eventId;

        #region Initialize
        public HostViewModel()
        {
            wcfService = new PrServiceClient();
        }

        protected override void InitFromBundle(IMvxBundle parameters)
        {
            if (parameters.Data.ContainsKey("SelectedEventId") && parameters.Data.ContainsKey("UserName"))
            {
                eventId = int.Parse(parameters.Data["SelectedEventId"]);
                UserName = "Hello " + parameters.Data["UserName"] + " :)";
                IsBusyLoadingGuests = true;
                Guests = null;
                Initialize();
            }

            else
            {
                this.Close(this);
            }
            base.InitFromBundle(parameters);
        }

        private void Initialize()
        {
           
            IsGuestChosen = false;
            Guests = null;//בגלל שאני רוצה שבטעינה השניה אחרי שעשו עדכון האורחים ייעלמו ויחזרו- בגלל זה זה כאן ולא בהשלמת האיבנט
            wcfService.GetEventByIDAsync(eventId);
            wcfService.GetEventByIDCompleted += wcfService_GetEventByIDCompleted;
        }

        private void wcfService_GetEventByIDCompleted(object sender, GetEventByIDCompletedEventArgs e)
        {
            if (e.Result == null) throw new NullReferenceException("There is no event to show");
            else
                SelectedEvent = e.Result;

            Guests = new ObservableCollection<Guest>(SelectedEvent.Guests);

            IsBusyLoadingGuests = false;
            IsGuestChosen = false;
            EventDate = SelectedEvent.StartDate;
            EventName = SelectedEvent.Name;
        }

        #endregion

        #region Properties To Update Guest

        private int _companionsThatArrived;
        public int CompanionsThatArrived
        {
            get { return _companionsThatArrived; }
            set { _companionsThatArrived = value; RaisePropertyChanged(() => CompanionsThatArrived); }
        }


        private bool _allCompanionsArrived;
        public bool AllCompanionsArrived
        {
            get
            {
                return _allCompanionsArrived;
            }
            set
            {
                _allCompanionsArrived = value;
                RaisePropertyChanged(() => AllCompanionsArrived);
            }
        }


        private bool _isAttended;
        public bool IsAttended
        {
            get { return _isAttended; }
            set { _isAttended = value; RaisePropertyChanged(() => IsAttended); }
        }

        #endregion

        #region Properties

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; RaisePropertyChanged(() => UserName); }
        }


        private string _guestToSearch;
        public string GuestToSearch
        {
            get { return _guestToSearch; }
            set { _guestToSearch = value; RaisePropertyChanged(() => GuestToSearch); }
        }


        private Guest _SelectedGuest;
        public Guest SelectedGuest
        {
            get { return _SelectedGuest; }
            set
            {
                _SelectedGuest = value;
                if (SelectedGuest != null)
                {
                    InitializeSelectedGuest();
                    IsGuestChosen = true;
                }
                RaisePropertyChanged(() => SelectedGuest);

            }
        }

        private void InitializeSelectedGuest()
        {
            SelectedGuest.Event = SelectedEvent;
            IsAttended = SelectedGuest.Atended;
            if (SelectedGuest.AllCompanionsArrived == true)
            {
                AllCompanionsArrived = true;
                SelectedGuest.AtendedCompanions = SelectedGuest.Companions;
                CompanionsThatArrived = SelectedGuest.Companions;
            }
            if (SelectedGuest.AtendedCompanions == null)
                CompanionsThatArrived = 0;
            else
                CompanionsThatArrived = (int)SelectedGuest.AtendedCompanions;
        }


        private ObservableCollection<Guest> _guests;
        public ObservableCollection<Guest> Guests
        {
            get { return _guests; }
            set
            {
                _guests = value;
                RaisePropertyChanged(() => Guests);
            }

        }


        private bool _IsBusyLoadingGuests;
        public bool IsBusyLoadingGuests
        {
            get { return _IsBusyLoadingGuests; }
            set
            {
                _IsBusyLoadingGuests = value;
                RaisePropertyChanged(() => IsBusyLoadingGuests);
            }
        }


        private bool _IsGuestChosen;
        public bool IsGuestChosen
        {
            get
            {
                return _IsGuestChosen;
            }
            set
            {
                _IsGuestChosen = value;
                RaisePropertyChanged(() => IsGuestChosen);
            }
        }


        private DateTimeOffset _eventDate = DateTimeOffset.Now;
        public DateTimeOffset EventDate
        {
            get { return _eventDate; }
            set { _eventDate = value; RaisePropertyChanged(() => EventDate); }
        }


        private string _eventName;
        public string EventName
        {
            get { return _eventName; }
            set { _eventName = value; RaisePropertyChanged(() => EventName); }
        }


        #endregion

        #region Commands
        public ICommand CloseViewCommand
        {
            get
            {
                return new MvxCommand(() => { ShowViewModel<WelcomViewModel>(); });
            }
        }
        public ICommand ChangeGuestStatusCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (AllCompanionsArrived) CompanionsThatArrived = SelectedGuest.Companions;
                    wcfService.ChangeGuestStatusAsync(eventId, SelectedGuest.Id, IsAttended, AllCompanionsArrived, CompanionsThatArrived);
                    wcfService.ChangeGuestStatusCompleted += wcfService_ChangeGuestStatusCompleted;
                });
            }
        }

        void wcfService_ChangeGuestStatusCompleted(object sender, ChangeGuestStatusCompletedEventArgs e)
        {
            if (e.Result)
                Initialize();
        }


        public ICommand ReLoginCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ShowViewModel<WelcomViewModel>();
                });
            }
        }

        public ICommand AdminCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ShowViewModel<LoginViewModel>();
                });
            }
        }

        public ICommand CancelUpdateCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (string.IsNullOrWhiteSpace(GuestToSearch))
                        Initialize();
                    else
                        Search();
                });
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Search();
                });
            }
        }

        private void Search()
        {
            if (string.IsNullOrWhiteSpace(GuestToSearch))
                Initialize();
            else
            {
                IsGuestChosen = false;
                IsBusyLoadingGuests = true;
                wcfService.GetGuestsByEventIdAndGuestNameAsync(eventId, GuestToSearch);
                wcfService.GetGuestsByEventIdAndGuestNameCompleted += wcfService_GetGuestsByEventIdAndGuestNameCompleted;
            }
        }

        void wcfService_GetGuestsByEventIdAndGuestNameCompleted(object sender, GetGuestsByEventIdAndGuestNameCompletedEventArgs e)
        {
            IsBusyLoadingGuests = false;
            Guests = null;
            Guests = e.Result;
        }

        #endregion


        
    }
}