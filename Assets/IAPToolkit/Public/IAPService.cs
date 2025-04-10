using System;
using UnityEngine;

#if UNITY_IAP
using UnityEngine.Purchasing;
#endif

namespace IAPToolkit
{
    public class IAPService : BaseIAPManager, IIAPService
    {
        public static IAPService Instance { get; private set; }

        public static IAPProductConfig ProductConfig { get; private set; }

        public static bool UseMockPurchasing { get; set; } = false;

        public PurchaseSuccessEvent OnPurchaseSuccess = new();
        public PurchaseFailedEvent OnPurchaseFailed = new();
        public SubscriptionExpiredEvent OnSubscriptionExpired = new();
        public PurchaseResultEvent OnPurchaseCompleted = new();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            ProductConfig = Resources.Load<IAPProductConfig>("IAPProductConfig");

#if UNITY_IAP
            if (ProductConfig != null)
                Initialize(ProductConfig);
#endif
        }

        /// <summary>
        /// Call this to initiate a purchase by internal key (not product ID).
        /// </summary>
        public new void Purchase(string productKey)
        {
            var entry = ProductConfig?.GetProductByKey(productKey);
            if (entry == null)
            {
                Debug.LogError($"[IAP] Product not found: {productKey}");
                return;
            }

#if UNITY_IAP
            if (!UseMockPurchasing)
            {
                base.Purchase(entry.productId);
                return;
            }
#endif
            Debug.Log($"[IAP] MOCK purchase simulated for {productKey}");

            var result = new PurchaseResult
            {
                Key = entry.key,
                ProductId = entry.productId,
                Success = true,
                IsCanceled = false,
                Expiry = DateTime.UtcNow.AddYears(1)
            };

            OnPurchaseSuccess?.Invoke(entry.key);
            OnPurchaseCompleted?.Invoke(result);
        }

        public bool IsProductOwned(string key)
        {
#if UNITY_IAP
            if (!UseMockPurchasing)
                return EncryptedPrefs.GetBool(key);
#endif
            return true;
        }

        public bool IsSubscriptionActive(string key)
        {
#if UNITY_IAP
            if (!UseMockPurchasing && EncryptedPrefs.HasKey($"{key}_expiry"))
                return DateTime.Parse(EncryptedPrefs.GetString($"{key}_expiry")) > DateTime.UtcNow;
#endif
            return true;
        }

        public DateTime? GetSubscriptionExpiry(string key)
        {
#if UNITY_IAP
            if (!UseMockPurchasing && EncryptedPrefs.HasKey($"{key}_expiry"))
                return DateTime.Parse(EncryptedPrefs.GetString($"{key}_expiry"));
#endif
            return DateTime.UtcNow.AddYears(1);
        }

#if UNITY_IAP
        protected override void OnIAPInitialized()
        {
            Debug.Log("[IAP] Initialized successfully.");
        }

        public override void OnProcessValidatedPurchase(IAPProductEntry entry, Product unityProduct)
        {
            if (entry.productType == IAPProductType.Subscription)
            {
                var expiry = IAPReceiptValidator.GetSubscriptionExpiry(unityProduct);
                if (expiry.HasValue)
                    EncryptedPrefs.SetString($"{entry.key}_expiry", expiry.Value.ToString("o"));
            }
            else
            {
                EncryptedPrefs.SetBool(entry.key, true);
            }

            PlayerPrefs.Save();

            var result = new PurchaseResult
            {
                Key = entry.key,
                ProductId = entry.productId,
                Success = true,
                Expiry = GetSubscriptionExpiry(entry.key)
            };

            OnPurchaseSuccess?.Invoke(entry.key);
            OnPurchaseCompleted?.Invoke(result);
        }

        public void TriggerPurchaseFailed(string productId, PurchaseFailureReason reason)
        {
            var entry = ProductConfig?.GetProductByStoreId(productId);
            if (entry == null) return;

            var result = new PurchaseResult
            {
                Key = entry.key,
                ProductId = entry.productId,
                Success = false,
                IsCanceled = reason == PurchaseFailureReason.UserCancelled,
                FailureReason = reason
            };

            OnPurchaseFailed?.Invoke(entry.key, reason);
            OnPurchaseCompleted?.Invoke(result);
        }

        public void RestorePurchases()
        {
#if UNITY_IOS
            var apple = storeExtensionProvider.GetExtension<IAppleExtensions>();
            apple?.RestoreTransactions(result => Debug.Log("[IAP] Restore result: " + result));
#else
            Debug.Log("[IAP] RestorePurchases is only available on iOS.");
#endif
        }
#else
        // Safe mocks when UNITY_IAP is disabled
        protected override void OnIAPInitialized()
        {
            Debug.Log("[IAP] Initialized in mock mode.");
        }

        public override void OnProcessValidatedPurchase(IAPProductEntry entry, object unityProduct)
        {
            Debug.Log("[IAP] Processed mock purchase.");
        }

        public void TriggerPurchaseFailed(string productId, object reason)
        {
            Debug.LogWarning("[IAP] Purchase failed (mock).");
        }

        public void RestorePurchases()
        {
            Debug.Log("[IAP] RestorePurchases skipped. UNITY_IAP not defined.");
        }
#endif
    }
}
