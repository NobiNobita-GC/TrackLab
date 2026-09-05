# TrackLab

TrackLab 是一个基于 .NET 8 和 WPF 的设备模块监控原型项目，用于配置设备模块、展示模块运行状态、查看数字量输入/输出（DI/DO），并探索模块化桌面应用的组织方式。

项目采用 Caliburn.Micro 实现 MVVM 与页面导航，使用独立的 Core、UI、Client 和 Simulator 项目划分领域逻辑、通用界面和可执行程序。

## 功能概览

- 按设备布局展示热板、冷板、Load Port 和传输机械手等模块
- 通过 JSON 配置和反射动态创建模块实例
- 使用有限状态机管理模块的 Idle、Running、Alarm 和 Disabled 状态流转
- 使用状态颜色展示模块运行状态，并按模块类型打开对应详情弹窗
- 按模块查看 DI/DO 点位及当前值
- 通过 JSON 配置应用菜单和模块，通过 XAML 配置设备布局
- 提供 Alarm List 页面骨架，便于后续接入告警数据
- 提供共享控件、图标资源和主题资源
- 提供独立的模拟器项目骨架，便于后续扩展设备仿真

> 当前项目处于原型开发阶段。模块定义来自本地配置文件，模块状态和 IO 数据仍保存在内存中，尚未接入真实设备、数据库或持久化运行数据。

## 技术栈

| 技术 | 用途 |
| --- | --- |
| .NET 8 | 应用运行时与基础类库 |
| WPF | Windows 桌面界面 |
| Caliburn.Micro 4.0.230 | MVVM、视图定位和生命周期管理 |
| Newtonsoft.Json 13.0.4 | 菜单配置反序列化 |
| System.Text.Json | 模块配置反序列化 |

## 项目结构

```text
TrackLab/
├─ Framework/                  # 公共框架（解决方案中同名分组）
│  ├─ Core/                    # 模块基类、状态机、IO 和菜单管理
│  │  └─ Core.csproj
│  └─ UI/                      # 通用控件、Converts、图标与主题
│     └─ UI.csproj
├─ TrackLabClient/             # 主应用
│  ├─ Config/                  # 菜单、模块和设备布局配置
│  ├─ Modules/                 # 具体设备模块
│  │  ├─ HotPlate/             # 热板
│  │  ├─ Cool/                 # 冷板
│  │  ├─ LoadPort/             # Load Port
│  │  └─ Robot/                # 机械手
│  ├─ View/
│  │  ├─ Status/               # 状态页及模块详情
│  │  ├─ Configuration/        # 硬件监视
│  │  └─ Log/                  # 告警页
│  └─ TrackLabClient.csproj
├─ TrackLabSimulator/          # 设备模拟器骨架，窗口位于 View/
└─ TrackLab.sln                # Visual Studio 解决方案
```

### 项目依赖关系

```text
TrackLabClient ──────┬──> Core
                      └──> UI ───> Core

TrackLabSimulator ───┬──> Core
                      └──> UI
```

## 环境要求

- Windows
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022，并安装“.NET 桌面开发”工作负载（推荐）

## 快速开始

### 1. 克隆仓库

```powershell
git clone https://github.com/NobiNobita-GC/TrackLab.git
cd TrackLab
```

### 2. 还原并构建

```powershell
dotnet restore TrackLab.sln
dotnet build TrackLab.sln
```

### 3. 运行客户端

```powershell
dotnet run --project .\TrackLabClient\TrackLabClient.csproj
```

也可以使用 Visual Studio 打开 `TrackLab.sln`，将 `TrackLabClient` 设置为启动项目后运行。

目录调整后，请重新打开解决方案并重新选择启动项目。公共项目在解决方案的 `Framework` 分组下，项目及命名空间分别为 `Core`、`UI`；客户端和模拟器分别为 `TrackLabClient`、`TrackLabSimulator`。

## 配置说明

### 菜单配置

客户端菜单定义在：

```text
TrackLabClient/Config/MenuConfig.json
```

每个可打开的菜单项通过 `ViewModelType` 指向客户端程序集中的完整 ViewModel 类型名：

```json
{
  "Name": "Status",
  "ViewModelType": "TrackLabClient.View.Status.StatusViewModel"
}
```

添加页面时需要：

1. 在 `TrackLabClient/View` 对应的功能子目录中创建配对的 `XxxView.xaml` 和 `XxxViewModel.cs`。
2. 确保 ViewModel 可通过无参构造函数创建。
3. 在 `MenuConfig.json` 中填写完整的 ViewModel 类型名。

构建时，菜单配置会复制到输出目录的 `Config` 文件夹。

### 模块配置

设备模块定义在：

