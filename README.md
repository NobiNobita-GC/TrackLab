# TrackLab

TrackLab 是一个基于 .NET 8 和 WPF 的设备模块监控原型项目，用于配置设备模块、展示模块运行状态、查看数字量输入/输出（DI/DO），并探索模块化桌面应用的组织方式。

项目采用 Caliburn.Micro 实现 MVVM 与页面导航，使用独立的 Core、UI、Client 和 Simulator 项目划分领域逻辑、通用界面和可执行程序。

## 功能概览

- 按设备布局展示热板、冷板、Load Port 和传输机械手等模块
- 通过 JSON 配置和反射动态创建模块实例
- 使用有限状态机管理模块的 Idle、Running、Alarm 和 Disabled 状态流转
- 使用状态颜色展示模块运行状态，并按模块类型打开对应详情弹窗
- 使用共享 `IODisplay` 控件按模块查看 DI/DO 点位，并随点位值变化更新显示
- 在硬件监视页通过 `Toggle WaferPresent` 按钮切换示例 DI 值
- 通过 JSON 配置应用菜单和模块，通过 XAML 配置设备布局
- 提供告警列表页：展示示例告警记录，支持关键字搜索、添加测试告警和移除选中告警
- 集中管理页面背景、面板边框、模块状态颜色、告警等级颜色及菜单矢量图标
- 提供独立的模拟器项目骨架，便于后续扩展设备仿真

> 当前项目处于原型开发阶段。模块定义来自本地配置文件，模块状态、IO 数据和告警记录仍保存在内存中，尚未接入真实设备、数据库或持久化运行数据。

## 技术栈

| 技术 | 用途 |
| --- | --- |
| .NET 8 | 应用运行时与基础类库 |
| WPF | Windows 桌面界面 |
| Caliburn.Micro 4.0.230 | MVVM、视图定位、生命周期管理与动作绑定 |
| Newtonsoft.Json 13.0.4 | 菜单配置反序列化 |
| System.Text.Json | 模块配置反序列化 |

## 项目结构

```text
TrackLab/
├─ Framework/                  # 公共框架（解决方案中同名分组）
│  ├─ Core/                    # 模块基类、状态机、IO、菜单和告警模型
│  │  ├─ Alarm/                # 告警模型（AlarmItem）和告警等级（AlarmLevel）
│  │  ├─ Modules/              # 模块基类、配置模型和 ModuleManager
│  │  ├─ StateMachine/         # FSM 状态流转与属性通知
│  │  ├─ IO/                   # IO 点位、注册和更新
│  │  ├─ Menu/                 # 配置菜单与 ViewModel 创建
│  │  └─ Core.csproj
│  └─ UI/                      # 通用控件、Converts、图标与主题
│     ├─ Common/               # ViewModelBase
│     ├─ Control/              # ModuleControl 与 Display/IODisplay
│     ├─ Converts/             # 菜单图标和模块状态转换器
│     ├─ Geometry/             # VectorIcons.xaml
│     ├─ Theme/                # MainColor.xaml 与 Color/ 配色字典
│     └─ UI.csproj
├─ TrackLabClient/             # 主应用
│  ├─ Bootstrapper.cs          # 初始化模块、示例 IO 并打开主窗口
│  ├─ Config/                  # 菜单、模块和设备布局配置
│  ├─ Modules/                 # 具体设备模块
│  │  ├─ HotPlate/             # 热板
│  │  ├─ Cool/                 # 冷板
│  │  ├─ LoadPort/             # Load Port
│  │  └─ Robot/                # 机械手
│  ├─ View/
│  │  ├─ HomeView / HomeViewModel           # 主窗口与菜单外壳
│  │  ├─ HomePageView / HomePageViewModel   # 首页
│  │  ├─ Status/               # 状态页及模块详情
│  │  ├─ Configuration/        # 硬件监视
│  │  └─ Log/                  # 告警页
│  └─ TrackLabClient.csproj
├─ TrackLabSimulator/          # 设备模拟器骨架，窗口位于 View/
└─ TrackLab.sln                # Visual Studio 解决方案
```

### 项目依赖关系

```text
TrackLabClient ─────┬──> Core
                   └──> UI ───> Core

TrackLabSimulator ──┬──> Core
                   └──> UI
```

