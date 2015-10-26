# 3. マルチスレッド対応

* [await async を用いたシンプルな非同期メソッドの作成と利用](http://www.ipentec.com/document/document.aspx?page=csharp-simple-async-method)

* [タスクの並列化 (タスク並列ライブラリ)](https://msdn.microsoft.com/ja-jp/library/dd537609.aspx)

* [非同期処理でUIスレッドを操作する方法](http://d.hatena.ne.jp/lironi5/20130621/p1)

* [タスクの実行とキャンセル、バックグラウンドからのUI更新](http://win8dev.hatenablog.com/entry/2012/12/22/213217)

* [ThreadじゃなくTaskを使おうか？](http://qiita.com/Temarin_PITA/items/ff74d39ae1cfed89d1c5)

* [連載：C# 5.0＆VB 11.0新機能「async／await非同期メソッド」入門](http://www.atmarkit.co.jp/ait/subtop/features/dotnet/app/masterasync_index.html)

1. csvを3つ用意
2. テーブルを全削除
3. ファイル読み込み
4. データベースに登録


* 同期処理でCSVファイルを読み込む
  - CsvImportManager

これはソースコードを予め書いておく。ざっと解説だけ行う。

デバッグ実行してみる。
ボタンをクリックすると、UIが固まってしまうことを確認。

* 非同期処理でCSVファイルを読み込むように修正する
  - CsvImportManager


```cs
using(var db = new MyDbContext())
{
  await db.Database.ExecuteSqlCommandAsync(@"TRUNCATE TABLE MyTable");
}
```
