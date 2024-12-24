using accANCollection.Daten;
using accANCollection.Daten.NMBSI;
using accANCollection.Klassen;
using accANCollection.UserControls.Basis;
using accANCollection.UserControls.Basis_Segmente;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NM.BSI.UC
{
    public partial class UC_GB_DetailGeneral : Basis_UC_DetailGenerallyView
    {
        //Logging
        //private Logging logging;
        //private bool isLogEnabled = false;
        private string alternativDateiNameWithoutExtensions = "Log_UC_GB_DetailGeneral";

        //Daten
        TBL_AN_BSIStandard_MainTree SessionTBL_AN_BSIStandard_MainTree  { get; set; }
        DA_NMBSI DA_NMBSI;

        /// <summary>
        /// ID des UC Datensatzes
        /// </summary>
        private int SelMainBlockID { get; set; }

        //UserControls
        Basis_Segment_UC_Description Basis_Segment_UC_AllgDetail { get; set; }

        public UC_GB_DetailGeneral(UCParameter uCParameter) : base(uCParameter)
        {
            InitializeComponent();
            this.SelLockID = 0;
        }

        public UC_GB_DetailGeneral() : base()
        {
            InitializeComponent();
        }

        #region Init

        public override void InitData()
        {
            DA_NMBSI = new DA_NMBSI(UCParameter);
        }


        public override void InitDisplay()
        {
            base.InitDisplay();
            bool newDS = false;
            SessionTBL_AN_BSIStandard_MainTree = new TBL_AN_BSIStandard_MainTree();
            InitData();
            SessionTBL_AN_BSIStandard_MainTree = DA_NMBSI.GetBSIStandard_MainTree(SelMainTreelID);
            if (SessionTBL_AN_BSIStandard_MainTree == null)
            {
                //Fehler
                XtraMessageBox.Show("Es konnte kein Eintrag geladen werden. Fehler beim Laden der Datailansicht.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (isLogEnabled) UCParameter.Logging.SetLogItem("InitDisplay - Fehler : Es konnte kein Eintrag geladen werden. Fehler beim Laden der Datailansicht.", 1, alternativDateiNameWithoutExtensions);

                if (this.Parent != null)
                {
                    this.Parent.Controls.Remove(this);
                }
                this.Dispose();

            }
            lControlHeader.Text = SessionTBL_AN_BSIStandard_MainTree.MainTree_Bez;
            lControlDetail.Text = SessionTBL_AN_BSIStandard_MainTree.MainTree_BezLang;
            LControlCSBNR.Visible = false;
            LControlLabelCSBNummer.Visible = false;
            LControlCSBNR.Text = string.Empty;
           
            //Laden RichEditor Beschreibung Allgemein

            TCMain.TabPages.Clear();
            //Tabs laden

            ////ProzessDiagram
            //XtraTabPage tabProzessDiagrammNavigationPage = new XtraTabPage();
            //Basis_Segment_UC_ProzessDiagram = new Basis_Segment_UC_ProzessDiagram(UCParameter);
            //tabProzessDiagrammNavigationPage.Text = "Prozessdiagramm";
            //tabProzessDiagrammNavigationPage.Controls.Add(Basis_Segment_UC_ProzessDiagram);
            //TCMain.TabPages.Add(tabProzessDiagrammNavigationPage);
            //Allgemein
            XtraTabPage tabAllGNavigationPage = new XtraTabPage();
            Basis_Segment_UC_AllgDetail = new Basis_Segment_UC_Description(UCParameter);
            tabAllGNavigationPage.Text = "Elementare Gefährdung";
            tabAllGNavigationPage.Controls.Add(Basis_Segment_UC_AllgDetail);
            TCMain.TabPages.Add(tabAllGNavigationPage);

            //ProzessGruppe
            //Prüfen noch, ob ein Dokument bereits vorhanden ist
            if (SessionTBL_AN_BSIStandard_MainTree.TBL_AN_BSIStandard_MainBlock != null)
            {
                SelMainBlockID = SessionTBL_AN_BSIStandard_MainTree.TBL_AN_BSIStandard_MainBlock.MainBlockID;
                //uC_AllgProzessBaustein.BeschreibungRTF = sessiontBL_AN_CSBProzesse_MainTree.TBL_AN_CSBProzesse_MainBlock.MainBlockDescription;
                Basis_Segment_UC_AllgDetail.richEditControlBeschreibungALLgemein.RtfText = SessionTBL_AN_BSIStandard_MainTree.TBL_AN_BSIStandard_MainBlock.MainBlockDescription;
            }
            else
            {
                //Kein Eintrag in der Datenbank vorhanden. Defaultwerte laden
                SelMainBlockID = 0;
                newDS = true;
                try
                {
                    //if (SessiontBL_AN_CSBProzesse_MainTree.MainTree_Typ_ID == 2)
                    //{
                    //    Basis_Segment_UC_AllgDetail.richEditControlBeschreibungALLgemein.LoadDocument(Directory.GetCurrentDirectory() + Properties.Settings.Default.DokuVorlage_ProzessGruppe);
                    //}
                    //if (SessiontBL_AN_CSBProzesse_MainTree.MainTree_Typ_ID == 3)
                    //{
                    //    Basis_Segment_UC_AllgDetail.richEditControlBeschreibungALLgemein.LoadDocument(Directory.GetCurrentDirectory() + Properties.Settings.Default.DokuVorlageBausteinCSB);
                    //}
                    //if (SessiontBL_AN_CSBProzesse_MainTree.MainTree_Typ_ID == 4)
                    //{
                    //    Basis_Segment_UC_AllgDetail.richEditControlBeschreibungALLgemein.LoadDocument(Directory.GetCurrentDirectory() + Properties.Settings.Default.DokuVorlageBausteinIT);
                    //}
                    //if (SessiontBL_AN_CSBProzesse_MainTree.MainTree_Typ_ID == 7)
                    //{
                    //    Basis_Segment_UC_AllgDetail.richEditControlBeschreibungALLgemein.LoadDocument(Directory.GetCurrentDirectory() + Properties.Settings.Default.DokuVorlageProzessbausteinUNI);
                    //}
                }
                catch (Exception ex)
                {
                    if (isLogEnabled) UCParameter.Logging.SetLogItem("InitDisplay - Fehler : " + ex.Message, 1, alternativDateiNameWithoutExtensions);

                }

            }

            //Dieser Part ist die neue Varinate. UNI Display + evtl Ausprägung wie IT poder CSB oder weitren Registerkarten
            //IQueryable<TBL_AN_ITProjekt_ITInventarBlock1> tBL_AN_ITProjekt_ITInventarBlock1s =  DA_CSBDatenModell.GetITInventarBlock(UCParameter.SelMainTreeID);
            //IQueryable<TBL_AN_UNIProzesse_MainBlock> tBL_AN_UNIProzesse_MainBlocks = DA_CSBDatenModell.Get_MainBlock(UCParameter.SelMainTreeID);
            //if (tBL_AN_UNIProzesse_MainBlocks != null)
            //{
            //    foreach (var item in tBL_AN_UNIProzesse_MainBlocks)
            //    {
            //        if (item != null)
            //        {

            //            if (item.TBL_AN_UNIProzesse_Ref_Expressions != null)
            //            {
            //                if (item.TBL_AN_UNIProzesse_Ref_Expressions.TBL_AN_ITProjekt_ITInventarBlock1 != null && item.TBL_AN_UNIProzesse_Ref_Expressions.RefTypID == 4)
            //                {
            //                    //Register IT Infrastruktur vorhanden
            //                    UCParameter.DocuSnapParameter.SelHostID = item.TBL_AN_UNIProzesse_Ref_Expressions.TBL_AN_ITProjekt_ITInventarBlock1.BlockITItemID;
            //                    UCParameter.DocuSnapParameter.SelTypID = (int)item.TBL_AN_UNIProzesse_Ref_Expressions.TBL_AN_ITProjekt_ITInventarBlock1.BlockITTypID;
            //                    Basis_UC_DetailITInventar = new Basis_UC_DetailITInventar(UCParameter, item.TBL_AN_UNIProzesse_Ref_Expressions.TBL_AN_ITProjekt_ITInventarBlock1.BlockITID);
            //                    Basis_UC_DetailITInventar.SessionLock = true;
            //                    Basis_UC_DetailITInventar.SetButtonSystemChangeVisible = true;
            //                    //uC_ITProzessBaustein = new UC_ITProzessBaustein(SessiontBL_AN_CSBProzesse_MainTree, AppConnectString, logInUser, logging);
            //                    XtraTabPage tabITNavigationPage = new XtraTabPage();
            //                    tabITNavigationPage.Text = "IT Infrastruktur";
            //                    tabITNavigationPage.Controls.Add(Basis_UC_DetailITInventar);
            //                    xtraTabControlMain.TabPages.Add(tabITNavigationPage);
            //                }
            //                if (item.TBL_AN_UNIProzesse_Ref_Expressions.TBL_AN_BlockCSB != null && item.TBL_AN_UNIProzesse_Ref_Expressions.RefTypID == 3)
            //                {
            //                    //Register CSB vorhanden
            //                    XtraTabPage tabCSBNavigationPage = new XtraTabPage();
            //                    //Basis_UC_DetailCSB = new Basis_UC_DetailCSB(UCParameter); // = new UC_CSBProzessBaustein(SessiontBL_AN_CSBProzesse_MainTree, AppConnectString, logInUser, logging);
            //                    UC_DetailCSB = new UC_DetailCSB(UCParameter);
            //                    tabCSBNavigationPage.Text = "CSB";
            //                    tabCSBNavigationPage.Controls.Add(UC_DetailCSB);
            //                    xtraTabControlMain.TabPages.Add(tabCSBNavigationPage);
            //                }

            //            }
            //        }
            //    }

            //}



            //Dieser Part ist die alte Variante mit selektiven Einträgen

            ////Zweite Registerkarte abhängig vom Datentyp 
            ////IT Infrastruktur
            //if (SessiontBL_AN_CSBProzesse_MainTree.MainTree_Typ_ID == 4)
            //{
            //    XtraTabPage tabITNavigationPage = new XtraTabPage();
            //    int tmpDSID = 0;
            //    if (SessiontBL_AN_CSBProzesse_MainTree != null)
            //    {
            //        if (SessiontBL_AN_CSBProzesse_MainTree.TBL_AN_CSBProzesse_MainBlock != null)
            //        {
            //            if (SessiontBL_AN_CSBProzesse_MainTree.TBL_AN_CSBProzesse_MainBlock.TBL_AN_UNIProzesse_Ref_Expressions != null)
            //            {
            //                if (SessiontBL_AN_CSBProzesse_MainTree.TBL_AN_CSBProzesse_MainBlock.TBL_AN_UNIProzesse_Ref_Expressions.TBL_AN_ITProjekt_ITInventarBlock1 != null)
            //                {
            //                    if (SessiontBL_AN_CSBProzesse_MainTree.TBL_AN_CSBProzesse_MainBlock.TBL_AN_UNIProzesse_Ref_Expressions.RefTypID == 4)
            //                    {
            //                        UCParameter.DocuSnapParameter.SelHostID = SessiontBL_AN_CSBProzesse_MainTree.TBL_AN_CSBProzesse_MainBlock.TBL_AN_UNIProzesse_Ref_Expressions.TBL_AN_ITProjekt_ITInventarBlock1.BlockITItemID;
            //                        UCParameter.DocuSnapParameter.SelTypID = (int)SessiontBL_AN_CSBProzesse_MainTree.TBL_AN_CSBProzesse_MainBlock.TBL_AN_UNIProzesse_Ref_Expressions.TBL_AN_ITProjekt_ITInventarBlock1.BlockITTypID;

            //                        tmpDSID = SessiontBL_AN_CSBProzesse_MainTree.TBL_AN_CSBProzesse_MainBlock.TBL_AN_UNIProzesse_Ref_Expressions.TBL_AN_ITProjekt_ITInventarBlock1.BlockITID;

            //                    }
            //                }
            //            }
            //        }
            //    }
            //    if (tmpDSID == 0 )
            //    {
            //        //Kein Datensatz verküpft
            //        UCParameter.DocuSnapParameter.SelHostID = String.Empty;
            //        UCParameter.DocuSnapParameter.SelTypID = 0;
            //    }
            //    Basis_UC_DetailITInventar = new Basis_UC_DetailITInventar(UCParameter, tmpDSID);
            //    Basis_UC_DetailITInventar.SessionLock = true;
            //    Basis_UC_DetailITInventar.SetButtonSystemChangeVisible = true;
            //    //uC_ITProzessBaustein = new UC_ITProzessBaustein(SessiontBL_AN_CSBProzesse_MainTree, AppConnectString, logInUser, logging);
            //    tabITNavigationPage.Text = "IT Infrastruktur";
            //    tabITNavigationPage.Controls.Add(Basis_UC_DetailITInventar);
            //    xtraTabControlMain.TabPages.Add(tabITNavigationPage);
            //}

            ////Zweite Registerkarte abhängig vom Datentyp 
            ////CSB
            //if (SessiontBL_AN_CSBProzesse_MainTree.MainTree_Typ_ID == 3)
            //{
            //    XtraTabPage tabCSBNavigationPage = new XtraTabPage();
            //    //Basis_UC_DetailCSB = new Basis_UC_DetailCSB(UCParameter); // = new UC_CSBProzessBaustein(SessiontBL_AN_CSBProzesse_MainTree, AppConnectString, logInUser, logging);
            //    UC_DetailCSB = new UC_DetailCSB(UCParameter);
            //    tabCSBNavigationPage.Text = "CSB";
            //    tabCSBNavigationPage.Controls.Add(UC_DetailCSB);
            //    xtraTabControlMain.TabPages.Add(tabCSBNavigationPage);
            //}
            //Dritte Registerkarte
            //Filemanger
            if (SessionTBL_AN_BSIStandard_MainTree.MainTree_Typ_ID == 2)
            {
                //Filemanager
                //XtraTabPage tabITNavigationFileManager = new XtraTabPage();
                //Basis_Segment_UC_FileManager = new Basis_Segment_UC_FileManager(UCParameter);
                //tabITNavigationFileManager.Text = "Dokumente";
                //tabITNavigationFileManager.Controls.Add(Basis_Segment_UC_FileManager);
                //TCMain.TabPages.Add(tabITNavigationFileManager);
            }
            //Vierte Registerkarte
            //Änderungsnachverfolgung
            XtraTabPage tabAenderungNavigationPage = new XtraTabPage();
            Basis_UC_Aenderungsverfolgung basis_UC_Aenderungsverfolgung = new Basis_UC_Aenderungsverfolgung(UCParameter, 1);
            tabAenderungNavigationPage.Text = "Änderungsnachverfolgung";
            tabAenderungNavigationPage.Controls.Add(basis_UC_Aenderungsverfolgung);
            TCMain.TabPages.Add(tabAenderungNavigationPage);


            //Anzeige
            DisplaySplitterPosition = Properties.Settings.Default.appSettingUCDetailSplitterPOS;
            //ribbonControl1.MdiMergeStyle = RibbonMdiMergeStyle.Always;

            //Property
            UCParameter.IsEditOn = false;
            LoadLastChangeFoot(DA_NMBSI.GetBSIStandard_EditListItemLastChange(UCParameter.SelMainTreeID));
            //Neuer Datensatz ist das erste Mal geöffnet. Nun wird er in der Datenbank angelegt
            if (newDS)
            {
                SaveContent();

            }
        }



        #endregion

        #region "Unterstützende Methoden"

        private void SetEdit()
        {
            if (Basis_Segment_UC_AllgDetail != null)
            {
                //Basis_Segment_UC_AllgDetail.IsEditOn = UCParameter.IsEditOn;
            }
          

        }

        public override void EditAll(object sender)
        {
            base.EditAll(sender);
            SetEdit();
        }

        public override void SaveAll()
        {
            base.SaveAll();
            SetEdit();
        }


        public override int SaveEditList(string tempPrgCommit, string tempUserCommit)
        {
            int tempDSID;
            tempDSID = base.SaveEditList(tempPrgCommit, tempUserCommit);
            TBL_AN_BSIStandard_EditList tBL_AN_BSIStandard_EditList = new TBL_AN_BSIStandard_EditList();
            tBL_AN_BSIStandard_EditList.EditList_MainTreeID = SelMainTreelID;
            tBL_AN_BSIStandard_EditList.EditList_PrgCommit = tempPrgCommit;
            tBL_AN_BSIStandard_EditList.EditList_UserCommit = tempUserCommit;
            tBL_AN_BSIStandard_EditList.EditList_Create = DateTime.Now;
            tBL_AN_BSIStandard_EditList.EditList_LastChange = DateTime.Now;
            tBL_AN_BSIStandard_EditList.EditList_UserName = UCParameter.MemberAndGroups.Member.DisplayName;
            tBL_AN_BSIStandard_EditList.EditList_UserStrSID = UCParameter.MemberAndGroups.Member.SID.ToString();
            tempDSID = DA_NMBSI.SetBSiStandard_EditListItem(tBL_AN_BSIStandard_EditList);
            LoadLastChangeFoot(DA_NMBSI.GetBSIStandard_EditListItemLastChange(UCParameter.SelMainTreeID));
            return tempDSID;
            //TBL_AN_ITProjekt_EditList tBL_AN_ITProjekt_EditList = new TBL_AN_ITProjekt_EditList();
            //tBL_AN_ITProjekt_EditList.EditList_MainTreeID = SelMainTreelID;
            //tBL_AN_ITProjekt_EditList.EditList_PrgCommit = tempPrgCommit;
            //tBL_AN_ITProjekt_EditList.EditList_UserCommit = tempUserCommit;
            //tBL_AN_ITProjekt_EditList.EditList_Create = DateTime.Now;
            //tBL_AN_ITProjekt_EditList.EditList_LastChange = DateTime.Now;
            //tBL_AN_ITProjekt_EditList.EditList_UserName = UCParameter.LogInUser.DisplayName;
            //tBL_AN_ITProjekt_EditList.EditList_UserStrSID = UCParameter.LogInUser.SSID.ToString();
            //return DA_ITProjekt.Set_EditListItem(tBL_AN_ITProjekt_EditList);
            //return base.SaveEditList(tempPrgCommit, tempUserCommit);

        }
        public override void LoadLastChangeFoot(ChangeManagmentParameter changeManagmentParameter)
        {
            base.LoadLastChangeFoot(changeManagmentParameter);
        }



        /// <summary>
        /// Speichert den aktuellen Inhalt des Variablen in die Datenbank für dieses UC
        /// </summary>
        /// <returns></returns>
        public override bool SaveContent()
        {
            base.SaveContent();
            //Schreibe Daten in Tabelle
            TBL_AN_BSIStandard_MainBlock tBL_AN_BSIStandard_MainBlock = new TBL_AN_BSIStandard_MainBlock();
            if (SelMainBlockID > 0)
            {
                //Datensatz vorhanden
                tBL_AN_BSIStandard_MainBlock.MainBlockID = SelMainBlockID;
                tBL_AN_BSIStandard_MainBlock.MainBlockCreate = SessionTBL_AN_BSIStandard_MainTree.TBL_AN_BSIStandard_MainBlock.MainBlockCreate;
            }
            else
            {
                //Nicht vorhanden
                tBL_AN_BSIStandard_MainBlock.MainBlockCreate = DateTime.Now;
            }
            tBL_AN_BSIStandard_MainBlock.MainBlockMainTreeID = SessionTBL_AN_BSIStandard_MainTree.MainTree_ID;
            tBL_AN_BSIStandard_MainBlock.MainBlockDescription = Basis_Segment_UC_AllgDetail.richEditControlBeschreibungALLgemein.RtfText;
            ////Diagramm
            //var stream = new MemoryStream();
            //Basis_Segment_UC_ProzessDiagram.diagramControlProzess.SaveDocument(stream);
            //tBL_AN_UNIProzesse_MainBlock.MainBlockProzessDiagram = stream.ToArray();
            //tBL_AN_UNIProzesse_MainBlock.MainBlockLastChange = DateTime.Now;
            DA_NMBSI.SetBSIStandardMainBlock(tBL_AN_BSIStandard_MainBlock);
            SelMainBlockID = tBL_AN_BSIStandard_MainBlock.MainBlockID;
            SessionTBL_AN_BSIStandard_MainTree = DA_NMBSI.GetBSIStandard_MainTree(SelMainTreelID);
            if (SelMainBlockID == 0)
            {
                MessageBox.Show("Fehler beim Speichern des HauptProzessBaustein. Änderungen konnten nicht übernommen werden. Lassen Sie die Logs nach Fehler analysieren.", "Fehler beim Speichern", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            ////IT Block speichern
            ////Wir speichern hier nur den Zurordnung des Datensatzes Basis_UC_DetailITInventar zum CSB Prozess, NICHT den Inhalt (SaveContent)
            //if (Basis_UC_DetailITInventar != null)
            //{
            //    //Speichern der Zuordnung
            //    if (!DA_CSBDatenModell.SetCSBProzesseReferenzITInventarBlock(Basis_UC_DetailITInventar.SelITInventarBlockID, SelMainBlockID))
            //    {
            //        //Fehler
            //        MessageBox.Show("Fehler beim Speichern des ProzessBaustein IT. Änderungen konnten nicht übernommen werden. Lassen Sie die Logs nach Fehler analysieren.", "Fehler beim Speichern", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return false;

            //    };
            //    SessiontBL_AN_CSBProzesse_MainTree = DA_CSBDatenModell.GetUNIProzesse_MainTree(SelMainTreelID);
            //}
            //if (Basis_UC_DetailUNI != null)
            //{
            //    //CSB Block speichern
            //    if (!Basis_UC_DetailUNI.SaveContent())
            //    {
            //        //Fehler
            //        MessageBox.Show("Fehler beim Speichern des ProzessBaustein CSB. Änderungen konnten nicht übernommen werden. Lassen Sie die Logs nach Fehler analysieren.", "Fehler beim Speichern", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return false;
            //    }
            //}
            UCParameter.UCMessageSaveContent();



            return true;


        }




        #endregion

        #region Ereignisse


        private void splitContainerContraol_Basis_SplitterPositionChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.appSettingUCDetailSplitterPOS = splitContainerContraol_Basis.SplitterPosition;
            Properties.Settings.Default.Save();
        }



        #endregion
    }
}
