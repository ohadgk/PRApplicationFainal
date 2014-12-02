using Cirrious.MvvmCross.ViewModels;
using PrApplication.Clients.Windows8.Core.PrServiceReference;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PrApplication.Clients.Windows8.Core.ViewModels
{
    public class EventsViewModel : MvxViewModel
    {
        PrServiceClient wcfService;

        #region Get All Users
        private ObservableCollection<User> _allUsers;

        public ObservableCollection<User> AllUsers
        {
            get { return _allUsers; }
            set { _allUsers = value;  RaisePropertyChanged(() => AllUsers);}
        }
        private bool _isUserListVisible;

        public bool IsUserListVisible
        {
            get { return _isUserListVisible; }
            set { _isUserListVisible = value; RaisePropertyChanged(() => IsUserListVisible); }
        }

        private bool _loadingUsers;

        public bool LoadingUsers
        {
            get { return _loadingUsers; }
            set { _loadingUsers = value; RaisePropertyChanged(() => LoadingUsers); }
        }
        public ICommand GetAllUsersCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    LoadingUsers = true;
                    wcfService.GetUsersAsync();
                    wcfService.GetUsersCompleted += wcfService_GetUsersCompleted;
                });
            }
        }

        void wcfService_GetUsersCompleted(object sender, GetUsersCompletedEventArgs e)
        {
            AllUsers = e.Result;

            LoadingUsers = false;
            IsUserListVisible = true;
            
        }
        #endregion


        #region Initialize
        public EventsViewModel()
        {
            IsBusyLoadingEvents = true;
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
            PrEvents = null;
            PrEvents = new ObservableCollection<Event>(e.Result);
            ShowRemoveEventButton = false;
            IsBusyLoadingEvents = false;
        }

        #endregion

        #region Page Props

        //binds to the visibility of the loading bar on the UI
        private bool _isBusyLoadingEvents;
        public bool IsBusyLoadingEvents
        {
            get { return _isBusyLoadingEvents; }
            set { _isBusyLoadingEvents = value; RaisePropertyChanged(() => IsBusyLoadingEvents); }
        }


        //binds to the events lst on the UI
        private ObservableCollection<Event> _prEvents;
        public ObservableCollection<Event> PrEvents
        {
            get { return _prEvents; }
            set
            {
                _prEvents = value;
                RaisePropertyChanged(() => PrEvents);
            }
        }


        //binds to the selected event from the event list on the UI
        private Event _SelectedEvent;
        public Event SelectedEvent
        {
            get { return _SelectedEvent; }
            set
            {
                _SelectedEvent = value;
                ShowRemoveEventButton = true;
                RaisePropertyChanged(() => SelectedEvent);
            }

        }


        //binds to the visibilty of the remove event button, only if event has been chosen the button will apear.
        private bool _showRemoveEventButton;
        public bool ShowRemoveEventButton
        {
            get { return _showRemoveEventButton; }
            set { _showRemoveEventButton = value; RaisePropertyChanged(() => ShowRemoveEventButton); }
        }

        #endregion

        #region Add Event

        //binds to the visibility of the add event window on the UI

        private bool _addEventChosen;
        public bool AddEventChosen
        {
            get { return _addEventChosen; }
            set { _addEventChosen = value; RaisePropertyChanged(() => AddEventChosen); }
        }

        public ICommand AddEventCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    AddEventChosen = true;
                });
            }
        }


        //binds to the date picker when 'add event' pressed on the UI

        private DateTimeOffset _selectedDate = DateTimeOffset.Now;
        public DateTimeOffset SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = value; RaisePropertyChanged(() => SelectedDate); }
        }


        //binds to the textbox event name 'when add' event pressed on the UI
        private string _eventName;
        public string EventName
        {
            get { return _eventName; }
            set { _eventName = value; RaisePropertyChanged(() => EventName); }
        }


        //when pressing add event this will occure:
        public ICommand CreateEventCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (!string.IsNullOrWhiteSpace(EventName) && (!(SelectedDate.Date < DateTimeOffset.Now.Date)))
                    {
                        wcfService.CreateEventAsync(EventName, (DateTime)SelectedDate.Date);
                        IsBusyLoadingEvents = true;
                        wcfService.CreateEventCompleted += wcfService_CreateEventCompleted;
                    }
                    else
                        SelectedDate = DateTimeOffset.Now.Date;
                });
            }
        }
        void wcfService_CreateEventCompleted(object sender, CreateEventCompletedEventArgs e)
        {
            if (e.Result)
            {
                Initialize();
                EventName = null;
                SelectedDate = DateTime.Now.Date;
            }
        }

        public ICommand CloseEventControl
        {
            get
            {
                return new MvxCommand(() =>
                {
                    AddEventChosen = false;
                    EventName = null;
                    SelectedDate = DateTimeOffset.Now.Date;
                });
            }
        }

        #endregion

        #region Remove Event

        //pressing on this button only available when event is selected. it will remove it.
        public ICommand RemoveEventCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (SelectedEvent == null)
                        return;

                    wcfService.RemoveEventAsync(SelectedEvent.Id);
                    IsBusyLoadingEvents = true;
                    wcfService.RemoveEventCompleted += wcfService_RemoveEventCompleted;
                });
            }
        }

        private void wcfService_RemoveEventCompleted(object sender, RemoveEventCompletedEventArgs e)
        {
            if (e.Result)
                Initialize();
        }

        #endregion

        #region Search Events

        //Binding to the text box of search event by name

        private string _eventNameToSearch;
        public string EventNameToSearch
        {
            get { return _eventNameToSearch; }
            set { _eventNameToSearch = value; RaisePropertyChanged(() => EventNameToSearch); }
        }
        private bool _isDateActive;
        public bool IsDateActive
        {
            get { return _isDateActive; }
            set { _isDateActive = value; RaisePropertyChanged(() => IsDateActive); }
        }


        //Binding to the date picker of search event by date

        private DateTimeOffset _eventDateToSearch = DateTimeOffset.Now;
        public DateTimeOffset EventDateToSearch
        {
            get { return _eventDateToSearch; }
            set { _eventDateToSearch = value; RaisePropertyChanged(() => EventDateToSearch); }
        }

        public ICommand SearchEventsCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    IsBusyLoadingEvents = true;
                    SearchEvents();
                });
            }
        }


        //Method who decide in which parameters the search will be done.
        //Every year the company have been refresh so if the selected year is low from the current year-it means that we don't want to search by year.
        private void SearchEvents()
        {
            //Case- Name And Date
            if (!string.IsNullOrWhiteSpace(EventNameToSearch) && IsDateActive)
            {
                wcfService.GetEventAsync(EventNameToSearch, EventDateToSearch.DateTime);
                wcfService.GetEventCompleted += wcfService_GetEventCompleted;
            }
            //Case-Name only
            else if (!string.IsNullOrWhiteSpace(EventNameToSearch) && !IsDateActive)
            {
                wcfService.GetEventsByEventNameAsync(EventNameToSearch);
                wcfService.GetEventsByEventNameCompleted += wcfService_GetEventsByEventNameCompleted;
            }
            //Case-Date only
            else if (string.IsNullOrWhiteSpace(EventNameToSearch) && IsDateActive)
            {
                wcfService.GetEventsByEventDateAsync(EventDateToSearch.DateTime);
                wcfService.GetEventsByEventDateCompleted += wcfService_GetEventsByEventDateCompleted;
            }
            //Case-No one
            else if (string.IsNullOrWhiteSpace(EventNameToSearch) && !IsDateActive)
            {
                Initialize();
            }
        }


        //Case- search by date.
        void wcfService_GetEventsByEventDateCompleted(object sender, GetEventsByEventDateCompletedEventArgs e)
        {
            PrEvents = null;
            PrEvents = new ObservableCollection<Event>(e.Result);
            EventDateToSearch = DateTimeOffset.Now;
            IsBusyLoadingEvents = false;
        }


        //Case-Search by name.
        void wcfService_GetEventsByEventNameCompleted(object sender, GetEventsByEventNameCompletedEventArgs e)
        {
            PrEvents = null;
            PrEvents = new ObservableCollection<Event>(e.Result);
            EventNameToSearch = null;
            IsBusyLoadingEvents = false;
        }


        //Case-Search by event name and date
        void wcfService_GetEventCompleted(object sender, GetEventCompletedEventArgs e)
        {
            PrEvents = null;
            PrEvents = new ObservableCollection<Event>();
            PrEvents.Add(e.Result);
            EventNameToSearch = null;
            EventDateToSearch = DateTimeOffset.Now;
            IsBusyLoadingEvents = false;
        }

        #endregion

        #region Create User

        //binds to the visibility of the add user window on the UI
        private bool _addUserChosen;
        public bool AddUserChosen
        {
            get { return _addUserChosen; }
            set { _addUserChosen = value; RaisePropertyChanged(() => AddUserChosen); }
        }

        public ICommand AddUserChooseCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    AddUserChosen = true;
                });
            }
        }


        //binds to the textbox user name when 'add user' pressed on the UI
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; RaisePropertyChanged(() => UserName); }
        }


        //binds to the password when 'add user' pressed on the UI
        private string _userPassword;
        public string UserPassword
        {
            get { return _userPassword; }
            set { _userPassword = value; RaisePropertyChanged(() => UserPassword); }
        }

        //binds to the check box Is Administor when 'add user' pressed on the UI
        private bool _userIsAdmin;
        public bool UserIsAdmin
        {
            get { return _userIsAdmin; }
            set { _userIsAdmin = value; RaisePropertyChanged(() => UserIsAdmin); }
        }


        //whn pressing add user this will occur:
        public ICommand CreateUserCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    UserHasBeenCreated = false;
                    UserHasntBeenCreated = false;
                    if (UserPassword != null)
                    {
                        wcfService.AddUserAsync(UserName, UserPassword, UserIsAdmin);
                        wcfService.AddUserCompleted += wcfService_AddUserCompleted;
                    }
                });
            }
        }

        void wcfService_AddUserCompleted(object sender, AddUserCompletedEventArgs e)
        {
            
            if (e.Result)
            {
                UserPassword = null;
                UserName = null;
            }
            UserHasBeenCreated = e.Result;
            UserHasntBeenCreated = !e.Result;
        }


        //binds to the succsess messege visability

        private bool _userHasBeenCreated;
        public bool UserHasBeenCreated
        {
            get { return _userHasBeenCreated; }
            set { _userHasBeenCreated = value; RaisePropertyChanged(() => UserHasBeenCreated); }
        }

        //binds to the error messege visability

        private bool _userHasntBeenCreated;
        public bool UserHasntBeenCreated
        {
            get { return _userHasntBeenCreated; }
            set { _userHasntBeenCreated = value; RaisePropertyChanged(() => UserHasntBeenCreated); }
        }

        public ICommand CloseAddUserControl
        {
            get
            {
                return new MvxCommand(() =>
                {
                    AddUserChosen = false;
                    UserName = null;
                    UserPassword = null;
                });
            }
        }

        #endregion

        #region Remove User

        //binds to the visibility of the remove user window on the UI

        private bool _removeUserChosen;
        public bool RemoveUserChosen
        {
            get { return _removeUserChosen; }
            set { _removeUserChosen = value; RaisePropertyChanged(() => RemoveUserChosen); }
        }

        public ICommand RemoveUserChooseCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    RemoveUserChosen = true;
                });
            }
        }

        //binds to the textbox user name when 'remove user' pressed on the UI

        private string _userNameToRemove;
        public string UserNameToRemove
        {
            get { return _userNameToRemove; }
            set { _userNameToRemove = value; RaisePropertyChanged(() => UserNameToRemove); }
        }

        public ICommand RemoveUserCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    wcfService.RemoveUserByUserNameAsync(UserNameToRemove);
                    wcfService.RemoveUserByUserNameCompleted += wcfService_RemoveUserByUserNameCompleted;
                });
            }
        }

        void wcfService_RemoveUserByUserNameCompleted(object sender, RemoveUserByUserNameCompletedEventArgs e)
        {
            UserHasBeenRemoved = e.Result;
            UserHasntBeenRemoved = !e.Result;
            if(e.Result)
                UserNameToRemove = null;
        }

        //binds to the succsess messege visability
        private bool _userHasBeenRemoved;
        public bool UserHasBeenRemoved
        {
            get { return _userHasBeenRemoved; }
            set { _userHasBeenRemoved = value; RaisePropertyChanged(() => UserHasBeenRemoved); }
        }


        //binds to the error messege visability
        private bool _userHasntBeenRemoved;
        public bool UserHasntBeenRemoved
        {
            get { return _userHasntBeenRemoved; }
            set { _userHasntBeenRemoved = value; RaisePropertyChanged(() => UserHasntBeenRemoved); }
        }

        public ICommand CloseRemoveUserControl
        {
            get
            {
                return new MvxCommand(() =>
                {
                    RemoveUserChosen = false;
                    UserNameToRemove = null;
                });
            }
        }

        #endregion

        #region Navigation

        //navigates to spesific event as admin, sends the event id to the naxt page.
        public ICommand OpenGuestsCommand
        {
            get
            {
                return new MvxCommand<Event>((x) =>
                {
                    ShowViewModel<GuestDetailsViewModel>(new { SelectedEventId = SelectedEvent.Id });
                });
            }
        }

        //navigates to the welcome page
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
        #endregion

        
        //represh the event lst.
        public ICommand RefreshEventCommand { get { return new MvxCommand(() => { Initialize(); }); } }





















  





    }
}
