using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using PrApplication.Clients.Windows8.Core.Messeges;
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
    public class GuestDetailsViewModel : MvxViewModel
    {
    
        PrServiceClient wcfService = new PrServiceClient();
        #region Initialize and essintual props //load the page and sets the current event to the props
        int eventId;
        Event CorrentEvent;

        byte[] GuestImage;
        protected override void InitFromBundle(IMvxBundle parameters)
        {
            if (parameters.Data.ContainsKey("SelectedEventId"))
            {
                //get the selected event id
                eventId = int.Parse(parameters.Data["SelectedEventId"]);

                IsBusyLoadingGuests = true;

                Guests = null;

                //register to the IMvxMessenger to get the picture from the UI file picker.-
                GetImageAsByteArray();

                Initialize();
            }

            else
            {
                this.Close(this);
            }
            base.InitFromBundle(parameters);
        }

        public void Initialize()
        {
            FaildToAddGuest = false;
            IsGuestChosen = false;
            wcfService.GetEventByIDAsync(eventId);

            wcfService.GetEventByIDCompleted += wcfService_GetEventByIDCompleted;
        }

        private void wcfService_GetEventByIDCompleted(object sender, GetEventByIDCompletedEventArgs e)
        {
            CorrentEvent = e.Result;

            Guests = null;
            Guests = new ObservableCollection<Guest>(CorrentEvent.Guests);

            IsBusyLoadingGuests = false;
        }

        private MvxSubscriptionToken Token;
        private void GetImageAsByteArray()
        {
            IMvxMessenger messenger = Mvx.Resolve<IMvxMessenger>();

            Token = messenger.Subscribe<ByteArrMessage>((messege) =>
            {
                GuestImage = messege.PicAsByteArray;
            });
        }
        #endregion

        #region Ui Settings Properties

        //binds to the visibility of the loading bar on the UI
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
        //binds to the guests lst on the UI
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
        //binds to the selected guest from the guests list on the UI
        private Guest _SelectedGuest;

        public Guest SelectedGuest
        {
            get { return _SelectedGuest; }
            set
            {
                _SelectedGuest = value;
                if (SelectedGuest != null)
                {
                    SelectedGuest.Event = CorrentEvent;
                    IsGuestChosen = true;
                }
                RaisePropertyChanged(() => SelectedGuest);

            }
        }

        //binds to the visibilty of the guest deatels window
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
        #endregion

        #region Add Guest
        //binds to the visibility of the add guest window on the UI
        public ICommand AddGuestCommand
        {
            get
            {
                return new MvxCommand(() => { AddGuestChosen = true; });
            }
        }
        //binds to the visibilty of the  add guest window
        private bool _addGuestChosen;

        public bool AddGuestChosen
        {
            get { return _addGuestChosen; }
            set { _addGuestChosen = value; RaisePropertyChanged(() => AddGuestChosen); }
        }
        //binds to the textbox guest first name when 'add guest' pressed on the UI
        private string _guestFirstName;

        public string GuestFirstName
        {
            get { return _guestFirstName; }
            set { _guestFirstName = value; RaisePropertyChanged(() => GuestFirstName); }
        }
        //binds to the textbox guest last name when 'add guest' pressed on the UI
        private string _guestLastName;

        public string GuestLastName
        {
            get { return _guestLastName; }
            set { _guestLastName = value; RaisePropertyChanged(() => GuestLastName); }
        }
        //binds to the textbox guest companions name when 'add guest' pressed on the UI
        private string _guestCompanions;

        public string GuestCompanions
        {
            get { return _guestCompanions; }
            set { _guestCompanions = value; RaisePropertyChanged(() => GuestCompanions); }
        }

        private int setGuestCompanions;
        //binds to the textbox guest Qr code when 'add guest' pressed on the UI
        private string _guestQrCode;

        public string GuestQrCode
        {
            get { return _guestQrCode; }
            set { _guestQrCode = value; RaisePropertyChanged(() => GuestQrCode); }
        }
        //when pressing add guest this will occure:
        public ICommand AddNewGuestCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    IsGuestChosen = false;
                    GetImageAsByteArray();
                    int.TryParse(GuestCompanions, out setGuestCompanions);
                    var g = new Guest
                    {
                        FirstName = GuestFirstName,
                        LastName = GuestLastName,
                        Companions = setGuestCompanions,
                        QRCode = GuestQrCode,
                        Event = CorrentEvent,
                        EventId = eventId,
                        Image = GuestImage
                    };



                    wcfService.AddGuestAsync(eventId, g);
                    IsBusyLoadingGuests = true;
                    wcfService.AddGuestCompleted += wcfService_AddGuestCompleted;

                });
            }
        }
        void wcfService_AddGuestCompleted(object sender, AddGuestCompletedEventArgs e)
        {
            if (e.Result)
            {
                Initialize();
                GuestFirstName = null;
                GuestLastName = null;
                GuestCompanions = null;
                GuestQrCode = null;

            }
            else
            { 
                FaildToAddGuest = true;
                Initialize();
            }
        }

        private bool _faildToAddGuest = false;

        public bool FaildToAddGuest
        {
            get { return _faildToAddGuest; }
            set { _faildToAddGuest = value; RaisePropertyChanged(() => GuestCompanions);}
        }
        
        public ICommand CloseGuestControl
        {
            get
            {
                return new MvxCommand(() =>
                {
                    AddGuestChosen = false;
                });
            }
        }
        #endregion

        #region Remove Guest
        //pressing on this button will remove the selected guest.
        public ICommand RemoveGuestCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (SelectedGuest != null)
                    {
                        IsGuestChosen = false;
                        wcfService.RemoveGuestAsync(eventId, SelectedGuest.Id);
                        IsBusyLoadingGuests = true;
                        wcfService.RemoveGuestCompleted += wcfService_RemoveGuestCompleted;
                    }
                });
            }
        }

        void wcfService_RemoveGuestCompleted(object sender, RemoveGuestCompletedEventArgs e)
        {
            if (e.Result)
                Initialize();

        }

        #endregion

        #region Nevigation
        public ICommand CloseViewCommand
        {
            get
            {
                return new MvxCommand(() => { this.Close(this); });
            }
        }

        #endregion
    }
}
