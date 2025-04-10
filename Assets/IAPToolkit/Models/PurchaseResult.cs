using UnityEngine.Purchasing;
using System;

namespace IAPToolkit
{
    public struct PurchaseResult
    {
        public string Key;
        public string ProductId;
        public bool Success;
        public bool IsCanceled;
        public PurchaseFailureReason? FailureReason;
        public DateTime? Expiry;
    }
}