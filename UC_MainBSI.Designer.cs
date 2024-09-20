namespace NM.BSI
{
    partial class UC_MainBSI
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_MainBSI));
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barBt_ElementareGefaehrdungNEU = new DevExpress.XtraBars.BarButtonItem();
            this.barBt_ElementareGefaehrdungDEL = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMainView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMainView.Panel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMainView.Panel2)).BeginInit();
            this.splitContainerMainView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollectionMainTree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControlMainDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerMainView
            // 
            this.splitContainerMainView.Size = new System.Drawing.Size(1158, 408);
            // 
            // imageCollectionMainTree
            // 
            this.imageCollectionMainTree.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollectionMainTree.ImageStream")));
            this.imageCollectionMainTree.InsertImage(global::NM.BSI.Properties.Resources.greenyellow_16x16, "greenyellow_16x16", typeof(global::NM.BSI.Properties.Resources), 0);
            this.imageCollectionMainTree.Images.SetKeyName(0, "greenyellow_16x16");
            this.imageCollectionMainTree.InsertImage(global::NM.BSI.Properties.Resources.open_16x16, "open_16x16", typeof(global::NM.BSI.Properties.Resources), 1);
            this.imageCollectionMainTree.Images.SetKeyName(1, "open_16x16");
            this.imageCollectionMainTree.InsertImage(global::NM.BSI.Properties.Resources.datavalidation_16x16, "datavalidation_16x16", typeof(global::NM.BSI.Properties.Resources), 2);
            this.imageCollectionMainTree.Images.SetKeyName(2, "datavalidation_16x16");
            // 
            // ribbonControlMainDetail
            // 
            this.ribbonControlMainDetail.ExpandCollapseItem.Id = 0;
            this.ribbonControlMainDetail.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barBt_ElementareGefaehrdungNEU,
            this.barBt_ElementareGefaehrdungDEL});
            this.ribbonControlMainDetail.MaxItemId = 8;
            this.ribbonControlMainDetail.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControlMainDetail.Size = new System.Drawing.Size(1162, 150);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "BSI Standard";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barBt_ElementareGefaehrdungNEU);
            this.ribbonPageGroup1.ItemLinks.Add(this.barBt_ElementareGefaehrdungDEL);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Risikomanagement";
            // 
            // barBt_ElementareGefaehrdungNEU
            // 
            this.barBt_ElementareGefaehrdungNEU.Caption = "Neu Elementare Gefährdung";
            this.barBt_ElementareGefaehrdungNEU.Id = 6;
            this.barBt_ElementareGefaehrdungNEU.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barBt_ElementareGefaehrdungNEU.ImageOptions.LargeImage")));
            this.barBt_ElementareGefaehrdungNEU.Name = "barBt_ElementareGefaehrdungNEU";
            this.barBt_ElementareGefaehrdungNEU.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBt_ElementareGefaehrdungNEU_ItemClick);
            // 
            // barBt_ElementareGefaehrdungDEL
            // 
            this.barBt_ElementareGefaehrdungDEL.Caption = "Löschen Elementare Gefährdung";
            this.barBt_ElementareGefaehrdungDEL.Id = 7;
            this.barBt_ElementareGefaehrdungDEL.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barBt_ElementareGefaehrdungDEL.ImageOptions.LargeImage")));
            this.barBt_ElementareGefaehrdungDEL.Name = "barBt_ElementareGefaehrdungDEL";
            this.barBt_ElementareGefaehrdungDEL.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBt_ElementareGefaehrdungDEL_ItemClick);
            // 
            // UC_MainBSI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UC_MainBSI";
            this.Size = new System.Drawing.Size(1162, 583);
            this.IsMainTreeIDChanged += new accANCollection.UserControls.Basis.SelMainTreeIDChange(this.UC_BSIStandard_IsMainTreeIDChanged);
            this.IsSelSelMainTreelTyp_IDChanged += new accANCollection.UserControls.Basis.SelMainTreelTyp_IDChange(this.UC_BSIStandard_IsSelSelMainTreelTyp_IDChange);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMainView.Panel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMainView.Panel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMainView)).EndInit();
            this.splitContainerMainView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageCollectionMainTree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControlMainDetail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem barBt_ElementareGefaehrdungNEU;
        private DevExpress.XtraBars.BarButtonItem barBt_ElementareGefaehrdungDEL;
    }
}
