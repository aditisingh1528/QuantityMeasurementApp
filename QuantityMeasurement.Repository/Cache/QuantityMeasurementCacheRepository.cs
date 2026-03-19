using System;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository.Interface;
using System.Collections.Generic;

namespace QuantityMeasurementRepository.Cache
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static QuantityMeasurementCacheRepository instance;

        private readonly List<QuantityMeasurementEntity> cache =
            new List<QuantityMeasurementEntity>();

        private QuantityMeasurementCacheRepository()
        {
        }

        public static QuantityMeasurementCacheRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new QuantityMeasurementCacheRepository();
            }

            return instance;
        }

        public void SaveMeasurement(QuantityMeasurementEntity entity)
        {
            cache.Add(entity);
        }

        public List<QuantityMeasurementEntity> GetAllMeasurements()
        {
            return cache;
        }
    }
}