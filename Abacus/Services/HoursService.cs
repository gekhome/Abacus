using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class HoursService : IDisposable
    {
        private AbacusDBEntities entities;

        public HoursService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<HoursViewModel> Read()
        {
            var data = (from d in entities.ΣΥΣ_ΩΡΕΣ
                        orderby d.HOUR_TEXT
                        select new HoursViewModel
                        {
                            HOUR_ID = d.HOUR_ID,
                            HOUR_TEXT = d.HOUR_TEXT,
                        }).ToList();
            return data;
        }

        public void Create(HoursViewModel data)
        {
            ΣΥΣ_ΩΡΕΣ entity = new ΣΥΣ_ΩΡΕΣ()
            {
                HOUR_TEXT = data.HOUR_TEXT
            };
            entities.ΣΥΣ_ΩΡΕΣ.Add(entity);
            entities.SaveChanges();

            data.HOUR_ID = entity.HOUR_ID;
        }

        public void Update(HoursViewModel data)
        {
            ΣΥΣ_ΩΡΕΣ entity = entities.ΣΥΣ_ΩΡΕΣ.Find(data.HOUR_ID);

            entity.HOUR_TEXT = data.HOUR_TEXT;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(HoursViewModel data)
        {
            ΣΥΣ_ΩΡΕΣ entity = entities.ΣΥΣ_ΩΡΕΣ.Find(data.HOUR_ID);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΣΥΣ_ΩΡΕΣ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public HoursViewModel Refresh(int entityId)
        {
            return entities.ΣΥΣ_ΩΡΕΣ.Select(d => new HoursViewModel
            {
                HOUR_ID = d.HOUR_ID,
                HOUR_TEXT = d.HOUR_TEXT
            }).Where(d => d.HOUR_ID.Equals(entityId)).FirstOrDefault();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}