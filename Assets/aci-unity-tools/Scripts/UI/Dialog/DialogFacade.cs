using Aci.Unity.UI.ViewControllers;
using System;

namespace Aci.Unity.UI.Dialog
{
    public class DialogFacade : IDialogFacade
    {
        private AlertDialogViewController.Factory m_AlertDialogFactory;
        private IDialogService m_DialogService;
        private ActivityIndicatorViewController.Factory m_ActivityIndicatorDialogFactory;

        [Zenject.Inject]
        public DialogFacade(IDialogService dialogService,
                            AlertDialogViewController.Factory alertDialogFactory,
                            ActivityIndicatorViewController.Factory activityIndicatorDialogFactory)
        {
            m_AlertDialogFactory = alertDialogFactory;
            m_DialogService = dialogService;
            m_ActivityIndicatorDialogFactory = activityIndicatorDialogFactory;
        }

        /// <inheritdoc />
        public IDialog DisplayActivity(string message)
        {
            return DisplayActivity(DialogPriority.High, message);
        }

        /// <inheritdoc />
        public IDialog DisplayActivity(DialogPriority priority, string message)
        {
            IDialog dialog = (DialogComponent) m_ActivityIndicatorDialogFactory.Create(message);
            m_DialogService.SendRequest(DialogRequest.Create(dialog, priority));
            return dialog;
        }

        /// <inheritdoc />
        public IDialog DisplayAlert(string title, string message, string cancel, string confirm, Action onConfirm = null, Action onCancel = null)
        {
            return DisplayAlert(DialogPriority.Medium, title, message, cancel, confirm, onConfirm, onCancel);
        }

        /// <inheritdoc />
        public IDialog DisplayAlert(DialogPriority priority, string title, string message, string cancel, string confirm, Action onConfirm = null, Action onCancel = null)
        {
            IDialog dialog = (DialogComponent) m_AlertDialogFactory.Create(title, message, cancel, confirm, onCancel, onConfirm);
            m_DialogService.SendRequest(DialogRequest.Create(dialog, priority));
            return dialog;
        }
    }
}
