using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class DocOperatorService : IDisposable
    {
        private AbacusDBEntities entities;

        public DocOperatorService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<DocOperatorViewModel> Read(int stationId)
        {
            var data = (from d in entities.ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ
                        orderby d.DOCADMIN_NAME
                        where d.DOCSTATION_ID == stationId
                        select new DocOperatorViewModel
                        {
                            DOCADMIN_ID = d.DOCADMIN_ID,
                            DOCSTATION_ID = d.DOCSTATION_ID,
                            DOCADMIN_NAME = d.DOCADMIN_NAME
                        }).ToList();
            return data;
        }

        public void Create(DocOperatorViewModel data, int stationId)
        {
            ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ entity = new ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ()
            {
                DOCSTATION_ID = stationId,
                DOCADMIN_NAME = data.DOCADMIN_NAME
            };
            entities.ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ.Add(entity);
            entities.SaveChanges();

            data.DOCADMIN_ID = entity.DOCADMIN_ID;
        }

        public void Update(DocOperatorViewModel data, int stationId)
        {
            ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ entity = entities.ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ.Find(data.DOCADMIN_ID);

            entity.DOCSTATION_ID = stationId;
            entity.DOCADMIN_NAME = data.DOCADMIN_NAME;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(DocOperatorViewModel data)
        {
            ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ entity = entities.ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ.Find(data.DOCADMIN_ID);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public DocOperatorViewModel Refresh(int entityId)
        {
            return entities.ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ.Select(d => new DocOperatorViewModel
            {
                DOCADMIN_ID = d.DOCADMIN_ID,
                DOCSTATION_ID = d.DOCSTATION_ID,
                DOCADMIN_NAME = d.DOCADMIN_NAME
            }).Where(d => d.DOCADMIN_ID.Equals(entityId)).FirstOrDefault();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}