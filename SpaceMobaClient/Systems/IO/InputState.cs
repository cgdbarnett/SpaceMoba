namespace SpaceMobaClient.Systems.IO
{
    /// <summary>
    /// A struct to store input state between frames.
    /// </summary>
    public class InputState
    {
        // Movement input
        public bool Forward;
        public bool Backward;
        public bool Left;
        public bool Right;
        public bool StrafLeft;
        public bool StrafRight;
        public bool Attack;

        /// <summary>
        /// Returns whether this struct is equal in value to another.
        /// </summary>
        /// <param name="obj">Object to compare with.</param>
        /// <returns>Bool of whether the two structs are equal.</returns>
        /// <remarks>Using default implementation as it will automatically
        /// handle when the struct grows and changes.</remarks>
        public override bool Equals(object obj)
        {
            if (obj is InputState)
            {
                return base.Equals(obj);
            }
            return false;
        }

        /// <summary>
        /// == operator for InputState.
        /// </summary>
        /// <param name="in1">First InputState.</param>
        /// <param name="in2">Second InputState.</param>
        /// <returns>Whether the two states are equal.</returns>
        public static bool operator ==(InputState in1, InputState in2)
        {
            return in1.Equals(in2);
        }

        /// <summary>
        /// != operator for InputState.
        /// </summary>
        /// <param name="in1">First InputState.</param>
        /// <param name="in2">Second InputState.</param>
        /// <returns>Whether the two states are not equal.</returns>
        public static bool operator !=(InputState in1, InputState in2)
        {
            return !(in1.Equals(in2));
        }

        /// <summary>
        /// Returns the hash code representing this struct.
        /// </summary>
        /// <returns>Returns a hashcode representing this struct.</returns>
        /// <remarks>Using default implementation as it will automatically
        /// handle when the struct grows and changes.</remarks>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
