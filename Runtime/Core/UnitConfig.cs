using UnityEngine;
using TechCosmos.UnitCore.Properties;
namespace TechCosmos.UnitCore.Core
{
    [CreateAssetMenu(fileName = "UnitConfig", menuName = "Tech-Cosmos/Unit/UnitConfig")]
    public class UnitConfig : ScriptableObject
    {
        [Header("基础属性")]
        public string unitId;
        public UnitTeam team;
        public float maxHealth = 100f;

        [Header("移动属性")]
        public float movementSpeed = 5f;
        public float stoppingDistance = 0.1f;

        [Header("攻击属性")]
        public float attackRange = 2f;
        public float attackCooldown = 1f;
        public float attackDamage = 10f;

        [Header("能力配置")]
        public string[] abilityIds = { "Move", "Attack" };
    }
}
