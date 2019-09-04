namespace Aci.Unity.UI.Navigation
{
    public struct AnimationOptions
    {
        public bool enabled { get; private set; }
        public bool playSynchronously { get; private set; }

        /// <summary>
        /// No Animation will be played
        /// </summary>
        public static readonly AnimationOptions None = new AnimationOptions() { enabled = false, playSynchronously = false };

        /// <summary>
        /// Animations for both screens entering and exiting will be player simultaneously
        /// </summary>
        public static readonly AnimationOptions Synchronous = new AnimationOptions() { enabled = true, playSynchronously = true };

        /// <summary>
        /// Animations will be played one after the other
        /// </summary>
        public static readonly AnimationOptions Asynchronous = new AnimationOptions() { enabled = true, playSynchronously = false };
    }
}
