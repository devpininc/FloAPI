namespace FloApi.Email.Config
{
    public class AwsSesSettings
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Region { get; set; }
        public SesSenderSettings SES { get; set; }
    }

    public class SesSenderSettings
    {
        public string FromAddress { get; set; }
        public string FromName { get; set; }
    }
}
