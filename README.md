# Vulkan on Unity

## 概要
Linuxホスト上のUnityエディターでVRChat SDKがプロジェクトに導入されているときにレンダリングバックエンドがOpenGLに設定されないように強制するためのエディタ拡張です。

## 必要な人
以下の**すべての条件**を満たす人だけがこのエディタ拡張を必要とします。

1. UnityエディターをLinuxマシン上で動かしている
     * Linuxでない場合は影響を受けないため、この拡張を導入する必要がありません。
2. VRChat SDKをUnityのプロジェクトに導入している
     * VRChat SDKを導入していない場合は影響を受けません。
3. OpenGLバックエンドに強制されることによって何らかの実害を受けている

## OpenGLバックエンドに強制されることによる実害とは？
現在、以下の症例が確認されています。

### 機能が豊富なシェーダーでサポートされているテクスチャの上限数に当たる
例えば、lilToonなどの機能が豊富なシェーダーで

```
Shader 'Hidden/ltspass_opaque' uses 52 texture parameters, more than the 32 supported by the current graphics device.
```
というエラーが出ている場合が該当します。

### lilToonのぼかし機能がエディタ上で正しく描画されていない
To Be Documented: OpenGLだと視点を動かすたびに異なる色で描画されますが、本来は半透明になる場合屈折して描画されるのが正常です。

### TexTransToolsのSingleGradation
[SingleGradationDecalの説明](https://ttt.rs64.net/docs/Reference/SingleGradationDecal)

TexTransToolsのポリシーにおいて、OpenGLのサポートはVulkanに比べて消極的とされています\[要出典\]。

参考: [GTX10XX や GTX9XX 系の環境で正しく TTT が動作しない](https://ttt.rs64.net/docs/FAQ#gtx10xx-%E3%82%84-gtx9xx-%E7%B3%BB%E3%81%AE%E7%92%B0%E5%A2%83%E3%81%A7%E6%AD%A3%E3%81%97%E3%81%8F-ttt-%E3%81%8C%E5%8B%95%E4%BD%9C%E3%81%97%E3%81%AA%E3%81%84)

## 導入手順

> [!WARNING]
> 続ける前に、定期的にバックアップやセーブを行う環境が揃っていることを確認してください。
> VRChatが執筆時点で推奨しているバージョン2022.3.22f1には、特定ホストOSにおいて描画パイプラインがVulkanになっていると不定期に**クラッシュするバグ**があります。

> [!NOTE]
> セットアップ・マシンリソースの準備の難易度が高いですが、上述の環境を整える代わりに [ReinaS-64982/VRCAvatarBuildServerTool](https://github.com/ReinaS-64892/VRCAvatarBuildServerTool) を使用することも可能です。

> [!NOTE]
> ALCOM など、コマンドラインからUnityエディターを起動する場合は `-force-vulkan` スイッチを与えても同じ効果が得られるため、このパッケージをインストールする必要はない可能性があります。
> ALCOM を使用する際の手順は[TexTransToolsのQ&Aページ](https://ttt.rs64.net/docs/FAQ#unityeditor-%E3%82%92-vulkan-%E3%81%A7%E8%B5%B7%E5%8B%95%E3%81%99%E3%82%8B)を、コマンドライン引数の詳細については [Unity のマニュアル](https://docs.unity3d.com/ja/2022.3/Manual/EditorCommandLineArguments.html) を参照してください。

1. このGitレポジトリをPackage Managerから追加する
2. 追加したらProject Settings > Player > Other settings > Auto Graphics API for Linuxのチェックを外す
3. ReorderableListでVulkanを一番上にする
4. Unityエディターを再起動する

注: VPMでの導入はサポートする気がありません。

## ライセンス
BSD 4-Clause License
