﻿using DevExpress.Blazor.DocumentMetadata;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Blazor {
    public class DemoPageConfiguration {
        public DemoPageConfiguration ParentPage { get; set; }

        public string Url { get; set; }
        public string Title { get; set; }
        public string TitleFormat { get; set; }
        public string Icon { get; set; }
        public string NavLinkText { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }


        public bool IsNew { get; set; }
        public bool IsUpdated { get; set; }

        public List<DemoPageConfiguration> DemoPages { get; set; } = new List<DemoPageConfiguration>();

        public string GetNavLinkText() => string.IsNullOrEmpty(NavLinkText) ? Title : NavLinkText;

        public string GetSeoTitle() => ParentPage == null ? Title : $"{ParentPage.GetSeoTitle()} - {Title}";
        public bool HasUpdates() => IsUpdated || DemoPages.Any(x => x.HasUpdates());
    }

    public class DemoConfiguration {
        public bool SiteMode { get; set; }
        public List<DemoPageConfiguration> DemoPages { get; set; } = new List<DemoPageConfiguration>();

        public void RegisterPagesMetadata(IDocumentMetadataRegistrator registrator) {
            registrator.Default()
                .Base("~/")
                .Charset("utf-8")
                .TitleFormat("Blazor: {0} | DevExpress")
                .Viewport("width=device-width, initial-scale=1.0")

                .OpenGraph("url", "https://dxpr.es/2WogLq7")
                .OpenGraph("type", "website")
                .OpenGraph("title", "Native Blazor Components powered by DevExpress")
                .OpenGraph("description", "Free DevExpress UI for Blazor ships with 11 user interface components (including a Data Grid, Pivot Grid and Scheduler) so you can design rich user experiences with both Blazor.")
                .OpenGraph("image", "https://static.devexpress.com/Products/Blazor/blazor-components-grid-pivot-scheduler-devexpress.jpg")

                .Meta("twitter:card", "summary")
                .Meta("twitter:site", "@@devexpress")

                .Script("highlight-js", "//cdnjs.cloudflare.com/ajax/libs/highlight.js/9.15.6/highlight.min.js", defer: false)
                .Script("demo-js", "~/lib/dx-demo.js", defer: false)
                .Script("dx-blazor-js", "~/lib/dx-blazor/dx-blazor.js", defer: false)

                .Script("facebook-jssdk", "https://connect.facebook.net/en_US/sdk.js#xfbml=1&version=v3.0", async: true)
                .Script("twitter-js", "https://platform.twitter.com/widgets.js", async: true)

                .StyleSheet("site-css", "~/css/site.css")
                .StyleSheet("highlight-css", "//cdnjs.cloudflare.com/ajax/libs/highlight.js/9.15.6/styles/default.min.css")
                .StyleSheet("demo-css", "~/css/dx-demo.css")
                .StyleSheet("dx-css", "~/lib/dx-blazor/dx-blazor.css")
                .StyleSheet(
                    name: "currentTheme",
                    styleSheetUrl: "css/switcher-resources/themes/pulse/bootstrap.min.css"
                );

            DemoPages.ForEach(pageMetadata => {
                RegisterPageMetadata(registrator, pageMetadata);
            });
        }

        void RegisterPageMetadata(IDocumentMetadataRegistrator registrator, DemoPageConfiguration pageMetadata) {
            if (pageMetadata.Url != null) {
                IDocumentMetadataBuilder metadataBuilder = registrator.Page(pageMetadata.Url);
                metadataBuilder.Title(pageMetadata.GetSeoTitle());
                if (!string.IsNullOrEmpty(pageMetadata.TitleFormat))
                    metadataBuilder.TitleFormat(pageMetadata.TitleFormat);
                if(!string.IsNullOrEmpty(pageMetadata.Keywords))
                    metadataBuilder.Meta("keywords", pageMetadata.Keywords);
                if(!string.IsNullOrEmpty(pageMetadata.Description))
                    metadataBuilder.Meta("description", pageMetadata.Description);
            }
            foreach (var childPageMetadata in pageMetadata.DemoPages) {
                childPageMetadata.ParentPage = pageMetadata;
                RegisterPageMetadata(registrator, childPageMetadata);
            }
        }
    }
}