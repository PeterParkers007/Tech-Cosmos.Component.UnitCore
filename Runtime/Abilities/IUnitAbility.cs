using UnitCore.Runtime.Core;
namespace UnitCore.Runtime.Abilities
{
    // 基础能力接口
    public interface IUnitAbility
    {
        string AbilityId { get; }
        void Initialize(IUnit unit);
        void Update(float deltaTime);
        void Dispose();
    }
}
