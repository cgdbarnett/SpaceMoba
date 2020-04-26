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
    }
}
