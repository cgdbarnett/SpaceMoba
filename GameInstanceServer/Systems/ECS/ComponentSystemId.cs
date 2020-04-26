using System;

namespace GameInstanceServer.Systems.ECS
{
    /// <summary>
    /// The identifier for a System within the ECS. Systems should extend this
    /// class and implement their new Id as a public static readonly
    /// ComponentSystemId.
    /// </summary>
    public partial class ComponentSystemId : IComparable
    {
        // Id of a particular component.
        public readonly int Id;
        private static int NextComponentId = 0;

        /// <summary>
        /// Creates a new ComponentSystemId.
        /// </summary>
        public ComponentSystemId()
        {
            Id = NextComponentId++;
        }

        /// <summary>
        /// Compares the id of this object to a given object for sorting.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            return Id.CompareTo(obj);
        }

        /// <summary>
        /// Overrides Object.Equals to only compare based on Id.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>Whether the ids match.</returns>
        public override bool Equals(object obj)
        {
            if(obj is ComponentSystemId)
            {
                return Id == ((ComponentSystemId)obj).Id;
            }
            else if(obj is int)
            {
                return Id == (int)obj;
            }
            else
            {
                throw (new ArgumentException());
            }
        }
    }
}
