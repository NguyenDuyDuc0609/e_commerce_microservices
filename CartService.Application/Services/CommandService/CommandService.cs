using CartService.Application.Features.Dtos;
using CartService.Application.Interfaces;
using CartService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Services.CommandService
{
    public sealed class CommandService(IUnitOfWork unitOfWork) : ICommandService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        public async Task<bool> AddItemToCart(Guid userId, AddItemDto itemDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var result = await _unitOfWork.Repository!.AddItemToCart(userId, itemDto);
                if (!result)
                {
                    throw new Exception("Failed to add item to the cart in the database.");
                }
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding an item to the cart.", ex);
            }
        }

        public async Task<bool> ClearCart(Guid userId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var result = await _unitOfWork.Repository!.ClearCart(userId);
                if (!result)
                {
                    throw new Exception("Failed to clear the cart in the database.");
                }
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while clearing the cart.", ex);
            }
        }

        public async Task<bool> DeleteItem(Guid userId, Guid itemId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var result = await _unitOfWork.Repository!.DeleteItem(userId, itemId);
                if (!result)
                {
                    throw new Exception("Failed to delete the item from the cart in the database.");
                }
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting an item from the cart.", ex);
            }
        }
    }
}
