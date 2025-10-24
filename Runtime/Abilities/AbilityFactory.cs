using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnitCore.Runtime.Abilities.Implementations;
using UnityEngine;

namespace UnitCore.Runtime.Abilities
{
    public static class AbilityFactory
    {
        private static Dictionary<string, Type> _abilityTypes = new Dictionary<string, Type>();
        private static bool _initialized = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            if (_initialized) return;

            ScanAndRegisterAbilities();
            _initialized = true;
        }

        private static void ScanAndRegisterAbilities()
        {
            _abilityTypes.Clear();

            // 扫描所有程序集中带有 AbilityAttribute 的类
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                // 跳过系统程序集提升性能
                if (assembly.FullName.StartsWith("System.") ||
                    assembly.FullName.StartsWith("Unity.") ||
                    assembly.FullName.StartsWith("UnityEngine.") ||
                    assembly.FullName.StartsWith("UnityEditor."))
                    continue;

                try
                {
                    var abilityTypes = assembly.GetTypes()
                        .Where(t =>
                            t.IsClass &&
                            !t.IsAbstract &&
                            typeof(IUnitAbility).IsAssignableFrom(t) &&
                            t.IsDefined(typeof(AbilityAttribute), false));

                    foreach (var type in abilityTypes)
                    {
                        var attribute = type.GetCustomAttribute<AbilityAttribute>();
                        if (attribute != null && !_abilityTypes.ContainsKey(attribute.AbilityId))
                        {
                            _abilityTypes[attribute.AbilityId] = type;
                            Debug.Log($"✅ 自动注册能力: {attribute.AbilityId} -> {type.Name}");
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"扫描程序集 {assembly.FullName} 时出错: {e.Message}");
                }
            }

            // 注册内置能力（确保覆盖）
            RegisterBuiltinAbilities();
        }

        private static void RegisterBuiltinAbilities()
        {
            // 内置能力手动注册，确保优先级
            _abilityTypes["Move"] = typeof(MoveAbility);
            _abilityTypes["Attack"] = typeof(AttackAbility);
        }

        public static IUnitAbility CreateAbility(string abilityId)
        {
            if (!_initialized) Initialize();

            if (_abilityTypes.TryGetValue(abilityId, out var type))
            {
                try
                {
                    return (IUnitAbility)Activator.CreateInstance(type);
                }
                catch (Exception e)
                {
                    Debug.LogError($"创建能力 {abilityId} 失败: {e.Message}");
                    return null;
                }
            }

            Debug.LogWarning($"未找到能力: {abilityId}");
            return null;
        }

        // 调试用：获取所有已注册的能力
        public static IEnumerable<string> GetRegisteredAbilityIds()
        {
            if (!_initialized) Initialize();
            return _abilityTypes.Keys;
        }
    }
}