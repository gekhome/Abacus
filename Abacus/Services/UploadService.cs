using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class UploadService : IDisposable
    {
        private AbacusDBEntities entities;

        public UploadService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public List<DocUploadsViewModel> Read(int stationId)
        {
            var data = (from d in entities.DOCUPLOADS
                        where d.STATION_ID == stationId
                        orderby d.SCHOOLYEAR_ID descending, d.UPLOAD_DATE descending
                        select new DocUploadsViewModel
                        {
                            UPLOAD_ID = d.UPLOAD_ID,
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            STATION_ID = d.STATION_ID,
                            UPLOAD_DATE = d.UPLOAD_DATE,
                            UPLOAD_NAME = d.UPLOAD_NAME,
                            UPLOAD_SUMMARY = d.UPLOAD_SUMMARY
                        }).ToList();
            return data;
        }

        public void Create(DocUploadsViewModel data, int stationId)
        {
            DOCUPLOADS entity = new DOCUPLOADS()
            {
                STATION_ID = stationId,
                SCHOOLYEAR_ID = data.SCHOOLYEAR_ID,
                UPLOAD_DATE = data.UPLOAD_DATE,
                UPLOAD_NAME = data.UPLOAD_NAME,
                UPLOAD_SUMMARY = data.UPLOAD_SUMMARY
            };
            entities.DOCUPLOADS.Add(entity);
            entities.SaveChanges();

            data.UPLOAD_ID = entity.UPLOAD_ID;
        }

        public void Update(DocUploadsViewModel data, int stationId)
        {
            DOCUPLOADS entity = entities.DOCUPLOADS.Find(data.UPLOAD_ID);

            entity.STATION_ID = stationId;
            entity.SCHOOLYEAR_ID = data.SCHOOLYEAR_ID;
            entity.UPLOAD_DATE = data.UPLOAD_DATE;
            entity.UPLOAD_NAME = data.UPLOAD_NAME;
            entity.UPLOAD_SUMMARY = data.UPLOAD_SUMMARY;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(DocUploadsViewModel data)
        {
            DOCUPLOADS entity = entities.DOCUPLOADS.Find(data.UPLOAD_ID);

            try
            {
                if (entity != null)
                {
                    entities.Entry(entity).State = EntityState.Deleted;
                    entities.DOCUPLOADS.Remove(entity);
                    entities.SaveChanges();
                }
            }
            catch { }
        }

        public DocUploadsViewModel Refresh(int entityId)
        {
            return entities.DOCUPLOADS.Select(d => new DocUploadsViewModel
            {
                UPLOAD_ID = d.UPLOAD_ID,
                SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                STATION_ID = d.STATION_ID,
                UPLOAD_DATE = d.UPLOAD_DATE,
                UPLOAD_NAME = d.UPLOAD_NAME,
                UPLOAD_SUMMARY = d.UPLOAD_SUMMARY
            }).Where(d => d.UPLOAD_ID.Equals(entityId)).FirstOrDefault();
        }

        public List<DocUploadsFilesViewModel> GetFiles(int uploadId)
        {
            var data = (from d in entities.DOCUPLOADS_FILES
                        where d.UPLOAD_ID == uploadId
                        orderby d.STATION_USER, d.SCHOOLYEAR_TEXT, d.FILENAME
                        select new DocUploadsFilesViewModel
                        {
                            ID = d.ID,
                            UPLOAD_ID = d.UPLOAD_ID,
                            STATION_USER = d.STATION_USER,
                            SCHOOLYEAR_TEXT = d.SCHOOLYEAR_TEXT,
                            FILENAME = d.FILENAME,
                            EXTENSION = d.EXTENSION
                        }).ToList();
            return (data);
        }

        public void DeleteFile(DocUploadsFilesViewModel data)
        {
            DOCUPLOADS_FILES entity = entities.DOCUPLOADS_FILES.Find(data.ID);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.DOCUPLOADS_FILES.Remove(entity);
                entities.SaveChanges();
            }
        }


        public void Dispose()
        {
            entities.Dispose();
        }
    }
}