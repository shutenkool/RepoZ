﻿using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;
using RepoZ.UI.Mac.Story.Model;
using RepoZ.Api.Git;

namespace RepoZ.UI.Mac.Story
{
    public partial class PopupViewController : AppKit.NSViewController
    {
        private IRepositoryInformationAggregator _aggregator;
        #region Constructors

        // Called when created from unmanaged code
        public PopupViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public PopupViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public PopupViewController() : base("PopupView", NSBundle.MainBundle)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        #endregion

        public override void ViewWillAppear()
        {
            base.ViewWillAppear();

            if (_aggregator != null)
                return;

            _aggregator = TinyIoC.TinyIoCContainer.Current.Resolve<IRepositoryInformationAggregator>();

            _aggregator.Repositories.Add(new RepositoryView(new Repository() { Name = "RepoZ", CurrentBranch = "Fix-Mac", AheadBy = 3, BehindBy = 1, Path = "/source/RepoZ" }));
            _aggregator.Repositories.Add(new RepositoryView(new Repository() { Name = "PoshX", CurrentBranch = "master", AheadBy = 0, BehindBy = 0, Path = "/source/PoshX" }));

            // Do any additional setup after loading the view.
            var datasource = new RepositoryTableDataSource(_aggregator.Repositories);
            RepoTab.DataSource = datasource;
            RepoTab.Delegate = new RepositoryTableDelegate(RepoTab, datasource);

            RepoTab.BackgroundColor = NSColor.Clear;
            RepoTab.EnclosingScrollView.DrawsBackground = false;
        }

        public new PopupView View => (PopupView)base.View;
    }
}
