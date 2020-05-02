namespace SpaceMobaClient.GamePlay.Network
{
    /// <summary>
    /// Enum of Outgoing Message Identifiers.
    /// (Op codes)
    /// </summary>
    public enum NetOpCode : short
    {
        ClientIsReady,
        StartGameCountdown,
        AssignLocalObject,
        RequestLocalObject,
        WelcomePacket,
        UpdatePlayerInput,
        CreateObject,
        UpdateObject,
        DestroyObject
    }
}
