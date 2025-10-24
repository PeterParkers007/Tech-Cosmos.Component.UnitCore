using UnityEditor;
using UnityEngine;
using UnitCore.Runtime.Abilities;
namespace UnitCore.Editor
{
    public class AbilityDebugger : EditorWindow
    {
        [MenuItem("Tech-Cosmos/能力调试器")]
        public static void ShowWindow()
        {
            GetWindow<AbilityDebugger>("能力调试器");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("扫描已注册能力"))
            {
                var abilities = AbilityFactory.GetRegisteredAbilityIds();
                foreach (var abilityId in abilities)
                {
                    Debug.Log($"找到能力: {abilityId}");
                }
            }
        }
    }
}
