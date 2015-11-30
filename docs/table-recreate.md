# UserRoleテーブルの修正とソースコードの修正

前回 (2015/11/26) の勉強会で *UserRole* テーブルを作成しましたが、
*プライマリーキー* が無いと *Users*, *Roles* の更新処理時にエラーとなってしまいます。

以下の作業を行って、テーブルとソースコードを修正してください。

<br><br>

## UserRole テーブルの削除

* *サーバー エクスプローラー* を表示
* *データ接続* -> *DrugInfoContext* -> *テーブル* -> *UserRole* を右クリック
* *削除* を選択

<br><br>

## UserRole テーブルの作成とデータ登録

* *サーバー エクスプローラー* を表示
* *データ接続* -> *DrugInfoContext* を右クリック
* *新しいクエリ* を選択

### テーブル作成

以下のSQLを入力し、*実行* ボタン(緑色の三角アイコン)をクリックします。

```sql
CREATE TABLE [dbo].[UserRole] (
  [Id] INT IDENTITY (1, 1) NOT NULL,
  [UserID] INT NOT NULL,
  [RoleID] INT NOT NULL,
  PRIMARY KEY CLUSTERED ([Id] ASC),
  CONSTRAINT [FK_dbo.UserRole_dbo.User_UserID] FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users] ([UserID]) ON DELETE CASCADE,
  CONSTRAINT [FK_dbo.UserRole_dbo.Role_RoleID] FOREIGN KEY ([RoleID])
    REFERENCES [dbo].[Roles] ([RoleID]) ON DELETE CASCADE
);
```

*サーバー エクスプローラー* にて、
*データ接続* -> *DrugInfoContext* -> *テーブル* に
*UserRole* テーブルが表示されることを確認します。

<br><br>

### データ登録

以下のSQLを入力し、*実行* ボタン(緑色の三角アイコン)をクリックします。

```sql
insert into dbo.UserRole ([UserId], [RoleId]) values (1, 1);
insert into dbo.UserRole ([UserId], [RoleId]) values (2, 2);
```

* *サーバー エクスプローラー* を表示
* *データ接続* -> *DrugInfoContext* -> *テーブル* -> *UserRole* を右クリック
* *テーブル データの表示* を選択

2件登録されていることを確認します。

<br><br>

## EDMファイルの再作成

* *Models* から `DrugInfoContext.edmx` を削除します。
* *Web.config* を開き、一番下にある `<connectionStrings><add name="DrugInfoContext"...></connectionStrings>` を削除します。
* edmxファイルを作成します。
  - *Models* を右クリック -> *追加* -> *新しい項目* を選択
  - *ADO.NET Entity Data Model* を選択
  - 名前を `DrugInfoContext` とし、*追加* をクリック
  - ウィザードが起動するので、すべてのテーブルを選択して完了します。

<br><br>

## ソースコードの修正

`CustomRoleProvider.cs` について、
`user.Roles` ではなく `user.UserRole` を使用するように修正します。

<br>

`Models/CustomRoleProvider.cs`

```cs
public override string[] GetRolesForUser(string userId)
{
    using (var db = new DrugInfoContext())
    {
        int id = int.Parse(userId);
        var user = db.Users
            .Where(u => u.UserId == id)
            .FirstOrDefault();

        if (user != null) {
            string[] roles = user.UserRole.Select(item => item.Roles.RoleName).ToArray();
            return roles;
        }
        return new string[] {};
    }
}
```

<br>

```cs
public override bool IsUserInRole(string userId, string roleName)
{
    using (var db = new DrugInfoContext())
    {
        int id = int.Parse(userId);
        var user = db.Users
            .Where(u => u.Id == id)
            .FirstOrDefault();

        if (user != null) {
            string[] roles = user.UserRole.Select(item => item.Roles.RoleName).ToArray();

            if (roles.Contains(roleName))
            {
                return true;
            }
        }
    }

    return false;
}
```

<br><br>

[5. メンバーシップ フレームワークによる認証機能の実装 (後編)](./asp-302.html) に戻る

<br><br>
