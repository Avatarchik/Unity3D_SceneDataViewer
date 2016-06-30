# Unity3D_SceneDataViewer

Unity3D である Scene を実行するとき、実行時間や、FPS、シーン上にある各種リソース数などを表示するためのツールです。
メモリリークが発生していないか確認したり、演出の確認のために利用することを目的とします。

![](https://github.com/XJINE/Unity3D_SceneDataViewer/blob/master/screenshot01.png)

## 主な機能

- 実行時刻 / 現在時刻を表示する機能。
- シーンが開始してから経過した時間を表示する機能。(dhms / s)
- ループ時間基準での現在の実行時間を表示する機能。 (dhms / s)
  - ループした回数を表示する機能 
  - ループ時間を指定したときに限る。
- FPS を表示する機能。(max / min)
  - 更新時間を指定したときに限る。
- 現在シーンに存在するリソースの数を表示する機能。(max / min)
  - 更新時間を指定したときに限る。 
  - UnityEngine.Object
  - Texture
  - AudioClip
  - Mesh
  - Material
  - GameObject
  - Component

## 注意点

### 処理負荷について

リソースデータは、``Resources.FindObjectsOfTypeAll`` を使って取得しています。
このメソッドはすべてのリソースを走査(検索)するため、高い負荷がかかります。
特にリソース数が増えるたびに負荷は大きくなるので、データの更新レートは適切に設定してください。

### FPS とリソースデータの更新について

FPS とリソースデータは Window が表示されていないときは更新されません。
処理負荷を考慮してそのような実装にしました。
