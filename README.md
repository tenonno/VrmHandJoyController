# VrmHandJoyController

### 導入手順

1. [JoyconLib](https://github.com/Looking-Glass/JoyconLib) を導入する  
zip をダウンロードして `JoyconLib-master/Packages/con.lookingglass.joyconlib/JoyconLib_scripts` をプロジェクトに追加する
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
![preview](https://i.gyazo.com/f7718083a8275c23248a256bfc1d03d1.png)  

### JoyCon のボタンと指の動き
`VrmHandJoyController` の `Hand Pose Setting` で JoyCon のボタンと指の動きを設定する
![preview](https://i.gyazo.com/17608bec20d5fa880f44032a8470db17.png)  
`DPAD_DOWN`: ボタンの名称  
`__Enabled`: ボタンを使用するか  
`__Spread`: 指の開きを制御する（一括）  
`__Muscles`: 指を曲がりを制御する（一括）  
`__Thumb Finger`: 親指の制御  
`____Enabled`: ボタンを押したときに親指を操作するか  
`____All`: 全ての関節を一括で曲げる  
`____Spread`: 指の開き  
`____1st`: 第一関節の曲がり  
`____2nd`: 第二関節の曲がり  
`____3rd`: 第三関節の曲がり  
`__Index Finger`: 人差し指の制御  
`__Middle Finger`: 中指の制御  
`__Ring Finger`: 薬指の制御  
`__Little Finger`: 子指の制御  

### デフォルトの手の形
`VrmHandJoyController` の `Default Hand Pose` を編集して JoyCon のボタンを押していないときの手の形を設定する  

### その他の設定
`Lerp Coeffcient`: 手の形を滑らかに変える処理の係数 (0.0 ~ 1.0), 小さいとゆっくり、大きいと速く形が変わる  
`Keep Root Bone Position`: RootBone の位置を保つようにするか, 内部で Animator を使う影響でモデルの位置がずれる場合の対策  
`JoyCon Status`: JoyCon が接続できているなら True, できていないなら False が表示される   

### 再生しながら手の動きを調整する
Unity を再生しながら `Hand Pose Setting` などを調整することでリアルタイムに手の動きを確認することができるが、再生中に編集した値は再生を止めるとリセットされてしまう  
再生中に編集した値を保持したい場合は、再生中に `VrmHandJoyController` コンポーネントの右上のメニューから `Copy Component` を選択して値をコピーして、再生を止めた後に `Paste Component Value` で値を貼り付けることができる  
