// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;

namespace PasswordManager.Infrastructure.Flyouts
{
    public interface IFlyoutViewModel
    {
        Action GoBack { get; set; }
        Action CloseFlyout { get; set; }
        void Open(object parameter, Action successAction);
    }
}
