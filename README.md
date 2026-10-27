# YequQteSkip · 夜雀之歌跳过

《东方夜雀食堂》（Touhou Mystia Izakaya）的 BepInEx 小 mod：手动做菜时不再弹出「夜雀之歌」小音游，直接按满分结算。不用开自动做菜，其余流程（奖励、倒计时）照游戏原样走。

## 安装

1. 先给游戏装好 **BepInEx 6（IL2CPP 版，be.783 上测过）**。它不在 BepInEx 的 GitHub Releases 里（那里是 5.x），要去构建站下：[BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.783](https://builds.bepinex.dev/projects/bepinex_be/783/BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.783%2Bc58c42d.zip)。解压到游戏 exe 所在的那一层，启动一次游戏让它生成 `BepInEx/interop`。第一次启动会联网下载 Unity 库，要等几分钟，进到主界面再关。
2. 到 [Releases](../../releases) 下载 `YequQteSkip.dll`。
3. 放进 `游戏目录/BepInEx/plugins/YequQteSkip/YequQteSkip.dll`。
4. 启动游戏，日志里出现「夜雀之歌跳过已加载」就成了。

### 装不上？

- **找不到对的版本**：文件名里要同时有 `Unity.IL2CPP` 和 `win-x64`。带 `Mono` 的、5.x 的都不认。
- **没生成 `BepInEx/interop`**：
  - 看 `BepInEx` 文件夹里有没有 `LogOutput.log`。没有的话，说明 BepInEx 没加载：检查 `winhttp.dll`、`doorstop_config.ini` 是不是和游戏 exe 在同一层，再看看杀毒软件有没有把 `winhttp.dll` 删掉。
  - 有日志但还是没生成：多半是第一次启动时 Unity 库没下载下来。开着加速器重新启动一次，等它跑完。

想恢复原样：删掉这个 dll，或者把 `BepInEx/config/xu.yequ.qteskip.cfg` 里的 `Enabled` 改成 `false`。

## 原理

手动开锅时游戏会走 `WorkSceneSustainedPannel.OpenQTEPanelInternalAsync` 弹出音游、等一个分数。这个 mod 用 Harmony 拦住这一步，不弹面板，直接交回已完成的 `1`（实测打满时就是 1），后面照常结算。出错会自动退回原版，照常弹音游。

不读写存档。

## 致谢

谢谢 [mystia-steward-companion（掌柜助手）](https://github.com/blockshy/mystia-steward-companion)。读它代码时才知道这一关可以绕过去，思路是从那儿来的。本 mod 没有使用它的代码，拦截的位置也不同。

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
