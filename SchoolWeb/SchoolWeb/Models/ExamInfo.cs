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
    
    public partial class ExamInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ExamInfo()
        {
            this.Score = new HashSet<Score>();
        }
    
        public int Em_id { get; set; }
        public string Em_No { get; set; }
        public string Em_Name { get; set; }
        public System.DateTime Em_DateTime { get; set; }
        public string Em_Time { get; set; }
        public int IsDel { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Score> Score { get; set; }
    }
}
