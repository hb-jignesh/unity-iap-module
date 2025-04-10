using UnityEngine;

namespace IAPToolkit
{
    [CreateAssetMenu(fileName = "IAPProductConfig", menuName = "IAP/Product Config")]
    public class IAPProductConfig : ScriptableObject
    {
        public IAPProductEntry[] products;

        public IAPProductEntry GetProductByKey(string key)
        {
            foreach (var p in products)
                if (p.key == key)
                    return p;

            Debug.LogWarning($"[IAP] Product key not found: {key}");
            return null;
        }

        public IAPProductEntry GetProductByStoreId(string id)
        {
            foreach (var p in products)
                if (p.productId == id)
                    return p;

            Debug.LogWarning($"[IAP] Product ID not found: {id}");
            return null;
        }
    }
}