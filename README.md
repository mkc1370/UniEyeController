# SimpleEyeController
Unityで視線制御をしたいときに使うアセット

## 特徴
* 簡単なセットアップで視線を制御することができます
* 初期状態で目に回転が入っていても正しく動作します

## 導入方法
### UPM (Git URL)
1. Unity Package Managerを開く
2. Add Package from Git URLを選択
3. `https://github.com/mkc1370/SimpleEyeController.git`を追加

## 使い方 
1. 目を動かしたいキャラクターのAnimatorに目のボーンを設定する
2. `EyeController`をアタッチする
3. Settingsの目の可動域を設定する

## 注意
* 現在のバージョンではStart時にキャラクターがAやTポーズで正面を向いている必要があります。
* 目のボーンが目（虹彩）より外側にある場合は正しく動作しない場合があります

## ToDo List
[Project](https://github.com/users/mkc1370/projects/3)