using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class MetaboliTypeService : IDisposable
    {
        private AbacusDBEntities entities;

        public MetaboliTypeService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<SysMetabolesViewModel> Read()
        {
            var data = (from d in entities.ΣΥΣ_ΜΕΤΑΒΟΛΕΣ
                        orderby d.METABOLI_TEXT
                        select new SysMetabolesViewModel
                        {
                            METABOLI_ID = d.METABOLI_ID,
                            METABOLI_TEXT = d.METABOLI_TEXT
                        }).ToList();
            return data;
        }

        public void Create(SysMetabolesViewModel data)
        {
            ΣΥΣ_ΜΕΤΑΒΟΛΕΣ entity = new ΣΥΣ_ΜΕΤΑΒΟΛΕΣ()
            {
                METABOLI_TEXT = data.METABOLI_TEXT,
            };
            entities.ΣΥΣ_ΜΕΤΑΒΟΛΕΣ.Add(entity);
            entities.SaveChanges();

            data.METABOLI_ID = entity.METABOLI_ID;
        }

        public void Update(SysMetabolesViewModel data)
        {
            ΣΥΣ_ΜΕΤΑΒΟΛΕΣ entity = entities.ΣΥΣ_ΜΕΤΑΒΟΛΕΣ.Find(data.METABOLI_ID);

            entity.METABOLI_TEXT = data.METABOLI_TEXT;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(SysMetabolesViewModel data)
        {
            ΣΥΣ_ΜΕΤΑΒΟΛΕΣ entity = entities.ΣΥΣ_ΜΕΤΑΒΟΛΕΣ.Find(data.METABOLI_ID);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΣΥΣ_ΜΕΤΑΒΟΛΕΣ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public SysMetabolesViewModel Refresh(int entityId)
        {
            return entities.ΣΥΣ_ΜΕΤΑΒΟΛΕΣ.Select(d => new SysMetabolesViewModel
            {
                METABOLI_ID = d.METABOLI_ID,
                METABOLI_TEXT = d.METABOLI_TEXT
            }).Where(d => d.METABOLI_ID.Equals(entityId)).FirstOrDefault();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}