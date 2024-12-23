using Application.Services.Interfaces;
using Domain.Entities.Discount;
using Domain.Interface;
using Domain.ViewModel.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Impelementation
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _repository;

        public DiscountService(IDiscountRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DiscountViewModel>> GetAllAsync()
        {
            var discounts = await _repository.GetAllAsync();
            return discounts.Select(d => new DiscountViewModel
            {
                Code = d.Code,
                IsPercentage = d.IsPercentage,
                Value = d.Value,
                StartDate = d.StartDate,
                EndDate = d.EndDate,
                IsActive = d.IsActive,
                UsageLimit = d.UsageLimit,
                IsDeleted=false
                
            }).ToList();
        }

        public async Task<DiscountViewModel?> GetByIdAsync(int id)
        {
            var discount = await _repository.GetByIdAsync(id);
            if (discount == null) return null;

            return new DiscountViewModel
            {
                Code = discount.Code,
                Id=discount.Id
                ,IsDeleted = discount.IsDeleted,
                IsPercentage = discount.IsPercentage,
                Value = discount.Value,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                IsActive = discount.IsActive,
                UsageLimit = discount.UsageLimit
            };
        }

        public async Task AddAsync(DiscountViewModel viewModel)
        {
            var discount = new Discount
            {
                Id = viewModel.Id,
                IsDeleted = viewModel.IsDeleted,
                CreateDate=DateTime.UtcNow,
                Code = viewModel.Code,
                IsPercentage = viewModel.IsPercentage,
                Value = viewModel.Value,
                StartDate = viewModel.StartDate,
                EndDate = viewModel.EndDate,
                IsActive = viewModel.IsActive ?? true,
                UsageLimit = viewModel.UsageLimit
            };

            await _repository.AddAsync(discount);
        }

        public async Task UpdateAsync(int id, DiscountViewModel viewModel)
        {
            var discount = await _repository.GetByIdAsync(id);
            if (discount == null) return;

            discount.Code = viewModel.Code;
            discount.IsPercentage = viewModel.IsPercentage;
            discount.Value = viewModel.Value;
            discount.StartDate = viewModel.StartDate;
            discount.EndDate = viewModel.EndDate;
            discount.IsActive = viewModel.IsActive ?? true;
            discount.UsageLimit = viewModel.UsageLimit;

            await _repository.UpdateAsync(discount);
        }

        public async Task DeleteAsync(int id)
        {
            var discount = await _repository.GetByIdAsync(id);
            if (discount == null) return;

            discount.IsDeleted=true;
            await _repository.UpdateAsync(discount);
        }
    }
}
