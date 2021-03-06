# 0. Webアプリケーションの定義

*ASP.NET* の解説に入るまえに、大前提となるWebの仕組みと
この勉強会で扱う Webアプリケーション の定義について解説します。

<br>

## Webはどのように構成されているのか

Webは、Webページを公開する *Webサーバー* と
Webページを閲覧するクライアントとなる *Web ブラウザ* で構成する
大規模な *クライアント/サーバーモデル* と言えます。

<br>

クライアントはサーバーに対して、「このページが見たい」という *リクエスト* を送信します。  
この時に「このページ」を表すのが *URL (Uniform Resource Locator)* です。

サーバーはリクエストを受けると、その URL に該当するリソース (HTMLや画像など) を *レスポンス* として返します。

<br>

この時にやり取りされる リクエストとレスポンスのやり取りの約束事のことを *プロトコル* といいます。

特にWebで使用するプロトコルは *HTTP (HyperText Transfer Protocol)* です。

<br><br>

### やりとりの中身

telnetコマンドで `http://example.com` にリクエストを投げることで、
*HTTP* のやりとりの中身について確認できます。

<br>

#### HTTPリクエスト

*telnet* で `example.com` の 80番ポートに接続します。

```
> telnet example.com 80
```

<br>

次に *HTTPリクエスト* を入力します。

*HTTPリクエスト* は以下の構造で成り立っています。

* メソッド名を含んだ *リクエストライン*
* *リクエストヘッダー*
* *空行*
* *ボディ* (省略可)

<br>

```
GET / HTTP/1.1
Host: example.com

```

<br>

`GET / HTTP/1.1` は *GETメソッドで パス "/" にHTTPのバージョン1.1でリクエストを送る* という意味です。  
(*HTTP* にはいくつかのバージョンが存在しますが、広く使用されているのは `1.1` です。)

*HTTP/1.1* には以下のメソッドが用意されています。

* GET (URLで示されたリソースをサーバーから取得する)
* POST (クライアントからサーバーへデータを送信する)
* PUT
* DELETE
* HEAD
* OPTION
* TRACE
* CONNECT

ただし、ブラウザやWebアプリでこれらすべてを使用することはありません。  
Webアプリの開発で使用するのは *GET* と *POST* になります。

<br>

`Host: example.com` は *リクエストヘッダー* で、リクエストに関する情報を定義します。

例えば、Webブラウザからリクエストを行った場合、そのブラウザの情報を示す *User-Agent* のヘッダーが入ります。

<br>

#### HTTPレスポンス

リクエストに対してサーバーから返される *HTTPレスポンス* は、以下の様な構成となっています。

* ステータスコードを含んだ *レスポンスライン*
* *レスポンスヘッダー*
* *空行*
* *ボディ*

<br>

実際のレスポンスは以下のようになっています。

```
HTTP/1.1 200 OK
Accept-Ranges: bytes
Cache-Control: max-age=604800
Content-Type: text/html
Date: Mon, 02 Nov 2015 09:31:22 GMT
Etag: "359670651"
Expires: Mon, 09 Nov 2015 09:31:22 GMT
Last-Modified: Fri, 09 Aug 2013 23:54:35 GMT
Server: ECS (pae/3796)
X-Cache: HIT
x-ec-custom-error: 1
Content-Length: 1270

<!doctype html>
<html>
<head>
    <title>Example Domain</title>

~~ 省略 ~~

</html>
```

<br><br>

### Webページを構成する要素

Webページは以下のような要素で構成されています。

* 構造を作る *HTML*
* 装飾する *CSS*
* 動きを与える *JavaScript*

これらのファイルを記述してWebページを作成していきます。

<br><br>

#### Webアプリケーションの定義

現在見ているこのページや、企業のコーポレートサイトなど、
ページ内のコンテンツが頻繁に変わらない・「凝ったこと」をしない場合、
予め作成した *HTML* などのファイルを Webサーバーから配信する形で問題ないでしょう。

このような形態のWebサイトを *静的サイト* と呼びます。

<br>

一方、ユーザーから投稿されたデータを保持したり、そのデータを読みだして表示するような
複雑なWebページを作成するためには、サーバー側で処理を記載する必要があります。

このように、ユーザーからのリクエストごとにサーバーサイドでプログラムが動いて
状況に合わせたレスポンスを返すようなWebサイトを *動的サイト* と呼びます。

<br>

今後、この勉強会で *Webアプリケーション* と言った場合は、後者の *動的サイト* を意味していると考えてください。

<br><br>


### 補足解説: HTML, CSS, JavaScriptについて

* [HTMLの基本](http://www.htmq.com/htmlkihon/001.shtml)
* [CSSの基本](http://www.htmq.com/csskihon/001.shtml)
* [いまさら聞けないJavaScript入門](http://www.atmarkit.co.jp/ait/articles/0707/17/news114.html)

<br><br>
