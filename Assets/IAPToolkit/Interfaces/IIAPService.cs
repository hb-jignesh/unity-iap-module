namespace IAPToolkit
{
    public interface IIAPService
    {
        void Purchase(string productKey);
        bool IsProductOwned(string productKey);
        bool IsSubscriptionActive(string productKey);
        System.DateTime? GetSubscriptionExpiry(string productKey);
        void RestorePurchases();
    }
}