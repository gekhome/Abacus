using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web;
using Abacus.DAL;


namespace Abacus.BPM
{
    public class Commit
    {
        DbUpdateConcurrencyException cex = new DbUpdateConcurrencyException();
        private System.Timers.Timer _delayTimer;
        public string CommitData(AbacusDBEntities db)
        {
            string emsg = "";
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException cex)
            {
                emsg += cex.Message;
                // delay N secs and try again
                delay(db, 2);
                return emsg;
            }
            catch (Exception ex)
            {
                emsg = "Προέκυψε γενικό σφάλμα κατά την αποθήκευση: " + "\n";
                emsg += ex.Message + "\n";
                emsg += "Επιστρέψτε στην προηγούμενη σελίδα και δοκιμάστε πάλι.";
            }
            return emsg;
        }

        private void delay(AbacusDBEntities db, int seconds)
        {
            _delayTimer = new System.Timers.Timer();
            _delayTimer.Interval = seconds * 1000;
            _delayTimer.Elapsed += (o, e) => db.SaveChanges();
            _delayTimer.Start();
        }

        private void _delayTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // do nothing
        }
    }
}