# TrackLab

基于 .NET 8 + WPF 的设备模块监控原型，用于配置设备模块、查看模块运行状态与 DI/DO 点位，并实践模块化桌面应用的组织方式。

> 原型阶段：模块状态、IO 数据和告警记录均保存在内存中，重启即恢复初始值。操作日志通过 EF Core 写入 MySQL；客户端启动前需配置数据库并应用迁移。Recipe 文件可在界面中只读浏览，用户配置保存在本地文件。整体未接入真实设备。

## 功能

- 按 XAML 布局展示热板、冷板、Load Port、传输机械手
- 通过 JSON + 反射动态创建模块与菜单
- 有限状态机管理 `Idle` / `Running` / `Alarm` / `Disabled` 状态流转
- 状态色块 + 按模块类型打开的详情弹窗
- 硬件监视页共享 `IODisplay` 控件查看 DI/DO，支持切换示例 DI 值
- 告警列表：按 `Name` / `Message` 双条件查询、取消查询、增删告警
- 操作日志：启动记录写入 MySQL，页面按时间倒序只读展示时间、用户、操作，支持刷新与记录数显示
- 中英文界面切换（菜单、页面标题、按钮、枚举、表头和弹窗，包含 Recipe 与操作日志页面），语言偏好持久化到本地
- Recipe 配方浏览：按目录树查看 `.rp` 文件及只读的 Header / Step / Config；基础层支持路径查找、类型识别与 JSON 读写
- 无边框最大化窗口，支持重启与关闭

## 技术栈

| 技术 | 版本 / 用途 |
| --- | --- |
| .NET 8 | `Core` 目标 `net8.0`，其余目标 `net8.0-windows` |
| WPF | Windows 桌面界面 |
| Caliburn.Micro | 4.0.230 — MVVM、视图定位、动作绑定 |
| Newtonsoft.Json | 13.0.4 — 菜单配置反序列化 |
| System.Text.Json | 模块配置、用户配置与 Recipe 反序列化 |
| Entity Framework Core | 8.0.6 — 数据访问、数据库迁移 |
| MySql.EntityFrameworkCore | 8.0.5 — MySQL 数据库提供程序 |

## 环境要求

- Windows
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 + “.NET 桌面开发”工作负载（推荐）
- 可连接的 MySQL 服务，以及有权初始化 `TrackLab` 数据库和读写日志表的账号
- EF Core CLI（`dotnet-ef` 8.0.6，用于应用已有迁移）

## 快速开始

```powershell
git clone https://github.com/NobiNobita-GC/TrackLab.git
cd TrackLab
```

在 `TrackLabClient/Config/UserConfig.json` 中配置数据库连接（该文件已被 Git 忽略；已有文件时合并以下键）：

```json
{
  "System.Language": "zh-CN",
  "System.DBConnection": "Server=127.0.0.1;Port=3306;User ID=tracklab;Password=REPLACE_WITH_YOUR_PASSWORD;"
}
```

将示例地址、账号和密码替换为实际值。代码会追加 `Database=TrackLab;AllowUserVariables=True;`，连接配置中无需再指定数据库名。

```powershell
dotnet build TrackLab.sln --configuration Debug

# 仅在尚未安装 dotnet-ef 时执行
dotnet tool install --global dotnet-ef --version 8.0.6

# 首次部署或新增迁移后执行；会更新所配置的数据库
dotnet ef database update --project .\Framework\Core\Core.csproj --startup-project .\TrackLabClient\TrackLabClient.csproj --configuration Debug --no-build

dotnet run --project .\TrackLabClient\TrackLabClient.csproj --configuration Debug --no-build
```

也可用 VS 打开 `TrackLab.sln`，将 `TrackLabClient` 设为启动项目；同样需要先完成数据库配置与迁移。

客户端显示主窗口前会写入一条 `System / TrackLab started` 日志。当前没有数据库失败时的降级处理，也不会在启动时自动执行迁移；缺少连接配置、数据库不可达或缺少日志表都可能阻止正常启动。

运行配置位于输出目录 `TrackLabClient/bin/Debug/net8.0-windows/Config/`。
`UserConfig.json` 存在于源码配置目录时会按 `PreserveNewest` 复制到输出目录；运行时与设计时数据库工厂均通过 `ConfigManager` 从 `AppContext.BaseDirectory/Config/UserConfig.json` 读取配置。若迁移提示连接未配置，需检查设计时使用的输出目录及其中的配置文件。
配方目录位于同级的 `Recipes/`；客户端启动并加载菜单时会自动创建空目录。仓库不附带配方文件，使用时需将 `.rp` 文件放入该目录，再重新启动客户端以加载目录树。

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
| `System → Operation Log` | 操作日志 | 时间、用户、操作只读列表，按时间倒序加载，支持刷新与记录数显示 |
| `System → Shut Down` | 关机 | 关闭或重启（均需二次确认） |
| `Recipe` | 配方 | 浏览 `.rp` 目录树，查看配方 Header、Step、Config |
| `Setup → Hardware Monitor` | 硬件监视 | 选择模块查看 DI/DO，可切换 `WaferPresent` |
| `Setup → Language` | 语言 | 切换 English / Chinese |

