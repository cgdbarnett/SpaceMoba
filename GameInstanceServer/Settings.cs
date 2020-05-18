namespace GameInstanceServer
{
    /// <summary>
    /// Global settings.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Times to stay in each state for.
        /// </summary>
        #region StateTimers

        public const int MaxWaitingTime = 15;
        public const int CountdownTime = 6;
        public const int EndgameTime = 15;

        #endregion

        /// <summary>
        /// Network replication settings.
        /// </summary>
        #region NetReplication

        // Milliseconds
        public const long ReplicationMaxUpdatePeriod = 2000;

        #endregion

        #region Game

        public static int MaxTeamSize = 3;

        #endregion
    }
}
