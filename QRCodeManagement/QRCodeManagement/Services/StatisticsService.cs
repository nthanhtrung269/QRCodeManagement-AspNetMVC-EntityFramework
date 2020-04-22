using System;
using System.Collections.Generic;
using QRCodeManagement.EntityManager.DbAccess;
using QRCodeManagement.EntityManager.DbEntity;
using QRCodeManagement.Models;

namespace QRCodeManagement.Services
{
    public class StatisticsService: IStatisticsService
    {
        private readonly IStaticticsManager _staticticsManager;
        /// <summary>
        /// The constructor is used for the production environment.
        /// </summary>
        public StatisticsService()
            : this(null)
        {
        }

        /// <summary>
        /// The constructor is used for Unit Testing & DI Container.
        /// </summary>
        /// <param name="staticticsManager"></param>
        public StatisticsService(IStaticticsManager staticticsManager)
        {
            _staticticsManager = staticticsManager ?? new StaticticsManager();
        }

        public bool InsertQrStatistics(StatisticsModel staticticsModel)
        {
            try
            {
                Statictic statictics = new Statictic
                {
                    City = staticticsModel.City,
                    Country = staticticsModel.Country,
                    Device = staticticsModel.Device,
                    LatTitude = staticticsModel.LatTitude,
                    LongTitude = staticticsModel.LongTitude,
                    Os = staticticsModel.Os,
                    QrId = staticticsModel.QrId,
                    ScanDate = staticticsModel.ScanDate
                };

                return _staticticsManager.Insert(statictics);
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        public bool DeleteQrStatistics(int id)
        {
            try
            {
                return _staticticsManager.Delete(id);
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        public bool DeleteQrStatistics(Statictic statictics)
        {
            try
            {
                return _staticticsManager.Delete(statictics);
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        public bool UpdateQrStatistics(StatisticsModel staticticsModel)
        {
            throw new NotImplementedException("This function no need to implement");
        }

        public IList<Statictic> GetAllQrStatistics()
        {
            try
            {
                return _staticticsManager.GetAll();
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public Statictic GetQrStatisticsById(int id)
        {
            try
            {
                return _staticticsManager.GetById(id);
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public IList<Statictic> GetQrStatisticsByQrId(string qrId)
        {
            try
            {
                return _staticticsManager.GetByQrCode(qrId);
            }
            catch (Exception exception)
            {
                return null;
            }
        }
    }
}