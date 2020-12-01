using System.Windows.Input;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Fragments.Measuring.ViewModel.Validators;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Fragments.Measuring.ViewModel.Elements
{
	public class DateTimeMeasuringElementViewModel:MeasuringElementViewModelBase, IDateTimeMeasuringElementViewModel
	{
	    private string _date;
	    private string _time;
	    private string _bufferTime;
	    private string _bufferDate;

        private bool _isInEditMode;
	    private ILocalizerService _localizerService = StaticContainer.Container.Resolve<ILocalizerService>();
	    public override string StrongName => MeasuringKeys.DATE_TIME_ELEMENT +
		                                     ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

	    public string Date
	    {
	        get { return this._date; }
	        set
	        {
	            this._date = value;
	            RaisePropertyChanged(); FireErrorsChanged();
            }
	    }

	    public string Time
	    {
	        get { return this._time; }
	        set
	        {
	            this._time = value;
	            RaisePropertyChanged();
                FireErrorsChanged();
            }
	    }

	    public void SetDateTime(string date, string time)
	    {
	        this._bufferTime = time;
	        this._bufferDate = date;
	        if (!this.IsInEditMode)
	        {
	            this.Date = date;
	            this.Time = time;
	        }
	    }

	    public ICommand SetSystemDateTimeCommand { get; set; }
		public ICommand SetTimeCommand { get; set; }

	    public bool IsInEditMode
	    {
	        get { return this._isInEditMode; }
	        set
	        {
	            this._isInEditMode = value;
                RaisePropertyChanged();
	            if (!this.IsInEditMode)
	            {
	                this.Date = _bufferDate;
	                this.Time = _bufferTime;
                }
	            FireErrorsChanged();
            }
	    }

	    protected override void OnValidate()
	    {
            var res=new DateTimeViewModelValidator(_localizerService).Validate(this);
	        SetValidationErrors(res);
        }
	}
}

