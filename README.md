# YequQteSkip · 夜雀之歌跳过

《东方夜雀食堂》（Touhou Mystia Izakaya）的 BepInEx 小 mod：手动做菜时不再弹出「夜雀之歌」小音游，直接按满分结算。不用开自动做菜，其余流程（奖励、倒计时）照游戏原样走。

## 安装

1. 先给游戏装好 **BepInEx 6（IL2CPP 版，be.783 上测过）**，启动一次游戏让它生成 `BepInEx/interop`。
2. 到 [Releases](../../releases) 下载 `YequQteSkip.dll`。
3. 放进 `游戏目录/BepInEx/plugins/YequQteSkip/YequQteSkip.dll`。
4. 启动游戏，日志里出现「夜雀之歌跳过已加载」就成了。

想恢复原样：删掉这个 dll，或者把 `BepInEx/config/xu.yequ.qteskip.cfg` 里的 `Enabled` 改成 `false`。

## 原理

手动开锅时游戏会走 `WorkSceneSustainedPannel.OpenQTEPanelInternalAsync` 弹出音游、等一个分数。这个 mod 用 Harmony 拦住这一步，不弹面板，直接交回已完成的 `1`（实测打满时就是 1），后面照常结算。出错会自动退回原版，照常弹音游。

不读写存档。

## 自己编译

需要 .NET 6 SDK。仓库里不带任何游戏或 BepInEx 的 dll，编译前自己从装好 BepInEx 的游戏目录拷：

- `BepInEx/core/*.dll` → `lib/core/`
- `BepInEx/interop/*.dll` → `lib/interop/`

然后：

```
dotnet build -c Release
```

产物在 `bin/Release/YequQteSkip.dll`。

## 说明

非官方 mod，与游戏开发方无关。在 Steam 版上自用测试过，别的版本不保证。
