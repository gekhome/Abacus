using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class ProductService : IDisposable
    {
        private AbacusDBEntities entities;

        public ProductService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public List<ProductViewModel> Read(int categoryId)
        {
            var data = (from d in entities.ΠΡΟΙΟΝΤΑ
                        where d.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ == categoryId
                        orderby d.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ1.ΔΑΠΑΝΗ_ΚΩΔ, d.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ
                        select new ProductViewModel
                        {
                            ΠΡΟΙΟΝ_ΚΩΔ = d.ΠΡΟΙΟΝ_ΚΩΔ,
                            ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ = d.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ,
                            ΠΡΟΙΟΝ_ΜΟΝΑΔΑ = d.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ,
                            ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ = d.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ,
                            ΠΡΟΙΟΝ_ΦΠΑ = d.ΠΡΟΙΟΝ_ΦΠΑ ?? 0
                        }).ToList();
            return data;
        }

        public void Create(ProductViewModel data, int categoryId)
        {
            ΠΡΟΙΟΝΤΑ entity = new ΠΡΟΙΟΝΤΑ()
            {
                ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ = categoryId,
                ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ = data.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ,
                ΠΡΟΙΟΝ_ΜΟΝΑΔΑ = data.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ,
                ΠΡΟΙΟΝ_ΦΠΑ = data.ΠΡΟΙΟΝ_ΦΠΑ
            };
            entities.ΠΡΟΙΟΝΤΑ.Add(entity);
            entities.SaveChanges();

            data.ΠΡΟΙΟΝ_ΚΩΔ = entity.ΠΡΟΙΟΝ_ΚΩΔ;
        }

        public void Update(ProductViewModel data, int categoryId)
        {
            ΠΡΟΙΟΝΤΑ entity = entities.ΠΡΟΙΟΝΤΑ.Find(data.ΠΡΟΙΟΝ_ΚΩΔ);

            entity.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ = categoryId;
            entity.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ = data.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ;
            entity.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ = data.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ;
            entity.ΠΡΟΙΟΝ_ΦΠΑ = data.ΠΡΟΙΟΝ_ΦΠΑ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(ProductViewModel data)
        {
            ΠΡΟΙΟΝΤΑ entity = entities.ΠΡΟΙΟΝΤΑ.Find(data.ΠΡΟΙΟΝ_ΚΩΔ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΠΡΟΙΟΝΤΑ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public ProductViewModel Refresh(int entityId)
        {
            return entities.ΠΡΟΙΟΝΤΑ.Select(d => new ProductViewModel
            {
                ΠΡΟΙΟΝ_ΚΩΔ = d.ΠΡΟΙΟΝ_ΚΩΔ,
                ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ = d.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ,
                ΠΡΟΙΟΝ_ΜΟΝΑΔΑ = d.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ,
                ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ = d.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ,
                ΠΡΟΙΟΝ_ΦΠΑ = d.ΠΡΟΙΟΝ_ΦΠΑ ?? 0
            }).Where(d => d.ΠΡΟΙΟΝ_ΚΩΔ.Equals(entityId)).FirstOrDefault();
        }


        public void Dispose()
        {
            entities.Dispose();
        }
    }
}