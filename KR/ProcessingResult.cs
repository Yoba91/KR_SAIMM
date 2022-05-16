namespace KR
{
	public class ProcessingResult
	{
		/// <summary>
		/// число заявок, поступивших в систему
		/// </summary>
		public double W1_inputDossierCount { get; set; } = 0;

		/// <summary>
		/// число заявок, обслуженных системой
		/// </summary>
		public double W2_processedDossierCount { get; set; } = 0;

		/// <summary>
		/// число потерянных заявок
		/// </summary>
		public double W3_failedDossierCount { get; set; } = 0;

		/// <summary>
		/// вероятность обслуживания пакета
		/// </summary>
		public double P1_probabilityProcessing { get; set; } = 0;

		/// <summary>
		/// вероятность отказа
		/// </summary>
		public double P2_probailityFailing { get; set; } = 0;

		/// <summary>
		/// время простоя вычислительной системы
		/// </summary>
		public double T_downtime { get; set; } = 0;
	}
}
