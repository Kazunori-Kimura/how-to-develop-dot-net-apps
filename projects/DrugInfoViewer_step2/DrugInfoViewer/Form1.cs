using OfficeOpenXml;
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
            // コンボボックスに薬効分類をセットする
            this.bindClassifications();
        }

        /// <summary>
        /// コンボボックスに薬効分類をセット
        /// </summary>
        private void bindClassifications()
        {
            using(var db = new DrugInfoContext())
            {
                var list = db.Classifications
                    .Select(item => new ClassificationViewModel
                    {
                        Code = item.ClassificationCode,
                        Name = item.Name,
                        classificationId = item.ClassificationId
                    }).ToList();

                // リストの先頭にブランクをセットする
                list.Insert(0, new ClassificationViewModel
                {
                    Code = "",
                    Name = "",
                    classificationId = 0
                });

                // コンボボックスの表示設定
                this.comboBox1.DisplayMember = "title";
                this.comboBox1.ValueMember = "ClassificationId";
                this.comboBox1.DataSource = list;
            }
        }

        /// <summary>
        /// 表示ボタンをクリックした際の動作
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
                    .Where(item => 
                        (clsId == 0
                            || item.ClassificationId == clsId) &&
                        (String.IsNullOrEmpty(this.txtDrugName.Text)
                            || item.Name.Contains(this.txtDrugName.Text)))
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

        /// <summary>
        /// 出力ボタンのクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            List<int> selectedList = new List<int>();

            // 選択している行を取得
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                selectedList.Add((int)row.Cells["Id"].Value);

                System.Diagnostics.Debug.WriteLine(row.Cells["Id"].Value);
            }

            if (selectedList.Count == 0)
            {
                MessageBox.Show("薬品を選択してください。");
                return;
            }

            // id順に並び替え
            selectedList.Sort();

            // 出力データの取得
            var data = this.getDrugData(selectedList);

            // Excel出力
            var filePath = this.outputExcel(data, "output.xlsx");

            MessageBox.Show(string.Format("Excelファイルを出力しました。\n{0}", filePath));
        }

        /// <summary>
        /// 出力用データを取得する
        /// </summary>
        /// <param name="drugIdList"></param>
        /// <returns></returns>
        private List<List<string>> getDrugData(List<int> drugIdList)
        {
            List<List<string>> data = new List<List<string>>();

            using (var db = new DrugInfoContext())
            {
                foreach (int id in drugIdList)
                {
                    var drug = db.Drugs
                        .Where(item => item.DrugId == id)
                        .FirstOrDefault();

                    var line = new List<string>();

                    if (drug != null)
                    {
                        line.Add(drug.Classifications.ClassificationCode);
                        line.Add(drug.Classifications.Name);
                        line.Add(drug.DrugCode);
                        line.Add(drug.Name);
                        line.Add(drug.Company);

                        data.Add(line);
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// Excelファイルを出力する
        /// </summary>
        /// <param name="drugData">薬品データ</param>
        /// <param name="fileName">ファイル名</param>
        private string outputExcel(List<List<string>> drugData, string fileName)
        {
            // 出力ファイル
            var file = new System.IO.FileInfo(fileName);
            // すでにファイルが存在する場合は削除
            if (file.Exists)
            {
                file.Delete();
            }

            // Excelファイルの作成
            using (var excel = new ExcelPackage(file))
            {
                // シート追加
                var sheet = excel.Workbook.Worksheets.Add("薬品情報");

                int rowIndex = 1, colIndex = 1;
                foreach(var row in drugData)
                {
                    colIndex = 1;
                    foreach(var col in row)
                    {
                        var cell = sheet.Cells[rowIndex, colIndex];
                        cell.Value = col;

                        colIndex++;
                    }
                    rowIndex++;
                }

                excel.Save();
            }
            return file.FullName;
        }
    }
}
