﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WarehouseSystem.Utilities;

internal class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {

        bool.TryParse(value.ToString(), out bool boolValue);

        return boolValue ? Visibility.Visible : Visibility.Hidden;
    }

    public object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        return new object();
    }
}