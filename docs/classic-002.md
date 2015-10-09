## 2. Windowsフォームアプリケーションの開発


### イベントサンプル

```cs
using System;
using System.Windows.Forms;

namespace DrugInfoViewer
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// ボタンのクリック回数
        /// </summary>
        private int count = 0;

        public Form1()
        {
            InitializeComponent();

            // ラベルの初期化
            this.UpdateLabel();
        }

        /// <summary>
        /// ボタンのクリック回数をカウントする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // クリック回数をカウントアップ
            this.count++;
            // ラベルの更新
            this.UpdateLabel();
        }

        /// <summary>
        /// ラベルの内容を更新する
        /// </summary>
        private void UpdateLabel()
        {
            this.label1.Text = string.Format("{0} 回 クリックしました。", this.count);
        }
    }
}
```

* `public`, `protected`, `private`
* メンバ、プロパティ、メソッド
* コンストラクタ
* `this` について

イベントの割り当て
