using System;
using System.Collections;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using System.Threading.Tasks;

namespace AJ.Generic.Service
{
    public class InitializeUnityServices : MonoBehaviour
    {
        private string environment = "production";
        async void Start()
        {
            try
            {
                var options = new InitializationOptions()
                    .SetEnvironmentName(environment);
                await UnityServices.InitializeAsync(options);
            }
            catch (Exception exception)
            {
                // An error occurred during services initialization.
                Debug.LogWarning(exception);
            }
        }
    }
}
