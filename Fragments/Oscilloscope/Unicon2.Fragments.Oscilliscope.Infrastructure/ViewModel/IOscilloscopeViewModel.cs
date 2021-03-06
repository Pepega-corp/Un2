﻿using System.Collections.Generic;
using System.Windows.Input;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.Oscilliscope.Infrastructure.ViewModel
{
    public interface IOscilloscopeViewModel : IFragmentViewModel, IDeviceContextConsumer
    {
        IOscilloscopeJournalViewModel OscilloscopeJournalViewModel { get; set; }
        ICommand LoadSelectedOscillogramsCommand { get; }
        int MaxLoadingProgress { get; set; }
        int CurrentLoadingProgress { get; set; }
        ICommand ShowOscillogramCommand { get; }
        ICommand StopLoadingCommand { get; }
        List<Oscillogram> Oscillograms { get; }


    }
}