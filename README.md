# Vulkan on Unity

## 概要
Linuxホスト上のUnityエディターでVRChat SDKがプロジェクトに導入されているときにレンダリングバックエンドがOpenGLに設定されないように強制するためのエディタ拡張です。

## 必要な人
以下の**すべての条件**を満たす人だけがこのエディタ拡張を必要とします。

1. UnityエディターをLinuxマシン上で動かしている
     Linuxでない場合は影響を受けないため、この拡張を導入する必要がありません。
2. VRChat SDKをUnityのプロジェクトに導入している
     VRChat SDKを導入していない場合は影響を受けません。
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
**重要: VPMでの導入はサポートする気がありません。**

1. このGitレポジトリをPackage Managerから追加する
2. 追加したらProject Settings > Player > Other settings > Auto Graphics API for Linuxのチェックを外す
3. ReorderableListでVulkanを一番上に
4. 再起動

## ライセンス
BSD 4-Clause License
