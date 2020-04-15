using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Aci.Unity.Events
{
    /// <summary>
    /// Triggers UnityEvents on collider/trigger events.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class UnityEventTrigger : MonoBehaviour
    {
        [Flags]
        public enum EventMode
        {
            OnEnter = 1,
            OnExit = OnEnter << 1,
            OnStay = OnEnter << 2
        }

        public enum CollisionMode
        {
            Collisions,
            Triggers
        }

        [Tooltip("Type of collisions that will trigger events.")]
        public CollisionMode collisionMode;
        [Tooltip("Type of trigger/collision events that will trigger events.")]
        public EventMode invokeOn;
        [Tooltip("GameObject allowed to trigger events, can be unsepecified.")]
        public GameObject activatingTarget;
        [Tooltip("Layers that will trigger events.")]
        public LayerMask activatingLayers;
        [Tooltip("Delay, in seconds, until events are triggered.")]
        float delay;

        [Tooltip("Events that will be triggered.")]
        public UnityEvent triggeredEvents;

        private void OnTriggerEnter(Collider other)
        {
            if (collisionMode != CollisionMode.Triggers || !invokeOn.HasFlag(EventMode.OnEnter))
                return;
            CheckTrigger(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if (collisionMode != CollisionMode.Triggers || !invokeOn.HasFlag(EventMode.OnStay))
                return;
            CheckTrigger(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (collisionMode != CollisionMode.Triggers || !invokeOn.HasFlag(EventMode.OnExit))
                return;
            CheckTrigger(other.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collisionMode != CollisionMode.Collisions || !invokeOn.HasFlag(EventMode.OnEnter))
                return;
            CheckTrigger(collision.other.gameObject);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collisionMode != CollisionMode.Collisions || !invokeOn.HasFlag(EventMode.OnStay))
                return;
            CheckTrigger(collision.other.gameObject);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collisionMode != CollisionMode.Collisions || !invokeOn.HasFlag(EventMode.OnExit))
                return;
            CheckTrigger(collision.other.gameObject);
        }

        private async void CheckTrigger(GameObject other)
        {
            if (((1 << other.layer) & activatingLayers.value) == 0)
                return;
            if (activatingTarget != null && other.gameObject != activatingTarget)
                return;
            await Task.Delay(Mathf.FloorToInt(1000 * delay));
            triggeredEvents.Invoke();
        }
    }
}
