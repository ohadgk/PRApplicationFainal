using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRApplication.Dal
{
    public class DbInit : DropCreateDatabaseIfModelChanges<PRApplicationDBContext>
    {
        protected override void Seed(PRApplicationDBContext context)
        {
            base.Seed(context);
        }

    }
}
