//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Salary_software
{
    using System;
    using System.Collections.Generic;
    
    public partial class Organization
    {
        public int ID_Organization { get; set; }
        public string NameOrg { get; set; }
        public string INN { get; set; }
        public string OGRN { get; set; }
        public string IMNS { get; set; }
        public string COD { get; set; }
        public string KPP { get; set; }
        public string OKTMO { get; set; }
        public string OKPO { get; set; }
        public int AddressID { get; set; }
        public int TypesTaxesID { get; set; }
    
        public virtual AddressBD AddressBD { get; set; }
        public virtual Typestaxes Typestaxes { get; set; }
    }
}
