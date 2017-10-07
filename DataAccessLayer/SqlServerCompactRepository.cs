using BusinessLayer;

namespace DataAccessLayer
{
    public class SqlServerCompactRepository : IRepository
	{
		public int SaveSpeaker(Speaker speaker)
		{
			//TODO: Save speaker to DB for now. For demo, just assume success and return 1.
			return 1;
		}
        public int ObtenerRegistrationFee(int? yearExperience)
        {
            /*
             Se trabajaría con una tabla que tenga 3 campos
             id  MaxExperiencia   RegistrationFee
             1     1                500
             2     3                250
             3     5                100
             4     9                50
            
             */
            return 50;
        }
        
    }
}
