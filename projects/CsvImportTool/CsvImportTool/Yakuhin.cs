namespace CsvImportTool
{
    /// <summary>
    /// CSVから読み込んだ薬品情報を格納する
    /// </summary>
    public class Yakuhin
    {
        /// <summary>
        /// 薬品コード
        /// </summary>
        public string DrugCode { get; set; }
        /// <summary>
        /// 医薬分類コード
        /// </summary>
        public string ClassificationCode { get; set; }
        /// <summary>
        /// 医薬分類群
        /// </summary>
        public string ClassificationName { get; set; }
        /// <summary>
        /// 医薬品名
        /// </summary>
        public string DrugName { get; set; }
        /// <summary>
        /// 会社名
        /// </summary>
        public string Company { get; set; }
    }
}
