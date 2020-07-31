# VrmHandJoyController

### 導入手順

1. [JoyconLib](https://github.com/tenonno/JoyconLib/releases) を導入する  
リリースページから最新の `JoyconLib.unitypackage` をダウンロードしてインポートする  
1. [Unity-Wiimote](https://github.com/Flafla2/Unity-Wiimote) を導入する  
zip をダウンロードして `Unity-Wiimote-master/Assets/Wiimote/Plugins/(OS)/hidapi.(dll|bundle)` をプロジェクトに追加する  
windows なら `win64/hidapi.dll`, mac なら `mac/hidapi.bundle`
1. [VoxHands](https://github.com/hiroki-o/VoxHands) を導入する  
zip をダウンロードして `VoxHands-master/Assets` をプロジェクトに追加する  
![preview](https://i.gyazo.com/4d174fe4eb298e3e422d6c1c4829d2f4.png)  
1. [VrmHandJoyController](https://github.com/tenonno/VrmHandJoyController/releases) を導入する  
リリースページから最新の `VrmHandJoyController.unitypackage` をダウンロードしてインポートする  

### 使い方
1. 使いたい VRM モデルをシーンに配置する  
1. `VrmHandJoyController/Prefabs/VrmHandJoyController.prefab` をシーンに配置する  
1. シーンに配置したモデルを `VrmHandJoyController` の `Model` に割り当てる  
![preview](https://i.gyazo.com/386e16e184082fb5af06e8593ea5c088.png)  

### 手の形を登録する
Project ウィンドウの任意のディレクトリで右クリック -> Create -> VrmHandJoyController -> HandPosePreset を行い `HandPosePreset` を生成することで手の形を登録できる  
![preview](https://i.gyazo.com/9e8920ca03cfd28f44b5eb9b54f479fa.png)  
`Name`: 手の形の名称  
`Spread`: 指の開きを制御する（一括）  
`Muscles`: 指を曲がりを制御する（一括）  
`Thumb Finger`: 親指の制御  
`__Enabled`: ボタンを押したときに親指を操作するか  
`__All`: 全ての関節を一括で曲げる  
`__Spread`: 指の開き  
`__1st`: 第一関節の曲がり  
`__2nd`: 第二関節の曲がり  
`__3rd`: 第三関節の曲がり  
`Index Finger`: 人差し指の制御  
`Middle Finger`: 中指の制御  
`Ring Finger`: 薬指の制御  
`Little Finger`: 子指の制御  

### JoyCon のボタンと指の動き
`VrmHandJoyController` の `Hand Pose Setting` で JoyCon のボタンと指の動きを設定する
![preview](https://i.gyazo.com/932f67f341054b5ff0fe455e5ed5d991.png)  
`DPAD_DOWN`: ボタンの名称  
`Left`: 左の JoyCon でボタンを押したときの手の形  
`Right`: 右の JoyCon でボタンを押したときの手の形  

### デフォルトの手の形
`VrmHandJoyController` の `Default Hand Pose` に `HandPosePreset` を指定して JoyCon のボタンを押していないときの手の形を設定する  

### その他の設定
`Lerp Coeffcient`: 手の形を滑らかに変える処理の係数 (0.0 ~ 1.0), 小さいとゆっくり、大きいと速く形が変わる  
`Hand Source`: ジョイコンの入力を左右のどちらの手に割り当てるか指定する  
`JoyCon Status`: JoyCon が接続できているなら True, できていないなら False が表示される   

### 再生しながら手の動きを調整する
Unity を再生しながら `HandPosePreset` のパラメータを調整することでリアルタイムに手の動きを確認することができる
