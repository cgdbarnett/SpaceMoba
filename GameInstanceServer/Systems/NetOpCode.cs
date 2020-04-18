namespace GameInstanceServer.Systems
{
    /// <summary>
    /// OpCodes of incoming and outgoing messages to the client.
    /// </summary>
    public enum NetOpCode : short
    {
        ClientIsReady,
        StartGameCountdown,
        AssignLocalObject,
        RequestLocalObject,
        WelcomePacket,
        UpdatePlayerInput,
        UpdateObjects
    }
}
