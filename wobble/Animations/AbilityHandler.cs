namespace wobble.Animations
{
    public class AbilityHandler
    {
        protected int Duration { get; }
        public int AbilityTimeLeft { get; set; }

        protected int Cooldown { get; }
        public int CoolDownTimeLeft { get; set; }

        public AbilityHandler(int duration, int cooldown)
        {
            this.Duration = duration;
            this.Cooldown = cooldown;
        }

        public bool IsActive()
        {
            return this.AbilityTimeLeft > 0;
        }

        public void ReduceAbilityTimer()
        {
            this.AbilityTimeLeft--;
        }

        public void ResetAbilityTimer()
        {
            this.AbilityTimeLeft = this.Duration;
        }

        public bool IsCoolingdown()
        {
            return this.CoolDownTimeLeft > 0;
        }

        public void ReduceCooldownTimer()
        {
            this.CoolDownTimeLeft--;
        }

        public void ResetCooldownTimer()
        {
            this.CoolDownTimeLeft = this.Cooldown;
        }


    }
}