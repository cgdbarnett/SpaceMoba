using Lidgren.Network;

namespace GameInstanceServer.Systems.ECS
{
    /// <summary>
    /// An interface for a Component with the ECS.
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// The identifier of the System that controls this component.
        /// </summary>
        ComponentSystemId ComponentSystem { get; }

        /// <summary>
        /// A flag to indicate whether this component should be serialized.
        /// </summary>
        bool Serializable { get; }

        /// <summary>
        /// Serialize this component into the outgoing message.
        /// </summary>
        /// <param name="msg">Message to append to.</param>
        void Serialize(NetOutgoingMessage msg);
    }
}
