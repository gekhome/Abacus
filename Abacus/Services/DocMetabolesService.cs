using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class DocMetabolesService : IDisposable
    {
        private AbacusDBEntities entities;

        public DocMetabolesService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<DocMetabolesViewModel> Read(int schoolyearId, int stationId)
        {
            var data = (from d in entities.ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ
                        where d.SCHOOLYEAR_ID == schoolyearId && d.STATION_ID == stationId
                        orderby d.DOC_DATE descending
                        select new DocMetabolesViewModel
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

        public void Create(DocMetabolesViewModel data, int schoolyearId, int stationId)
        {
            ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ entity = new ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ()
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
            entities.ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ.Add(entity);
            entities.SaveChanges();

            data.DOC_ID = entity.DOC_ID;
            data.PERIFERIAKI_ID = entity.PERIFERIAKI_ID;
        }

        public void Update(DocMetabolesViewModel data, int schoolyearId, int stationId)
        {
            ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ entity = entities.ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ.Find(data.DOC_ID);

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

        public void Destroy(DocMetabolesViewModel data)
        {
            ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ entity = entities.ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ.Find(data.DOC_ID);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public DocMetabolesViewModel Refresh(int entityId)
        {
            return entities.ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ.Select(d => new DocMetabolesViewModel
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

        public DocMetabolesViewModel GetRecord(int entityId)
        {
            var data = (from d in entities.ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ
                        where d.DOC_ID == entityId
                        select new DocMetabolesViewModel
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

        public ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ SetRecord(DocMetabolesViewModel data, int entityId, int stationId)
        {
            ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ entity = entities.ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ.Find(entityId);

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

        public List<MetabolesReportViewModel> ReadReport(int yearId, int monthId, int stationId)
        {
            var data = (from d in entities.METABOLES_REPORT
                        orderby d.ΠΡΟΣΩΠΙΚΟ.ΕΠΩΝΥΜΟ, d.ΠΡΟΣΩΠΙΚΟ.ΟΝΟΜΑ
                        where d.BNS == stationId && d.METABOLI_YEAR == yearId && d.METABOLI_MONTH == monthId
                        select new MetabolesReportViewModel
                        {
                            RECORD_ID = d.RECORD_ID,
                            BNS = d.BNS,
                            EMPLOYEE_ID = d.EMPLOYEE_ID,
                            SCHOOL_YEAR = d.SCHOOL_YEAR,
                            METABOLI_MONTH = d.METABOLI_MONTH,
                            METABOLI_YEAR = d.METABOLI_YEAR,
                            METABOLI_TEXT = d.METABOLI_TEXT
                        }).ToList();
            return data;
        }


        public void Dispose()
        {
            entities.Dispose();
        }
    }
}