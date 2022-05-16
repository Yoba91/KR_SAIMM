using System;
using System.Collections.Generic;
using System.Linq;

namespace KR
{
	public class AverangeDossiers
	{
		private List<ProcessingResult> _results;

		public AverangeDossiers(List<ProcessingResult> results)
		{
			_results = results;
		}
		/// <summary>
		/// число заявок, поступивших в систему
		/// </summary>
		public double W1_inputDossierCount => GetAverange(x => x.W1_inputDossierCount);

		/// <summary>
		/// число заявок, обслуженных системой
		/// </summary>
		public double W2_processedDossierCount => GetAverange(x => x.W2_processedDossierCount);

		/// <summary>
		/// число потерянных заявок
		/// </summary>
		public double W3_failedDossierCount => GetAverange(x => x.W3_failedDossierCount);

		/// <summary>
		/// вероятность обслуживания пакета
		/// </summary>
		public double P1_probabilityProcessing => GetAverange(x => x.P1_probabilityProcessing);

		/// <summary>
		/// вероятность отказа
		/// </summary>
		public double P2_probailityFailing => GetAverange(x => x.P2_probailityFailing);

		/// <summary>
		/// время простоя вычислительной системы
		/// </summary>
		public double T_downtime => GetAverange(x => x.T_downtime);

		private double GetAverange(Func<ProcessingResult, double> property)
		{
			return _results.Sum(property) / _results.Count;
		}
	}
}
