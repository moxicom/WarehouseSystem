﻿using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Input;
using WarehouseSystem.Enums;
using WarehouseSystem.Models;
using WarehouseSystem.Utilities;

namespace WarehouseSystem.ViewModels
{
    internal class ItemDialogVM : BaseViewModel
    {

        // Fields
        private string _pageTitle;
        private string _title;
        private string _description;
        private int _amount;
        private bool _isAmountVisible;

        public event EventHandler<ItemDialogData>? DialogClosing; // item or category

        public ItemDialogVM(ItemDialogType type, ItemDialogMode mode)
        {
            _pageTitle = string.Empty;
            _title = string.Empty;
            _description = string.Empty;
            OkButtonCommand = new RelayCommand(OkButtonClickHandler);
            CancelButtonCommand = new RelayCommand(CancelButtonClickHandler);
            ShowCorrectProperties(type, mode);
        }

        // Parameters
        public ICommand OkButtonCommand { get; }
        public ICommand CancelButtonCommand { get; }

        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                _pageTitle = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public int Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged();
            }
        }

        public bool IsAmountVisible
        {
            get => _isAmountVisible;
            set
            {
                _isAmountVisible = value;
                OnPropertyChanged();
            }
        }

        // Methods
        private void OkButtonClickHandler()
        {
            var data = new ItemDialogData
            {
                Title = _title,
                Description = _description,
                Amount = _amount,
            };
            DialogClosing?.Invoke(this, data);
        }

        private void CancelButtonClickHandler()
        {
            DialogClosing?.Invoke(this, null);
        }

        private void ShowCorrectProperties(ItemDialogType type, ItemDialogMode mode)
        {
            if (type == ItemDialogType.Item)
            {
                IsAmountVisible = true;
                if (mode == ItemDialogMode.Insert)
                    PageTitle = "Добавление товара";
                if (mode == ItemDialogMode.Update)
                    PageTitle = "Обновление товара";
            }
            else
            {
                IsAmountVisible = false;
                if (mode == ItemDialogMode.Insert)
                    PageTitle = "Добавление категории";
                if (mode == ItemDialogMode.Update)
                    PageTitle = "Обновление категории";
            }
        }
    }
}
