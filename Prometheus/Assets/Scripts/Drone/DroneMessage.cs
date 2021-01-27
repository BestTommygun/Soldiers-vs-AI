using UnityEngine;

namespace Assets.Scripts.Drone
{
    public enum MessageType
    {
        Enemy
    }
    public readonly struct DroneMessage
    {
        public readonly MessageType Type;
        public readonly GameObject Target;

        public DroneMessage(MessageType type) => (Type, Target) = (type, null);
        public DroneMessage(MessageType type, GameObject target) => (Type, Target) = (type, target);
    }
}
