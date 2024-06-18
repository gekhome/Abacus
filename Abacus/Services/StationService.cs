using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class StationService : IDisposable
    {
        private AbacusDBEntities entities;

        public StationService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<StationsGridViewModel> Read()
        {
            var data = (from d in entities.ΣΥΣ_ΣΤΑΘΜΟΙ
                        orderby d.ΕΠΩΝΥΜΙΑ
                        select new StationsGridViewModel
                        {
                            ΣΤΑΘΜΟΣ_ΚΩΔ = d.ΣΤΑΘΜΟΣ_ΚΩΔ,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΠΕΡΙΦΕΡΕΙΑΚΗ = d.ΠΕΡΙΦΕΡΕΙΑΚΗ,
                            ΥΠΕΥΘΥΝΟΣ = d.ΥΠΕΥΘΥΝΟΣ
                        }).ToList();
            return data;
        }

        public void Create(StationsGridViewModel data)
        {
            ΣΥΣ_ΣΤΑΘΜΟΙ entity = new ΣΥΣ_ΣΤΑΘΜΟΙ()
            {
                ΕΠΩΝΥΜΙΑ = data.ΕΠΩΝΥΜΙΑ,
                ΠΕΡΙΦΕΡΕΙΑΚΗ = data.ΠΕΡΙΦΕΡΕΙΑΚΗ,
                ΥΠΕΥΘΥΝΟΣ = data.ΥΠΕΥΘΥΝΟΣ
            };
            entities.ΣΥΣ_ΣΤΑΘΜΟΙ.Add(entity);
            entities.SaveChanges();

            data.ΣΤΑΘΜΟΣ_ΚΩΔ = entity.ΣΤΑΘΜΟΣ_ΚΩΔ;
        }

        public void Update(StationsGridViewModel data)
        {
            ΣΥΣ_ΣΤΑΘΜΟΙ entity = entities.ΣΥΣ_ΣΤΑΘΜΟΙ.Find(data.ΣΤΑΘΜΟΣ_ΚΩΔ);

            entity.ΕΠΩΝΥΜΙΑ = data.ΕΠΩΝΥΜΙΑ;
            entity.ΠΕΡΙΦΕΡΕΙΑΚΗ = data.ΠΕΡΙΦΕΡΕΙΑΚΗ;
            entity.ΥΠΕΥΘΥΝΟΣ = data.ΥΠΕΥΘΥΝΟΣ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(StationsGridViewModel data)
        {
            ΣΥΣ_ΣΤΑΘΜΟΙ entity = entities.ΣΥΣ_ΣΤΑΘΜΟΙ.Find(data.ΣΤΑΘΜΟΣ_ΚΩΔ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΣΥΣ_ΣΤΑΘΜΟΙ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public StationsGridViewModel Refresh(int entityId)
        {
            return entities.ΣΥΣ_ΣΤΑΘΜΟΙ.Select(d => new StationsGridViewModel
            {
                ΣΤΑΘΜΟΣ_ΚΩΔ = d.ΣΤΑΘΜΟΣ_ΚΩΔ,
                ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                ΠΕΡΙΦΕΡΕΙΑΚΗ = d.ΠΕΡΙΦΕΡΕΙΑΚΗ,
                ΥΠΕΥΘΥΝΟΣ = d.ΥΠΕΥΘΥΝΟΣ
            }).Where(d => d.ΣΤΑΘΜΟΣ_ΚΩΔ == entityId).FirstOrDefault();
        }

        public StationsViewModel GetRecord(int entityId)
        {
            var data = (from s in entities.ΣΥΣ_ΣΤΑΘΜΟΙ
                        where s.ΣΤΑΘΜΟΣ_ΚΩΔ == entityId
                        select new StationsViewModel
                        {
                            ΣΤΑΘΜΟΣ_ΚΩΔ = s.ΣΤΑΘΜΟΣ_ΚΩΔ,
                            ΕΠΩΝΥΜΙΑ = s.ΕΠΩΝΥΜΙΑ,
                            ΥΠΕΥΘΥΝΟΣ = s.ΥΠΕΥΘΥΝΟΣ,
                            ΥΠΕΥΘΥΝΟΣ_ΦΥΛΟ = s.ΥΠΕΥΘΥΝΟΣ_ΦΥΛΟ,
                            ΤΑΧ_ΔΙΕΥΘΥΝΣΗ = s.ΤΑΧ_ΔΙΕΥΘΥΝΣΗ,
                            ΤΗΛΕΦΩΝΑ = s.ΤΗΛΕΦΩΝΑ,
                            ΓΡΑΜΜΑΤΕΙΑ = s.ΓΡΑΜΜΑΤΕΙΑ,
                            ΦΑΞ = s.ΦΑΞ,
                            EMAIL = s.EMAIL,
                            ΚΙΝΗΤΟ = s.ΚΙΝΗΤΟ,
                            ΑΝΑΠΛΗΡΩΤΗΣ = s.ΑΝΑΠΛΗΡΩΤΗΣ,
                            ΑΝΑΠΛΗΡΩΤΗΣ_ΦΥΛΟ = s.ΑΝΑΠΛΗΡΩΤΗΣ_ΦΥΛΟ,
                            ΔΙΑΧΕΙΡΙΣΤΗΣ = s.ΔΙΑΧΕΙΡΙΣΤΗΣ,
                            ΔΙΑΧΕΙΡΙΣΤΗΣ_ΦΥΛΟ = s.ΔΙΑΧΕΙΡΙΣΤΗΣ_ΦΥΛΟ,
                            ΠΕΡΙΦΕΡΕΙΑΚΗ = s.ΠΕΡΙΦΕΡΕΙΑΚΗ,
                            ΠΕΡΙΦΕΡΕΙΑ = s.ΠΕΡΙΦΕΡΕΙΑ
                        }).FirstOrDefault();
            return data;
        }


        public void Dispose()
        {
            entities.Dispose();
        }
    }
}