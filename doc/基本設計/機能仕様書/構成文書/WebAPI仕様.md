# Seat Monitoring API仕様

目次

- [Seat Monitoring API仕様](#seat-monitoring-api%e4%bb%95%e6%a7%98)
  - [監視座席名のリストを取得](#%e7%9b%a3%e8%a6%96%e5%ba%a7%e5%b8%ad%e5%90%8d%e3%81%ae%e3%83%aa%e3%82%b9%e3%83%88%e3%82%92%e5%8f%96%e5%be%97)
    - [HTTPリクエスト](#http%e3%83%aa%e3%82%af%e3%82%a8%e3%82%b9%e3%83%88)
    - [レスポンス](#%e3%83%ac%e3%82%b9%e3%83%9d%e3%83%b3%e3%82%b9)
  - [監視座席の状態を取得](#%e7%9b%a3%e8%a6%96%e5%ba%a7%e5%b8%ad%e3%81%ae%e7%8a%b6%e6%85%8b%e3%82%92%e5%8f%96%e5%be%97)
    - [HTTPリクエスト](#http%e3%83%aa%e3%82%af%e3%82%a8%e3%82%b9%e3%83%88-1)
    - [レスポンス](#%e3%83%ac%e3%82%b9%e3%83%9d%e3%83%b3%e3%82%b9-1)

## 監視座席名のリストを取得

監視座席名のリストを取得する。

### HTTPリクエスト

`GET http://{IPアドレス}/SeatMonitoring/api/seatNameList`

### レスポンス

ステータスコード ***200*** と以下のJSONオブジェクトを返す。

|プロパティ|タイプ|説明|
|:--|:--|:--|
|devicePath|String|Webカメラが接続されているポートへの一意なパス|
|seatName|String|監視座席の名前|

レスポンスの例

```JSON
{
    "seatName":"Alice"
},
{
    "seatName":"Bob"
}
```

## 監視座席の状態を取得

指定した監視座席名の状態を取得する。指定した監視座席名に対応するWebカメラを認識できない場合、処理は失敗し、HTTP 500 (Internal Server Error) が返却される。

### HTTPリクエスト

`GET http://{IPアドレス}/SeatMonitoring/api/seats/{seatName}`

seatName：***String***(required)　監視座席名　例) Alice

### レスポンス

ステータスコード ***200*** と以下のJSONオブジェクトを返す。

|プロパティ|タイプ|説明|
|:--|:--|:--|
|humanExist|boolean|監視座席の判定結果|

レスポンスの例

```JSON
{
    "humanExist":true
}
```
