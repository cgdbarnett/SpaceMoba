namespace GameInstanceServer.Game.Objects
{
    /// <summary>
    /// Implemented by objects that can be orbited such as motherships,
    /// planets and debri.
    /// </summary>
    public interface IOrbitable
    {
        /// <summary>
        /// Returns the distance at which objects should orbit.
        /// </summary>
        /// <returns>Orbit radius.</returns>
        int GetOrbitDistance();
    }
}
