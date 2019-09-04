using System.Threading.Tasks;
using UnityEngine;

namespace Aci.Unity.UI
{
    public class TweenerTransition : MonoBehaviour, ITransition
    {
        [SerializeField]
        private TweenerDirectorDecorator m_EnterGroup;

        [SerializeField]
        private TweenerDirectorDecorator m_ExitGroup;

        public Task EnterAsync()
        {
            if (m_EnterGroup != null)
                return ExecuteTween(m_EnterGroup);

            return Task.CompletedTask;
        }

        public Task ExitAsync()
        {
            if (m_ExitGroup != null)
                return ExecuteTween(m_ExitGroup);

            return Task.CompletedTask;
        }        

        private Task ExecuteTween(TweenerDirectorDecorator tweenerDirector)
        {
            return tweenerDirector.playReverse ? tweenerDirector.director.PlayReverseAsync() : tweenerDirector.director.PlayForwardsAsync();
        }
    }
}