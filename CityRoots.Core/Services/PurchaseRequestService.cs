using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Services
{
    public class PurchaseRequestService:IPurchaseRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PurchaseRequestService(IUnitOfWork unitOfWork) { 
        
        this._unitOfWork = unitOfWork;
        }

    }
}
