using UnityEngine;

namespace IAPToolkit
{
    public abstract class BaseIAPManager : MonoBehaviour
#if UNITY_IAP
        , UnityEngine.Purchasing.IStoreListener
#endif
    {
#if UNITY_IAP
        protected UnityEngine.Purchasing.IStoreController storeController;
        protected UnityEngine.Purchasing.IExtensionProvider storeExtensionProvider;

        public void Initialize(IAPProductConfig config)
        {
            if (storeController != null) return;

            var builder = UnityEngine.Purchasing.ConfigurationBuilder.Instance(
                UnityEngine.Purchasing.StandardPurchasingModule.Instance());

            foreach (var product in config.products)
            {
                builder.AddProduct(product.productId, product.GetUnityProductType());
            }

            UnityEngine.Purchasing.UnityPurchasing.Initialize(this, builder);
        }

        public void Purchase(string productId)
        {
            if (storeController == null)
            {
                Debug.LogWarning("IAP not initialized.");
                return;
            }

            var product = storeController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
                storeController.InitiatePurchase(product);
            else
                Debug.LogWarning("Product unavailable: " + productId);
        }

        public abstract void OnProcessValidatedPurchase(IAPProductEntry entry, UnityEngine.Purchasing.Product unityProduct);
        protected abstract void OnIAPInitialized();

        public void OnInitialized(UnityEngine.Purchasing.IStoreController controller, UnityEngine.Purchasing.IExtensionProvider extensions)
        {
            storeController = controller;
            storeExtensionProvider = extensions;
            OnIAPInitialized();
        }

        public UnityEngine.Purchasing.PurchaseProcessingResult ProcessPurchase(UnityEngine.Purchasing.PurchaseEventArgs args)
        {
            var productId = args.purchasedProduct.definition.id;
            var entry = IAPService.ProductConfig.GetProductByStoreId(productId);

            if (entry != null && IAPReceiptValidator.Validate(args.purchasedProduct))
            {
                OnProcessValidatedPurchase(entry, args.purchasedProduct);
            }

            return UnityEngine.Purchasing.PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, UnityEngine.Purchasing.PurchaseFailureReason reason)
        {
            IAPService.Instance.TriggerPurchaseFailed(product.definition.id, reason);
        }

        public void OnInitializeFailed(UnityEngine.Purchasing.InitializationFailureReason error)
        {
            Debug.LogError("[IAP] Initialization failed: " + error);
        }

#else
        // When UNITY_IAP is NOT enabled, these are no-ops so the project still compiles

        public void Initialize(IAPProductConfig config) { }
        public void Purchase(string productId) { }

        public abstract void OnProcessValidatedPurchase(IAPProductEntry entry, object unityProduct);
        protected abstract void OnIAPInitialized();
#endif
    }
}
