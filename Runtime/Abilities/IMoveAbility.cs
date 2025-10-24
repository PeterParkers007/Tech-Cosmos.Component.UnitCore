using UnityEngine;
namespace UnitCore.Runtime.Abilities
{
    // 移动能力接口
    public interface IMoveAbility : IUnitAbility
    {
        bool IsMoving { get; }
        void MoveTo(Vector2 position);
        void Stop();
    }
}
