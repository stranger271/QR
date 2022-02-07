using System.Data.Entity;

namespace QR.Models
{
    public class Connect
    {     
        public class BaseDbContext : DbContext
        {             
            public BaseDbContext() : base("stat") { }
        }
    }
}