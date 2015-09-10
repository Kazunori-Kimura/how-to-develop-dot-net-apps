# .NET Framework 開発入門

## .NET Frameworkとは？

参考: [.NET](https://msdn.microsoft.com/ja-jp/vstudio/aa496123.aspx)

*.NET Framework* とは、マイクロソフトが開発したアプリケーションの開発・実行環境です。

最新バージョンは *.NET Framework 4.6* になります。  
.NET Frameworkのシステム要件は以下のページで確認できます。

- [.NET Framework システム要件](https://msdn.microsoft.com/ja-jp/library/vstudio/8z6watww.aspx)

<br>

## .NET アプリケーションの開発言語について

.NET Frameworkでは、どの言語で記述されたプログラムも *共通中間言語(CIL)* にコンパイルされます。  
この共通中間言語を *共通言語ランタイム(CLR)* が読み込むことで、アプリケーションが実行されます。

*Visual Studio* では *C#* , *VisualBasic* , *F#* が.NETアプリケーションの開発言語として使用できますが、どの言語で開発しても
.NETアプリケーションとして実現できることは変わりません。

この勉強会では *C#* を使用します。

<br>

------

<br>

## Visual Studio について

- [visualstudio.com](https://www.visualstudio.com/)

上記サイトにアクセスすると、トップに3つの項目があります。

* Visual Studio Code
* Visual Studio Online
* Visual Studio

<br>

### Visual Studio Code

* 無償で使用できます。
* Windowsだけでなく、Mac OS X と Linux 向けも提供されています。
* ASP.NET 5、Node.jsの開発が可能です。
  - 30以上のプログラム言語の構文のハイライト表示、ブラケット マッチングに対応しており、プログラムエディタとして使用できます。
* プレビュー版です。

<br>

### Visual Studio Online

*Team Foundation Server* のSaaS版です。

* バージョン管理 (TFVC, Gitに対応)
* 継続的インテグレーション (CI)
* バグチケット管理

<br>

### Visual Studio

Windows向けの統合開発環境です。

Community, Professional, Enterpriseの3つのエディションがあります。  
通常の企業での開発に使用する場合は Professional以上が必要です。

Visual Studio 2015 では以下のアプリケーションが開発できます。

<br>

#### Windowsアプリ

* Win32 デスクトップ アプリ
  - C++ MFCによって作成する、Windowsでのみ動くアプリケーション
* .NET アプリ
  - .NET Framework上で動作するアプリケーション
* Windowsストアアプリ  
  - Windows Phone 8以降で動作するアプリケーション
* UWP(Universal Windows Platform)アプリ
  - Windows 10がサポートするさまざまなデバイスで動作させることが可能なアプリケーション

Win32 デスクトップ アプリと .NETアプリは *従来の Windows アプリ :Classic Windows Application (CWA)* と呼ばれます。

<br>

#### Webアプリ

* ASP.NET 4.6
  - ASP.NET Web Forms
  - ASP.NET MVC
  - ASP.NET Web API
  - ASP.NET SygnalR
* ASP.NET 5
  - まだ製品版ではありません。
  - Web Formsが廃止されており、デフォルトでASP.NET MVCが選択されます。
* Node.js
* Python

<br>

#### モバイル アプリ

* JavaScriptによるハイブリッドアプリ
  - *Apache Cordova* によって HTML5とJavaScriptを使用した Windows, Android, iOS向けのモバイル アプリを開発できます。
* C#によるネイティブアプリ
  - *Xamarin* によって C#を使用した Windows, Android, iOS向けのモバイル アプリを開発できます。
  - 別途 Xamarinのライセンスが必要です。
* C++によるネイティブアプリ
  - *Visual C++ for Cross-Platform Mobile Development* によって C++を使用した Windows, Android向けのモバイル アプリを開発できます。

<br>
<br>
