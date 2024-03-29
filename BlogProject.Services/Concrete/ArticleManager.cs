﻿using AutoMapper;
using BlogProject.Data.Abstract;
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
    public class ArticleManager : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ArticleManager(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }       
  
        public async Task<IDataResult<ArticleDto>> Get(int articleId)
        {
            var article =await _unitOfWork.Articles.GetAsync(a=>a.Id == articleId,a=>a.User,a =>a.Category);
            if (article!=null)
            {
                return new DataResult<ArticleDto>(ResultStatus.Success, new ArticleDto
                {
                    Article=article,
                    ResultStatus=ResultStatus.Success,
                });
            }
            return new DataResult<ArticleDto>(ResultStatus.Error, message: "Böyle bir makale bulunamadı.",null);
        }

        public async Task<IDataResult<ArticleListDto>> GetAll()
        {
            var articles=await _unitOfWork.Articles.GetAllAsync(null,a=>a.User,a=>a.Category);
            if (articles.Count>-1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles=articles,
                    ResultStatus = ResultStatus.Success,
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, message: "Makaleler bulunamadı.", null);
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByCategory(int categoryId)
        {
            var result = await _unitOfWork.Categories.AnyAsync(c => c.Id == categoryId);
            if (result) { 
            var articles = await _unitOfWork.Articles.GetAllAsync(a => a.CategoryId == categoryId && !a.IsDeleted && a.IsActive,
                a => a.User, a => a.Category);
                if (articles.Count > -1)
                {
                    return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                    {
                        Articles = articles,
                        ResultStatus = ResultStatus.Success,
                    });
                }
                return new DataResult<ArticleListDto>(ResultStatus.Error, message: "Makaleler bulunamadı.", null);
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, message: "Böyle bir kategori bulunamadı.", null);
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByNonDeleted()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync( a => !a.IsDeleted, a => a.User,a=>a.Category);
            if (articles.Count>-1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success,new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus=ResultStatus.Success,
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, message: "Makaleler bulunamadı.", null);
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActive()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(a => !a.IsDeleted && a.IsActive,a=>a.User,a=>a.Category);
            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = ResultStatus.Success,
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, message: "Makaleler bulunamadı.", null);
        }

        public async Task<IResult> Add(ArticleAddDto articleAddDto, string createdByName)
        {
            var article=_mapper.Map<Article>(articleAddDto);
            article.CreatedByName = createdByName;
            article.ModifiedByName = createdByName;
            article.UserId = 1;
            await _unitOfWork.Articles.AddAsync(article);
            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, message: $"{articleAddDto} başlıklı makale başarıyla eklenmiştir.");
        }

        public async Task<IResult> Update(ArticleUpdateDto articleUpdateDto, string modifiedByName)
        {
            var article = _mapper.Map<Article>(articleUpdateDto);
            article.ModifiedByName = modifiedByName;
            await _unitOfWork.Articles.UpdateAsync(article);
            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, message: $"{articleUpdateDto} başlıklı makale başarıyla güncellenmiştir.");
        }

        public async Task<IResult> Delete(int articleId, string modifiedByName)
        {
            var result =await _unitOfWork.Articles.AnyAsync(a=>a.Id==articleId);
            if (result)
            {
                var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId);
                article.IsDeleted=true;
                article.ModifiedByName = modifiedByName;
                article.ModifiedDate = DateTime.Now;
                await _unitOfWork.Articles.UpdateAsync(article);
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, message: $"{article.Title} başlıklı makale başarıyla silinmiştir.");
            }
            return new Result(ResultStatus.Error, message: "Böyle bir makale bulunamadı.");
        }

        public async Task<IResult> HardDelete(int articleId)
        {
            var result = await _unitOfWork.Articles.AnyAsync(a => a.Id == articleId);
            if (result)
            {
                var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId);
                await _unitOfWork.Articles.DeleteAsync(article);
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, message: $"{article.Title} başlıklı makale başarıyla veritabanından silinmiştir.");
            }
            return new Result(ResultStatus.Error, message: "Böyle bir makale bulunamadı.");
        }

    }
}
