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
    public partial class DetailForm : Form
    {
        /// <summary>
        /// 薬品ID
        /// </summary>
        public int drugId = 0;

        public DetailForm()
        {
            InitializeComponent();
        }

        private void DetailForm_Load(object sender, EventArgs e)
        {
            // ラベルの初期化
            this.lblId.Text = "";
            this.lblCode.Text = "";
            this.lblMessage.Text = "";

            // コンボボックスの読み込み
            this.bindClassification();

            if (this.drugId != 0)
            {
                // 変更・閲覧モード
                this.lblId.Text = this.drugId.ToString();
                this.btnAdd.Text = "更新";

                // データ取得
                this.loadDrugInfo();

                if (!"".Equals(this.lblCode.Text))
                {
                    // 薬品コードが存在する場合は更新不可とする
                    this.btnAdd.Enabled = false;
                }
            }
        }

        /// <summary>
        /// コンボボックスのデータ読み込み
        /// </summary>
        private void bindClassification()
        {
            using (var db = new DrugInfoContext())
            {
                db.Database.Connection.Open();
                this.cmbClassification.DisplayMember = "Name";
                this.cmbClassification.ValueMember = "ClassificationId";
                this.cmbClassification.DataSource = db.Classifications.ToList();

                this.cmbClassification.FormattingEnabled = true;
                this.cmbClassification.Format += new ListControlConvertEventHandler(formatComboLabel);
            }
        }

        /// <summary>
        /// コンボボックスの表示内容を設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void formatComboLabel(object sender, ListControlConvertEventArgs e)
        {
            var item = (Classifications)e.ListItem;
            string displayText = string.Format("{0}:{1}", item.ClassificationCode, item.Name);
            e.Value = displayText;
        }

        /// <summary>
        /// 薬品情報の読み込み
        /// </summary>
        private void loadDrugInfo()
        {
            using (var db = new DrugInfoContext())
            {
                var drug = db.Drugs
                    .Where(item => item.DrugId == this.drugId)
                    .FirstOrDefault();

                if (drug != null)
                {
                    this.lblCode.Text = drug.DrugCode;
                    this.txtName.Text = drug.Name;
                    this.txtCompany.Text = drug.Company;
                    this.cmbClassification.SelectedValue = drug.ClassificationId;
                }
            }
        }

        /// <summary>
        /// 閉じるボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 追加/更新ボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try {
                if (this.drugId == 0)
                {
                    // 登録処理
                    this.addDrugInfo();
                }
                else
                {
                    // 更新処理
                    this.updateDrugInfo();
                }

                MessageBox.Show("登録が完了しました");
                this.Close();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        private void addDrugInfo()
        {
            using (var db = new DrugInfoContext())
            {
                var drug = new Drugs();
                drug.DrugCode = "";
                drug.Name = this.txtName.Text;
                drug.Company = this.txtCompany.Text;
                drug.ClassificationId = (int)this.cmbClassification.SelectedValue;

                db.Drugs.Add(drug);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        private void updateDrugInfo()
        {
            using (var db = new DrugInfoContext())
            {
                var drug = db.Drugs.Where(item => item.DrugId == this.drugId).First();

                drug.Name = this.txtName.Text;
                drug.Company = this.txtCompany.Text;
                drug.ClassificationId = (int)this.cmbClassification.SelectedValue;

                db.SaveChanges();
            }
        }
    }
}
