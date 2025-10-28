using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechCosmos.UnitCore.Core;
namespace TechCosmos.UnitCore.Abilities
{
    // ���������ӿ�
    public interface IAttackAbility : IUnitAbility
    {
        bool IsAttacking { get; }
        void Attack(IUnit target);
        void StopAttack();
    }
}

