using Aci.Unity.UI.Tweening;
using System.Threading.Tasks;
using UnityEngine;

namespace Aci.Unity.UI.Navigation
{
    public class TweenerScreenTransition : ScreenTransition
    {
        [System.Serializable]
        public class TweenerDirectorWrapper
        {
            public bool playReverse;
            public TweenerDirector director;
        }

        [SerializeField]
        private bool m_UseSameTweenForReturnAndDestroy = true;

        [SerializeField]
        private TweenerDirectorWrapper m_EnterTween;
        [SerializeField]
        private TweenerDirectorWrapper m_ReturnTween;
        [SerializeField]
        private TweenerDirectorWrapper m_LeavingTween;
        [SerializeField]
        private TweenerDirectorWrapper m_DestroyedTween;

        public override void DisplayImmediately()
        {
            for (int i = 0; i < m_EnterTween.director.tweeners.Length; i++)
                m_EnterTween.director.tweeners[i].Seek(0);
        }

        public override Task OnScreenBeingRemoved()
        {
            return m_UseSameTweenForReturnAndDestroy ? ExecuteTween(m_LeavingTween) : ExecuteTween(m_DestroyedTween);
        }

        public override Task OnScreenEntering()
        {
            return ExecuteTween(m_EnterTween);
        }

        public override Task OnScreenLeaving()
        {
            return ExecuteTween(m_LeavingTween);
        }

        public override Task OnScreenReturning()
        {
            return m_UseSameTweenForReturnAndDestroy ? ExecuteTween(m_EnterTween) : ExecuteTween(m_ReturnTween);
        }

        private Task ExecuteTween(TweenerDirectorWrapper tweener)
        {
            return !tweener.playReverse? tweener.director.PlayForwardsAsync() : tweener.director.PlayReverseAsync();
        }
    }
}
