using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class ProductStationService : IDisposable
    {
        private AbacusDBEntities entities;

        public ProductStationService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public List<ProductStationViewModel> Read(int stationId, int categoryId)
        {
            var data = (from d in entities.ΠΡΟΙΟΝΤΑ_ΒΝΣ
                        where d.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ == categoryId && d.ΒΝΣ == stationId
                        orderby d.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ, d.ΠΡΟΙΟΝ_ΦΠΑ
                        select new ProductStationViewModel
                        {
                            ΠΡΟΙΟΝ_ΚΩΔ = d.ΠΡΟΙΟΝ_ΚΩΔ,
                            ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ = d.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ,
                            ΠΡΟΙΟΝ_ΜΟΝΑΔΑ = d.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ,
                            ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ = d.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ,
                            ΠΡΟΙΟΝ_ΦΠΑ = d.ΠΡΟΙΟΝ_ΦΠΑ ?? 0,
                            ΒΝΣ = d.ΒΝΣ
                        }).ToList();
            return data;
        }

        public void Create(ProductStationViewModel data, int stationId, int categoryId)
        {
            ΠΡΟΙΟΝΤΑ_ΒΝΣ entity = new ΠΡΟΙΟΝΤΑ_ΒΝΣ()
            {
                ΒΝΣ = stationId,
                ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ = categoryId,
                ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ = data.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ,
                ΠΡΟΙΟΝ_ΜΟΝΑΔΑ = data.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ,
                ΠΡΟΙΟΝ_ΦΠΑ = data.ΠΡΟΙΟΝ_ΦΠΑ
            };
            entities.ΠΡΟΙΟΝΤΑ_ΒΝΣ.Add(entity);
            entities.SaveChanges();

            data.ΠΡΟΙΟΝ_ΚΩΔ = entity.ΠΡΟΙΟΝ_ΚΩΔ;
        }

        public void Update(ProductStationViewModel data, int stationId, int categoryId)
        {
            ΠΡΟΙΟΝΤΑ_ΒΝΣ entity = entities.ΠΡΟΙΟΝΤΑ_ΒΝΣ.Find(data.ΠΡΟΙΟΝ_ΚΩΔ);

            entity.ΒΝΣ = stationId;
            entity.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ = categoryId;
            entity.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ = data.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ;
            entity.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ = data.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ;
            entity.ΠΡΟΙΟΝ_ΦΠΑ = data.ΠΡΟΙΟΝ_ΦΠΑ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(ProductStationViewModel data)
        {
            ΠΡΟΙΟΝΤΑ_ΒΝΣ entity = entities.ΠΡΟΙΟΝΤΑ_ΒΝΣ.Find(data.ΠΡΟΙΟΝ_ΚΩΔ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΠΡΟΙΟΝΤΑ_ΒΝΣ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public ProductStationViewModel Refresh(int entityId)
        {
            return entities.ΠΡΟΙΟΝΤΑ_ΒΝΣ.Select(d => new ProductStationViewModel
            {
                ΠΡΟΙΟΝ_ΚΩΔ = d.ΠΡΟΙΟΝ_ΚΩΔ,
                ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ = d.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ,
                ΠΡΟΙΟΝ_ΜΟΝΑΔΑ = d.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ,
                ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ = d.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ,
                ΠΡΟΙΟΝ_ΦΠΑ = d.ΠΡΟΙΟΝ_ΦΠΑ ?? 0,
                ΒΝΣ = d.ΒΝΣ
            }).Where(d => d.ΠΡΟΙΟΝ_ΚΩΔ.Equals(entityId)).FirstOrDefault();
        }

        public List<sqlProductListViewModel> Search(int stationId)
        {
            var data = (from d in entities.sqlPRODUCT_LIST
                        where d.ΒΝΣ == stationId
                        orderby d.ΔΑΠΑΝΗ_ΚΩΔ, d.ΚΑΤΗΓΟΡΙΑ, d.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ, d.ΜΟΝΑΔΑ
                        select new sqlProductListViewModel
                        {
                            ΠΡΟΙΟΝ_ΚΩΔ = d.ΠΡΟΙΟΝ_ΚΩΔ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ = d.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ,
                            ΜΟΝΑΔΑ = d.ΜΟΝΑΔΑ,
                            ΚΑΤΗΓΟΡΙΑ_ΚΩΔ = d.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ,
                            ΜΟΝΑΔΑ_ΚΩΔ = d.ΜΟΝΑΔΑ_ΚΩΔ,
                            ΠΡΟΙΟΝ_ΦΠΑ = d.ΠΡΟΙΟΝ_ΦΠΑ,
                            ΒΝΣ = d.ΒΝΣ
                        }).ToList();
            return data;
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}