using CityRoots.Core.DTOs.FeedBack;
using CityRoots.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface ICommunicationService
    {
        Task<IEnumerable<FeedBackDisplay>> GetAll();
        Task<FeedBackDisplay> GetById(int id);
        Task<FeedBackDisplay> Add(FeedBackRequest feedBack,string userId);
        Task<FeedBackDisplay> Update(int id,FeedBackRequest feedBack);
        Task Delete(int id);
        Task SendSupportAsync(Support support);

    }
}
