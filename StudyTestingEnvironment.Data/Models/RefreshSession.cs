using System;

namespace StudyTestingEnvironment.Data.Models
{
    public class RefreshSession
    {                
        public Guid Id { get; set; }
        
        public string UserAgent { get; set; }

        public string FingerPrint { get; set; }

        public string IpAddress { get; set; }

        public DateTime ExpiresInUTC { get; set; }

        public DateTime CreateAtUTC { get; set; } = DateTime.UtcNow;

        public override bool Equals(object obj)
        {
            if (obj is RefreshSession session) 
                return Id.Equals(session.Id);
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}