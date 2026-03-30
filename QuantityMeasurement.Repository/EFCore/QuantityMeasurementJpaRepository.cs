using Microsoft.EntityFrameworkCore;
using QuantityMeasurementModel.Entities;

namespace QuantityMeasurementRepository.EFCore
{
    public class QuantityMeasurementJpaRepository : IQuantityMeasurementJpaRepository
    {
        private readonly QuantityMeasurementDbContext _context;

        public QuantityMeasurementJpaRepository(QuantityMeasurementDbContext context)
        {
            _context = context;
        }

        // basic CRUD

        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity.Id == 0)
                _context.QuantityMeasurements.Add(entity);
            else
                _context.QuantityMeasurements.Update(entity);

            _context.SaveChanges();
        }

        public QuantityMeasurementEntity? FindById(int id)
        {
            return _context.QuantityMeasurements.Find(id);
        }

        public List<QuantityMeasurementEntity> FindAll()
        {
            return _context.QuantityMeasurements
                           .OrderByDescending(e => e.CreatedAt)
                           .ToList();
        }

        public void Delete(QuantityMeasurementEntity entity)
        {
            _context.QuantityMeasurements.Remove(entity);
            _context.SaveChanges();
        }

        public List<QuantityMeasurementEntity> FindByOperation(string operation)
        {
            return _context.QuantityMeasurements
                           .Where(e => e.Operation == operation)
                           .OrderByDescending(e => e.CreatedAt)
                           .ToList();
        }

        public List<QuantityMeasurementEntity> FindByThisMeasurementType(string measurementType)
        {
            return _context.QuantityMeasurements
                           .Where(e => e.ThisMeasurementType == measurementType)
                           .OrderByDescending(e => e.CreatedAt)
                           .ToList();
        }

        public List<QuantityMeasurementEntity> FindByCreatedAtAfter(DateTime date)
        {
            return _context.QuantityMeasurements
                           .Where(e => e.CreatedAt > date)
                           .OrderByDescending(e => e.CreatedAt)
                           .ToList();
        }

        public List<QuantityMeasurementEntity> FindSuccessfulOperations(string operation)
        {
            return _context.QuantityMeasurements
                           .Where(e => e.Operation == operation && !e.IsError)
                           .OrderByDescending(e => e.CreatedAt)
                           .ToList();
        }

        public long CountByOperationAndIsErrorFalse(string operation)
        {
            return _context.QuantityMeasurements
                           .Count(e => e.Operation == operation && !e.IsError);
        }

        public List<QuantityMeasurementEntity> FindByIsErrorTrue()
        {
            return _context.QuantityMeasurements
                           .Where(e => e.IsError)
                           .OrderByDescending(e => e.CreatedAt)
                           .ToList();
        }
    }
}
