using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Claims.DTOs;
using Claims.Api.Modules.Claims.Interface;
using Claims.Api.Modules.Claims.Interface.Repository;
using Claims.Api.Modules.Claims.Interface.Service;

namespace Claims.Api.Modules.Claims.Services
{
    public class SettlementService : ISettlementService
    {
        private ISettlementRepository _settlementRepository;
        private IUnitOfWork _unitOfWork;

        public SettlementService(ISettlementRepository settlementRepository, IUnitOfWork unitOfWork)
        {
            _settlementRepository = settlementRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> CreateAsync(Guid claimId, Settlement settlement)
        {
            await _settlementRepository.AddAsync(settlement);

            await _unitOfWork.SaveChangesAsync();

            return settlement.Id;
        }

        public Task<List<Settlement>> GetAllAsync()
        {
            return _settlementRepository.GetAllAsync();
        }
    }
}