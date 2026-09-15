# TrackLab

基于 .NET 8 + WPF 的设备模块监控原型，用于配置设备模块、查看模块运行状态与 DI/DO 点位，并实践模块化桌面应用的组织方式。

> ⚠️ 原型阶段：模块状态、IO 数据和告警记录均保存在内存中，重启即恢复初始值，尚未接入真实设备或持久化。

## 功能

- 按 XAML 布局展示热板、冷板、Load Port、传输机械手
- 通过 JSON + 反射动态创建模块与菜单
- 有限状态机管理 `Idle` / `Running` / `Alarm` / `Disabled` 状态流转
- 状态色块 + 按模块类型打开的详情弹窗
- 硬件监视页共享 `IODisplay` 控件查看 DI/DO，支持切换示例 DI 值
- 告警列表：按 `Name` / `Message` 双条件查询、取消查询、增删告警
- 中英文界面切换，语言偏好持久化到本地
- 无边框最大化窗口，支持重启与关闭

## 技术栈

| 技术 | 版本 / 用途 |
| --- | --- |
| .NET 8 | `Core` 目标 `net8.0`，其余目标 `net8.0-windows` |
| WPF | Windows 桌面界面 |
| Caliburn.Micro | 4.0.230 — MVVM、视图定位、动作绑定 |
| Newtonsoft.Json | 13.0.4 — 菜单配置反序列化 |
| System.Text.Json | 模块配置与用户配置反序列化 |

## 环境要求

- Windows
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 + “.NET 桌面开发”工作负载（推荐）

## 快速开始

```powershell
git clone https://github.com/NobiNobita-GC/TrackLab.git
cd TrackLab
dotnet build TrackLab.sln --configuration Debug
dotnet run --project .\TrackLabClient\TrackLabClient.csproj
```

也可用 VS 打开 `TrackLab.sln`，将 `TrackLabClient` 设为启动项目。

运行配置位于输出目录 `TrackLabClient/bin/Debug/net8.0-windows/Config/`。

<details>
<summary>模拟器（可选，目前仅有空窗口）</summary>

```powershell
dotnet run --project .\TrackLabSimulator\TrackLabSimulator.csproj
```

尚无仿真或通信逻辑；客户端演示数据由自身启动流程创建，无需先启动模拟器。
</details>

## 界面导览

| 菜单路径 | 页面 | 说明 |
| --- | --- | --- |
| `Home Page` | 首页 | 介绍占位页 |
| `System → Status` | 设备状态 | 设备布局总览，点击模块打开详情 |
| `System → Alarm List` | 告警列表 | 双条件查询、增删告警 |
| `System → Shut Down` | 关机 | 关闭或重启（均需二次确认） |
| `Setup → Hardware Monitor` | 硬件监视 | 选择模块查看 DI/DO，可切换 `WaferPresent` |
| `Setup → Language` | 语言 | 切换 English / Chinese |

**典型操作**

1. `System → Status` → 点击热板 → `Start Process` / `Complete`，观察 `Idle → Running → Idle`。
2. `Setup → Hardware Monitor` → 选择模块 → `Toggle WaferPresent`，观察 DI 值变化（表格本身只读）。
3. `System → Alarm List` → 输入关键字 → `Query`（`Name` 与 `Message` 同时生效）→ `Cancel` 清空恢复全部。

## 项目结构

```text
TrackLab/
├─ Framework/
│  ├─ Core/          # 领域层：模块基类、状态机、IO、菜单、告警、配置、多语言
│  └─ UI/            # 通用层：控件、转换器、图标、主题、多语言扩展
├─ TrackLabClient/   # 主应用：Bootstrapper、Config、Modules、View
└─ TrackLabSimulator/# 模拟器骨架
```

依赖方向单向，`Core` 不引用 WPF，`UI` 依赖 `Core`，客户端与模拟器依赖两者：

```text
TrackLabClient ──┬──> Core
                 └──> UI ───> Core
```

## 配置

### 菜单 `Config/MenuConfig.json`

`ViewModelType` 填写客户端程序集中的完整类型名，分组菜单用 `SubMenuItems` 嵌套、无需自己的 `ViewModelType`。

```json
{ "Name": "Status", "ViewModelType": "TrackLabClient.View.Status.StatusViewModel" }
```

新增页面需四步：建 `XxxView.xaml` + `XxxViewModel.cs` 配对 → 继承 `ViewModelBase` 且有无参构造函数 → 登记到 `MenuConfig.json` → 在 `VectorIcons.xaml` 添加图标（key = 去掉空格的菜单名 + `Icon`）。

