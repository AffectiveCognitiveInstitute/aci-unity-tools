using System.Threading.Tasks;

namespace Aci.Unity.UI
{
    public interface ITransition
    {
        Task EnterAsync();
        Task ExitAsync();
    }

    public interface ITransitionExtensions : ITransition
    {
        void ResetToBeginning(TransitionType transitionType);

        void Sample(TransitionType type, float factor);
    }

    public enum TransitionType
    {
        Enter,
        Exit
    }
}
