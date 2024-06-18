using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class CostGeneralService : IDisposable
    {
        private AbacusDBEntities entities;

        public CostGeneralService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<CostGeneralViewModel> Read(int stationId, DateTime date)
        {
            var data = (from d in entities.ΔΑΠΑΝΗ_ΓΕΝΙΚΗ
                        where d.ΒΝΣ == stationId && d.ΗΜΕΡΟΜΗΝΙΑ == date
                        orderby d.ΕΞΤΡΑ_ΚΑΤΗΓΟΡΙΑ.ΚΑΤΗΓΟΡΙΑ, d.ΠΕΡΙΓΡΑΦΗ
                        select new CostGeneralViewModel
                        {
                            ΚΩΔΙΚΟΣ = d.ΚΩΔΙΚΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΣΧΟΛ_ΕΤΟΣ = d.ΣΧΟΛ_ΕΤΟΣ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΠΕΡΙΓΡΑΦΗ = d.ΠΕΡΙΓΡΑΦΗ,
                            ΣΥΝΟΛΟ = d.ΣΥΝΟΛΟ
                        }).ToList();
            return (data);
        }

        public void Create(CostGeneralViewModel data, int stationId, int schoolyearId, DateTime date)
        {
            ΔΑΠΑΝΗ_ΓΕΝΙΚΗ entity = new ΔΑΠΑΝΗ_ΓΕΝΙΚΗ()
            {
                ΗΜΕΡΟΜΗΝΙΑ = date,
                ΒΝΣ = stationId,
                ΣΧΟΛ_ΕΤΟΣ = schoolyearId,
                ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ,
                ΠΕΡΙΓΡΑΦΗ = data.ΠΕΡΙΓΡΑΦΗ,
                ΣΥΝΟΛΟ = data.ΣΥΝΟΛΟ
            };
            entities.ΔΑΠΑΝΗ_ΓΕΝΙΚΗ.Add(entity);
            entities.SaveChanges();

            data.ΚΩΔΙΚΟΣ = entity.ΚΩΔΙΚΟΣ;
        }

        public void Update(CostGeneralViewModel data, int stationId, int schoolyearId, DateTime date)
        {
            ΔΑΠΑΝΗ_ΓΕΝΙΚΗ entity = entities.ΔΑΠΑΝΗ_ΓΕΝΙΚΗ.Find(data.ΚΩΔΙΚΟΣ);

            entity.ΗΜΕΡΟΜΗΝΙΑ = date;
            entity.ΒΝΣ = stationId;
            entity.ΣΧΟΛ_ΕΤΟΣ = schoolyearId;
            entity.ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ;
            entity.ΠΕΡΙΓΡΑΦΗ = data.ΠΕΡΙΓΡΑΦΗ;
            entity.ΣΥΝΟΛΟ = data.ΣΥΝΟΛΟ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(CostGeneralViewModel data)
        {
            ΔΑΠΑΝΗ_ΓΕΝΙΚΗ entity = entities.ΔΑΠΑΝΗ_ΓΕΝΙΚΗ.Find(data.ΚΩΔΙΚΟΣ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΔΑΠΑΝΗ_ΓΕΝΙΚΗ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public CostGeneralViewModel Refresh(int entityId)
        {
            return entities.ΔΑΠΑΝΗ_ΓΕΝΙΚΗ.Select(d => new CostGeneralViewModel
            {
                ΚΩΔΙΚΟΣ = d.ΚΩΔΙΚΟΣ,
                ΒΝΣ = d.ΒΝΣ,
                ΣΧΟΛ_ΕΤΟΣ = d.ΣΧΟΛ_ΕΤΟΣ,
                ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                ΠΕΡΙΓΡΑΦΗ = d.ΠΕΡΙΓΡΑΦΗ,
                ΣΥΝΟΛΟ = d.ΣΥΝΟΛΟ
            }).Where(d => d.ΚΩΔΙΚΟΣ.Equals(entityId)).FirstOrDefault();
        }

        public List<sqlCostGeneralViewModel> Search(int stationId, DateTime? theDate1, DateTime? theDate2)
        {
            List<sqlCostGeneralViewModel> data = new List<sqlCostGeneralViewModel>();

            if (stationId > 0 && theDate1 != null && theDate2 != null)
            {
                data = (from d in entities.sqlΔΑΠΑΝΗ_ΓΕΝΙΚΗ
                        where d.ΒΝΣ == stationId && (d.ΗΜΕΡΟΜΗΝΙΑ >= theDate1 && d.ΗΜΕΡΟΜΗΝΙΑ <= theDate2)
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ, d.ΚΑΤΗΓΟΡΙΑ, d.ΠΕΡΙΓΡΑΦΗ
                        select new sqlCostGeneralViewModel
                        {
                            ΚΩΔΙΚΟΣ = d.ΚΩΔΙΚΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΠΕΡΙΓΡΑΦΗ = d.ΠΕΡΙΓΡΑΦΗ,
                            ΠΟΣΟΤΗΤΑ = d.ΠΟΣΟΤΗΤΑ,
                            ΤΙΜΗ_ΜΟΝΑΔΑ = d.ΤΙΜΗ_ΜΟΝΑΔΑ,
                            ΣΥΝΟΛΟ = d.ΣΥΝΟΛΟ
                        }).ToList();
            }
            return (data);
        }

        public List<SumExtraExpenseDayViewModel> Aggregate(int stationId, DateTime? theDate1, DateTime? theDate2)
        {
            List<SumExtraExpenseDayViewModel> data = new List<SumExtraExpenseDayViewModel>();

            if (stationId > 0 && theDate1 != null && theDate2 != null)
            {
                data = (from d in entities.sqlΣΥΝΟΛΟ_ΕΞΤΡΑ_ΗΜΕΡΑ
                        where d.ΒΝΣ == stationId && (d.ΗΜΕΡΟΜΗΝΙΑ >= theDate1 && d.ΗΜΕΡΟΜΗΝΙΑ <= theDate2)
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ
                        select new SumExtraExpenseDayViewModel
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