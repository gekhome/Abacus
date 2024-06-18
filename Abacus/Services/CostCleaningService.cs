using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class CostCleaningService : IDisposable
    {
        private AbacusDBEntities entities;

        public CostCleaningService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<CostCleaningViewModel> Read(int stationId, DateTime date)
        {
            var data = (from d in entities.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ
                        where d.ΒΝΣ == stationId && d.ΗΜΕΡΟΜΗΝΙΑ == date
                        orderby d.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ.ΚΑΤΗΓΟΡΙΑ, d.ΠΡΟΙΟΝ
                        select new CostCleaningViewModel
                        {
                            ΚΩΔΙΚΟΣ = d.ΚΩΔΙΚΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΣΧΟΛ_ΕΤΟΣ = d.ΣΧΟΛ_ΕΤΟΣ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΠΡΟΙΟΝ = d.ΠΡΟΙΟΝ,
                            ΠΟΣΟΤΗΤΑ = d.ΠΟΣΟΤΗΤΑ,
                            ΤΙΜΗ_ΜΟΝΑΔΑ = d.ΤΙΜΗ_ΜΟΝΑΔΑ,
                            ΣΥΝΟΛΟ = d.ΣΥΝΟΛΟ
                        }).ToList();
            return (data);
        }

        public void Create(CostCleaningViewModel data, int stationId, int schoolyearId, DateTime date)
        {
            ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ entity = new ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ()
            {
                ΗΜΕΡΟΜΗΝΙΑ = date,
                ΒΝΣ = stationId,
                ΣΧΟΛ_ΕΤΟΣ = schoolyearId,
                ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ,
                ΠΡΟΙΟΝ = data.ΠΡΟΙΟΝ,
                ΠΟΣΟΤΗΤΑ = data.ΠΟΣΟΤΗΤΑ,
                ΤΙΜΗ_ΜΟΝΑΔΑ = data.ΤΙΜΗ_ΜΟΝΑΔΑ,
                ΣΥΝΟΛΟ = data.ΠΟΣΟΤΗΤΑ * data.ΤΙΜΗ_ΜΟΝΑΔΑ
            };
            entities.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ.Add(entity);
            entities.SaveChanges();

            data.ΚΩΔΙΚΟΣ = entity.ΚΩΔΙΚΟΣ;
        }

        public void Update(CostCleaningViewModel data, int stationId, int schoolyearId, DateTime date)
        {
            ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ entity = entities.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ.Find(data.ΚΩΔΙΚΟΣ);

            entity.ΗΜΕΡΟΜΗΝΙΑ = date;
            entity.ΒΝΣ = stationId;
            entity.ΣΧΟΛ_ΕΤΟΣ = schoolyearId;
            entity.ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ;
            entity.ΠΡΟΙΟΝ = data.ΠΡΟΙΟΝ;
            entity.ΠΟΣΟΤΗΤΑ = data.ΠΟΣΟΤΗΤΑ;
            entity.ΤΙΜΗ_ΜΟΝΑΔΑ = data.ΤΙΜΗ_ΜΟΝΑΔΑ;
            entity.ΣΥΝΟΛΟ = data.ΠΟΣΟΤΗΤΑ * data.ΤΙΜΗ_ΜΟΝΑΔΑ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(CostCleaningViewModel data)
        {
            ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ entity = entities.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ.Find(data.ΚΩΔΙΚΟΣ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public CostCleaningViewModel Refresh(int entityId)
        {
            return entities.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ.Select(d => new CostCleaningViewModel
            {
                ΚΩΔΙΚΟΣ = d.ΚΩΔΙΚΟΣ,
                ΒΝΣ = d.ΒΝΣ,
                ΣΧΟΛ_ΕΤΟΣ = d.ΣΧΟΛ_ΕΤΟΣ,
                ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                ΠΡΟΙΟΝ = d.ΠΡΟΙΟΝ,
                ΠΟΣΟΤΗΤΑ = d.ΠΟΣΟΤΗΤΑ,
                ΤΙΜΗ_ΜΟΝΑΔΑ = d.ΤΙΜΗ_ΜΟΝΑΔΑ,
                ΣΥΝΟΛΟ = d.ΣΥΝΟΛΟ
            }).Where(d => d.ΚΩΔΙΚΟΣ.Equals(entityId)).FirstOrDefault();
        }

        public List<sqlCostCleaningViewModel> Search(int stationId, DateTime? theDate1, DateTime? theDate2)
        {
            List<sqlCostCleaningViewModel> data = new List<sqlCostCleaningViewModel>();

            if (stationId > 0 && theDate1 != null && theDate2 != null)
            {
                data = (from d in entities.sqlΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ
                        where d.ΒΝΣ == stationId && (d.ΗΜΕΡΟΜΗΝΙΑ >= theDate1 && d.ΗΜΕΡΟΜΗΝΙΑ <= theDate2)
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ, d.ΚΑΤΗΓΟΡΙΑ, d.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ
                        select new sqlCostCleaningViewModel
                        {
                            ΚΩΔΙΚΟΣ = d.ΚΩΔΙΚΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΠΡΟΙΟΝ_ΜΟΝΑΔΑ = d.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ,
                            ΠΟΣΟΤΗΤΑ = d.ΠΟΣΟΤΗΤΑ,
                            ΤΙΜΗ_ΜΟΝΑΔΑ = d.ΤΙΜΗ_ΜΟΝΑΔΑ,
                            ΣΥΝΟΛΟ = d.ΣΥΝΟΛΟ
                        }).ToList();
            }
            return (data);
        }

        public List<SumCleaningDayViewModel> Aggregate(int stationId, DateTime? theDate1, DateTime? theDate2)
        {
            List<SumCleaningDayViewModel> data = new List<SumCleaningDayViewModel>();

            if (stationId > 0 && theDate1 != null && theDate2 != null)
            {
                data = (from d in entities.sqlΣΥΝΟΛΟ_ΚΑΘΑΡΙΟΤΗΤΑ_ΗΜΕΡΑ
                        where d.ΒΝΣ == stationId && (d.ΗΜΕΡΟΜΗΝΙΑ >= theDate1 && d.ΗΜΕΡΟΜΗΝΙΑ <= theDate2)
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ
                        select new SumCleaningDayViewModel
                        {
                            ROWID = d.ROWID,
                            ΣΧΟΛ_ΕΤΟΣ = d.ΣΧΟΛ_ΕΤΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΜΗΝΑΣ = d.ΜΗΝΑΣ,
                            TOTAL_DAY = d.TOTAL_DAY
                        }).ToList();
            }
            return (data);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}