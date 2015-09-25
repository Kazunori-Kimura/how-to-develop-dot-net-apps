using System.Collections.Generic;
using System.Linq;

namespace CsvImportTool
{
    class Program
    {
        /// <summary>
        /// 取り込み対象のCSVファイル
        /// </summary>
        const string FILE_PATH = @"C:\CSV\in\yakuhin.csv";

        /// <summary>
        /// CSVを読み込み、薬品情報のリストを返す
        /// </summary>
        /// <param name="path">CSVファイルの絶対パス</param>
        /// <returns>薬品情報のリスト</returns>
        static List<Yakuhin> ReadCsv(string path)
        {
            List<Yakuhin> list = new List<Yakuhin>();
            var enc = new System.Text.UTF8Encoding(false);

            using (var reader = new System.IO.StreamReader(path, enc))
            {
                var csv = new CsvHelper.CsvReader(reader);
                while (csv.Read())
                {
                    string drugCode = csv.GetField<string>(0);
                    string clsCode = csv.GetField<string>(1);
                    string clsName = csv.GetField<string>(2);
                    string drugName = csv.GetField<string>(3);
                    string company = csv.GetField<string>(4);

                    var yakuhin = new Yakuhin
                    {
                        DrugCode = drugCode,
                        ClassificationCode = clsCode,
                        ClassificationName = clsName,
                        DrugName = drugName,
                        Company = company
                    };

                    list.Add(yakuhin);
                }
            }

            return list;
        }

        /// <summary>
        /// 薬品情報のリストをデータベースに登録する
        /// </summary>
        /// <param name="list">薬品情報リスト</param>
        /// <returns>薬品情報の登録件数</returns>
        static int RegistDrugInfo(List<Yakuhin> list)
        {
            int count = 0;

            using (var db = new DrugInfoContext())
            {
                foreach(var yakuhin in list)
                {
                    // 医薬分類
                    var classification = db.Classifications.FirstOrDefault(
                        item => item.ClassificationCode == yakuhin.ClassificationCode);

                    if (classification == null)
                    {
                        // 医薬分類を登録する
                        classification = new Classifications
                        {
                            ClassificationCode = yakuhin.ClassificationCode,
                            Name = yakuhin.ClassificationName
                        };
                        db.Classifications.Add(classification);
                    }

                    // 薬品情報
                    var drug = db.Drugs.FirstOrDefault(
                        item => item.DrugCode == yakuhin.DrugCode);
                    
                    if (drug == null)
                    {
                        // 薬品情報を登録する
                        drug = new Drugs
                        {
                            DrugCode = yakuhin.DrugCode,
                            Name = yakuhin.DrugName,
                            Company = yakuhin.Company,
                            ClassificationId = classification.ClassificationId
                        };
                        db.Drugs.Add(drug);

                        // 登録件数をインクリメント
                        count++;
                    }

                    // DB更新
                    db.SaveChanges();
                }
            }

            return count;
        }
        
        static void Main(string[] args)
        {
            System.Diagnostics.Debug.WriteLine("Start");

            // CSV読み込み
            var list = ReadCsv(FILE_PATH);

            // DB登録
            int count = RegistDrugInfo(list);

            System.Diagnostics.Debug.WriteLine(
                string.Format("{0}件 登録しました。", count));
            
            System.Diagnostics.Debug.WriteLine("End");
        }
    }
}
