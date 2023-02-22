namespace wobble.Animations
{
    public class AbilityHandler
    {
        protected int AbilityMaxTimer { get; }
        protected int AbilityTimer { get; set; }

        public AbilityHandler(int abilityMaxTimer)
        {
            this.AbilityMaxTimer = abilityMaxTimer;
        }

        public bool HasAbilityTimeLeft()
        {
            return this.AbilityTimer > 0;
        }

        public void ReduceTimer()
        {
            this.AbilityTimer--;
        }

        public void ResetAbilityTimer()
        {
            this.AbilityTimer = this.AbilityMaxTimer;
        }
    }
}