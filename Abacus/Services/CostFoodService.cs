using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class CostFoodService : IDisposable
    {
        private AbacusDBEntities entities;

        public CostFoodService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<CostFeedingViewModel> Read(int stationId, DateTime date)
        {
            var data = (from d in entities.ΔΑΠΑΝΗ_ΤΡΟΦΗ
                        where d.ΒΝΣ == stationId && d.ΗΜΕΡΟΜΗΝΙΑ == date
                        orderby d.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ.ΚΑΤΗΓΟΡΙΑ, d.ΠΡΟΙΟΝ
                        select new CostFeedingViewModel
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

        public void Create(CostFeedingViewModel data, int stationId, int schoolyearId, DateTime date)
        {
            ΔΑΠΑΝΗ_ΤΡΟΦΗ entity = new ΔΑΠΑΝΗ_ΤΡΟΦΗ()
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
            entities.ΔΑΠΑΝΗ_ΤΡΟΦΗ.Add(entity);
            entities.SaveChanges();

            data.ΚΩΔΙΚΟΣ = entity.ΚΩΔΙΚΟΣ;
        }

        public void Update(CostFeedingViewModel data, int stationId, int schoolyearId, DateTime date)
        {
            ΔΑΠΑΝΗ_ΤΡΟΦΗ entity = entities.ΔΑΠΑΝΗ_ΤΡΟΦΗ.Find(data.ΚΩΔΙΚΟΣ);

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

        public void Destroy(CostFeedingViewModel data)
        {
            ΔΑΠΑΝΗ_ΤΡΟΦΗ entity = entities.ΔΑΠΑΝΗ_ΤΡΟΦΗ.Find(data.ΚΩΔΙΚΟΣ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΔΑΠΑΝΗ_ΤΡΟΦΗ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public CostFeedingViewModel Refresh(int entityId)
        {
            return entities.ΔΑΠΑΝΗ_ΤΡΟΦΗ.Select(d => new CostFeedingViewModel
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

        public List<sqlCostFoodViewModel> Search(int stationId, DateTime? theDate1, DateTime? theDate2)
        {
            List<sqlCostFoodViewModel> data = new List<sqlCostFoodViewModel>();

            if (stationId > 0 && theDate1 != null && theDate2 != null)
            {
                data = (from d in entities.sqlΔΑΠΑΝΗ_ΤΡΟΦΕΙΟ
                        where d.ΒΝΣ == stationId && (d.ΗΜΕΡΟΜΗΝΙΑ >= theDate1 && d.ΗΜΕΡΟΜΗΝΙΑ <= theDate2)
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ, d.ΚΑΤΗΓΟΡΙΑ, d.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ
                        select new sqlCostFoodViewModel
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

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}