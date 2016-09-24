public static class GameData
{
    //Health Player
    public const int MinHealth = 0;
    public const int MaxHealth = 230;
    public const float DefaultTime = 2;
    public const float HitTime = 10f;

    public const float sprintSpeed = 4f;

    public static float batteryPower = 100f;

    //Damage
    public const byte DamageByKick = 30;
    public const byte DamageByLaser = 10;
    public const byte DamageByBullet = 15;
    public const byte DamageByExplosion = 40;

    private static int health = 200;

    public static int Health
    {
        get
        {
            return health;
        }
        set
        {
            if (MinHealth <= value && value <= MaxHealth)
            {
                health = value;
            }
        }
    }

}