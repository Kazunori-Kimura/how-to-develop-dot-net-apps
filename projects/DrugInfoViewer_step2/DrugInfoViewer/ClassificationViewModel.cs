using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugInfoViewer
{
    class ClassificationViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// コンボボックスのDisplayText
        /// </summary>
        public string title {
            get
            {
                if (string.IsNullOrEmpty(this.Code))
                {
                    return string.Empty;
                }
                return string.Format("{0}:{1}", this.Code, this.Name);
            }
        }

        /// <summary>
        /// コンボボックスのValue
        /// </summary>
        public int classificationId { get; set; }
    }
}