> `MenuManager` 启动时递归实例化所有菜单 ViewModel，类型名写错或缺无参构造函数会导致启动失败。

### 模块 `Config/ModuleConfig.json`

```json
{ "Index": 201, "Name": "HP01", "ClassName": "TrackLabClient.Modules.HotPlate.HotPlateModule" }
```

类型须继承 `ModuleBase`，提供 `(int index, string name)` 构造函数，`Index` 与 `Name` 均不可重复。当前共 21 个模块：

| 类型 | 名称 | 索引 |
| --- | --- | --- |
| Load Port | `LP01`–`LP04` | 101–104 |
| Robot | `ROBOT01` | 150 |
| Hot Plate | `HP01`–`HP10` | 201–210 |
| Cool Plate | `CP01`–`CP06` | 301–306 |

### 布局 `Config/LayoutConfig.xaml`

用普通 WPF `Grid` 描述设备位置，模块名以 `TextBlock` 占位（如 `LP01`、`HP01`）。运行时按文本匹配模块，替换为绑定数据的 `ModuleControl`。

四个约束：

1. 根节点必须是 `Grid`；占位符须位于根或嵌套 `Grid` 内，不会遍历 `Border`、`StackPanel`。
2. 占位文本 `Trim()` 后须与模块名完全一致。
3. 未匹配的占位符保留为普通文本；匹配成功的控件继承其行/列与跨行列设置。
4. 该文件不编译为页面，仅复制到输出目录，改动后需重新构建。

### 用户配置 `Config/UserConfig.json`

`ConfigManager` 以键值对形式读写该文件，首次运行不存在时会自动创建。当前用于保存语言偏好（键 `System.Language`）。

### 多语言

资源文件位于 `Framework/Core/Language/`（`Lang-Word.resx` / `.en.resx` / `.zh-CN.resx`）。XAML 中通过附加属性使用，key 会自动去除空格：

```xml
<Button lan:LanguageExtension.Text="Shut Down" />
```

语言切换时 `LanguageManager.LanguageChanged` 触发，`LanguageExtension` 自动刷新已挂载的控件文本。

## 设计要点

**模块继承体系** — 所有模块继承 `ModuleBase`，而 `ModuleBase` 继承 `FSM`。按职责分三类基类：`ProcessModuleBase`（热板/冷板，提供 `StartProcess` / `CompleteProcess`）、`CarrierModuleBase`（Load Port）、`RobotModuleBase`（机械手）。

**状态流转** — 只允许以下转换，其余由 `ChangeState` 拒绝：

```text
Unknown  → Idle
Idle     → Running | Alarm | Disabled
Running  → Idle | Alarm
Alarm    → Idle
Disabled → Idle
```

**UI 扩展点** — View 与 ViewModel 按 Caliburn.Micro 命名约定匹配；`x:Name` 同名按钮自动绑定 `Click` 到 ViewModel 方法，带参数时用 `cal:Message.Attach`。模块详情页按 `{ModuleType}ModuleViewModel` 命名并接收模块实例，缺失时回退到通用 `ModuleDetailViewModel`。

**主题** — `MainColor.xaml` 为入口，当前加载 `DeepBlue.xaml`，另有 `LightBlue.xaml`。两套字典须保持相同资源 key（`PageBackgroundBrush`、`PanelBackgroundBrush`、`PanelBorderBrush`、`Module*Color`、`Alarm*Color`）。模块状态色由 `ModuleStatusConverter` 按 `Module{State}Color` 查找。

## 当前进度

| 项目 | 状态 |
| --- | --- |
| TrackLabClient | 布局、详情、DI/DO 监视、告警、关机、语言切换均已实现；首页为占位 |
| Core | 模块反射加载、FSM、IO、菜单、告警、配置、多语言已实现 |
| UI | `ModuleControl`、`IODisplay`、转换器、两套主题已实现 |
| TrackLabSimulator | 仅空窗口 |
| 测试 | 尚无测试项目 |

提交前请执行 `dotnet build TrackLab.sln --configuration Debug`。构建通过不等于功能验证，建议按“界面导览”手动回归关键路径。

## Roadmap

- [ ] 接入真实设备通信或设备抽象层
- [ ] IO / 状态 / 告警迁移到持久化存储
- [ ] 实现 Simulator 仿真与通信
- [ ] 告警查询接入真实数据，补充确认与清除流程
- [ ] 补充日志与异常处理
- [ ] 为 Core 层添加单元测试
