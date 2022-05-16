using System;
using System.Collections.Generic;
using System.Linq;

namespace KR
{
	public class DossierFactory
	{
		private readonly double T = 0;

		private readonly double LAMBDA = 0;

		private readonly double MU = 0;

		private readonly int M = 0;

		private ProcessingResult _processingResult;

		private Random _random = new Random();

		/// <summary>
		/// Конструктор фабрики заявок
		/// </summary>
		/// <param name="_T">Предельное время</param>
		public DossierFactory(double _T, double _lambda, double _mu, int _M)
		{
			T = _T;
			LAMBDA = _lambda;
			MU = _mu;
			M = _M;
		}

		public ProcessingResult StartProcessing()
		{
			_processingResult = new ProcessingResult();

			var processorStatus = ProseccorStatuses.NotReady;
			
			double lastTime = 0;
			bool isCompleted = false;

			double tLambda = GetExpRandom(LAMBDA);
			double tMu = double.PositiveInfinity;
			double t = GetEventTime(tLambda, tMu);
			var dossier = new Dossier(GetDossierStatus(t, tLambda, tMu));
			int queue = 0;

			while (dossier.Status != DossierStatuses.Failed)
			{
				switch (dossier.Status)
				{
					case DossierStatuses.InQueue:
						_processingResult.W1_inputDossierCount++;
						tLambda = t + GetExpRandom(LAMBDA);
						switch (processorStatus)
						{
							case ProseccorStatuses.Ready:
								queue = IncrementQueue(queue);
								break;
							case ProseccorStatuses.NotReady:
								processorStatus = ProseccorStatuses.Ready;
								tMu = t + GetExpRandom(MU);
								if(isCompleted)
								{
									isCompleted = false;
									_processingResult.T_downtime += t - lastTime;
								}
								break;
							default:
								break;
						}
						break;
					case DossierStatuses.Completed:
						isCompleted = true;
						lastTime = t;
						_processingResult.W2_processedDossierCount++;
						processorStatus = ProseccorStatuses.NotReady;
						tMu = CorrectTMu(queue, t);
						queue = DecrementQueue(queue, ref processorStatus);
						break;
					default:
						break;
				}
				t = GetEventTime(tLambda, tMu);
				dossier = new Dossier(GetDossierStatus(t, tLambda, tMu));
			}

			if(processorStatus == ProseccorStatuses.Ready)
			{
				_processingResult.W2_processedDossierCount++;
			}
			_processingResult.W3_failedDossierCount += queue;
			_processingResult.P1_probabilityProcessing = _processingResult.W2_processedDossierCount / _processingResult.W1_inputDossierCount;
			_processingResult.P2_probailityFailing = 1 - _processingResult.P1_probabilityProcessing;

			return _processingResult;
		}

		private int IncrementQueue(int queue)
		{
			if(queue < M)
			{
				queue++;
				return queue;
			}
			_processingResult.W3_failedDossierCount++;
			return queue;
		}

		private int DecrementQueue(int queue, ref ProseccorStatuses processorStatus)
		{
			if(queue > 0)
			{
				processorStatus = ProseccorStatuses.Ready;
				queue--;
				return queue;
			}
			return queue;
		}

		private double CorrectTMu(int queue, double t)
		{
			if(queue > 0)
			{
				return t + GetExpRandom(MU);
			}
			return double.PositiveInfinity;
		}

		/// <summary>
		/// Возвращает статус заявки (альфа)
		/// </summary>
		/// <param name="t">момент времени наступления события</param>
		/// <param name="tLambda">случайная величина от Лямбды</param>
		/// <param name="tMu">случайная величина от Мю</param>
		/// <returns></returns>
		private DossierStatuses GetDossierStatus(double t, double tLambda, double tMu)
		{
			if (t == T)
			{
				return DossierStatuses.Failed;
			}
			
			if (t == tMu)
			{
				return DossierStatuses.Completed;
			}
			
			if (t == tLambda)
			{
				return DossierStatuses.InQueue;
			}

			return DossierStatuses.None;
		}

		/// <summary>
		/// Возвращает момент времени наступления события
		/// </summary>
		/// <param name="t1Lambda">случайная величина от лямбда</param>
		/// <param name="t2Mu">случайная величина от Мю</param>
		/// <returns></returns>
		private double GetEventTime(double t1Lambda, double t2Mu)
			=> Enumerable.Min(new double[] { t1Lambda, t2Mu, T });

		/// <summary>
		/// Возвращает случайное число по закону экспоненциального распределения
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private double GetExpRandom(double value)
		{
			var randomX = _random.Next(0, 100);
			var power = -value * randomX;
			return value * Math.Pow(Math.E, power);
		}
	}
}
