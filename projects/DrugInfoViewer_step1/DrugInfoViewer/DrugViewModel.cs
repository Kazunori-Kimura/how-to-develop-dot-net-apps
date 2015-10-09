using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugInfoViewer
{
    class DrugViewModel
    {
        [DisplayName("薬品ID")]
        public int Id { get; set; }

        [DisplayName("薬品名")]
        public string Name { get; set; }

        [DisplayName("薬品コード")]
        public string Code { get; set; }
    }
}
