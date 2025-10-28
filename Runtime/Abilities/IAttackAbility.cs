using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechCosmos.UnitCore.Core;
namespace TechCosmos.UnitCore.Abilities
{
    // 攻击能力接口
    public interface IAttackAbility : IUnitAbility
    {
        bool IsAttacking { get; }
        void Attack(IUnit target);
        void StopAttack();
    }
}

