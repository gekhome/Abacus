using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class CostOtherService : IDisposable
    {
        private AbacusDBEntities entities;

        public CostOtherService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<CostOtherViewModel> Read(int stationId, DateTime date)
        {
            var data = (from d in entities.ΔΑΠΑΝΗ_ΑΛΛΗ
                        where d.ΒΝΣ == stationId && d.ΗΜΕΡΟΜΗΝΙΑ == date
                        orderby d.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ.ΚΑΤΗΓΟΡΙΑ, d.ΠΡΟΙΟΝ
                        select new CostOtherViewModel
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

        public void Create(CostOtherViewModel data, int stationId, int schoolyearId, DateTime date)
        {
            ΔΑΠΑΝΗ_ΑΛΛΗ entity = new ΔΑΠΑΝΗ_ΑΛΛΗ()
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
            entities.ΔΑΠΑΝΗ_ΑΛΛΗ.Add(entity);
            entities.SaveChanges();

            data.ΚΩΔΙΚΟΣ = entity.ΚΩΔΙΚΟΣ;
        }

        public void Update(CostOtherViewModel data, int stationId, int schoolyearId, DateTime date)
        {
            ΔΑΠΑΝΗ_ΑΛΛΗ entity = entities.ΔΑΠΑΝΗ_ΑΛΛΗ.Find(data.ΚΩΔΙΚΟΣ);

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

        public void Destroy(CostOtherViewModel data)
        {
            ΔΑΠΑΝΗ_ΑΛΛΗ entity = entities.ΔΑΠΑΝΗ_ΑΛΛΗ.Find(data.ΚΩΔΙΚΟΣ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΔΑΠΑΝΗ_ΑΛΛΗ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public CostOtherViewModel Refresh(int entityId)
        {
            return entities.ΔΑΠΑΝΗ_ΑΛΛΗ.Select(d => new CostOtherViewModel
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

        public List<sqlCostOtherViewModel> Search(int stationId, DateTime? theDate1, DateTime? theDate2)
        {
            List<sqlCostOtherViewModel> data = new List<sqlCostOtherViewModel>();

            if (stationId > 0 && theDate1 != null && theDate2 != null)
            {
                data = (from d in entities.sqlΔΑΠΑΝΗ_ΑΛΛΗ
                        where d.ΒΝΣ == stationId && (d.ΗΜΕΡΟΜΗΝΙΑ >= theDate1 && d.ΗΜΕΡΟΜΗΝΙΑ <= theDate2)
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ, d.ΚΑΤΗΓΟΡΙΑ, d.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ
                        select new sqlCostOtherViewModel
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

        public List<SumOtherExpenseDayViewModel> Aggregate(int stationId, DateTime? theDate1, DateTime? theDate2)
        {
            List<SumOtherExpenseDayViewModel> data = new List<SumOtherExpenseDayViewModel>();

            if (stationId > 0 && theDate1 != null && theDate2 != null)
            {
                data = (from d in entities.sqlΣΥΝΟΛΟ_ΓΕΝΙΚΗ_ΗΜΕΡΑ
                        where d.ΒΝΣ == stationId && (d.ΗΜΕΡΟΜΗΝΙΑ >= theDate1 && d.ΗΜΕΡΟΜΗΝΙΑ <= theDate2)
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ
                        select new SumOtherExpenseDayViewModel
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