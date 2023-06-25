using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
namespace AJ.Generic.Tools
{
    public abstract class GameObjectsManager : BaseObjectManager
    {
        [SerializeField] protected int gameObjectCount = 10;
        [SerializeField] protected bool isDisable = false;
        private int amount = 0;
        private readonly List<GameObject> prefabs = new();
        public event System.Action<List<GameObject>> GetGameObjects;
        protected override void Initialization()
        {
            LoadGameObjectAsync();
        }
        protected virtual void LoadGameObjectAsync()
        {
            var instantiationParameters = CreateParameters();
            amount = gameObjectCount;
            for (int i = 0; i < gameObjectCount; i++)
            {
                var asyncOperationHandle = Addressables.InstantiateAsync(Address, instantiationParameters);
                asyncOperationHandle.Completed += Handle_Completed;
            }
            StartCoroutine(ISetGameObjects());
        }
        private void Handle_Completed(AsyncOperationHandle<GameObject> operation)
        {
            if (operation.Status == AsyncOperationStatus.Succeeded)
            {
                var result = operation.Result;
                result.AddComponent<GameObjectRelease>();
                result.SetActive(isDisable);
                prefabs.Add(operation.Result);
                amount--;
            } else
            {
                Debug.LogError($"Asset for {Address} failed to load.");
            }
        }
        protected abstract InstantiationParameters CreateParameters();
        private IEnumerator ISetGameObjects()
        {
            yield return new WaitUntil(() => amount <= 0);
            GetGameObjects?.Invoke(prefabs);
        }
    }
}
