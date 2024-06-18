using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class ProistamenoiService : IDisposable
    {
        private AbacusDBEntities entities;

        public ProistamenoiService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<ProistamenosViewModel> Read()
        {
            var data = (from d in entities.Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ
                        orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ
                        select new ProistamenosViewModel
                        {
                            ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ = d.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΠΡΟΙΣΤΑΜΕΝΟΣ = d.ΠΡΟΙΣΤΑΜΕΝΟΣ,
                            ΦΥΛΟ = d.ΦΥΛΟ
                        }).ToList();
            return data;
        }

        public void Create(ProistamenosViewModel data)
        {
            Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ entity = new Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ()
            {
                ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                ΠΡΟΙΣΤΑΜΕΝΟΣ = data.ΠΡΟΙΣΤΑΜΕΝΟΣ,
                ΦΥΛΟ = data.ΦΥΛΟ
            };
            entities.Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ.Add(entity);
            entities.SaveChanges();

            data.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ = entity.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ;
        }

        public void Update(ProistamenosViewModel data)
        {
            Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ entity = entities.Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ.Find(data.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ);

            entity.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ = data.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ;
            entity.ΠΡΟΙΣΤΑΜΕΝΟΣ = data.ΠΡΟΙΣΤΑΜΕΝΟΣ;
            entity.ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
            entity.ΦΥΛΟ = data.ΦΥΛΟ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(ProistamenosViewModel data)
        {
            Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ entity = entities.Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ.Find(data.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public ProistamenosViewModel Refresh(int entityId)
        {
            return entities.Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ.Select(d => new ProistamenosViewModel
            {
                ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ = d.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ,
                ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                ΠΡΟΙΣΤΑΜΕΝΟΣ = d.ΠΡΟΙΣΤΑΜΕΝΟΣ,
                ΦΥΛΟ = d.ΦΥΛΟ
            }).Where(d => d.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ == entityId).FirstOrDefault();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}