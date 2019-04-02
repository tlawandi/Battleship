namespace Battleship
{
    /// <summary>
    /// I like to keep business concepts separate so that they can organically grow. hence why we have a player status and a ship status. To me they are different business concepts even though the still have the same value.
    /// </summary>
    public enum ShipStatus {
        Alive = 1,
        Sunk = 2
    }
}