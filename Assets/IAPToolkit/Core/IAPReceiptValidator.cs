using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using System;
using System.Collections.Generic;

namespace IAPToolkit
{
    public static class IAPReceiptValidator
    {
#if UNITY_IAP
        public static bool Validate(Product product)
        {
            if (product == null || string.IsNullOrEmpty(product.receipt)) return false;

            try
            {
                var wrapper = MiniJson.JsonDecode(product.receipt) as Dictionary<string, object>;
                string store = wrapper["Store"] as string;
                string payload = wrapper["Payload"] as string;

                IPurchaseReceipt[] receipts = store switch
                {
                    "GooglePlay" => new GooglePlayValidator(GooglePlayTangle.Data(), UnityEngine.Application.identifier).Validate(payload),
                    "AppleAppStore" => new AppleValidator(AppleTangle.Data()).Validate(payload),
                    _ => null
                };

                foreach (var receipt in receipts)
                {
                    if (receipt.productID == product.definition.id)
                    {
                        if (receipt is SubscriptionReceipt sub)
                            return sub.expirationDate > DateTime.UtcNow;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning("Receipt validation failed: " + ex.Message);
            }

            return false;
        }

        public static DateTime? GetSubscriptionExpiry(Product product)
        {
            if (product == null || string.IsNullOrEmpty(product.receipt)) return null;

            try
            {
                var wrapper = MiniJson.JsonDecode(product.receipt) as Dictionary<string, object>;
                string store = wrapper["Store"] as string;
                string payload = wrapper["Payload"] as string;

                IPurchaseReceipt[] receipts = store switch
                {
                    "GooglePlay" => new GooglePlayValidator(GooglePlayTangle.Data(), UnityEngine.Application.identifier).Validate(payload),
                    "AppleAppStore" => new AppleValidator(AppleTangle.Data()).Validate(payload),
                    _ => null
                };

                foreach (var receipt in receipts)
                {
                    if (receipt.productID == product.definition.id && receipt is SubscriptionReceipt sub)
                        return sub.expirationDate;
                }
            }
            catch { }

            return null;
        }
#else
        public static bool Validate(Product product) => true;
        public static DateTime? GetSubscriptionExpiry(Product product) => DateTime.UtcNow.AddYears(1);
#endif
    }
}
