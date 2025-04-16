using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankAccountAPI.Models;
using BankAccountAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BankAccountAPI.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly BankAccountDBContext _dbContext;
        public ClientRepository(BankAccountDBContext bankAccountDBContext)
        {
            _dbContext = bankAccountDBContext;
        }

        public async Task<List<BankClientModel>> SearchAllClients()
        {
           return await _dbContext.BankClient.ToListAsync(); 
        }

        public async Task<BankClientModel> SearchClientByCPF(string cpf)
        {
            BankClientModel clientByCPF = await _dbContext.BankClient.FirstOrDefaultAsync(c => c.CPF == cpf);

            return clientByCPF;
        }

        public async Task<BankClientModel> AddClient(BankClientModel client)
        {
            await  _dbContext.BankClient.AddAsync(client);
            await _dbContext.SaveChangesAsync();

            return client;
        }

        public async Task<BankClientModel> UpdateClient(BankClientModel client, string cpf)
        {
            BankClientModel clientByCPF = await SearchClientByCPF(cpf);

            clientByCPF.UpdateClient(client.ClientName, client.ClientEmail, client.ClientTel);

            _dbContext.BankClient.Update(clientByCPF);
            await _dbContext.SaveChangesAsync();

            return clientByCPF;
        }

        public async Task<bool> DeleteClient(string cpf)
        {
            BankClientModel clientByCPF = await SearchClientByCPF(cpf);

            _dbContext.BankClient.Remove(clientByCPF);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}