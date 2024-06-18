using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class EidikotitesService : IDisposable
    {
        private AbacusDBEntities entities;

        public EidikotitesService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<EidikotitesViewModel> Read()
        {
            var data = (from d in entities.ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ
                        orderby d.KLADOS, d.EIDIKOTITA_TEXT
                        select new EidikotitesViewModel
                        {
                            EIDIKOTITA_ID = d.EIDIKOTITA_ID,
                            EIDIKOTITA_CODE = d.EIDIKOTITA_CODE,
                            EIDIKOTITA_TEXT = d.EIDIKOTITA_TEXT,
                            KLADOS = d.KLADOS ?? 0
                        }).ToList();

            return data;
        }

        public void Create(EidikotitesViewModel data)
        {
            ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ entity = new ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ()
            {
                EIDIKOTITA_CODE = data.EIDIKOTITA_CODE,
                EIDIKOTITA_TEXT = data.EIDIKOTITA_TEXT,
                KLADOS = data.KLADOS
            };
            entities.ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ.Add(entity);
            entities.SaveChanges();

            data.EIDIKOTITA_ID = entity.EIDIKOTITA_ID;
        }

        public void Update(EidikotitesViewModel data)
        {
            ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ entity = entities.ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ.Find(data.EIDIKOTITA_ID);

            entity.EIDIKOTITA_CODE = data.EIDIKOTITA_CODE;
            entity.EIDIKOTITA_TEXT = data.EIDIKOTITA_TEXT;
            entity.KLADOS = data.KLADOS;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(EidikotitesViewModel data)
        {
            ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ entity = entities.ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ.Find(data.EIDIKOTITA_ID);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public EidikotitesViewModel Refresh(int entityId)
        {
            return entities.ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ.Select(d => new EidikotitesViewModel
            {
                EIDIKOTITA_ID = d.EIDIKOTITA_ID,
                EIDIKOTITA_CODE = d.EIDIKOTITA_CODE,
                EIDIKOTITA_TEXT = d.EIDIKOTITA_TEXT,
                KLADOS = d.KLADOS ?? 0
            }).Where(d => d.EIDIKOTITA_ID == entityId).FirstOrDefault();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}