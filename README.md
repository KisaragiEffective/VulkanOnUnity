# Vulkan on Unity

## 概要
Linuxホスト上のUnityエディターでVRChat SDKがプロジェクトに導入されているときにレンダリングバックエンドがOpenGLに設定されないように強制するためのエディタ拡張です。

このプロジェクトが役に立った場合は、同じ問題を指摘している [Canny のチケット](https://feedback.vrchat.com/sdk-bug-reports/p/vrc-sdk-keeps-re-enabling-auto-graphics-for-linux-forcing-unity-to-open-in-openg?utm_source=chatgpt.com) に upvote してください。

## 必要な人
以下の**すべての条件**を満たす人だけがこのエディタ拡張を必要とします。

1. UnityエディターをLinuxマシン上で動かしている
     Linuxでない場合は影響を受けないため、この拡張を導入する必要がありません。
2. VRChat SDKをUnityのプロジェクトに導入している
     VRChat SDKを導入していない場合は影響を受けません。
3. OpenGLバックエンドに強制されることによって何らかの実害を受けている
     VRChat SDKは、前述のCannyのチケットで説明されているように、Linux環境においてプロジェクトの設定を無視し、結果として毎回の起動で**Editorの**レンダリングバックエンドをOpenGLに設定します。

## OpenGLバックエンドに強制されることによる実害
現在、VRChat SDKがEditorのレンダリングバックエンドをOpenGLバックエンドに強制することで生じる実害として、以下の症例が確認されています。

### 機能が豊富なシェーダーでサポートされているテクスチャの上限数に当たる
例えば、lilToonなどの機能が豊富なシェーダーで
```
Shader 'Hidden/ltspass_opaque' uses 52 texture parameters, more than the 32 supported by the current graphics device.
```
というエラーが出ている場合が該当します。

`32` [^1]は通常、ドライバーなどの環境によって変動しますが、`glxinfo -l | grep GL_MAX_TEXTURE_IMAGE_UNITS` によって容易に確かめることができます。

[^1]: https://forums.developer.nvidia.com/t/question-about-shaders-and-l2-texture-caches/238594

```
$ glxinfo -l | grep GL_MAX_TEXTURE_IMAGE_UNITS
    GL_MAX_TEXTURE_IMAGE_UNITS = 32
    GL_MAX_TEXTURE_IMAGE_UNITS = 32
```

一方で、Vulkanにはこの制限がないこともあります。

```
$ vulkaninfo | grep -E 'max(PerStageDescriptor|DescriptorSet)(SampledImages|Samplers)|maxPerStageResources'
        maxPerStageDescriptorSamplers                   = 1048576
        maxPerStageDescriptorSampledImages              = 1048576
        maxPerStageResources                            = 4294967295
        maxDescriptorSetSamplers                        = 1048576
        maxDescriptorSetSampledImages                   = 1048576
        maxPerStageDescriptorSamplers                   = 1000000
        maxPerStageDescriptorSampledImages              = 1000000
        maxPerStageResources                            = 1000000
        maxDescriptorSetSamplers                        = 1000000
        maxDescriptorSetSampledImages                   = 1000000
```

(注: これはnvidiaのプロプライエタリドライバーで実行した場合の結果です。実際には環境で使用するドライバーによって容易に変わり得ます)

### lilToonのぼかし機能がエディタ上で正しく描画されていない
To Be Documented: OpenGLだと視点を動かすたびに異なる色で描画されますが、本来は半透明になる場合屈折して描画されるのが正常です。

### TexTransToolsのSingleGradation
[SingleGradationDecalの説明](https://ttt.rs64.net/docs/Reference/SingleGradationDecal)

TexTransToolsのポリシーにおいて、OpenGLのサポートはVulkanに比べて消極的とされています\[要出典\]。

参考: [GTX10XX や GTX9XX 系の環境で正しく TTT が動作しない](https://ttt.rs64.net/docs/FAQ#gtx10xx-%E3%82%84-gtx9xx-%E7%B3%BB%E3%81%AE%E7%92%B0%E5%A2%83%E3%81%A7%E6%AD%A3%E3%81%97%E3%81%8F-ttt-%E3%81%8C%E5%8B%95%E4%BD%9C%E3%81%97%E3%81%AA%E3%81%84)

### Editor.log が肥大化する

lilToonでは、通常32枚以上のテクスチャがサンプルされます。

[その結果として](https://github.com/lilxyzw/lilToon/issues/387)、`State comes from an incompatible keyword space` というエラーが毎フレームスタックトレースを伴ってコンソールに出力されます。

それだけではなく、同時に書き込まれているEditor.logも肥大化するため、長い時間放っておくと容易にディスクを食いつぶします。

## 導入手順
**重要: VPMでの導入はサポートする気がありません。**

1. このGitレポジトリをPackage Managerから追加する
2. 追加したらProject Settings > Player > Other settings > Auto Graphics API for Linuxのチェックを外す
3. リストにVulkanを付け足す
4. ReorderableListでVulkanをドラッグして一番上にする
5. エディタを再起動する

## ライセンス
BSD 4-Clause License
