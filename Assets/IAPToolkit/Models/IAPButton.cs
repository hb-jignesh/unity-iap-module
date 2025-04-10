using UnityEngine;
using UnityEngine.UI;

namespace IAPToolkit
{
    [RequireComponent(typeof(Button))]
    public class IAPButton : MonoBehaviour
    {
        public string productKey;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (string.IsNullOrEmpty(productKey))
            {
                Debug.LogWarning("[IAPButton] No product key assigned.");
                return;
            }

            if (IAPService.Instance == null)
            {
                Debug.LogWarning("[IAPButton] IAPService not initialized.");
                return;
            }

            IAPService.Instance.Purchase(productKey);
        }
    }
}