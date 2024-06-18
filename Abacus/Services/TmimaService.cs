using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class TmimaService : IDisposable
    {
        private AbacusDBEntities entities;

        public TmimaService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<TmimaViewModel> Read(int stationId)
        {
            var data = (from d in entities.ΤΜΗΜΑ
                        where d.ΒΝΣ == stationId
                        orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ descending, d.ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ, d.ΑΥΞΩΝ_ΑΡΙΘΜΟΣ
                        select new TmimaViewModel
                        {
                            ΤΜΗΜΑ_ΚΩΔ = d.ΤΜΗΜΑ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΑΥΞΩΝ_ΑΡΙΘΜΟΣ = d.ΑΥΞΩΝ_ΑΡΙΘΜΟΣ,
                            ΧΑΡΑΚΤΗΡΙΣΜΟΣ = d.ΧΑΡΑΚΤΗΡΙΣΜΟΣ,
                            ΟΝΟΜΑΣΙΑ = d.ΟΝΟΜΑΣΙΑ
                        }).ToList();
            return (data);
        }

        public void Create(TmimaViewModel data, int stationId)
        {
            ΤΜΗΜΑ entity = new ΤΜΗΜΑ()
            {
                ΒΝΣ = stationId,
                ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ,
                ΑΥΞΩΝ_ΑΡΙΘΜΟΣ = data.ΑΥΞΩΝ_ΑΡΙΘΜΟΣ,
                ΧΑΡΑΚΤΗΡΙΣΜΟΣ = data.ΧΑΡΑΚΤΗΡΙΣΜΟΣ,
                ΟΝΟΜΑΣΙΑ = Common.BuildTmimaName(data)
            };
            entities.ΤΜΗΜΑ.Add(entity);
            entities.SaveChanges();

            data.ΤΜΗΜΑ_ΚΩΔ = entity.ΤΜΗΜΑ_ΚΩΔ;
        }

        public void Update(TmimaViewModel data, int stationId)
        {
            ΤΜΗΜΑ entity = entities.ΤΜΗΜΑ.Find(data.ΤΜΗΜΑ_ΚΩΔ);

            entity.ΒΝΣ = stationId;
            entity.ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
            entity.ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ;
            entity.ΑΥΞΩΝ_ΑΡΙΘΜΟΣ = data.ΑΥΞΩΝ_ΑΡΙΘΜΟΣ;
            entity.ΧΑΡΑΚΤΗΡΙΣΜΟΣ = data.ΧΑΡΑΚΤΗΡΙΣΜΟΣ;
            entity.ΟΝΟΜΑΣΙΑ = Common.BuildTmimaName(data);

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(TmimaViewModel data)
        {
            ΤΜΗΜΑ entity = entities.ΤΜΗΜΑ.Find(data.ΤΜΗΜΑ_ΚΩΔ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΤΜΗΜΑ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public TmimaViewModel Refresh(int entityId)
        {
            return entities.ΤΜΗΜΑ.Select(d => new TmimaViewModel
            {
                ΤΜΗΜΑ_ΚΩΔ = d.ΤΜΗΜΑ_ΚΩΔ,
                ΒΝΣ = d.ΒΝΣ,
                ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                ΑΥΞΩΝ_ΑΡΙΘΜΟΣ = d.ΑΥΞΩΝ_ΑΡΙΘΜΟΣ,
                ΧΑΡΑΚΤΗΡΙΣΜΟΣ = d.ΧΑΡΑΚΤΗΡΙΣΜΟΣ,
                ΟΝΟΜΑΣΙΑ = d.ΟΝΟΜΑΣΙΑ
            }).Where(d => d.ΤΜΗΜΑ_ΚΩΔ == entityId).FirstOrDefault();
        }

        public TmimaViewModel GetRecord(int tmimaId)
        {
            var data = (from d in entities.ΤΜΗΜΑ
                        where d.ΤΜΗΜΑ_ΚΩΔ == tmimaId
                        select new TmimaViewModel
                        {
                            ΤΜΗΜΑ_ΚΩΔ = d.ΤΜΗΜΑ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΑΥΞΩΝ_ΑΡΙΘΜΟΣ = d.ΑΥΞΩΝ_ΑΡΙΘΜΟΣ,
                            ΧΑΡΑΚΤΗΡΙΣΜΟΣ = d.ΧΑΡΑΚΤΗΡΙΣΜΟΣ,
                            ΟΝΟΜΑΣΙΑ = d.ΟΝΟΜΑΣΙΑ
                        }).FirstOrDefault();
            return (data);
        }

        public List<sqlTmimaInfoViewModel> ReadInfo(int stationId = 0)
        {
            List<sqlTmimaInfoViewModel> data = new List<sqlTmimaInfoViewModel>();

            if (stationId > 0)
            {
                 data = (from d in entities.sqlTMIMATA_INFO
                            where d.ΒΝΣ == stationId
                            orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ descending, d.ΤΜΗΜΑ_ΟΝΟΜΑ
                            select new sqlTmimaInfoViewModel
                            {
                                ΤΜΗΜΑ_ΚΩΔ = d.ΤΜΗΜΑ_ΚΩΔ,
                                ΤΜΗΜΑ_ΟΝΟΜΑ = d.ΤΜΗΜΑ_ΟΝΟΜΑ,
                                SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                                ΒΝΣ = d.ΒΝΣ,
                                ΒΝΣ_ΟΝΟΜΑ = d.ΒΝΣ_ΟΝΟΜΑ,
                                ΠΕΡΙΦΕΡΕΙΑΚΗ = d.ΠΕΡΙΦΕΡΕΙΑΚΗ,
                                ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ
                            }).ToList();
            }
            else
            {
                data = (from d in entities.sqlTMIMATA_INFO
                        orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ descending, d.ΒΝΣ_ΟΝΟΜΑ, d.ΤΜΗΜΑ_ΟΝΟΜΑ
                        select new sqlTmimaInfoViewModel
                        {
                            ΤΜΗΜΑ_ΚΩΔ = d.ΤΜΗΜΑ_ΚΩΔ,
                            ΤΜΗΜΑ_ΟΝΟΜΑ = d.ΤΜΗΜΑ_ΟΝΟΜΑ,
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            ΒΝΣ = d.ΒΝΣ,
                            ΒΝΣ_ΟΝΟΜΑ = d.ΒΝΣ_ΟΝΟΜΑ,
                            ΠΕΡΙΦΕΡΕΙΑΚΗ = d.ΠΕΡΙΦΕΡΕΙΑΚΗ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ
                        }).ToList();
            }
            return (data);
        }

        public List<sqlTmimaChildViewModel> GetChildren(int tmimaId)
        {
            var data = (from d in entities.sqlTMIMA_CHILDREN
                        where d.ΤΜΗΜΑ == tmimaId
                        orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ
                        select new sqlTmimaChildViewModel
                        {
                            CHILD_ID = d.CHILD_ID,
                            ΑΜ = d.ΑΜ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΟΝΟΜΑΤΕΠΩΝΥΜΟ = d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ,
                            ΠΑΤΡΩΝΥΜΟ = d.ΠΑΤΡΩΝΥΜΟ,
                            ΦΥΛΟ = d.ΦΥΛΟ,
                            ΗΛΙΚΙΑ = d.ΗΛΙΚΙΑ,
                            ΗΜΝΙΑ_ΕΓΓΡΑΦΗ = d.ΗΜΝΙΑ_ΕΓΓΡΑΦΗ
                        }).ToList();

            return (data);
        }

        public List<EducatorTmimaViewModel> GetEducators(int tmimaId)
        {
            var data = (from d in entities.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ
                        where d.ΤΜΗΜΑ_ΚΩΔ == tmimaId
                        orderby d.ΠΡΟΣΩΠΙΚΟ.ΕΠΩΝΥΜΟ, d.ΠΡΟΣΩΠΙΚΟ.ΟΝΟΜΑ
                        select new EducatorTmimaViewModel
                        {
                            RECORD_ID = d.RECORD_ID,
                            ΒΝΣ = d.ΒΝΣ,
                            ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ = d.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΤΜΗΜΑ_ΚΩΔ = d.ΤΜΗΜΑ_ΚΩΔ
                        }).ToList();

            return data;
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}