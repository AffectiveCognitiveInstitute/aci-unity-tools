using Aci.Unity.UI.ViewControllers;
using System;
using UnityEngine;
using Zenject;

namespace Aci.Unity.UI.Dialog
{
    public class DialogFacadeInstaller : MonoInstaller<DialogFacadeInstaller>
    {
        [SerializeField, Tooltip("The transform dialogs are parented to.")]
        private Transform m_DialogParentTransform;

        [SerializeField]
        private GameObject m_ActivityDialogPrefab;

        [SerializeField]
        private GameObject m_AlertDialogPrefab;

        public override void InstallBindings()
        {
            Container.Bind<IDialogFacade>().To<DialogFacade>().AsCached();

            Container.BindFactory<string, ActivityIndicatorViewController, ActivityIndicatorViewController.Factory>().
                      FromMonoPoolableMemoryPool(x => x.FromComponentInNewPrefab(m_ActivityDialogPrefab).UnderTransform(m_DialogParentTransform));

            Container.BindFactory<string, string, string, string, Action, Action, AlertDialogViewController, AlertDialogViewController.Factory>().
                      FromMonoPoolableMemoryPool(x => x.WithInitialSize(3).FromComponentInNewPrefab(m_AlertDialogPrefab).UnderTransform(m_DialogParentTransform));
        }
    }
}