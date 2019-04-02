namespace Battleship
{
    public class Player
    {
        private readonly Fleet _fleet;

        public Player(Fleet fleet)
        {
            _fleet = fleet;
        }

        public PlayerStatus GetStatus()
        {
            return _fleet.GetStatus() == FleetStatus.Destroyed ? PlayerStatus.Lost : PlayerStatus.Alive;
        }

    }
}