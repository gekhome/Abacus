using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class VATScaleService : IDisposable
    {
        private AbacusDBEntities entities;

        public VATScaleService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public List<VATScaleViewModel> Read()
        {
            var data = (from d in entities.ΦΠΑ_ΤΙΜΕΣ
                        orderby d.FPA_VALUE
                        select new VATScaleViewModel
                        {
                            FPA_ID = d.FPA_ID,
                            FPA_VALUE = d.FPA_VALUE
                        }).ToList();
            return data;
        }

        public void Create(VATScaleViewModel data)
        {
            ΦΠΑ_ΤΙΜΕΣ entity = new ΦΠΑ_ΤΙΜΕΣ()
            {
                FPA_VALUE = data.FPA_VALUE,
            };
            entities.ΦΠΑ_ΤΙΜΕΣ.Add(entity);
            entities.SaveChanges();

            data.FPA_ID = entity.FPA_ID;
        }

        public void Update(VATScaleViewModel data)
        {
            ΦΠΑ_ΤΙΜΕΣ entity = entities.ΦΠΑ_ΤΙΜΕΣ.Find(data.FPA_ID);

            entity.FPA_VALUE = data.FPA_VALUE;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(VATScaleViewModel data)
        {
            ΦΠΑ_ΤΙΜΕΣ entity = entities.ΦΠΑ_ΤΙΜΕΣ.Find(data.FPA_ID);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΦΠΑ_ΤΙΜΕΣ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public VATScaleViewModel Refresh(int entityId)
        {
            return entities.ΦΠΑ_ΤΙΜΕΣ.Select(d => new VATScaleViewModel
            {
                FPA_ID = d.FPA_ID,
                FPA_VALUE = d.FPA_VALUE
            }).Where(d => d.FPA_ID.Equals(entityId)).FirstOrDefault();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}