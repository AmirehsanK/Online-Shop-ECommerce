using Application.Services.Interfaces;
using Application.Tools;
using Domain.Entities.Discount;
using Domain.Interface;
using Domain.ViewModel.Discount;
using Infra.Data.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discount = Domain.Entities.Discount.Discount;

namespace Application.Services.Impelementation
{
    public class DiscountService(IDiscountRepository repository) : IDiscountService
    {
        public async Task<List<DiscountViewModel>> GetAllAsync()
        {
            var discounts = await repository.GetAllAsync();
            return discounts.Select(d => new DiscountViewModel
            {
                Id=d.Id,
                Code = d.Code,
                IsPercentage = d.IsPercentage,
                Value = d.Value,
                StartDate = d.StartDate?.ToShamsi().ToString(),
                EndDate = d.EndDate?.ToShamsi().ToString(),
                IsActive = d.IsActive,
                UsageLimit = d.UsageLimit,
                IsDeleted=false
                
            }).ToList();
        }

        public async Task<DiscountViewModel?> GetByIdAsync(int id)
        {
            var discount = await repository.GetByIdAsync(id);
            if (discount == null) return null;

            return new DiscountViewModel
            {
                Code = discount.Code,
                Id=discount.Id
                ,IsDeleted = discount.IsDeleted,
                IsPercentage = discount.IsPercentage,
                Value = discount.Value,
                StartDate = discount.StartDate?.ToShamsi().ToString(),
                EndDate = discount.EndDate?.ToShamsi().ToString(),
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
                StartDate = viewModel.StartDate.ToMiladiString(),
                EndDate = viewModel.EndDate.ToMiladiString(),
                IsActive = viewModel.IsActive,
                UsageLimit = viewModel.UsageLimit
            };

            await repository.AddAsync(discount);
            await repository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, DiscountEditViewModel viewModel)
        {
            var discount = await repository.GetByIdAsync(id);
            if (discount == null) return;

            discount.Code = viewModel.Code;
            discount.IsPercentage = viewModel.IsPercentage;
            //discount.Value = viewModel.Value;
            discount.StartDate = viewModel.StartDate.ToMiladiString();
            discount.EndDate = viewModel.EndDate.ToMiladiString();
            discount.IsActive = (bool)viewModel.IsActive;
            discount.UsageLimit = viewModel.UsageLimit;

            repository.Update(discount);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var discount = await repository.GetByIdAsync(id);
            if (discount == null) return;

            discount.IsDeleted=true;
            repository.Update(discount);
            await repository.SaveChangesAsync();
        }

        public async Task<List<int>> GetUserDiscount(int discountId)
        {
            return await repository.GetUserDiscount(discountId);
        }

        public Task<List<int>> GetProductDiscount(int discountId)
        {
            return repository.GetProductDiscount(discountId);
        }

        public async Task AssignProductDiscountAsync(List<int> productIds, int discountId)
        {
            var existingProductIds = await repository.GetProductDiscount(discountId);

            var productsToRemove = existingProductIds.Except(productIds).ToList();

            var productsToAdd = productIds.Except(existingProductIds).ToList();

            foreach (var productId in productsToRemove)
            {
                await repository.RemoveProductDiscountAsync(productId, discountId);
            }

            foreach (var productId in productsToAdd)
            {
                await repository.AssignProductDiscountAsync(productId, discountId);
            }
            await repository.SaveChangesAsync();
        }

        public async Task AssignUserDiscountAsync(List<int> userIds, int discountId)
        {
            var existingUserIds = await repository.GetUserDiscount(discountId);

            var usersToRemove = existingUserIds.Except(userIds).ToList();

            var usersToAdd = userIds.Except(existingUserIds).ToList();

            foreach (var userId in usersToRemove)
            {
                await repository.RemoveUserDiscountAsync(userId, discountId);
            }

            foreach (var userId in usersToAdd)
            {
                await repository.AssignUserDiscountAsync(userId, discountId);
            }
            await repository.SaveChangesAsync();
        }

        public async Task<List<DiscountListAdminViewModel>> GetAllForAdminAsync()
        {
            var activeDiscounts = await repository.GetActiveDiscounts();

            var discountList = activeDiscounts.Select(d => new DiscountListAdminViewModel
            {
                Id = d.Id,
                Code = d.Code,
                IsPercentage = d.IsPercentage,
                Value = d.Value,
                IsActive = d.IsActive
            }).Take(3).ToList();

            return discountList;
        }
    }
}
