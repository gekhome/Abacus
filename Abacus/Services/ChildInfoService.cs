using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class ChildInfoService : IDisposable
    {
        private AbacusDBEntities entities;

        public ChildInfoService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<sqlChildInfoViewModel> ReadGlobal(int activeId = 0)
        {
            var data = new List<sqlChildInfoViewModel>();

            if (activeId == 2)
            {
                data = (from d in entities.sqlCHILDREN_INFO
                        where d.ΕΝΕΡΓΟΣ == true
                        orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ
                        select new sqlChildInfoViewModel
                        {
                            CHILD_ID = d.CHILD_ID,
                            ΑΜ = d.ΑΜ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΠΕΡΙΦΕΡΕΙΑΚΗ = d.ΠΕΡΙΦΕΡΕΙΑΚΗ,
                            ΟΝΟΜΑΤΕΠΩΝΥΜΟ = d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ,
                            ΠΑΤΡΩΝΥΜΟ = d.ΠΑΤΡΩΝΥΜΟ,
                            ΜΗΤΡΩΝΥΜΟ = d.ΜΗΤΡΩΝΥΜΟ,
                            ΦΥΛΟ = d.ΦΥΛΟ,
                            ΗΜΝΙΑ_ΓΕΝΝΗΣΗ = d.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ,
                            ΗΛΙΚΙΑ = d.ΗΛΙΚΙΑ,
                            ΔΙΕΥΘΥΝΣΗ = d.ΔΙΕΥΘΥΝΣΗ,
                            ΤΗΛΕΦΩΝΑ = d.ΤΗΛΕΦΩΝΑ,
                            EMAIL = d.EMAIL,
                            ΕΝΕΡΓΟΣ = d.ΕΝΕΡΓΟΣ ?? false
                        }).ToList();
            }
            else
            {
                data = (from d in entities.sqlCHILDREN_INFO
                        orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ
                        select new sqlChildInfoViewModel
                        {
                            CHILD_ID = d.CHILD_ID,
                            ΑΜ = d.ΑΜ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΠΕΡΙΦΕΡΕΙΑΚΗ = d.ΠΕΡΙΦΕΡΕΙΑΚΗ,
                            ΟΝΟΜΑΤΕΠΩΝΥΜΟ = d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ,
                            ΠΑΤΡΩΝΥΜΟ = d.ΠΑΤΡΩΝΥΜΟ,
                            ΜΗΤΡΩΝΥΜΟ = d.ΜΗΤΡΩΝΥΜΟ,
                            ΦΥΛΟ = d.ΦΥΛΟ,
                            ΗΜΝΙΑ_ΓΕΝΝΗΣΗ = d.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ,
                            ΗΛΙΚΙΑ = d.ΗΛΙΚΙΑ,
                            ΔΙΕΥΘΥΝΣΗ = d.ΔΙΕΥΘΥΝΣΗ,
                            ΤΗΛΕΦΩΝΑ = d.ΤΗΛΕΦΩΝΑ,
                            EMAIL = d.EMAIL,
                            ΕΝΕΡΓΟΣ = d.ΕΝΕΡΓΟΣ ?? false
                        }).ToList();
            }
            return (data);
        }

        public IEnumerable<sqlChildInfoViewModel> ReadStation(int stationId, int activeId = 0)
        {
            var data = new List<sqlChildInfoViewModel>();

            if (activeId == 2)
            {
                data = (from d in entities.sqlCHILDREN_INFO
                        where d.ΣΤΑΘΜΟΣ_ΚΩΔ == stationId && d.ΕΝΕΡΓΟΣ == true
                        orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ
                        select new sqlChildInfoViewModel
                        {
                            CHILD_ID = d.CHILD_ID,
                            ΑΜ = d.ΑΜ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΠΕΡΙΦΕΡΕΙΑΚΗ = d.ΠΕΡΙΦΕΡΕΙΑΚΗ,
                            ΟΝΟΜΑΤΕΠΩΝΥΜΟ = d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ,
                            ΠΑΤΡΩΝΥΜΟ = d.ΠΑΤΡΩΝΥΜΟ,
                            ΜΗΤΡΩΝΥΜΟ = d.ΜΗΤΡΩΝΥΜΟ,
                            ΦΥΛΟ = d.ΦΥΛΟ,
                            ΗΜΝΙΑ_ΓΕΝΝΗΣΗ = d.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ,
                            ΗΛΙΚΙΑ = d.ΗΛΙΚΙΑ,
                            ΔΙΕΥΘΥΝΣΗ = d.ΔΙΕΥΘΥΝΣΗ,
                            ΤΗΛΕΦΩΝΑ = d.ΤΗΛΕΦΩΝΑ,
                            EMAIL = d.EMAIL,
                            ΕΝΕΡΓΟΣ = d.ΕΝΕΡΓΟΣ ?? false
                        }).ToList();
            }
            else
            {
                data = (from d in entities.sqlCHILDREN_INFO
                        where d.ΣΤΑΘΜΟΣ_ΚΩΔ == stationId
                        orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ
                        select new sqlChildInfoViewModel
                        {
                            CHILD_ID = d.CHILD_ID,
                            ΑΜ = d.ΑΜ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΠΕΡΙΦΕΡΕΙΑΚΗ = d.ΠΕΡΙΦΕΡΕΙΑΚΗ,
                            ΟΝΟΜΑΤΕΠΩΝΥΜΟ = d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ,
                            ΠΑΤΡΩΝΥΜΟ = d.ΠΑΤΡΩΝΥΜΟ,
                            ΜΗΤΡΩΝΥΜΟ = d.ΜΗΤΡΩΝΥΜΟ,
                            ΦΥΛΟ = d.ΦΥΛΟ,
                            ΗΜΝΙΑ_ΓΕΝΝΗΣΗ = d.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ,
                            ΗΛΙΚΙΑ = d.ΗΛΙΚΙΑ,
                            ΔΙΕΥΘΥΝΣΗ = d.ΔΙΕΥΘΥΝΣΗ,
                            ΤΗΛΕΦΩΝΑ = d.ΤΗΛΕΦΩΝΑ,
                            EMAIL = d.EMAIL,
                            ΕΝΕΡΓΟΣ = d.ΕΝΕΡΓΟΣ ?? false
                        }).ToList();
            }
            return (data);
        }

        public IEnumerable<sqlEgrafesInfoViewModel> GetEgrafes(int childId)
        {
            var data = (from d in entities.sqlEGRAFES_INFO
                        orderby d.ΗΜΝΙΑ_ΕΓΓΡΑΦΗ
                        where d.ΠΑΙΔΙ_ΚΩΔ == childId
                        select new sqlEgrafesInfoViewModel
                        {
                            ΕΓΓΡΑΦΗ_ΚΩΔ = d.ΕΓΓΡΑΦΗ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΠΑΙΔΙ_ΚΩΔ = d.ΠΑΙΔΙ_ΚΩΔ,
                            ΤΜΗΜΑ_ΟΝΟΜΑ = d.ΤΜΗΜΑ_ΟΝΟΜΑ,
                            ΗΜΝΙΑ_ΕΓΓΡΑΦΗ = d.ΗΜΝΙΑ_ΕΓΓΡΑΦΗ,
                            ΗΜΝΙΑ_ΠΕΡΑΣ = d.ΗΜΝΙΑ_ΠΕΡΑΣ
                        }).ToList();
            return (data);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}