# ASP.NET MVC開発入門

ASP.NETは、Microsoftが開発・提供しているWebアプリケーションフレームワークです。

ここでは、ASP.NET MVCを使用したWebアプリケーションの開発について解説します。

<br>
<br>

## 1. ASP.NET MVC の概要

ASP.NET および ASP.NET MVC の概要について解説します。

* ASP.NET の概要
* ASP.NET MVC の概要

<br>

## 2. ASP.NET MVC の基礎

非常に簡単な ASP.NET MVC のサンプルを作成を通して Model,View,Controllerの役割について解説します。

* 「ASP.NET Webアプリケーション」プロジェクトの作成
* Modelクラスの作成
* Controllerクラスの作成
  - アクション メソッド
  - ActionResultクラス
  - Nullable型
* View
  - 共通レイアウトの作成
  - Viewファイルの作成
  - Razor
  - コードナゲット と コメントの書き方
  - HTMLヘルパー
* ルーティングの設定
* 動作確認

<br>

## 3. EntityFramework によるコードファースト開発

*EntityFramework* を使用した *コードファースト開発* について解説します。

コード ファースト開発は *EntityFramework 4.1* から提供された機能です。

データ構造を表現する `POCO` (Plain Old Clr Object: 特別なクラスやインターフェイスを継承していないクラス(のオブジェクト)) と
POCOを管理する Contextクラスを定義することで、EntityFrameworkが必要なテーブルを生成します。
(Databaseの操作は必要ありません。)

詳細については [Entity Framework (EF) の概要](http://msdn.microsoft.com/ja-jp/data/ee712907) を参照してください。


* 「ASP.NET Webアプリケーション」プロジェクトの作成
* NuGetによる EntityFrameworkのインストール
  - NuGetとは
  - EntityFrameworkとは
  - O/Rマッパーとは
  - インストール手順
* Modelクラスの作成
  - POCOの作成
  - DbContextクラスの作成
  - 属性(Annotation) について
* 共通レイアウトの作成
* Controllerの作成
  - スキャフォールディングによる自動生成
* ルーティングの設定
* 動作確認
* 生成されたソースコードの解説

<br>

## 4. EntityFramework によるデータベースファースト開発

すでにデータベースにテーブルが存在する場合でも *EntityFramework* で開発できます。

*SQL Server LocalDB* にテーブルを作成し、そのデータ構造に合わせて Modelクラスを実装していく手順について、解説します。

<br>

## 5. メンバーシップ フレームワークによる認証機能の実装

*メンバーシップ フレームワーク* はASP.NET 2.0 以降から採用された認証ライブラリです。
比較的シンプルに実装できるため、広く利用されています。

今回は 4. で作成したアプリケーションに認証・認可機能を実装していきます。

* Providerクラスの実装
  - MembershipProviderの実装
  - RoleProviderの実装
* ログイン画面の作成
  - ViewModelの作成
  - LoginControllerの作成
  - Viewの作成
* フォーム認証の設定
* 動作確認

<br>
<br>
