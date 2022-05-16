using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace KR
{
	public class BaseViewModel : INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string caller = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(caller));
			}
		}
	}

	public class MainViewModel : BaseViewModel
	{
		private const double LAMBDA = 0.29d;

		private const double MU = 0.23d;

		private const int T_CONST = 275;

		private const int M_CONST = 20;

		private const int COUNT_REPEAT = 95;

		private double _lambda;
		private double _mu;
		private int _t;
		private int _m;
		private int _countRepeat;

		private RelayCommand startCommand;

		public MainViewModel()
		{
			startCommand = new RelayCommand(StartProcess);
			ProcessingResults = new ObservableCollection<ProcessingResult>();
			DispersionResults = new ObservableCollection<DispersionDossiers>();
			AverangeResults = new ObservableCollection<AverangeDossiers>();

			Lambda = LAMBDA;
			Mu = MU;
			T = T_CONST;
			M = M_CONST;
			CountRepeat = COUNT_REPEAT;
		}


		public double Lambda
		{
			get { return _lambda; }
			set { _lambda = value; OnPropertyChanged(nameof(Lambda)); }
		}

		public double Mu
		{
			get { return _mu; }
			set { _mu = value; OnPropertyChanged(nameof(Mu)); }
		}

		public int T
		{
			get { return _t; }
			set { _t = value; OnPropertyChanged(nameof(T)); }
		}

		public int M
		{
			get { return _m; }
			set { _m = value; OnPropertyChanged(nameof(M)); }
		}

		public int CountRepeat
		{
			get { return _countRepeat; }
			set { _countRepeat = value; OnPropertyChanged(nameof(CountRepeat)); }
		}

		public RelayCommand StartCommand
		{
			get { return startCommand; }
		}

		public ObservableCollection<ProcessingResult> ProcessingResults { get; set; }
		public ObservableCollection<DispersionDossiers> DispersionResults { get; set; }
		public ObservableCollection<AverangeDossiers> AverangeResults { get; set; }

		private AverangeDossiers _averange;

		public AverangeDossiers Averange
		{
			get { return _averange; }
			set { _averange = value; OnPropertyChanged(nameof(Averange)); }
		}

		private DispersionDossiers _dispersion;

		public DispersionDossiers Dispersion
		{
			get { return _dispersion; }
			set { _dispersion = value; OnPropertyChanged(nameof(Dispersion)); }
		}

		private void StartProcess(object inputParam)
		{
			var factory = new DossierFactory(T, Lambda, Mu, M);
			ProcessingResults.Clear();
			for (int i = 0; i < CountRepeat; i++)
			{
				ProcessingResults.Add(factory.StartProcessing());
			}
			AverangeResults.Clear();
			AverangeResults.Add(new AverangeDossiers(ProcessingResults.ToList()));
			DispersionResults.Clear();
			DispersionResults.Add(new DispersionDossiers(ProcessingResults.ToList()));
		}
	}
}
