using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class PersonnelTypeService : IDisposable
    {
        private AbacusDBEntities entities;

        public PersonnelTypeService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<PersonnelTypeViewModel> Read()
        {
            var data = (from d in entities.ΣΥΣ_ΠΡΟΣΩΠΙΚΟ
                        select new PersonnelTypeViewModel
                        {
                            PROSOPIKO_ID = d.PROSOPIKO_ID,
                            PROSOPIKO_TEXT = d.PROSOPIKO_TEXT
                        }).ToList();
            return data;
        }

        public void Create(PersonnelTypeViewModel data)
        {
            ΣΥΣ_ΠΡΟΣΩΠΙΚΟ entity = new ΣΥΣ_ΠΡΟΣΩΠΙΚΟ()
            {
                PROSOPIKO_TEXT = data.PROSOPIKO_TEXT
            };
            entities.ΣΥΣ_ΠΡΟΣΩΠΙΚΟ.Add(entity);
            entities.SaveChanges();

            data.PROSOPIKO_ID = entity.PROSOPIKO_ID;
        }

        public void Update(PersonnelTypeViewModel data)
        {
            ΣΥΣ_ΠΡΟΣΩΠΙΚΟ entity = entities.ΣΥΣ_ΠΡΟΣΩΠΙΚΟ.Find(data.PROSOPIKO_ID);

            entity.PROSOPIKO_TEXT = data.PROSOPIKO_TEXT;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(PersonnelTypeViewModel data)
        {
            ΣΥΣ_ΠΡΟΣΩΠΙΚΟ entity = entities.ΣΥΣ_ΠΡΟΣΩΠΙΚΟ.Find(data.PROSOPIKO_ID);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΣΥΣ_ΠΡΟΣΩΠΙΚΟ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public PersonnelTypeViewModel Refresh(int entityId)
        {
            return entities.ΣΥΣ_ΠΡΟΣΩΠΙΚΟ.Select(d => new PersonnelTypeViewModel
            {
                PROSOPIKO_ID = d.PROSOPIKO_ID,
                PROSOPIKO_TEXT = d.PROSOPIKO_TEXT
            }).Where(d => d.PROSOPIKO_ID.Equals(entityId)).FirstOrDefault();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}