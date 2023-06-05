namespace wobble.Animations
{
    public class AbilityHandler
    {
        protected int ActiveTime { get; }
        protected int CooldownTime { get; }
        private int TimeLeft { get; set; }

        public AbilityHandler(int duration, int cooldown)
        {
            this.ActiveTime = duration;
            this.CooldownTime = cooldown;
        }

        public bool IsActive()
        {
            return this.TimeLeft > 0 && this.TimeLeft <= this.ActiveTime;
        }

        public bool IsCoolingdown()
        {
            return this.TimeLeft > this.ActiveTime && this.TimeLeft <= this.getTotalTime();
        }

        public void ReduceAbilityTimer()
        {
            this.TimeLeft--;
        }

        public void Reset()
        {
            this.TimeLeft = this.getTotalTime();
        }

        private int getTotalTime()
        {
            return this.ActiveTime + this.CooldownTime;
        }
    }
}