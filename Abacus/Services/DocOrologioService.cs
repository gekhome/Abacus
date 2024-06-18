using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class DocOrologioService : IDisposable
    {
        private AbacusDBEntities entities;

        public DocOrologioService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<DocProgrammaViewModel> Read(int schoolyearId, int stationId)
        {
            var data = (from d in entities.ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ
                        where d.SCHOOLYEAR_ID == schoolyearId && d.STATION_ID == stationId
                        orderby d.DOC_DATE descending
                        select new DocProgrammaViewModel
                        {
                            DOC_ID = d.DOC_ID,
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            STATION_ID = d.STATION_ID,
                            PERIFERIAKI_ID = d.PERIFERIAKI_ID,
                            ADMIN_ID = d.ADMIN_ID,
                            DOC_YEAR = d.DOC_YEAR,
                            DOC_MONTH = d.DOC_MONTH,
                            DOC_DATE = d.DOC_DATE,
                            DOC_PROTOCOL = d.DOC_PROTOCOL,
                            CORRECTION = d.CORRECTION ?? false,
                            CORRECTION_DATE = d.CORRECTION_DATE
                        }).ToList();
            return data;
        }

        public void Create(DocProgrammaViewModel data, int schoolyearId, int stationId)
        {
            ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ entity = new ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ()
            {
                SCHOOLYEAR_ID = schoolyearId,
                STATION_ID = stationId,
                PERIFERIAKI_ID = Common.GetPeriferiakiFromStation(stationId),
                ADMIN_ID = data.ADMIN_ID,
                DOC_YEAR = data.DOC_YEAR,
                DOC_MONTH = data.DOC_MONTH,
                DOC_DATE = data.DOC_DATE,
                DOC_PROTOCOL = data.DOC_PROTOCOL
            };
            entities.ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ.Add(entity);
            entities.SaveChanges();

            data.DOC_ID = entity.DOC_ID;
            data.PERIFERIAKI_ID = entity.PERIFERIAKI_ID;
        }

        public void Update(DocProgrammaViewModel data, int schoolyearId, int stationId)
        {
            ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ entity = entities.ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ.Find(data.DOC_ID);

            entity.SCHOOLYEAR_ID = schoolyearId;
            entity.STATION_ID = stationId;
            entity.PERIFERIAKI_ID = Common.GetPeriferiakiFromStation(stationId);
            entity.ADMIN_ID = data.ADMIN_ID;
            entity.DOC_YEAR = data.DOC_YEAR;
            entity.DOC_MONTH = data.DOC_MONTH;
            entity.DOC_DATE = data.DOC_DATE;
            entity.DOC_PROTOCOL = data.DOC_PROTOCOL;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(DocProgrammaViewModel data)
        {
            ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ entity = entities.ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ.Find(data.DOC_ID);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public DocProgrammaViewModel Refresh(int entityId)
        {
            return entities.ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ.Select(d => new DocProgrammaViewModel
            {
                DOC_ID = d.DOC_ID,
                SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                STATION_ID = d.STATION_ID,
                PERIFERIAKI_ID = d.PERIFERIAKI_ID,
                ADMIN_ID = d.ADMIN_ID,
                DOC_YEAR = d.DOC_YEAR,
                DOC_MONTH = d.DOC_MONTH,
                DOC_DATE = d.DOC_DATE,
                DOC_PROTOCOL = d.DOC_PROTOCOL,
                CORRECTION = d.CORRECTION ?? false,
                CORRECTION_DATE = d.CORRECTION_DATE
            }).Where(d => d.DOC_ID.Equals(entityId)).FirstOrDefault();
        }

        public DocProgrammaViewModel GetRecord(int entityId)
        {
            var data = (from d in entities.ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ
                    where d.DOC_ID == entityId
                    select new DocProgrammaViewModel
                    {
                        DOC_ID = d.DOC_ID,
                        SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                        STATION_ID = d.STATION_ID,
                        PERIFERIAKI_ID = d.PERIFERIAKI_ID,
                        ADMIN_ID = d.ADMIN_ID,
                        DOC_YEAR = d.DOC_YEAR,
                        DOC_MONTH = d.DOC_MONTH,
                        DOC_DATE = d.DOC_DATE,
                        DOC_PROTOCOL = d.DOC_PROTOCOL,
                        DOC_SXETIKA = d.DOC_SXETIKA,
                        CORRECTION = d.CORRECTION ?? false,
                        CORRECTION_DATE = d.CORRECTION_DATE
                    }).FirstOrDefault();
            return data;
        }

        public ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ SetRecord(DocProgrammaViewModel data, int entityId, int stationId)
        {
            ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ entity = entities.ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ.Find(entityId);

            entity.SCHOOLYEAR_ID = data.SCHOOLYEAR_ID;
            entity.STATION_ID = stationId;
            entity.PERIFERIAKI_ID = Common.GetPeriferiakiFromStation(stationId);
            entity.ADMIN_ID = data.ADMIN_ID;
            entity.DOC_YEAR = data.DOC_YEAR;
            entity.DOC_MONTH = data.DOC_MONTH;
            entity.DOC_DATE = data.DOC_DATE;
            entity.DOC_PROTOCOL = data.DOC_PROTOCOL;
            entity.DOC_SXETIKA = data.DOC_SXETIKA;
            entity.CORRECTION = data.CORRECTION;
            entity.CORRECTION_DATE = data.CORRECTION_DATE;

            return entity;
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}