﻿using BlogProject.Entities.Concrete;
using BlogProject.Shared.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Data.Abstract
{
    public interface IUserRepository:IEntityRepository<User>
    {

    }
}
