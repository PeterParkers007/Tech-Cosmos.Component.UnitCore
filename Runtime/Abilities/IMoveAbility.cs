using UnityEngine;
namespace TechCosmos.UnitCore.Abilities
{
    // �ƶ������ӿ�
    public interface IMoveAbility : IUnitAbility
    {
        bool IsMoving { get; }
        void MoveTo(Vector2 position);
        void Stop();
    }
}
