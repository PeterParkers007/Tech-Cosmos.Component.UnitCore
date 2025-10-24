using UnityEditor;
using UnityEngine;
using UnitCore.Runtime.Abilities;
namespace UnitCore.Editor
{
    public class AbilityDebugger : EditorWindow
    {
        [MenuItem("Tech-Cosmos/����������")]
        public static void ShowWindow()
        {
            GetWindow<AbilityDebugger>("����������");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("ɨ����ע������"))
            {
                var abilities = AbilityFactory.GetRegisteredAbilityIds();
                foreach (var abilityId in abilities)
                {
                    Debug.Log($"�ҵ�����: {abilityId}");
                }
            }
        }
    }
}
