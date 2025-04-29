using System;
using System.Reflection;
using HarmonyLib;
using UnityEditor;
using UnityEngine;
#if LVU_HAS_VRCSDK3
using VRC.Editor;
#endif

namespace KisaragiMarine.LinuxVulkanOnUnity
{
    [InitializeOnLoad]
    internal static class Patcher
    {
        static Patcher()
        {
#if LVU_HAS_VRCSDK3
            if (Application.platform != RuntimePlatform.LinuxEditor)
            {
                Debug.Log("[VRCSDKForceVulkan] Not running on Linux Editor, skipping patch.");
                return;
            }

            try
            {
                var harmony = new Harmony("io.github.kisaragieffective.vrchat.forcevulkan");

                var envConfigType = typeof(EnvConfig);
                var method = envConfigType.GetMethod("SetDefaultGraphicsAPIs", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                if (method == null)
                {
                    Debug.LogWarning("[VRCSDKForceVulkan] Could not find SetDefaultGraphicsAPIs method.");
                    return;
                }

                harmony.Patch(method, prefix: new HarmonyMethod(typeof(Patcher), nameof(SkipGraphicsAPIOverride)));
                Debug.Log("[VRCSDKForceVulkan] Successfully patched SetDefaultGraphicsAPIs.");
            }
            catch (Exception e)
            {
                Debug.LogError("[VRCSDKForceVulkan] Failed to patch: " + e);
            }
#endif
        }

        private static bool SkipGraphicsAPIOverride()
        {
            Debug.Log("[VRCSDKForceVulkan] Skipping VRCSDK graphics API override (forcing Vulkan).");
            return false;
        }
    }
}
