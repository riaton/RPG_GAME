# ItemShopEvent.cs の概要

## 役割
- アイテムショップイベントを表す `MassEvent` の派生クラス。

## フィールド
- `Items: List<Item>`
  - ショップで扱うアイテム一覧。
- `Message, AskBuyMessage, BuyMessage, NotEnoughMoneyMessage, ItemCountOverMessage, CloseMessage: string`
  - メッセージ文言（複数行対応、`TextArea`）。

## メソッド
- `override void Exec(RPGSceneManager manager)`
  - `RPGSceneManager.ItemShopMenu` を開き、このイベント（`this`）を渡して初期化する。
