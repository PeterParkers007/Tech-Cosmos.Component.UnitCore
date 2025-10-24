using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitCore.Runtime.Core;
namespace UnitCore.Runtime.Abilities
{
    // ���������ӿ�
    public interface IAttackAbility : IUnitAbility
    {
        bool IsAttacking { get; }
        void Attack(IUnit target);
        void StopAttack();
    }
}

