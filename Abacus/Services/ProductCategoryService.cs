using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class ProductCategoryService : IDisposable
    {
        private AbacusDBEntities entities;

        public ProductCategoryService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public List<ProductCategoryViewModel> Read()
        {
            var data = (from d in entities.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ
                        orderby d.ΔΑΠΑΝΗ_ΚΩΔ, d.ΚΑΤΗΓΟΡΙΑ
                        select new ProductCategoryViewModel
                        {
                            ΚΑΤΗΓΟΡΙΑ_ΚΩΔ = d.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΔΑΠΑΝΗ_ΚΩΔ = d.ΔΑΠΑΝΗ_ΚΩΔ
                        }).ToList();
            return (data);
        }

        public void Create(ProductCategoryViewModel data)
        {
            ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ entity = new ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ()
            {
                ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ,
                ΔΑΠΑΝΗ_ΚΩΔ = data.ΔΑΠΑΝΗ_ΚΩΔ
            };
            entities.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ.Add(entity);
            entities.SaveChanges();

            data.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ = entity.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ;
        }

        public void Update(ProductCategoryViewModel data)
        {
            ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ entity = entities.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ.Find(data.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ);

            entity.ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ;
            entity.ΔΑΠΑΝΗ_ΚΩΔ = data.ΔΑΠΑΝΗ_ΚΩΔ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(ProductCategoryViewModel data)
        {
            ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ entity = entities.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ.Find(data.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public ProductCategoryViewModel Refresh(int entityId)
        {
            return entities.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ.Select(d => new ProductCategoryViewModel
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