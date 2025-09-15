## 役割
- マス上のイベントの基底クラス。
- `ScriptableObject` として定義され、各イベントアセットの親となる。

## フィールド
- `Tile: TileBase`
  - このイベントが紐づくタイルアセット。

## メソッド
- `virtual void Exec(RPGSceneManager manager)`
  - イベント実行メソッド。基底では未実装（空）。
  - 派生クラスで具体的なイベント処理を実装するフック。
