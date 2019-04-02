﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using MangaChecker.Data.Interfaces;
using MangaChecker.Providers.Interfaces;
using MangaChecker.ViewModels.ViewModels.Adding_ViewModels;

namespace MangaChecker.ViewModels.ViewModels
{
    public class AddMangaViewModel : INotifyPropertyChanged
    {
        public AddMangaViewModel(IEnumerable<IProvider> providerService, IDbContext dbContext)
        {
            NormalDataContext = new NormalViewModel();
            NormalCommand = new ActionCommand(() =>
            {
                if (TranIndex == 0)
                {
                    return;
                }
                TranIndex = 0;
                NormalDataContext = new NormalViewModel();
            });
            AdvancedCommand = new ActionCommand(() =>
            {
                if (TranIndex == 1)
                {
                    return;
                }
                TranIndex = 1;
                AdvancedDataContext = new AdvancedViewModel(providerService, dbContext);
            });
        }

        public int TranIndex { get; set; }

        public NormalViewModel NormalDataContext { get; set; }

        public AdvancedViewModel AdvancedDataContext { get; set; }

        public ICommand NormalCommand { get; }

        public ICommand AdvancedCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}