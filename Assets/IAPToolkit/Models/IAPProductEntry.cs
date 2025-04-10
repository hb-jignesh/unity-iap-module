using UnityEngine;

namespace IAPToolkit
{
    [System.Serializable]
    public class IAPProductEntry
    {
        public string key;
        public string productId;
        public bool overrideStoreIds;
        public string androidProductId;
        public string iosProductId;
        public IAPProductType productType;
        public string price;

#if UNITY_IAP
        public UnityEngine.Purchasing.ProductType GetUnityProductType() => productType switch
        {
            IAPProductType.Consumable => UnityEngine.Purchasing.ProductType.Consumable,
            IAPProductType.NonConsumable => UnityEngine.Purchasing.ProductType.NonConsumable,
            IAPProductType.Subscription => UnityEngine.Purchasing.ProductType.Subscription,
            _ => UnityEngine.Purchasing.ProductType.Consumable
        };
#endif

        public string GetStoreSpecificProductId()
        {
#if UNITY_ANDROID
            return overrideStoreIds && !string.IsNullOrEmpty(androidProductId) ? androidProductId : productId;
#elif UNITY_IOS
            return overrideStoreIds && !string.IsNullOrEmpty(iosProductId) ? iosProductId : productId;
#else
            return productId;
#endif
        }
    }

    public enum IAPProductType
    {
        Consumable,
        NonConsumable,
        Subscription
    }
}