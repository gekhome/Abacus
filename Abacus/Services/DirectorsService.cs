using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class DirectorsService : IDisposable
    {
        private AbacusDBEntities entities;

        public DirectorsService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<DirectorViewModel> Read()
        {
            var data = (from d in entities.Δ_ΔΙΕΥΘΥΝΤΕΣ
                        orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ
                        select new DirectorViewModel
                        {
                            ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ = d.ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΔΙΕΥΘΥΝΤΗΣ = d.ΔΙΕΥΘΥΝΤΗΣ,
                            ΦΥΛΟ = d.ΦΥΛΟ
                        }).ToList();
            return data;
        }

        public void Create(DirectorViewModel data)
        {
            Δ_ΔΙΕΥΘΥΝΤΕΣ entity = new Δ_ΔΙΕΥΘΥΝΤΕΣ()
            {
                ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                ΔΙΕΥΘΥΝΤΗΣ = data.ΔΙΕΥΘΥΝΤΗΣ,
                ΦΥΛΟ = data.ΦΥΛΟ
            };
            entities.Δ_ΔΙΕΥΘΥΝΤΕΣ.Add(entity);
            entities.SaveChanges();

            data.ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ = entity.ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ;
        }

        public void Update(DirectorViewModel data)
        {
            Δ_ΔΙΕΥΘΥΝΤΕΣ entity = entities.Δ_ΔΙΕΥΘΥΝΤΕΣ.Find(data.ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ);

            entity.ΔΙΕΥΘΥΝΤΗΣ = data.ΔΙΕΥΘΥΝΤΗΣ;
            entity.ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
            entity.ΦΥΛΟ = data.ΦΥΛΟ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(DirectorViewModel data)
        {
            Δ_ΔΙΕΥΘΥΝΤΕΣ entity = entities.Δ_ΔΙΕΥΘΥΝΤΕΣ.Find(data.ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.Δ_ΔΙΕΥΘΥΝΤΕΣ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public DirectorViewModel Refresh(int entityId)
        {
            return entities.Δ_ΔΙΕΥΘΥΝΤΕΣ.Select(d => new DirectorViewModel
            {
                ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ = d.ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ,
                ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                ΔΙΕΥΘΥΝΤΗΣ = d.ΔΙΕΥΘΥΝΤΗΣ,
                ΦΥΛΟ = d.ΦΥΛΟ
            }).Where(d => d.ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ == entityId).FirstOrDefault();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}