namespace SimpleBattleCity
{
    public class PlayerData
    {
        public const string KeyOwner = "Owner";
        public const string KeyLivesLeft = "LivesLeft";
        public const string KeyPlayerLoadedStage = "PlayerLoadedStage";

        public int Owner = 1;
        public const int LivesMax = 2;
        public int LivesLeft = 2;
        // public int Score = 0;
        // public int KillsSmall = 0;
        // public int KillsFast = 0;
        // public int KillsBig = 0;
        // public int KillsArmored = 0;

        public bool PlayerLoadedStage;
    }
}