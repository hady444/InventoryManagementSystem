using AutoMapper;
using BLL.Services.Abstraction;
using BLL.ViewModel;
using Contract;
using DAL.Models;
using DAL.Repos.Abstraction;

namespace BLL.Services.Implementation
{
    public class StockTransactionSerivce: IStockTransactionSerivce
    {
        private readonly IStockTransactionRepository _transactionRepo;
        private readonly IWarehouseStockRepository _stockRepo;
        private readonly IMapper _mapper;
        public StockTransactionSerivce(IStockTransactionRepository transactionRepo, IWarehouseStockRepository stockRepo, IMapper mapper)
        {
            _transactionRepo = transactionRepo;
            _stockRepo = stockRepo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<StockTransactionVM>> GetTransactions(TransactionFilterVM filter)
        {
            return await _transactionRepo.GetAllAsync(filter);
        }
        public async Task<StockTransaction?> GetByIdAsync(int id)
        {
            return await _transactionRepo.GetByIdAsync(id);
        }
        public async Task<StockTransactionVM?> GetDetailsAsync(int id)
        {
            return await _transactionRepo.GetDetailsAsync(id);
        }

        public async Task<Response> CreateAsync(CreateTransactionVM vm)
        {
            if (vm == null)
                return new Response(false, null, "Invalid input");

            if (vm.Quantity <= 0)
                return new Response(false, "Quantity", "Quantity must be greater than zero");

            // current stock
            var currentStock = await _stockRepo.GetOrCreateStockAsync(vm.ProductId, vm.WarehouseId);

            int signedQty;
            try
            {
                signedQty = GetSignedQuantity(vm.Type, vm.Quantity);
            }
            catch (Exception ex)
            {
                return new Response(false, ex.Message.StartsWith("Quantity") ? "Quantity" : "Type", ex.Message);
            }

            // prevent negative stock
            if (currentStock.Quantity + signedQty < 0)
                return new Response(false, "Quantity", "Insufficient stock");
            currentStock.Quantity += signedQty;
            var transaction = _mapper.Map<StockTransaction>(vm);
            try
            {
                await _transactionRepo.CreateAsync(transaction);
                await _transactionRepo.SaveAsync();
                return new Response(true, null, null);
            }
            catch (Exception ex)
            {
                return new Response(false, null, ex.Message);
            }
        }


        public async Task<Response> UpdateAsync(int id, CreateTransactionVM vm)
        {
            var transaction = await _transactionRepo.GetByIdAsync(id);
            if (transaction == null)
                return new Response(false, null, "Transaction not found");

            var stock = await _stockRepo.GetOrCreateStockAsync(
                transaction.ProductId,
                transaction.WarehouseId);
            int oldSigned;
            try
            {
                oldSigned = GetSignedQuantity(transaction.Type, transaction.Quantity);
            }
            catch (Exception ex) {
                return new Response(false, ex.Message.StartsWith("Quantity") ? "Quantity" : "Type", ex.Message);
            }
            int newSigned;
            try
            {
                newSigned = GetSignedQuantity(vm.Type, vm.Quantity);
            }
            catch (Exception ex)
            {
                return new Response(false, ex.Message.StartsWith("Quantity")? "Quantity": "Type", ex.Message);
            }
            var finalStock = stock.Quantity - oldSigned + newSigned;

            if (finalStock < 0)
                return new Response(false, "Quantity", "Insufficient stock");
            stock.Quantity = finalStock;
            transaction.Type = vm.Type;
            transaction.Quantity = vm.Quantity;
            transaction.Notes = vm.Notes;
            try
            {
                await _transactionRepo.SaveAsync();
                return new Response(true, null, null);
            }
            catch (Exception ex)
            {
                return new Response(false, null, ex.Message);
            }
        }

        public async Task<Response> SoftDeleteAsync(int id)
        {
            var transaction = await _transactionRepo.GetByIdAsync(id);
            if (transaction == null || transaction.IsDeleted)
                return new Response(false, null, "Transaction not found");

            var stock = await _stockRepo.GetOrCreateStockAsync(
                transaction.ProductId,
                transaction.WarehouseId);

            int signed = GetSignedQuantity(transaction.Type, transaction.Quantity);

            if (stock.Quantity - signed < 0)
                return new Response(false, null, "Cannot delete transaction");

            // reverse stock
            stock.Quantity -= signed;

            // soft delete transaction
            transaction.IsDeleted = true;
            transaction.DeletedAt = DateTime.UtcNow;

            await _transactionRepo.SaveAsync();
            return new Response(true, null, null);
        }

        private int GetSignedQuantity(TransactionType type, int quantity)
        {
            if (quantity <= 0)
                throw new InvalidOperationException("Quantity must be greater than zero");

            return type switch
            {
                TransactionType.In => quantity,
                TransactionType.Out => -quantity,
                _ => throw new InvalidOperationException("Invalid transaction type")
            };
        }
    }
}
