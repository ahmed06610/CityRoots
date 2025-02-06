using CityRoots.Core.Interfaces.Services;
using CityRoots.EF.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IChatRepository Chat { get; }
        ICropRepository Crop { get; }
        ICycleRepository Cycle { get; }
        ICycleUpdateRepository CycleUpdate { get; }
        IFarmerRepository Farmer { get; }
        IFarmRepository Farm { get; }
        IHarvestRepository Harvest { get; }
        IInvestmentRequestRepository InvestmentRequest { get; }
        IInvestorRepository Investor { get; }
        ILandParcelRepository LandParcel { get; }
        IMerchantRepository Merchant { get; }
        INotificationRepository Notification { get; }
        IOpenInvestmentCycleRepository OpenInvestmentCycle { get; }
        IPaymentRepository Payment { get; }
        IPurchaseRepository Purchase { get; }
        IScheduleRepository Schedule { get; }
        IFeedBackRepository FeedBack { get; }
        IAiPredictRepository AiPredict { get; }
        ICycleNotificationLogRepository CycleNotificationLog { get; }
        IHarvestNotificationLogRepository HarvestNotificationLog { get; }
        IScheduleNotificationLogRepository ScheduleNotificationLog { get; }
        IInteractionOfMerchant InteractionOfMerchant { get; }
        IInteractionOfInvestor InteractionOfInvestor { get; }
        IFavoriteFarmers FavoriteFarmers { get; }
        IRateRepository Rate { get; }

        Task<int> CompleteAsync();
        Task RollbackAsync();
        Task CommitAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
