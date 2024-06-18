using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class ApoxorisiTypeService : IDisposable
    {
        private AbacusDBEntities entities;

        public ApoxorisiTypeService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<SysApoxorisiViewModel> Read()
        {
            var data = (from d in entities.ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ
                        orderby d.ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ
                        select new SysApoxorisiViewModel
                        {
                            ΑΠΟΧΩΡΗΣΗ_ΚΩΔ = d.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ,
                            ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ = d.ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ
                        }).ToList();
            return data;
        }

        public void Create(SysApoxorisiViewModel data)
        {
            ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ entity = new ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ()
            {
                ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ = data.ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ,
            };
            entities.ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ.Add(entity);
            entities.SaveChanges();

            data.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ = entity.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ;
        }

        public void Update(SysApoxorisiViewModel data)
        {
            ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ entity = entities.ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ.Find(data.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ);

            entity.ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ = data.ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(SysApoxorisiViewModel data)
        {
            ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ entity = entities.ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ.Find(data.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public SysApoxorisiViewModel Refresh(int entityId)
        {
            return entities.ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ.Select(d => new SysApoxorisiViewModel
            {
                ΑΠΟΧΩΡΗΣΗ_ΚΩΔ = d.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ,
                ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ = d.ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ
            }).Where(d => d.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ.Equals(entityId)).FirstOrDefault();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}