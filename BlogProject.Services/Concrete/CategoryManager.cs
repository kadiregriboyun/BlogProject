﻿using AutoMapper;
using BlogProject.Data.Abstract;
using BlogProject.Data.Concrete;
using BlogProject.Entities.Concrete;
using BlogProject.Entities.Dtos;
using BlogProject.Services.Abstract;
using BlogProject.Shared.Utilities.Results.Abstract;
using BlogProject.Shared.Utilities.Results.ComplexTypes;
using BlogProject.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Services.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryManager(IMapper mapper,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }       
        
        public async Task<IDataResult<CategoryDto>> Get(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetAsync(c=>c.Id==categoryId,c=>c.Articles);
            if (category!=null)
            {
                return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto
                {
                    Category=category,
                    ResultStatus=ResultStatus.Success
                });
            }
            return new DataResult<CategoryDto>(ResultStatus.Error, message:"Böyle bir kategori bulunamadı.",null);
        }

        public async Task<IDataResult<CategoryListDto>> GetAll()
        {
            var categories=await _unitOfWork.Categories.GetAllAsync(null,c=>c.Articles);
            if (categories.Count>-1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories=categories,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error,message:"Hiç bir kategori bulunamadı",new CategoryListDto
            {
                Categories=null,
                ResultStatus = ResultStatus.Error,
                Message="Hiçbir kategori bulunamadı."
            });
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeleted()
        {
            var categories=await _unitOfWork.Categories.GetAllAsync(c=>!c.IsDeleted,c=>c.Articles);
            if (categories.Count>-1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success,new CategoryListDto
                {
                    Categories=categories,
                    ResultStatus=ResultStatus.Success,
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, message: "Hiç bir kategori bulunamadı", null);
        }
        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActive()
        {
            var categories=await _unitOfWork.Categories.GetAllAsync(c=>! c.IsDeleted && c.IsActive,
            c=>c.Articles);
            if (categories.Count>-1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories=categories,
                    ResultStatus=ResultStatus.Success,
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, message: "Hiç bir kategori bulunamadı", null);
        }
        public async Task<IResult> Add(CategoryAddDto categoryAddDto, string createdByName)
        {
            var category=_mapper.Map<Category>(categoryAddDto);
            category.CreatedByName = createdByName;
            category.ModifiedByName = createdByName;
            await _unitOfWork.Categories.AddAsync(category);//çok hızlı gerçekleşir.
            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, message: $"{categoryAddDto.Name} adlı kategori başarıyla eklenmiştir");
        }

        public async Task<IResult> Update(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
        {
            var category=_mapper.Map<Category>(categoryUpdateDto);
            category.ModifiedByName= modifiedByName;
            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, message: $"{categoryUpdateDto.Name} adlı kategori başarıyla güncellenmiştir.");          
        }

        public async Task<IResult> Delete(int categoryId, string modifiedByName)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {
                category.IsDeleted = true;
                category.ModifiedByName = modifiedByName;
                category.ModifiedDate = DateTime.Now;
                await _unitOfWork.Categories.UpdateAsync(category);
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, message: $"{category.Name} adlı kategori başarıyla silinmiştir.");
            }
            return new Result(ResultStatus.Error, message: "Böyle bir kategori bulunamadı");
        }

        public async Task<IResult> HardDelete(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {
                await _unitOfWork.Categories.DeleteAsync(category);
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, message: $"{category.Name} adlı kategori veritabanından başarıyla silinmiştir.");
            }
            return new Result(ResultStatus.Error, message: "Böyle bir kategori bulunamadı");
        }
       
    }
}
