using System;
using System.Collections.Generic;
using System.Linq;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Api.Budgets.Shared.ViewModels;

namespace BudgetBuddy.Api.Budgets.Shared.Mappers
{
    public class BudgetMapper
    {
        public void Map(BudgetViewModel viewModel, Budget budget)
        {
            budget.StartDate = new DateTime(viewModel.Year, viewModel.Month, 1);
            MapLineItems(viewModel.Categories, budget.LineItems);
        }

        private static void MapLineItems(IEnumerable<BudgetCategoryViewModel> categoryViewModels, ICollection<BudgetLineItem> lineItems)
        {
            foreach (var categoryViewModel in categoryViewModels)
            {
                var categoryLineItems = lineItems.Where(c => c.Category.Name == categoryViewModel.Name).ToList();
                if (categoryLineItems.Any())
                    MapLineItems(categoryViewModel.LineItems, categoryLineItems);
                else
                    AddLineItems(categoryViewModel.LineItems, categoryViewModel, lineItems);
            }
        }

        private static void AddLineItems(BudgetLineItemViewModel[] lineItemViewModels, BudgetCategoryViewModel categoryViewModel, ICollection<BudgetLineItem> lineItems)
        {
            foreach (var lineItemViewModel in lineItemViewModels)
                AddLineItem(lineItemViewModel, categoryViewModel, lineItems);
        }

        private static void AddLineItem(BudgetLineItemViewModel lineItemViewModel, BudgetCategoryViewModel categoryViewModel, ICollection<BudgetLineItem> lineItems)
        {
            var lineItem = CreateLineItem(lineItemViewModel, categoryViewModel);
            lineItems.Add(lineItem);
        }

        private static void MapLineItems(BudgetLineItemViewModel[] lineItemViewModels, List<BudgetLineItem> lineItems)
        {
            foreach (var lineItemViewModel in lineItemViewModels)
            {
                var lineItem = lineItems.SingleOrDefault(l => l.Name == lineItemViewModel.Name);
                if (lineItem != null)
                    MapLineItem(lineItemViewModel, lineItem);
            }
        }

        private static void MapLineItem(BudgetLineItemViewModel lineItemViewModel, BudgetLineItem lineItem)
        {
            lineItem.Actual = lineItemViewModel.Actual;
            lineItem.Estimate = lineItemViewModel.Estimate;
        }

        private static BudgetLineItem CreateLineItem(BudgetLineItemViewModel lineItem, BudgetCategoryViewModel category)
        {
            return new BudgetLineItem
            {
                Actual = lineItem.Actual,
                Estimate = lineItem.Estimate,
                Name = lineItem.Name,
                Id = lineItem.Id,
                Category = CreateCategory(category)
            };
        }

        private static Category CreateCategory(BudgetCategoryViewModel category)
        {
            return new Category
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}
