# 🎯 UnitCore - 模块化单位实体系统

**属于 [Tech-Cosmos](https://github.com/Tech-Cosmos) 技术宇宙**

[![Unity 版本](https://img.shields.io/badge/Unity-2022.3%2B-000000?style=flat-square&logo=unity)](https://unity.com)
[![许可证: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat-square)](https://opensource.org/licenses/MIT)

> **"优秀的单位系统不是功能的堆砌，而是实体哲学的具象化。"**

一个为RTS、MOBA和战术游戏打造的生产就绪、数据驱动的单位框架。专为Tech-Cosmos生态系统中的工业化游戏内容创作而设计。

## ✨ 核心特性

### 🏗️ 模块化架构
- **插件化能力系统** - 无需代码更改即可添加/移除能力
- **数据驱动配置** - 基于ScriptableObject的单位定义
- **基于接口的设计** - 清晰的职责分离

### ⚡ 核心系统
- **完整的生命周期管理** - 创建、更新、销毁
- **事件驱动架构** - 通过类型化事件实现响应式游戏玩法
- **协程安全的能力** - 内置移动和攻击能力
- **可扩展设计** - 一行代码自定义能力注册

### 🎮 生产就绪
- **完整测试覆盖** - 验证的核心系统
- **性能优化** - 在可能的情况下实现零分配更新
- **内存安全** - 正确的资源清理
- **编辑器集成** - 可视化调试工具

## 🚀 快速开始

### 安装

1. **通过Unity包管理器:**
   从Git URL添加包:
   `https://github.com/Tech-Cosmos/Tech-Cosmos.Component.UnitCore.git`

2. **导入示例** 获取开箱即用的使用案例

### 基础用法

在Unity编辑器中创建单位配置:
- 右键 → Create → Tech-Cosmos → Unit → UnitConfig
- 配置单位属性和能力
- 向GameObject添加Unit组件并分配配置

## 🧩 一行代码能力扩展

[Ability("Heal")]
public class HealAbility : IUnitAbility
{
    public string AbilityId => "Heal";
    
    public void Initialize(IUnit unit) { }
    public void Update(float deltaTime) { }
    public void Dispose() { }
    
    public void ExecuteHeal() => Debug.Log("执行治疗!");
}

## 🏗️ 架构概览

graph TB
    A[UnitConfig] --> B[Unit Entity]
    B --> C[Ability System]
    B --> D[Property System]
    B --> E[Event System]
    C --> F[MoveAbility]
    C --> G[AttackAbility]
    C --> H[Custom Abilities]
    D --> I[Health Management]
    E --> J[Health Events]
    E --> K[Combat Events]

## 📚 核心组件

| 组件 | 职责 | 关键接口 |
|-----------|----------------|----------------|
| **Unit** | 实体核心 | IUnit, IUnitLifecycle |
| **UnitConfig** | 数据定义 | ScriptableObject |
| **AbilitySystem** | 插件管理 | IUnitAbility |
| **EventSystem** | 响应式通信 | UnitEvent |

## 🔗 依赖项

- **Unity** 2022.3+
- **UGUI** (用于基础UI集成)
- **无外部依赖** - 完全自包含

## 🛠️ 开发

### 构建自定义能力

1. 实现IUnitAbility接口
2. 添加[Ability("你的能力ID")]特性
3. 在UnitConfig中添加能力ID
4. 使用unit.GetAbility<你的能力>()

### 事件系统使用

unit.EventSystem.Subscribe<HealthChangedEvent>(e => {
    Debug.Log($"生命值: {e.CurrentHealth}");
});

## 📖 文档

- [快速开始](Documentation~/GettingStarted.md)
- [架构指南](Documentation~/Architecture.md)
- [API参考](Documentation~/APIReference.md)
- [示例](Samples~/BasicUsage/)

## 🤝 贡献

我们欢迎贡献！请参阅:
- [贡献指南](CONTRIBUTING.md)
- [行为准则](CODE_OF_CONDUCT.md)

## 📄 许可证

MIT许可证 - 详见[LICENSE](LICENSE)文件

---

**属于 [Tech-Cosmos](https://github.com/Tech-Cosmos) 技术宇宙**

*构建更好的游戏，从构建更好的单位开始。*