using CityRoots.Core.Interfaces;
using CityRoots.EF.Data;
using CityRoots.EF.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace CityRoots.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction _transaction;
        private readonly ApplicationDbContext _context;

        // Repositories
        public ICropRepository Crop { get; private set; }
        public ICycleRepository Cycle { get; private set; }
        public IChatRepository Chat { get; private set; }
        public ICycleUpdateRepository CycleUpdate { get; private set; }
        public IFarmerRepository Farmer { get; private set; }
        public IFarmRepository Farm { get; private set; }
        public IHarvestRepository Harvest { get; private set; }
        public IInvestmentRequestRepository InvestmentRequest { get; private set; }
        public IInvestorRepository Investor { get; private set; }
        public ILandParcelRepository LandParcel { get; private set; }
        public IMerchantRepository Merchant { get; private set; }
        public INotificationRepository Notification { get; private set; }
        public IOpenInvestmentCycleRepository OpenInvestmentCycle { get; private set; }
        public IPaymentRepository Payment { get; private set; }
        public IPurchaseRepository Purchase { get; private set; }
        public IScheduleRepository Schedule { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Crop = new CropRepository(context);
            Cycle = new CycleRepository(context);
            Chat = new ChatRepository(context);
            CycleUpdate = new CycleUpdateRepository(context);
            Farmer = new FarmerRepository(context);
            Farm = new FarmRepository(context);
            Harvest = new HarvestRepository(context);
            InvestmentRequest = new InvestmentRequestRepository(context);
            Investor = new InvestorRepository(context);
            LandParcel = new LandParcelRepository(context);
            Merchant = new MerchantRepository(context);
            Notification = new NotificationRepository(context);
            OpenInvestmentCycle = new OpenInvestmentCycleRepository(context);
            Payment = new PaymentRepository(context);
            Purchase = new PurchaseRepository(context);
            Schedule = new ScheduleRepository(context);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}
