using UnityEngine;
using TechCosmos.UnitCore.Properties;
namespace TechCosmos.UnitCore.Core
{
    [CreateAssetMenu(fileName = "UnitConfig", menuName = "Tech-Cosmos/Unit/UnitConfig")]
    public class UnitConfig : ScriptableObject
    {
        [Header("��������")]
        public string unitId;
        public UnitTeam team;
        public float maxHealth = 100f;

        [Header("�ƶ�����")]
        public float movementSpeed = 5f;
        public float stoppingDistance = 0.1f;

        [Header("��������")]
        public float attackRange = 2f;
        public float attackCooldown = 1f;
        public float attackDamage = 10f;

        [Header("��������")]
        public string[] abilityIds = { "Move", "Attack" };
    }
}
