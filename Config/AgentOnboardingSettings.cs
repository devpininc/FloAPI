namespace FloAPI.Config
{
    public class AgentOnboardingSettings
    {
        public int TrialDays { get; set; } = 14;
        public bool SendMagicLinkOnCreate { get; set; } = true;
    }
}
