using eyemusic45.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace eyemusic45.DAL
{
    public class allData : DbContext
    {

        public allData()
            : base("allData")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}