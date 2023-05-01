namespace wobble
{
    public static class Constants
    {
        public static Vector joystickVector = new Vector(0.2F, 0.75F, 0);

        public static LevelSettings LEVEL_A = new LevelSettings()
        {
            initPlayerVector = new Vector(0.5F, 0.5F, 0.0),
            initPistoleerVector = new Vector(0.75F, 0.1F, Utils.getRandomAngle()),
            initRammerVector = new Vector(0.1F, 0.1F, Utils.getRandomAngle()),
        };
    }

    public class LevelSettings
    {
        public Vector initPlayerVector;
        public Vector initPistoleerVector;
        public Vector initRammerVector;
    }
}
