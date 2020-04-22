namespace QRCodeManagement.Services
{
    public interface IConfigurationManagerWrapper
    {
        string DisplayEmailName { get; }

        string FromEmail { get; }

        string StatisticDomain { get; }
    }
}