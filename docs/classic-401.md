# 3. 非同期処理の基礎 - (2)

それでは、実際に非同期処理を行うアプリケーションを開発します。

CSVの内容を読み込み、データベースに取り込むアプリケーションを作成していきます。

<br>

## 準備

300行のCSVファイルを5つ用意しましたので、それを `C:¥CSV¥in` に保存してください。  
(この5つ以外のファイルは削除するか、別フォルダに移動させてください。)

* [その1](yakuhin1.csv)
* [その2](yakuhin2.csv)
* [その3](yakuhin3.csv)
* [その4](yakuhin4.csv)
* [その5](yakuhin5.csv)

<br><br>

## プロジェクトの作成

*CsvImportManager* という名前で *Windowsフォーム アプリケーション* を作成します。

以前使用した *CsvHelper* と *EntityFramework* を使用するので、*NuGet* からインストールします。

<br><br>

## 多重起動の禁止

[Mutexクラス](https://msdn.microsoft.com/ja-jp/library/system.threading.mutex.aspx) を使用して、
アプリケーションが複数立ち上がらないようにします。

`Program.cs`

```cs
static class Program
{
    /// <summary>
    /// アプリケーションのメイン エントリ ポイントです。
    /// </summary>
    [STAThread]
    static void Main()
    {
        bool createdNew;

        // Mutexの作成
        var mutex = new System.Threading.Mutex(true, "CsvImportManager", out createdNew);

        if (createdNew == false)
        {
            // 新たにMutexが作成できない -> すでに起動中であると判断
            MessageBox.Show("多重起動はできません。");
            return;
        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form1());

        // Mutexを解放
        mutex.ReleaseMutex();
    }
}
```

アプリを多重起動しようとすると、メッセージボックスを表示して起動をキャンセルします。

<br><br>

## コントロールの配置

CSVの取り込み処理を開始するボタンを配置します。

また、現在の処理過程を表示するようにラベルを配置します。

<br>

### コントロールの初期化処理

プログレスバーとラベルの初期化処理を実装します。

```cs
/// <summary>
/// プログレスバー、ラベルの初期化
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void Form1_Load(object sender, EventArgs e)
{
    this.label1.Text = @"";
    this.label2.Text = @"";
    initializeProgressBar(this.progressBar1, 0, 100, 0);
    initializeProgressBar(this.progressBar2, 0, 100, 0);
}

/// <summary>
/// プログレスバーの初期化処理
/// </summary>
/// <param name="bar"></param>
/// <param name="min"></param>
/// <param name="max"></param>
/// <param name="value"></param>
private void initializeProgressBar(ProgressBar bar, int min, int max, int value)
{
    bar.Minimum = min;
    bar.Maximum = max;
    bar.Value = value;
}

```

<br><br>

## EDMの作成

これまで同様、 *データベース・ファースト* で開発していきます。

(1) *ソリューション エクスプローラー* をクリックします。

(2) プロジェクト名を右クリックし、*追加* -> *新しい項目* をクリックします。

(3) *データ* から *ADO.NET Entity Data Model* を選択し、名前を `DrugInfoModel` とします。

(4) *データベースから EF Designer* を選択し、 *次へ* をクリックします。

(5) サーバーが選択されていることを確認します。

(6) *接続設定に名前を付けて App.Config に保存* にチェックをし、 `DrugInfoContext` と入力して *次へ* をクリックします。

(7) バージョンの選択ウィンドウが表示される場合は、 *Entity Framework 6.x* を選択して *次へ* をクリックします。

(8) *テーブル* にチェックを入れて *完了* をクリックします。

(9) *セキュリティ警告* が表示される場合は、 *今後このメッセージを表示しない* にチェックし、 *OK* をクリックします。

(10) edmxファイルが生成され、デザイナーに取り込まれたテーブルの情報が表示されます。

<br><br>

## CSVファイルの読み込み処理

CSVファイルの読み込み処理を実装していきます。

<br>

1. フォルダ内のファイルを列挙し
2. CsvHelperでファイルの中身を読み取り
3. Modelに格納し
4. リストを返します。

<br>

### 薬効分類、薬品情報クラスの作成

CSVから取り込んだデータを格納するModelクラスを作成します。


<br>

### CSVファイルの読み込み処理

非同期メソッドでCSVファイルを読み込む処理を作成します。

```cs
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CsvImportManager
{
    public partial class Form1 : Form
    {
        /* ~~ 省略 ~~ */

        /// <summary>
        /// CSV取り込みボタンのクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button1_Click(object sender, EventArgs e)
        {
            label2.Text = "開始";
            // csvファイル読み込み
            var task = await ReadCsvFiles();

            System.Diagnostics.Debug.WriteLine(
                string.Join("\r\n",
                    task.Item1
                    .Select(item => item.Code + ":" + item.Name).ToArray()));

            label2.Text = "完了";
        }

        /// <summary>
        /// CSVファイルの読み込み処理
        /// </summary>
        /// <returns></returns>
        private async Task<Tuple<List<Bunrui>, List<Yakuhin>>> ReadCsvFiles()
        {
            // フォルダ内のCSVファイルを取得する
            var files = Directory.GetFiles(CSV_DIR, "*.csv", SearchOption.TopDirectoryOnly);
            progressBar1.Maximum = files.Length;

            var data = await Task.Run(() => {
                var bunruiList = new List<Bunrui>();
                var yakuhinList = new List<Yakuhin>();

                var tasks = new List<Task<Tuple<List<Bunrui>, List<Yakuhin>>>>();

                foreach (var file in files)
                {
                    System.Diagnostics.Debug.WriteLine(file);
                    var task = ReadCsv(file);
                    tasks.Add(task);
                }
                // すべてのタスクが完了するのを待つ
                Task.WaitAll(tasks.ToArray());

                // 結果をマージ
                foreach(var task in tasks)
                {
                    MergeBunduiList(bunruiList, task.Result.Item1);
                    MergeYakuhinList(yakuhinList, task.Result.Item2);
                }

                return Tuple.Create(bunruiList, yakuhinList);
            });

            return data;
        }

        /// <summary>
        /// CSVファイルを読み込み、リストに格納する
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private async Task<Tuple<List<Bunrui>, List<Yakuhin>>> ReadCsv(string file)
        {
            var data = await Task.Run(() => {
                var classes = new List<Bunrui>();
                var drugs = new List<Yakuhin>();

                var fi = new FileInfo(file);
                if (fi.Exists)
                {
                    // shift_jis
                    var enc = Encoding.GetEncoding("shift_jis");
                    using (var reader = new StreamReader(fi.FullName, enc))
                    {
                        var csv = new CsvHelper.CsvReader(reader);

                        while (csv.Read())
                        {
                            string drugCode = csv.GetField<string>(0);
                            string clsCode = csv.GetField<string>(1);
                            string clsName = csv.GetField<string>(2);
                            string drugName = csv.GetField<string>(3);
                            string company = csv.GetField<string>(4);

                            // 分類コード
                            var cls = classes.Where(item => item.Code.Equals(clsCode)).FirstOrDefault();
                            if (cls == null)
                            {
                                var bunrui = new Bunrui
                                {
                                    Code = clsCode,
                                    Name = clsName
                                };
                                classes.Add(bunrui);
                            }

                            // 薬品
                            var yakuhin = new Yakuhin
                            {
                                Code = drugCode,
                                Name = drugName,
                                Company = company,
                                BunruiCode = clsCode
                            };
                            drugs.Add(yakuhin);
                        }
                    }
                }

                return Tuple.Create(classes, drugs);
            });

            return data;
        }

        /// <summary>
        /// 薬効分類リストの結合
        /// list1にlist2をマージする
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        private void MergeBunduiList(List<Bunrui> list1, List<Bunrui> list2)
        {
            foreach(var bunrui in list2)
            {
                var b = list1.Where(item => item.Code.Equals(bunrui.Code)).FirstOrDefault();

                if (b == null)
                {
                    list1.Add(bunrui);
                }
            }
        }

        /// <summary>
        /// 薬品情報リストの結合
        /// list1にlist2をマージする
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        private void MergeYakuhinList(List<Yakuhin> list1, List<Yakuhin> list2)
        {
            foreach(var yakuhin in list2)
            {
                var y = list1.Where(item => item.Code.Equals(yakuhin.Code)).FirstOrDefault();

                if (y == null)
                {
                    list1.Add(yakuhin);
                }
            }
        }
    }
}
```

*Tupleクラス*




## データの登録処理

### テーブルの初期化処理

```cs
using(var db = new MyDbContext())
{
  await db.Database.ExecuteSqlCommandAsync(@"TRUNCATE TABLE MyTable");
}
```


----------

ちなみに...

一つのCSVファイルを複数に分割する際に使用したプログラムを紹介しておきます。

```cs
using System.Collections.Generic;

namespace ConsoleApplication1
{
    class Program
    {
        /// <summary>
        /// 元ファイルパス
        /// </summary>
        const string CSV_FILE_PATH = @"C:\Users\kimura\Documents\医薬品サンプル.csv";
        /// <summary>
        /// 作成ファイル名
        /// </summary>
        const string FILE_NAME = @"yakuhin{0}.csv";
        /// <summary>
        /// 作成ファイル行数
        /// </summary>
        const int LINE_NUM = 300;
        /// <summary>
        /// 作成ファイル数
        /// </summary>
        const int FILE_NUM = 5;

        /// <summary>
        /// 元ファイルの読み込み
        /// </summary>
        /// <returns></returns>
        private static List<string> ReadCsvData()
        {
            var list = new List<string>();
            var enc = System.Text.Encoding.GetEncoding("shift_jis");

            var lines = new List<string>();

            int lineCount = -1; //ヘッダー行をスキップするため、-1から始める

            using (var reader = new System.IO.StreamReader(CSV_FILE_PATH, enc))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    lineCount++;
                    if (lineCount == 0)
                    {
                        continue;
                    }

                    lines.Add(line);

                    if (lineCount % LINE_NUM == 0)
                    {
                        // 改行コードでリストを結合して登録
                        list.Add(string.Join("\r\n", lines.ToArray()));
                        lines.Clear();
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// CSVファイルの書き出し
        /// </summary>
        /// <param name="data"></param>
        private static void WriteCsvFiles(List<string> data)
        {
            var enc = System.Text.Encoding.GetEncoding("shift_jis");

            int fileCount = 1;

            foreach(string lines in data)
            {
                if (fileCount > FILE_NUM)
                {
                    // ファイル数を超えたら終了
                    break;
                }

                string fileName = string.Format(FILE_NAME, fileCount);
                using (var writer = new System.IO.StreamWriter(fileName, false, enc))
                {
                    writer.WriteLine(lines);
                }

                fileCount++;
            }
        }

        static void Main(string[] args)
        {
            var list = ReadCsvData();

            WriteCsvFiles(list);
        }
    }
}
```

----------
