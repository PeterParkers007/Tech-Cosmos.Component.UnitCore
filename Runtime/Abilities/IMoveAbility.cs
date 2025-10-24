using UnityEngine;
namespace UnitCore.Runtime.Abilities
{
    // �ƶ������ӿ�
    public interface IMoveAbility : IUnitAbility
    {
        bool IsMoving { get; }
        void MoveTo(Vector2 position);
        void Stop();
    }
}
