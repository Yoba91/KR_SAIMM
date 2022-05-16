using System;
using System.Collections.Generic;
using System.Linq;

namespace KR
{
	public class DispersionDossiers
	{
		private List<ProcessingResult> _results;

		public DispersionDossiers(List<ProcessingResult> results)
		{
			_results = results;
		}
		/// <summary>
		/// число заявок, поступивших в систему
		/// </summary>
		public double W1_inputDossierCount => GetDispersion(x => x.W1_inputDossierCount);

		/// <summary>
		/// число заявок, обслуженных системой
		/// </summary>
		public double W2_processedDossierCount => GetDispersion(x => x.W2_processedDossierCount);

		/// <summary>
		/// число потерянных заявок
		/// </summary>
		public double W3_failedDossierCount => GetDispersion(x => x.W3_failedDossierCount);

		/// <summary>
		/// вероятность обслуживания пакета
		/// </summary>
		public double P1_probabilityProcessing => GetDispersion(x => x.P1_probabilityProcessing);

		/// <summary>
		/// вероятность отказа
		/// </summary>
		public double P2_probailityFailing => GetDispersion(x => x.P2_probailityFailing);

		/// <summary>
		/// время простоя вычислительной системы
		/// </summary>
		public double T_downtime => GetDispersion(x => x.T_downtime);

		private double GetDispersion(Func<ProcessingResult, double> property)
		{
			var averange = _results.Sum(property) / _results.Count;
			var collection = _results.Select(property).ToList();
			double dispersion = 0;
			foreach (var result in collection)
			{
				dispersion += Math.Pow(averange - result, 2);
			}
			return dispersion;
		}
	}
}
