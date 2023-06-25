using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Security;

namespace AJ.Generic.Service
{
    /// <summary>
    /// 商品购买。
    /// </summary>
    public class AJIAP<T> : IDetailedStoreListener where T : AJIAP<T>, new()
    {
        protected AJIAP()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            builder.Configure<IGooglePlayConfiguration>().SetServiceDisconnectAtInitializeListener(() =>
            {
                AJFirebase.Log("Unable to connect to the Google Play Billing service. " +
                    "User may not have a Google account on their device.");
            });
            AddProduct(builder);    
            UnityPurchasing.Initialize(this, builder);
        }
        protected static T _instance;
        protected IStoreController controller;
        protected IExtensionProvider extensions;
        public static T Instance => _instance;
        public IStoreController Controller => controller;
        /// <summary>
        /// 启动IAP单例。
        /// </summary>
        /// <returns></returns>
        public static T Activate()
        {
            if (_instance != null) return _instance;
            _instance = new();
            return _instance;
        }
        public static T Activate(params ProductDefinition[] productDefinitions)
        {
            if (_instance != null) return _instance;
            _instance = new();
            return _instance;
        }
        #region 加载IAP
        void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            this.controller = controller;
            this.extensions = extensions;
            Initialized(controller, extensions);
        }
        void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
        {
            var log = System.String.Format("初始化失败,错误代码{0}", error);
            AJFirebase.Log(log);
            InitializeFailed(error);
        }
        void IStoreListener.OnInitializeFailed(InitializationFailureReason error, string message)
        {
            var log = System.String.Format("初始化失败,错误代码{0},{1}", error, message);
            AJFirebase.Log(log);
            InitializeFailed(error, message);
        }
        void IStoreListener.OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            var log = System.String.Format(
                "购买{0}失败,商品类型{1},错误代码{2}", product.definition.id, product.definition.type, failureReason);
            AJFirebase.Log(log);
            PurchaseFailed(product, failureReason);
        }
        void IDetailedStoreListener.OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            
        }
        PurchaseProcessingResult IStoreListener.ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            var purchaseProcessingResult = ProcessPurchase(purchaseEvent);
            return purchaseProcessingResult;
        }
        #endregion
        #region 自定义
        protected virtual void AddProduct(ConfigurationBuilder builder)
        {
            // foreach (var productDefinition in productDefinitions)
            // {
            //     if (productDefinition.type != ProductType.Consumable) serviceProductIDs.Add(productDefinition.id);
            //     builder.AddProduct(productDefinition.id, ProductType.Consumable); 
            // } 
        }
        protected virtual void Initialized(IStoreController controller, IExtensionProvider extensions)
        {
            // FetchingAdditionalProducts(controller, productDefinitions);
            // extensions.GetExtension<IAppleExtensions>().RestoreTransactions(RestoreTransactions);
        }
        protected virtual void RestoreTransactions(bool result, string error)
        {
            if (result) 
            {
                // This does not mean anything was restored,
                // merely that the restoration process succeeded.
                // foreach (var id in serviceProductIDs)
                // {
                //     var product = controller.products.WithID(id);
                //     ValidPurchase(product);
                // }          
            } else {
                // Restoration failed. `error` contains the failure reason.
            }
        }
        protected virtual void FetchingAdditionalProducts(IStoreController controller, HashSet<ProductDefinition> productDefinitions)
        {
            var additional = productDefinitions;
            Action onSuccess = () => {
                AJFirebase.Log("Fetched successfully!");
                foreach (var product in controller.products.all) {
                    AJFirebase.Log(product.definition.id);
                    // if (product.definition.type != ProductType.Consumable) serviceProductIDs.Add(product.definition.id);
                }
            };

            Action<InitializationFailureReason, string> onFailure = (result, error) => {
                AJFirebase.Log("Fetching failed for the specified reason: " + result);
            };

            controller.FetchAdditionalProducts(additional, onSuccess, onFailure);
        }
        protected virtual void InitializeFailed(InitializationFailureReason error)
        {
            
        }
        protected virtual void InitializeFailed(InitializationFailureReason error, string message)
        {
            
        }
        protected virtual void PurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            
        }
        protected virtual PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            return PurchaseProcessingResult.Complete;
        }
        protected virtual void ValidPurchase(Product product)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor) return;
            var validPurchase = false;
            var purchaseId = "";
            var purchasedProduct = product;
            try
            {
                var validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
                var result = validator.Validate(purchasedProduct.receipt);
                foreach (IPurchaseReceipt productReceipt in result) 
                {
                    var google = productReceipt as GooglePlayReceipt;
                    if (google != null)
                    {
                        if (google.purchaseState == GooglePurchaseState.Purchased)
                        {
                            AJFirebase.Log("Purchased:" + google.productID);
                            validPurchase = true;
                            purchaseId = google.productID;
                        } else if (google.purchaseState == GooglePurchaseState.Cancelled)
                        {
                            AJFirebase.Log("Cancelled:" + google.productID);
                            validPurchase = false;
                        } else if (google.purchaseState == GooglePurchaseState.Refunded)
                        {
                            AJFirebase.Log("Refunded:" + google.productID);
                            validPurchase = false;
                        }
                    } else
                    {
                        AJFirebase.Log("空的:" + google);
                        validPurchase = false;
                    }
                }
            } catch (IAPSecurityException)
            {
                AJFirebase.Log("Invalid receipt, not unlocking content");
                validPurchase = false;
            }
            if (validPurchase)
            {
                // purchase?.Invoke(purchaseId);
                // services?.Invoke(validPurchase, purchaseId);
            }
        }
        #endregion
    }
}