```text
TrackLabClient/Config/ModuleConfig.json
```

每个模块配置包含唯一索引、模块名称和完整类型名：

```json
{
  "Index": 201,
  "Name": "HP01",
  "ClassName": "TrackLabClient.Modules.HotPlate.HotPlateModule"
}
```

客户端启动时，`Bootstrapper` 将客户端程序集传入 `ModuleManager.Load`，管理器读取配置，通过反射创建模块，并调用 `InitializeAll()` 将模块初始化为 `Idle`。配置中的类型必须继承 `ModuleBase`，并提供 `(int index, string name)` 构造函数；模块索引和名称不能重复。

当前配置包含：

- 热板：`HP01`～`HP10`
- 冷板：`CP01`～`CP06`
- Load Port：`LP01`～`LP04`
- 传输机械手：`ROBOT01`

### 设备布局配置

状态页的设备布局定义在：

```text
TrackLabClient/Config/LayoutConfig.xaml
```

布局使用普通 WPF `Grid` 描述 Load Port、传输区和工艺模块的位置。运行时会读取其中的模块名称占位符，例如 `LP01`、`ROBOT01` 或 `HP01`，并替换为绑定对应模块数据的 `ModuleControl`。

调整布局时需要注意：

1. 占位 `TextBlock` 的文本必须与 `ModuleManager` 中注册的模块名称完全一致。
2. 未找到对应模块的占位符会保留为普通文本。
3. `LayoutConfig.xaml` 作为内容文件复制到输出目录，因此修改后需要重新构建或手动同步输出文件。
4. 点击已匹配的模块控件会按模块类型查找对应 ViewModel；没有专属页面时使用通用详情弹窗。

### IO 演示数据

当前 IO 演示数据仍在 `TrackLabClient/Bootstrapper.cs` 的 `InitializeRuntimeData()` 中创建，包括：

- 示例 DI/DO：WaferPresent、VacuumOK、VacuumValve、HeaterOn 等

这些数据仅保存在进程内存中，应用重新启动后会恢复为初始值。

## 模块架构

目录组织参考 Rubi：公共能力放入 `Framework`，具体设备实现放入 `TrackLabClient/Modules`，页面按功能分组。公共框架不引用客户端，模块加载时由客户端显式提供实现程序集。

模块基类位于 `Framework/Core/Modules`，按职责划分为三类：

- `ProcessModuleBase`：工艺模块基类，提供开始和完成工艺操作；热板和冷板继承该类型。
- `CarrierModuleBase`：载具模块基类；Load Port 继承该类型。
- `RobotModuleBase`：机械手模块基类；传输机械手继承该类型。

所有模块最终继承 `ModuleBase`，而 `ModuleBase` 继承有限状态机 `FSM`。当前允许的状态流转为：

```text
Unknown  -> Idle
Idle     -> Running | Alarm | Disabled
Running  -> Idle | Alarm
Alarm    -> Idle
Disabled -> Idle
```

`HotPlateModuleViewModel` 已提供工艺开始与完成操作；冷板和其他模块的专属交互仍在逐步完善。

## 开发约定

- 通用模型、模块基类和状态机放在 `Framework/Core`；具体设备实现放在 `TrackLabClient/Modules`。
- 模块状态流转通过 `FSM` 和模块公开方法完成，避免直接修改状态。
- 可复用控件、主题、图标和转换器放在 `Framework/UI`。
- 具体页面和应用级交互放在 `TrackLabClient`。
- View 与 ViewModel 使用 Caliburn.Micro 的命名约定进行匹配。
- 新增模块类型时同步添加模块实现、`ModuleConfig.json` 配置；需要专属详情页时在 `TrackLabClient/View/Status` 中使用 `{ModuleType}ModuleViewModel` 命名，并提供接收模块实例的构造函数。
- 提交代码前至少执行一次：

  ```powershell
  dotnet build TrackLab.sln --configuration Debug
  ```

## 当前状态

- `TrackLabClient`：可运行的主应用原型，已支持配置化设备布局、模块专属详情弹窗、硬件监视页和告警页骨架
- `Core`：已包含模块反射创建、模块分层、有限状态机、IO 和菜单管理基础实现
- `UI`：已包含模块控件、状态转换器、图标与主题
- `TrackLabSimulator`：仅有基础窗口，设备仿真逻辑待实现
- 自动化测试：暂未建立测试项目

## 后续方向

- 接入真实设备通信或统一的设备抽象层
- 将 IO 和运行状态迁移到配置或持久化存储
- 完善 Simulator 与客户端之间的数据交互
- 接入真实告警数据，完善 Alarm List 的查询、确认和清除流程
- 增加日志、异常处理和运行状态诊断
- 为 Core 层补充单元测试
