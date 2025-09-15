## 概要
- `MapTo: Map`
  - 遷移先マップのPrefab。
- `StartTile: TileBase`
  - 遷移先での開始位置を示すタイル。
- `StartDirection: Direction`
  - 遷移先でのプレイヤー向き。
- `override void Exec(RPGSceneManager manager)`
  - 現在の `ActiveMap` 破棄、`MapTo` をインスタンス化して `ActiveMap` に設定。
  - 新マップで `StartTile` の座標を検索し、プレイヤーの位置と向きを初期化。

## レビュー結果
