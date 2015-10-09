using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrugInfoViewer
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 詳細フォーム
        /// </summary>
        DetailForm detailForm = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using(var db = new DrugInfoContext())
            {
                db.Database.Connection.Open();
                this.comboBox1.DisplayMember = "Name";
                this.comboBox1.ValueMember = "ClassificationId";
                this.comboBox1.DataSource = db.Classifications.ToList();

                this.comboBox1.FormattingEnabled = true;
                this.comboBox1.Format += new ListControlConvertEventHandler(comboBox1_Format);
            }
        }

        /// <summary>
        /// コンボボックスの表示内容を設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="evt"></param>
        private void comboBox1_Format(object sender, ListControlConvertEventArgs evt)
        {
            var item = (Classifications)evt.ListItem;
            string displayText = string.Format("{0}:{1}", item.ClassificationCode, item.Name);
            evt.Value = displayText;
        }

        /// <summary>
        /// OKボタンをクリックした際の動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // 現在選択されている薬効分類を取得する
            System.Diagnostics.Debug.WriteLine(
                string.Format("SelectedValue={0}", this.comboBox1.SelectedValue));

            using (var db = new DrugInfoContext())
            {
                // 薬効分類IDを元に薬品情報を取得
                int clsId = (int)this.comboBox1.SelectedValue;
                var list = db.Drugs
                    .Where(item => item.ClassificationId == clsId)
                    .Select(item => new DrugViewModel { Id = item.DrugId,
                        Name = item.Name,
                        Code = item.DrugCode })
                    .ToList();
                this.dataGridView1.DataSource = list;
                this.dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
        }
        
        /// <summary>
        /// ダブルクリックで詳細フォームを開く
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                var id = this.dataGridView1.Rows[e.RowIndex].Cells["Id"].Value;
                System.Diagnostics.Debug.WriteLine(id);

                // モーダルダイアログを開く
                this.openDialog((int)id);
            }
        }

        /// <summary>
        /// 登録ボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // モーダルダイアログを開く
            this.openDialog();
        }

        /// <summary>
        /// モーダルダイアログを開く
        /// </summary>
        private void openDialog()
        {
            this.openDialog(0);
        }

        /// <summary>
        /// モーダルダイアログを開く
        /// </summary>
        /// <param name="id"></param>
        private void openDialog(int id)
        {
            if (this.detailForm == null)
            {
                this.detailForm = new DetailForm();
            }
            this.detailForm.drugId = id;
            // モーダルダイアログとして開く
            this.detailForm.ShowDialog(this);
        }
    }
}
