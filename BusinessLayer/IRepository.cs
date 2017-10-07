namespace BusinessLayer
{
    public interface IRepository
	{
		int SaveSpeaker(Speaker speaker);
        int ObtenerRegistrationFee(int? yearExperience);

    }
}
