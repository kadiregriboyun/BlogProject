﻿using BlogProject.Entities.Concrete;
using BlogProject.Entities.Dtos;
using BlogProject.Shared.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Services.Abstract
{
    public interface ICategoryService
    {
        Task<IDataResult<Category>> Get(int categoryId);
        Task<IDataResult<IList<Category>>> GetAll();
        Task<IDataResult<IList<Category>>> GetAllByNonDeleted();
        Task<IResult> Add(CategoryAddDto categoryAddDto,string createdByName);
        Task<IResult> Update(CategoryUpdateDto categoryUpdateDto,string modifierByName);
        Task<IResult> Delete(int categoryId,string modifiedByName);
        Task<IResult> HardDelete(int categoryId);//Tamamen silme
    }
}