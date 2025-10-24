using UnitCore.Runtime.Core;
namespace UnitCore.Runtime.Abilities
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
