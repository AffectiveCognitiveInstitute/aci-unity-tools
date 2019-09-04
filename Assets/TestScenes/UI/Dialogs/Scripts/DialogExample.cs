using Aci.Unity.UI.Dialog;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class DialogExample : MonoBehaviour
{
    private IDialogFacade m_DialogFacade;

    [Zenject.Inject]
    private void Construct(IDialogFacade dialogFacade)
    {
        m_DialogFacade = dialogFacade;
    }

    public void ShowAlertDialog()
    {
        m_DialogFacade.DisplayAlert("Example Alert", "This is an example of the alert dialog with two buttons.", "Cancel", "OK",
            () => Debug.Log("Clicked Confirm"), () => Debug.Log("Clicked Cancel"));
    }

    public async void ShowActivityIndicator()
    {
        IDialog dialog = m_DialogFacade.DisplayActivity("Showing activity");

        await Task.Delay(TimeSpan.FromSeconds(2));

        dialog.Dismiss();
    }
}