## 环境要求

- Windows
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)；使用其他兼容 SDK 构建时，运行 WPF 程序仍需 .NET 8 Windows Desktop Runtime
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
dotnet build TrackLab.sln --configuration Debug --no-restore
```

### 3. 运行客户端

```powershell
dotnet run --project .\TrackLabClient\TrackLabClient.csproj
```

也可以使用 Visual Studio 打开 `TrackLab.sln`，将 `TrackLabClient` 设置为启动项目后运行。

客户端默认 Debug 输出目录为 `TrackLabClient/bin/Debug/net8.0-windows/`，运行时配置位于该目录下的 `Config/`。

### 4. 运行模拟器（可选）

```powershell
dotnet run --project .\TrackLabSimulator\TrackLabSimulator.csproj
```

模拟器目前只打开基础窗口，尚无仿真或通信逻辑；客户端演示数据由自身启动流程创建，无需先启动模拟器。

### 功能体验

1. 启动后默认进入 `Home Page`，当前首页为介绍占位页。
2. 进入 `System → Status` 查看设备布局；点击热板模块可打开详情，通过 `Start Process` 和 `Complete` 演示 `Idle → Running → Idle` 状态流转。冷板详情目前只展示模块信息。
3. 进入 `Setup → Hardware Monitor`，在左侧选择模块（默认选中第一个），点击 `Toggle WaferPresent` 观察 DI 值变化。DI/DO 表格本身为只读。
4. 进入 `System → Alarm List` 查看示例告警；在搜索框输入关键字后点击 `Query` 按 `Message` 过滤，点击 `Add Test Alarm` 追加一条示例告警，或选中表格行后点击 `Remove Selected` 删除。数据仅存在内存中，重启后恢复。

### 启动流程

`App.xaml` 加载 UI 资源字典并创建 `Bootstrapper`；`Bootstrapper.OnStartup()` 依次加载模块配置、初始化模块、注册示例 IO，随后异步打开 `HomeViewModel`。主窗口通过 `MenuManager` 读取菜单并创建页面 ViewModel，切换页面时调用自定义的 `Active()` / `Deactivate()` 方法。状态页每次激活都会重新读取设备布局，硬件监视页每次激活都会默认选择第一个模块。

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

当前菜单结构：

- `Home Page` → `TrackLabClient.View.HomePageViewModel`
- `System`
  - `Status` → `TrackLabClient.View.Status.StatusViewModel`
  - `Alarm List` → `TrackLabClient.View.Log.AlarmViewModel`
- `Setup`
  - `Hardware Monitor` → `TrackLabClient.View.Configuration.HardwareMonitorViewModel`

添加页面时需要：

1. 在 `TrackLabClient/View` 对应的功能子目录中创建配对的 `XxxView.xaml` 和 `XxxViewModel.cs`。
2. ViewModel 继承 `UI.Common.ViewModelBase`，并可通过公共无参构造函数创建。
3. 在 `MenuConfig.json` 中填写完整的 ViewModel 类型名。
4. 在 `Framework/UI/Geometry/VectorIcons.xaml` 中为菜单名配置图标，资源 key 使用“去掉空格的菜单名 + `Icon`”，如 `Hardware Monitor` 对应 `HardwareMonitorIcon`。

分组菜单通过 `SubMenuItems` 嵌套子菜单，可省略自身的 `ViewModelType`。`MenuManager` 在加载菜单时递归创建配置的 ViewModel，因此类型名错误或缺少公共无参构造函数会影响客户端启动。

构建时，菜单配置会复制到输出目录的 `Config` 文件夹；修改源码配置后需重新构建并重启客户端。

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

当前配置共 21 个模块，索引按类型分段：

| 类型 | 模块名称 | 索引范围 | 数量 |
| --- | --- | --- | --- |
| Load Port | `LP01`～`LP04` | 101～104 | 4 |
| 传输机械手 | `ROBOT01` | 150 | 1 |
| 热板 | `HP01`～`HP10` | 201～210 | 10 |
| 冷板 | `CP01`～`CP06` | 301～306 | 6 |

### 设备布局配置

状态页的设备布局定义在：

```text
TrackLabClient/Config/LayoutConfig.xaml
```

布局使用普通 WPF `Grid` 描述 Load Port、传输区和工艺模块的位置，顶部按 `LOAD PORT`、`TRANSFER`、`PROCESS MODULE` 三个区域标题分区。运行时会读取其中的模块名称占位符，例如 `LP01`、`ROBOT01` 或 `HP01`，并替换为绑定对应模块数据的 `ModuleControl`。

调整布局时需要注意：

1. 布局根节点必须是 `Grid`；模块占位符应直接放在根 `Grid` 或嵌套 `Grid` 中，当前替换逻辑不会遍历 `Border`、`StackPanel` 等其他容器。
2. 占位 `TextBlock` 的文本去掉首尾空白后，必须与 `ModuleManager` 中注册的模块名称完全一致。
3. 未找到对应模块的占位符会保留为普通文本；替换成功的控件会继承占位符的行、列和跨行跨列设置。
4. `LayoutConfig.xaml` 不编译为页面，而是复制到输出目录的 `Config` 文件夹。修改源码后需要重新构建；若修改的是输出文件，重新进入状态页即可重新加载。
5. 点击已匹配的模块控件会按模块类型查找对应 ViewModel；没有专属页面时使用通用详情弹窗。

### IO 演示数据

当前 IO 演示数据在 [Bootstrapper.cs](TrackLabClient/Bootstrapper.cs) 的 `InitializeRuntimeData()` 中创建：

| 模块 | DI 初始值 | DO 初始值 |
| --- | --- | --- |
| `HP01` | `WaferPresent = true`、`VacuumOK = true` | `VacuumValve = false`、`HeaterOn = true` |
| `HP02` | `WaferPresent = false` | `VacuumValve = false` |
| `CP01` | `WaferPresent = false` | `CoolingValve = false` |
| `LP01` | `CarrierPresent = false` | `DoorOpen = false` |

其他模块暂未注册 IO，选择后显示空列表。`Toggle WaferPresent` 只更新当前模块已有的同名 DI，没有该点位时不会执行操作。

`IOManager.UpdateDI()` / `UpdateDO()` 按模块名和点位名查找数据，调用 `IOPoint.SetValue()` 并通过 `INotifyPropertyChanged` 通知界面。`IODisplay` 的 `Items` 依赖属性接收点位集合，以只读表格展示 `Index`、`Name` 和 `Value`。这些数据仅保存在进程内存中，应用重新启动后会恢复为初始值。

### 告警数据

告警页由 [AlarmViewModel.cs](TrackLabClient/View/Log/AlarmViewModel.cs) 提供示例数据，告警模型定义在 `Framework/Core/Alarm`：

| 类型 | 字段 / 取值 |
| --- | --- |
| `AlarmItem` | `Time`、`Module`、`Level`、`Message`、`Cause`、`Solution` |
| `AlarmLevel` | `Info`、`Warning`、`Error` |

告警页以只读表格展示上述字段，并按等级着色（`AlarmInfoColor` / `AlarmWarningColor` / `AlarmErrorColor`）。`Alarms` 保存全量数据，`DisplayedAlarms` 为经查询条件过滤后实际展示的集合，表格与 `Count` 均绑定后者。当前支持：

- `SearchMessage`：页面顶部的搜索输入框，按 `Message` 字段过滤，比较时不区分大小写，首尾空白会被忽略。
- `Query`：按当前 `SearchMessage` 过滤列表。关键字为空时展示全部。
- `AddTestAlarm`：追加一条 `Info` 级示例告警，并刷新当前列表。
- `RemoveSelectedAlarm`：从全量集合中删除表格选中的告警，并刷新当前列表；未选中时不可用。

初始包含 `Robot` 的 Error 级和 `LoadPort` 的 Warning 级各一条示例记录。告警数据同样只保存在内存中。

### 主题与菜单图标

客户端在 [App.xaml](TrackLabClient/App.xaml) 中合并共享主题和图标字典：

| 文件 | 用途 |
| --- | --- |
| [MainColor.xaml](Framework/UI/Theme/MainColor.xaml) | 主题入口，当前加载 `Color/DeepBlue.xaml` |
| [DeepBlue.xaml](Framework/UI/Theme/Color/DeepBlue.xaml) / [LightBlue.xaml](Framework/UI/Theme/Color/LightBlue.xaml) | 定义同名的页面、面板、边框、模块状态和告警等级画刷 |
| [VectorIcons.xaml](Framework/UI/Geometry/VectorIcons.xaml) | 菜单 `Geometry` 图标资源 |

调整配色可编辑当前字典，或将 `MainColor.xaml` 的 `Source` 改为 `/UI;component/Theme/Color/LightBlue.xaml`，重新构建后启动。两个字典均需保留以下资源 key：

- `PageBackgroundBrush`、`PanelBackgroundBrush`、`PanelBorderBrush`
- `ModuleUnknownColor`、`ModuleIdleColor`、`ModuleRunningColor`、`ModuleAlarmColor`、`ModuleDisabledColor`
- `AlarmInfoColor`、`AlarmWarningColor`、`AlarmErrorColor`

硬件监视页和告警页使用共享背景和边框画刷；`ModuleControl` 通过 `ModuleStatusConverter` 按 `Module{State}Color` 查找状态画刷，状态未知时使用 `ModuleUnknownColor`。当前未提供运行时主题切换入口，主窗口外壳（顶栏、侧边菜单和二级菜单弹窗）仍使用固定深色。

菜单图标由 `StringToIconConverter` 按菜单名查找。绘图约定为 24×24 坐标体系，菜单中的 `Path` 以 20×20、`Stretch="Uniform"` 和白色填充显示。新增或替换图标时应保留对应资源 key，并沿用该坐标约定。

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

`HotPlateModuleViewModel` 已提供工艺开始与完成操作；冷板详情目前只展示模块信息，Load Port 和机械手使用通用详情弹窗。热板、冷板页面的温度仍为 `-- °C` 占位文本，尚未绑定实时温度。

## 开发约定

- 通用模型、模块基类、状态机和告警模型放在 `Framework/Core`；具体设备实现放在 `TrackLabClient/Modules`。
- 模块状态流转通过 `FSM` 和模块公开方法完成，避免直接修改状态。
- 可复用控件、主题、图标和转换器放在 `Framework/UI`。
- 具体页面和应用级交互放在 `TrackLabClient`。
- View 与 ViewModel 使用 Caliburn.Micro 的命名约定进行匹配；按钮动作通过 `cal:Message.Attach` 绑定同名方法。
- 新增模块类型时同步更新 `Core.Modules.ModuleType`、模块实现和 `ModuleConfig.json`；需要出现在状态页时补充 `LayoutConfig.xaml` 占位符。
- 需要专属详情页时，在 `TrackLabClient/View/Status` 中使用 `{ModuleType}ModuleViewModel` 命名，并提供接收模块实例的公共构造函数。
- 新增 DI/DO 展示复用 `IODisplay`；更新点位值通过 `IOManager` 或 `IOPoint.SetValue()` 触发属性通知。
- 新增菜单同步配置图标 key；新增共享配色资源时同步维护两个主题字典。
- 提交代码前至少执行一次：

  ```powershell
  dotnet build TrackLab.sln --configuration Debug
  ```

## 当前状态

- `TrackLabClient`：主应用原型，已实现配置化设备布局、模块详情弹窗、只读 DI/DO 监视、DI 值切换演示和内存态告警列表；首页仍为占位内容
- `Core`：已包含模块反射创建、模块分层、有限状态机、IO、菜单管理和告警模型基础实现
- `UI`：已包含 `ModuleControl`、`IODisplay`、状态与图标转换器、共享画刷及两套配色字典
- `TrackLabSimulator`：仅有基础窗口，设备仿真逻辑待实现
- 自动化测试：暂未建立测试项目

构建成功不等于完整功能验证。提交前可按“功能体验”中的路径检查布局、热板状态变化、DI 显示和告警增删；当前未提供自动化测试命令。

## 后续方向

- 接入真实设备通信或统一的设备抽象层
- 将 IO 和运行状态迁移到配置或持久化存储
- 完善 Simulator 与客户端之间的数据交互
- 将告警列表接入真实告警数据，完善搜索过滤、确认和清除流程
- 增加日志、异常处理和运行状态诊断
- 为 Core 层补充单元测试
