using System;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace ValheimPassword
{
    [BepInPlugin("org.bepinex.plugins.valheimpassword", "Valheim Password", "0.6.0")]
    public class ValheimPassword : BaseUnityPlugin
    {
        private static readonly Harmony harmony = new Harmony("com.valheimpassword.patches");

        void Awake()
        {
            harmony.PatchAll();
        }

    }

    public class ServerPasswordPatches
    {
        private static string GetArg(string name)
        {
            var args = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == name && args.Length > i + 1)
                {
                    return args[i + 1];
                }
            }
            return null;
        }

        [HarmonyPatch(typeof(InputField), "OnFocus")]
        class PatchPasswordFieldFocus {
            public static void Postfix(InputField __instance) {
                if(__instance.inputType == InputField.InputType.Password) {
                    var passwordArg = GetArg("+password");
                    if(!String.IsNullOrEmpty(passwordArg))
                    {
                        __instance.text = passwordArg;
                        __instance.GetComponent<InputFieldSubmit>()?.m_onSubmit?.Invoke(passwordArg);
                    }
                }
            }
        }
    }
}
