using UnityEngine.Events;
using UnityEngine.Purchasing;
using System;

namespace IAPToolkit
{
    [Serializable]
    public class PurchaseSuccessEvent : UnityEvent<string> { }

    [Serializable]
    public class PurchaseFailedEvent : UnityEvent<string, PurchaseFailureReason> { }

    [Serializable]
    public class SubscriptionExpiredEvent : UnityEvent<string, DateTime> { }

    [Serializable]
    public class PurchaseResultEvent : UnityEvent<PurchaseResult> { }
}