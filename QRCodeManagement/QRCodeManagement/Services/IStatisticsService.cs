using System.Collections.Generic;
using QRCodeManagement.EntityManager.DbEntity;
using QRCodeManagement.Models;

namespace QRCodeManagement.Services
{
    public interface IStatisticsService
    {
        bool InsertQrStatistics(StatisticsModel statictics);

        bool DeleteQrStatistics(int id);

        bool DeleteQrStatistics(Statictic statictics);

        bool UpdateQrStatistics(StatisticsModel statictics);

        IList<Statictic> GetAllQrStatistics();

        Statictic GetQrStatisticsById(int id);

        IList<Statictic> GetQrStatisticsByQrId(string qrId);

    }
}