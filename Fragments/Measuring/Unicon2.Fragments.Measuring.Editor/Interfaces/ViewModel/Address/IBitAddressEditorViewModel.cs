﻿using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Address
{
    public interface IBitAddressEditorViewModel : IStronglyNamed
    {
        int FunctionNumber { get; set; }
        ushort Address { get; set; }
        bool IsBitNumberInWordActual { get; set; }
        ushort BitNumberInWord { get; set; }

    }
}