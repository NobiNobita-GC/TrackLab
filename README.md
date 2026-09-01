# TrackLab

TrackLab 是一个基于 .NET 8 和 WPF 的设备模块监控原型项目，用于展示模块运行状态、查看数字量输入/输出（DI/DO），并探索模块化桌面应用的组织方式。

项目采用 Caliburn.Micro 实现 MVVM 与页面导航，使用独立的 Core、UI、Client 和 Simulator 项目划分领域逻辑、通用界面和可执行程序。

## 功能概览

- 按设备布局展示热盘、冷盘、Load Port 和传输机械手等模块
- 使用状态颜色展示模块运行状态，并可点击模块打开详情弹窗
- 按模块查看 DI/DO 点位及当前值
- 通过 JSON 配置应用菜单，通过 XAML 配置设备布局
- 提供共享控件、图标资源和主题资源
- 提供独立的模拟器项目骨架，便于后续扩展设备仿真

> 当前项目处于原型开发阶段。模块和 IO 数据由客户端启动时在内存中初始化，尚未接入真实设备、数据库或持久化配置。

## 技术栈

| 技术 | 用途 |
| --- | --- |
| .NET 8 | 应用运行时与基础类库 |
| WPF | Windows 桌面界面 |
| Caliburn.Micro 4.0.230 | MVVM、视图定位和生命周期管理 |
| Newtonsoft.Json 13.0.4 | 菜单配置反序列化 |

## 项目结构

```text
TrackLab/
├─ TrackLab.Client/       # 主应用、页面、ViewModel 和启动数据
│  ├─ Config/             # 菜单配置与设备布局配置
│  └─ View/               # 应用页面及对应 ViewModel
├─ TrackLab.Core/         # 模块、IO 和菜单等核心模型与管理器
├─ TrackLab.UI/           # 通用控件、转换器、图标与主题资源
├─ TrackLab.Simulator/    # 设备模拟器项目（当前为基础骨架）
└─ TrackLab.sln           # Visual Studio 解决方案
```

### 项目依赖关系

```text
TrackLab.Client ──────┬──> TrackLab.Core
                      └──> TrackLab.UI ───> TrackLab.Core

TrackLab.Simulator ───┬──> TrackLab.Core
                      └──> TrackLab.UI
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
dotnet run --project .\TrackLab.Client\TrackLab.Client.csproj
```

也可以使用 Visual Studio 打开 `TrackLab.sln`，将 `TrackLab.Client` 设置为启动项目后运行。

## 配置说明

### 菜单配置

客户端菜单定义在：

```text
TrackLab.Client/Config/MenuConfig.json
```

每个可打开的菜单项通过 `ViewModelType` 指向客户端程序集中的完整 ViewModel 类型名：

```json
{
  "Name": "Status",
  "ViewModelType": "TrackLab.Client.View.StatusViewModel"
}
```

添加页面时需要：

1. 在 `TrackLab.Client/View` 中创建配对的 `XxxView.xaml` 和 `XxxViewModel.cs`。
2. 确保 ViewModel 可通过无参构造函数创建。
3. 在 `MenuConfig.json` 中填写完整的 ViewModel 类型名。

构建时，菜单配置会复制到输出目录的 `Config` 文件夹。

### 设备布局配置

状态页的设备布局定义在：

```text
TrackLab.Client/Config/LayoutConfig.xaml
```

布局使用普通 WPF `Grid` 描述 Load Port、传输区和工艺模块的位置。运行时会读取其中的模块名称占位符，例如 `LP01`、`ROBOT01` 或 `HP01`，并替换为绑定对应模块数据的 `ModuleControl`。

调整布局时需要注意：

1. 占位 `TextBlock` 的文本必须与 `ModuleManager` 中注册的模块名称完全一致。
2. 未找到对应模块的占位符会保留为普通文本。
3. `LayoutConfig.xaml` 作为内容文件复制到输出目录，因此修改后需要重新构建或手动同步输出文件。
4. 点击已匹配的模块控件会打开详情弹窗，展示模块名称、索引、类型和状态。

### 演示数据

当前模块与 IO 演示数据在 `TrackLab.Client/Bootstrapper.cs` 的 `InitializeRuntimeData()` 中创建，包括：

- 热盘：`HP01`～`HP10`
- 冷盘：`CP01`～`CP06`
- Load Port：`LP01`～`LP04`
- 传输机械手：`ROBOT01`
- 示例 DI/DO：WaferPresent、VacuumOK、VacuumValve、HeaterOn 等

这些数据仅保存在进程内存中，应用重新启动后会恢复为初始值。

## 开发约定

- 领域模型和业务状态放在 `TrackLab.Core`。
- 可复用控件、主题、图标和转换器放在 `TrackLab.UI`。
- 具体页面和应用级交互放在 `TrackLab.Client`。
- View 与 ViewModel 使用 Caliburn.Micro 的命名约定进行匹配。
- 提交代码前至少执行一次：

  ```powershell
  dotnet build TrackLab.sln --configuration Debug
  ```

## 当前状态

- `TrackLab.Client`：可运行的主应用原型，已支持配置化设备布局和模块详情弹窗
- `TrackLab.Core`：已包含热盘、冷盘、Load Port、机械手、IO 和菜单管理基础实现
- `TrackLab.UI`：已包含模块控件、状态转换器、图标与主题
- `TrackLab.Simulator`：仅有基础窗口，设备仿真逻辑待实现
- 自动化测试：暂未建立测试项目

## 后续方向

- 接入真实设备通信或统一的设备抽象层
- 将启动演示数据迁移到配置或持久化存储
- 完善 Simulator 与客户端之间的数据交互
- 增加日志、异常处理和运行状态诊断
- 为 Core 层补充单元测试
