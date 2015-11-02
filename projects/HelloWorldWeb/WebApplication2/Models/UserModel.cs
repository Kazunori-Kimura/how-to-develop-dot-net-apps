using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class UserModel
    {
        /// <summary>
        /// 名前
        /// </summary>
        [DisplayName("お名前")]
        public string Name { get; set; }
    }
}