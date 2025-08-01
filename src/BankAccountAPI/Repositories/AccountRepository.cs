using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankAccountAPI.Models;
using BankAccountAPI.Data;
using Microsoft.EntityFrameworkCore;
using BankAccountAPI.Repositories.Interfaces;

namespace BankAccountAPI.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BankAccountDBContext _dbContext;
        public AccountRepository(BankAccountDBContext bankAccountDBContext)
        {
            _dbContext = bankAccountDBContext;
        }

        public async Task<List<BankAccountModel>> SearchAllAccounts()
        {
           return await _dbContext.BankAccount.Include(x => x.BankClient).ToListAsync(); 
        }

        public async Task<BankAccountModel> SearchAccountById(int id)
        {
            BankAccountModel accountById = await _dbContext.BankAccount.FirstOrDefaultAsync(c => c.AccountId == id);

            return accountById;
        }

        public async Task<BankAccountModel> AddAccount(BankAccountModel account)
        {
            await  _dbContext.BankAccount.AddAsync(account);
            await _dbContext.SaveChangesAsync();

            return account;
        }

        public async Task<BankAccountModel> DepositBalance(decimal deposit, int id)
        {
            BankAccountModel accountById = await SearchAccountById(id);
            accountById.Deposit(deposit);

            _dbContext.BankAccount.Update(accountById);
            await _dbContext.SaveChangesAsync();

            return accountById;
        }

        public async Task<BankAccountModel> WithdrawBalance(decimal withdraw, int id)
        {
            BankAccountModel accountById = await SearchAccountById(id);
            accountById.Withdraw(withdraw);

            _dbContext.BankAccount.Update(accountById);
            await _dbContext.SaveChangesAsync();

            return accountById;
        }

        public async Task<BankAccountModel> TransferBalance(decimal transfer, int accountId, int recipientId)
        {
            BankAccountModel accountById = await SearchAccountById(accountId);
            BankAccountModel recipientById = await SearchAccountById(recipientId);
            accountById.Withdraw(transfer);
            recipientById.Deposit(transfer);

            _dbContext.BankAccount.Update(accountById);
            _dbContext.BankAccount.Update(recipientById);
            await _dbContext.SaveChangesAsync();

            return accountById;
        }

        public async Task<bool> DeleteAccount(int id)
        {
            BankAccountModel accountById = await SearchAccountById(id);

            _dbContext.BankAccount.Remove(accountById);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}