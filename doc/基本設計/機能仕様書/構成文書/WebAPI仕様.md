# Seat Monitoring API仕様

## 監視座席の状態を取得

全ての監視座席の状態を取得する。

### HTTPリクエスト

`GET http://{IPアドレス}/SeatMonitoring/api/seats`

### レスポンス

ステータスコード ***200*** と以下のJSONオブジェクトを返す。

|プロパティ|タイプ|説明|
|:--|:--|:--|
|seatName|String|監視座席の名前|
|cameraExist|boolean|カメラの存在|
|humanExist|boolean|監視座席の判定結果|

レスポンスの例

```JSON
{
    "seatName":"Alice"
    "cameraExist":true
    "humanExist":true
},
{
    "seatName":"Bob"
    "cameraExist":false
    "humanExist":null
}
```
