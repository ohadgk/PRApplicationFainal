using Cirrious.MvvmCross.ViewModels;
using PrApplication.Clients.Windows8.Core.PrServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PrApplication.Clients.Windows8.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        PrServiceClient wcfService;


        #region Initialize and essintual props

        int eventId;

        string enPass;
        public LoginViewModel()
        {
            wcfService = new PrServiceClient();
        }
        protected override void InitFromBundle(IMvxBundle parameters)
        {
            eventId = 0;

            if (parameters.Data.ContainsKey("SelectedEventId"))
            {
                eventId = int.Parse(parameters.Data["SelectedEventId"]);
            }

            base.InitFromBundle(parameters);
        }
        #endregion

        #region Page FullProps

        //binds to the visibility of the error massege
        private bool _isErrorMassegeShown;

        public bool IsErrorMassegeShown
        {
            get { return _isErrorMassegeShown; }
            set { _isErrorMassegeShown = value; RaisePropertyChanged(() => IsErrorMassegeShown); }
        }
        //binds to the user name texbox to get user input
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; RaisePropertyChanged(() => UserName); }
        }
        //binds to the password texbox to get user input
        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; RaisePropertyChanged(() => Password); }
        }
        #endregion

        #region Navigation
        public ICommand CloseViewCommand
        {
            get
            {
                return new MvxCommand(() => { ShowViewModel<WelcomViewModel>(); });
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(UserName))
                        LoginFaild();
                    else
                    {
                        //encrypt the password so it will math the password on the db
                        wcfService.EncryptPassAsync(Password);
                        wcfService.EncryptPassCompleted += wcfService_EncryptPassCompleted;
                        wcfService.GetUsersAsync();
                        wcfService.GetUsersCompleted += wcfService_GetUsersCompleted;
                    }


                });
            }
        }
        private void wcfService_EncryptPassCompleted(object sender, EncryptPassCompletedEventArgs e)
        {
            enPass = e.Result;
        }

        //navigates to the right page by cheking if the user input is admin, if it is it takes him to the admin page,
        //if not, there must be an eventId to make login as host possible, if there is it will navigate to the host page.
        void wcfService_GetUsersCompleted(object sender, GetUsersCompletedEventArgs e)
        {
            foreach (var user in e.Result)
            {
                if (UserName == user.UserName && enPass == user.Password)
                {
                    if (user.IsAdmin)
                        ShowViewModel<EventsViewModel>();

                    else if (eventId != 0)
                        ShowViewModel<HostViewModel>(new { SelectedEventId = eventId, UserName = UserName });
                }
            }
            LoginFaild();
        }
        //show message, clead input fields.
        private void LoginFaild()
        {
            IsErrorMassegeShown = true;
            UserName = "";
            Password = "";
        }
        #endregion
    }
}