**典型操作**

1. `System → Status` → 点击热板 → `Start Process` / `Complete`，观察 `Idle → Running → Idle`。
2. `Setup → Hardware Monitor` → 选择模块 → `Toggle WaferPresent`，观察 DI 值变化（表格本身只读）。
3. `System → Alarm List` → 输入关键字 → `Query`（`Name` 与 `Message` 同时生效）→ `Cancel` 清空恢复全部。
4. `Recipe` → 选择 `.rp` 配方 → 查看只读的 Header、Step、Config；点击 Step 中的 `Module Name` 或 `Recipe Name` 单元格可展开完整内容。
5. `System → Operation Log` → 查看启动记录 → `Refresh` 重新读取数据库。切换到该页面时也会加载记录。

## 项目结构

```text
TrackLab/
├─ Framework/
│  ├─ Core/          # 模块、状态机、IO、菜单、告警、Recipe、配置、多语言、数据库与日志
│  │  ├─ DataBase/   # DBManager、TrackLabDbContext、设计时数据库工厂
│  │  ├─ Log/        # 操作日志实体
│  │  └─ Migrations/ # EF Core 数据库迁移
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

> `Name` 同时充当界面翻译的 key（去掉空格后先查 `Lang-Phrase`，再查 `Lang-Word`）。新增菜单若忘记补翻译条目，界面会显示去掉空格后的 key，中英文切换对该项无效。操作日志菜单 `Operation Log` 对应翻译键 `OperationLog` 和图标键 `OperationLogIcon`。

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

> 模块占位符必须用普通 `Text="LP01"` 书写，**不能**改用 `lan:LanguageExtension.Text`——替换逻辑读取的是 `TextBlock.Text`，改用附加属性会导致匹配失败。区域标题（`LoadPortArea` 等）不受此限制。

### 用户配置 `Config/UserConfig.json`

`ConfigManager` 以字符串键值对形式读写该文件，路径为 `AppContext.BaseDirectory/Config/UserConfig.json`。当前客户端启动需要有效的数据库连接配置，因此首次运行前就应准备该文件。

| 键 | 用途 |
| --- | --- |
| `System.Language` | 语言偏好，支持 `en`、`zh-CN`（兼容 `cn`）；未配置时默认英文 |
| `System.DBConnection` | MySQL 基础连接字符串，数据库名由代码固定为 `TrackLab` |

语言切换会更新运行目录中的配置文件。源码配置文件与运行目录中的副本是两个文件，部署或重新构建时需留意 `PreserveNewest` 的覆盖行为。

### 数据库与操作日志

`Framework/Core/DataBase/DBManager.cs` 从用户配置创建 EF Core 上下文；`DBContextFactory` 为迁移工具提供设计时上下文。已有 `InitialCreate` 迁移负责创建 `OperationLogs` 表：

| 字段 | 含义 |
| --- | --- |
| `Id` | 自增主键 |
| `Time` | 操作时间 |
| `User` | 用户或来源 |
| `Action` | 操作描述 |

`AddOperationLog()` 写入并保存日志；`GetOperationLogs()` 以无跟踪查询读取全部记录，按 `Time` 降序返回。客户端目前只在启动时自动写入一条记录，尚未为其他界面操作统一记录日志；`User = System` 也不代表已实现用户登录。

操作日志页面为只读列表，切换进入时加载数据，也可手动刷新。当前没有筛选、分页、导出、删除或自动定时刷新；记录较多时仍会一次性读取全部数据。告警页继续使用内存中的演示数据，与操作日志表独立。

### Recipe 配方

配方以文件夹树组织，文件扩展名为 `.rp`，内容为 JSON。`RecipeManager` 在运行目录下维护 `Recipes/` 根目录（首次实例化时自动创建，不属于 `Config/`，也不随构建复制）。当前页面在创建时读取目录树；运行期间外部新增文件后，需重新启动客户端才能重新扫描。

| 类型 | 职责 |
| --- | --- |
| `RecipeManager` | 单例，提供 `Recipes/` 根路径、配方树、按相对路径查找节点，以及递归枚举全部配方 |
| `RecipeNodeItem` | 递归构建目录树，区分文件夹与 `.rp` 配方；提供类型、相对路径以及 `Load()` / `Save()` |
| `RecipeData` | 配方内容：`Header` / `Config` 为键值对，`Step` 为键值对列表；支持 JSON 序列化与 `Copy()` |

`RecipeNodeItem` 暴露的相对配方路径不包含 `.rp` 扩展名。例如 `Recipes\ProcessA\Demo.rp` 对应：

| 属性 | 值 | 说明 |
| --- | --- | --- |
| `Name` | `Demo` | 配方文件名 |
| `PathWithType` | `ProcessA\Demo` | 相对 `Recipes/` 根目录的完整配方路径 |
| `RecipeType` | `ProcessA` | 相对路径的第一层目录 |
| `Path` | `Demo` | 去除第一层类型目录后的路径 |

`FindNodeByPath()` 使用 `PathWithType` 格式查找配方节点；`FindAllRecipe()` 会递归返回树中的所有 `.rp` 配方节点。

`Recipe` 菜单已接入只读浏览页面：左侧选择配方，右侧显示名称、类型以及 Header / Step / Config。Step 表格展示 `StepNo`、`ModuleType`、`ModuleName`、`RecipeName`、`IsValid`；点击较长的模块名或配方名可查看完整值。界面尚无新建、复制、编辑或保存操作；`RecipeNodeItem` 的读写与复制方法目前仅供代码调用。

### 多语言

资源按用途分为两组，均位于 `Framework/Core/Language/`：

| 文件 | 用途 | 示例 key |
| --- | --- | --- |
| `Lang-Word.*.resx` | 单词与短语（菜单、表头、按钮、枚举值） | `AlarmList`、`HotPlate`、`Running` |
| `Lang-Phrase.*.resx` | 完整句子（提示语、确认框） | `Status.NeedRecipe`、`ShutDown.Confirm` |

每组含默认、`.en`、`.zh-CN` 三个变体，key 须保持一致。`GetString` **先查 `Lang-Phrase`，再查 `Lang-Word`**，均未命中时原样返回 key。

XAML 通过附加属性使用，key 会自动去除空格；枚举值同样可翻译：

```xml
<TextBlock lan:LanguageExtension.Text="ModuleInformation" />
<TextBlock lan:LanguageExtension.Text="{Binding Module.State}" />
```

代码中可用前缀重载，等价于 `"ShutDown" + "." + "Confirm"`：

```csharp
LanguageManager.GetString("ShutDown", "Confirm");
```

语言切换时 `LanguageManager.LanguageChanged` 触发，`LanguageExtension` 自动刷新已挂载的控件文本。

Recipe 的页面标题、分组、表头和详情弹窗已接入语言资源。配方内容、模块标识、告警与日志中的业务数据按原值展示，不会随界面语言自动翻译。

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
| TrackLabClient | 布局、详情、DI/DO 监视、告警、操作日志、关机、语言切换均已实现；首页为占位 |
| Core | 模块反射加载、FSM、IO、菜单、告警、配置、多语言、Recipe 文件读写及 MySQL 操作日志访问已实现 |
| 操作日志 | 启动写入、倒序只读查看和刷新已实现；筛选、分页与其他操作的自动记录待补充 |
| Recipe 页面 | 已实现 `.rp` 目录树和 Header / Step / Config 只读查看；尚无界面编辑与保存 |
| UI | `ModuleControl`、`IODisplay`、转换器、两套主题已实现 |
| TrackLabSimulator | 仅空窗口 |
| 测试 | 尚无测试项目 |

提交前请执行 `dotnet build TrackLab.sln --configuration Debug`。构建通过不等于功能验证，建议按“界面导览”手动回归关键路径。

当前 `.gitignore` 的 `[Ll]og/` 规则也会匹配源码目录 `TrackLabClient/View/Log/`。操作日志页的 `OperationView.xaml`、`OperationView.xaml.cs`、`OperationViewModel.cs` 已存在于本地，但尚未纳入 Git 跟踪；发布相关菜单前需确保这三个文件一并提交，否则其他克隆副本无法取得页面实现。

## Roadmap

- [ ] 接入真实设备通信或设备抽象层
- [ ] 为 Recipe 页面补充新建、复制、Step / Config 编辑与保存
- [ ] IO / 状态 / 告警迁移到持久化存储
- [ ] 实现 Simulator 仿真与通信
- [ ] 告警查询接入真实数据，补充确认与清除流程
- [ ] 为更多用户操作写入日志，增加日志筛选、分页与导出
- [ ] 补充数据库连接与读写失败时的异常处理
- [ ] 为 Core 层添加单元测试
