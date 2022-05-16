namespace KR
{
	public class Dossier
	{
		public Dossier(DossierStatuses status)
		{
			Status = status;
		}

		public DossierStatuses Status { get; set; }
	}
}
