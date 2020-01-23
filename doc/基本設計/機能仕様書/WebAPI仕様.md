# Seat Monitoring API仕様

## 監視座席の状態を取得

全ての監視座席の状態を取得する。オブジェクトはlinkCameraAndNameファイルの1行目からみた順番と同じ順番になる。

### HTTPリクエスト

`GET http://{IPアドレス}/SeatMonitoring/api/seats`

### レスポンス

ステータスコード ***200*** と以下のJSONオブジェクトを返す。

|プロパティ|タイプ|説明|
|:--|:--|:--|
|name|string|監視座席の名前|
|status|string|監視座席の状態判定ができたかどうか|

レスポンスの例

```JSON
{
    "name":"Alice",
    "status":"Exists",
},
{
    "name":"Bob",
    "status":"NotExists"
},
{
    "name":"Cate",
    "status":"Failure"
}
```
