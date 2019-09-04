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

    public void ShowLowPriorityAlertDialog()
    {
        m_DialogFacade.DisplayAlert(DialogPriority.Low, "Example Alert", "This is an example of a low priority alert dialog.", "Cancel", "OK",
            () => Debug.Log("Clicked Confirm"), () => Debug.Log("Clicked Cancel"));
    }

    public void ShowMediumPriorityAlertDialog()
    {
        m_DialogFacade.DisplayAlert(DialogPriority.Medium, "Example Alert", "This is an example of a medium priority alert dialog.", "Cancel", "OK",
            () => Debug.Log("Clicked Confirm"), () => Debug.Log("Clicked Cancel"));
    }

    public void ShowHighPriorityAlertDialog()
    {
        m_DialogFacade.DisplayAlert(DialogPriority.High, "Example Alert", "This is an example of a high priority alert dialog.", "Cancel", "OK",
            () => Debug.Log("Clicked Confirm"), () => Debug.Log("Clicked Cancel"));
    }

    public async void ShowActivityIndicator()
    {
        IDialog dialog = m_DialogFacade.DisplayActivity("Showing activity");

        await Task.Delay(TimeSpan.FromSeconds(2));

        dialog.Dismiss();
    }
}
