using TechCosmos.UnitCore.Core;
namespace TechCosmos.UnitCore.Abilities
{
    // ���������ӿ�
    public interface IUnitAbility
    {
        string AbilityId { get; }
        void Initialize(IUnit unit);
        void Update(float deltaTime);
        void Dispose();
    }
}
