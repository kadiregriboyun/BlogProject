﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Shared.Entities.Abstract
{
    public abstract class EntityBase
    {
        public int Id { get; set; }
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;//override Created= new DareRime(2020/01/01);
        public virtual DateTime ModifiedDate { get; set; }=DateTime.Now;
        public virtual bool IsDeleted { get; set; } = false;
        public virtual bool IsActive { get; set; } = true;
        public virtual string CreatedByName { get; set; } = "Admin";
        public virtual string ModifiedByName { get; set; } = "Admin";
        public virtual string Note { get; set; }
    }
}
