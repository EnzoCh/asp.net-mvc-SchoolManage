//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SchoolWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClassInfo
    {
        public int Cla_id { get; set; }
        public string Cla_No { get; set; }
        public string Cla_Name { get; set; }
        public System.DateTime Cla_CreateTime { get; set; }
        public string Cla_Man { get; set; }
        public int IsDel { get; set; }
        public string Remark { get; set; }
    
        public virtual Staff Staff { get; set; }
    }
}
