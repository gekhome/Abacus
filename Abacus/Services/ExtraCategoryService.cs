using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class ExtraCategoryService : IDisposable
    {
        private AbacusDBEntities entities;

        public ExtraCategoryService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public List<ExtraCategoryViewModel> Read()
        {
            var data = (from d in entities.ΕΞΤΡΑ_ΚΑΤΗΓΟΡΙΑ
                        select new ExtraCategoryViewModel
                        {
                            ΚΑΤΗΓΟΡΙΑ_ΚΩΔ = d.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΔΑΠΑΝΗ_ΚΩΔ = d.ΔΑΠΑΝΗ_ΚΩΔ
                        }).ToList();
            return data;
        }

        public void Create(ExtraCategoryViewModel data)
        {
            ΕΞΤΡΑ_ΚΑΤΗΓΟΡΙΑ entity = new ΕΞΤΡΑ_ΚΑΤΗΓΟΡΙΑ()
            {
                ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ,
                ΔΑΠΑΝΗ_ΚΩΔ = 3
            };
            entities.ΕΞΤΡΑ_ΚΑΤΗΓΟΡΙΑ.Add(entity);
            entities.SaveChanges();

            data.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ = entity.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ;
        }

        public void Update(ExtraCategoryViewModel data)
        {
            ΕΞΤΡΑ_ΚΑΤΗΓΟΡΙΑ entity = entities.ΕΞΤΡΑ_ΚΑΤΗΓΟΡΙΑ.Find(data.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ);

            entity.ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ;
            entity.ΔΑΠΑΝΗ_ΚΩΔ = 3;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(ExtraCategoryViewModel data)
        {
            ΕΞΤΡΑ_ΚΑΤΗΓΟΡΙΑ entity = entities.ΕΞΤΡΑ_ΚΑΤΗΓΟΡΙΑ.Find(data.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΕΞΤΡΑ_ΚΑΤΗΓΟΡΙΑ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public ExtraCategoryViewModel Refresh(int entityId)
        {
            return entities.ΕΞΤΡΑ_ΚΑΤΗΓΟΡΙΑ.Select(d => new ExtraCategoryViewModel
            {
                ΚΑΤΗΓΟΡΙΑ_ΚΩΔ = d.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ,
                ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                ΔΑΠΑΝΗ_ΚΩΔ = d.ΔΑΠΑΝΗ_ΚΩΔ
            }).Where(d => d.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ.Equals(entityId)).FirstOrDefault();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}