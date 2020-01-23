using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace SeatMonitoringAPI.Models
{
     /// <summary>
     /// 初期化処理が必要なシングルトンクラス
     /// キーに「Moniker」と「Name」が存在するJson形式のStreamをInitializeメソッドに渡して初期化する
     /// 初期化は1度のみ可能で、2度目の初期化には例外を発生する
     /// 初期化されていない状態でInstanceをGetすると例外を発生する
     /// </summary>
    public class Configuration
    {
        private static Configuration instance = null;
        public static Configuration Instance
        {
            get
            {
                if (instance == null)   // 初期化されていない場合
                {
                    throw new InvalidOperationException("Configurationが初期化されていません。");
                }
                return instance;
            }
        }
        public readonly ReadOnlyCollection<SeatDefinition> seatDefinitions;

        /// <summary>
        /// Json形式のStreamをSeatDefinition型のListに変換してInstanseプロパティに代入するコンストラクタ
        /// </summary>
        /// <param name="streamReader"></param>
        private Configuration(StreamReader streamReader)
        {
            dynamic jsonObj;

            // Json形式の文字列をデシリアライズ
            jsonObj = JsonConvert.DeserializeObject<dynamic>(streamReader.ReadToEnd());

            var seatDefinitions = new List<SeatDefinition>();
            foreach(var jsonObjElement in jsonObj)
            {
                string moniker = jsonObjElement.Moniker;
                string name = jsonObjElement.Name;

                if (moniker == null || name == null)
                {
                    throw new InvalidOperationException("Jsonファイルのキーが不正です。");
                }

                seatDefinitions.Add(new SeatDefinition(moniker, name));
            }
            this.seatDefinitions = new ReadOnlyCollection<SeatDefinition>(seatDefinitions);
        }

        /// <summary>
        /// 初期化用メソッド
        /// Streamを受け取って自身のコンストラクタを呼び出し、インスタンスを作成する
        /// </summary>
        /// <param name="streamReader"></param>
        public static void Initialize(StreamReader streamReader)
        {
            instance = instance == null ? new Configuration(streamReader) : throw new InvalidOperationException("Configurationは既に初期化されています。");
        }
    }
}