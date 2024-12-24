using accANCollection.Daten;
using accANCollection.Daten.NMBSI;
using accANCollection.Klassen;
using accANCollection.UserControls.Basis;
using NM.BSI.Dialog;
using NM.BSI.UC;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Columns;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using accLog;

namespace NM.BSI
{
    public partial class UC_MainBSI : Basis_UC_MainDetailView
    {
        //Logging
        private string alternativDateiNameWithoutExtensions = "Log_UC_MainBSI";

        //Property
        DA_NMBSI DA_NMBSI;
        UC_GB_DetailGeneral UC_GB_DetailGeneral;
        string regKey = "Software\\NordseeMilch\\BSIStandard\\TreeListMain";
        TreeListColumn treeListColumnMainTree_MainBlock_ID;

        public UC_MainBSI(UCParameter uCParameter) : base(uCParameter)
        {
            InitializeComponent();
            SelMainTreelID = 0;
            SelMainTreelTyp_ID = 0;
            //barStaticItem1.Caption = "ModulVersion: " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            LabelHeaderMainDetail = "BSI IT-Grundschutz Kompendium - Definition und Stammdaten BSI Standard";
            //UCParameter.AppProjektID = Properties.Settings.Default.appSettingsApplicationID;
            //UCParameter.FileMangerFolder = Properties.Settings.Default.appSettingsFileManagerFolder;
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitForm1));
            InitDaten();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
        }

        public UC_MainBSI() : base()
        {
            InitializeComponent();
        }

        #region InitLocal

        private void InitDaten()
        {
            DC_ANBSIDataContext dC_ANBSIDataContext = new DC_ANBSIDataContext(UCParameter.AppNMDataTransferConnectionString);
            dC_ANBSIDataContext.ObjectTrackingEnabled = false;
            //InitDisplay(dC_ANBSIDataContext.TBL_AN_BSIStandard_MainTree.Where(t => t.MainTree_TagDel ==  Properties.Settings.Default.appSettingMainTreeShowDeleteItems | t.MainTree_TagDel == false));
            
            DA_NMBSI = new DA_NMBSI(UCParameter);
            InitDisplay(DA_NMBSI.GetBSIStandard_MainTrees());
        }

        #endregion

        #region "Methoden aus Erblasser-Klasse"
        #region Init

        public override void InitDisplay(IQueryable queryable)
        {
            base.InitDisplay(queryable);

            //if ()
            //{

            //}
            //Zusatzspalte erstellen
            treeListColumnMainTree_MainBlock_ID = new TreeListColumn();
            treeListColumnMainTree_MainBlock_ID.FieldName = "TBL_AN_BSIStandard_MainBlock.MainBlockID";
            treeListColumnMainTree_MainBlock_ID.Caption = "MainBlockID";
            treeListColumnMainTree_MainBlock_ID.Visible = true;
            treeListColumnMainTree_MainBlock_ID.OptionsColumn.AllowSort = false;
            treeListColumnMainTree_MainBlock_ID.MaxWidth = 100;


            treeListMainView.Columns.Add(treeListColumnMainTree_MainBlock_ID);
            ribbonPageItems.Text = "BSI-Standard";
            splitContainerMainView.SplitterPosition = Properties.Settings.Default.appSettingUCNMBSISplitterPOS;
        }

        public override void Basis_UC_MainDetailView_Load(object sender, EventArgs e)
        {
            base.Basis_UC_MainDetailView_Load(sender, e);
            treeListMainView.RestoreLayoutFromRegistry(regKey);
        }

        public override void UpdateProzessschemaView()
        {
            base.UpdateProzessschemaView();
        }

        #endregion

        #endregion

        #region "Unterstützende Methoden"

        /// <summary>
        /// Neuer Eintrag nach Typ in MainTree
        /// </summary>
        /// <param name="nodeTyp"></param>
        private void NewNode(int nodeTyp)
        {
            TBL_AN_BSIStandard_MainTree tBL_AN_BSIStandard_MainTree  = new TBL_AN_BSIStandard_MainTree();
            tBL_AN_BSIStandard_MainTree.MainTree_ParentID = SelMainTreelID;
            tBL_AN_BSIStandard_MainTree.MainTree_Typ_ID = nodeTyp;
            if (nodeTyp == 1)
            {
                tBL_AN_BSIStandard_MainTree.MainTree_Bez = "Neuer Ordner";
                tBL_AN_BSIStandard_MainTree.MainTree_BezLang = "Ordner";
            }
            //if (nodeTyp == 2)
            //{
            //    tBL_AN_CSBProzesse_MainTree.MainTree_Bez = "Neuer Prozess";
            //    tBL_AN_CSBProzesse_MainTree.MainTree_BezLang = "Prozessbeschreibung";
            //}

            if (nodeTyp == 3)
            {
                tBL_AN_BSIStandard_MainTree.MainTree_Bez = "Neue Gefährdung";
                tBL_AN_BSIStandard_MainTree.MainTree_BezLang = "Einzelgefährdung";
            }

            //if (nodeTyp == 4)
            //{
            //    tBL_AN_CSBProzesse_MainTree.MainTree_Bez = "Neuer ProzessIT";
            //    tBL_AN_CSBProzesse_MainTree.MainTree_BezLang = "Prozessbaustein IT";
            //}

            //if (nodeTyp == 7)
            //{
            //    tBL_AN_CSBProzesse_MainTree.MainTree_Bez = "Neuer UNI Prozess";
            //    tBL_AN_CSBProzesse_MainTree.MainTree_BezLang = "Prozessbaustein";
            //}

            tBL_AN_BSIStandard_MainTree.MainTree_ID = 0; //NeuerDatensatz
            if (DA_NMBSI.SetBSIStandard_MainTree(tBL_AN_BSIStandard_MainTree) == 0)
            {
                //Fehler
                XtraMessageBox.Show("Es konnte kein neuer Eintrag angelegt werden. Fehler beim Speichern.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (isLogEnabled) UCParameter.Logging.SetLogItem("NewNode - Fehler : Es konnte kein neuer Eintrag erstellt werden.", 1, alternativDateiNameWithoutExtensions);


            }
            UpdateProzessschemaView();
            ExpandSelektNode();

        }

        /// <summary>
        /// Löschen selektierten Eintrag
        /// </summary>
        private void DelNode()
        {
            if (SelMainTreelID > 0)
            {
                if (DA_NMBSI.CheckBSIStandardSubRelation(SelMainTreelID) > 0)
                {
                    //Es sind Unterkategien vorhanden und deshalb soll nicht gelöscht werden
                    XtraMessageBox.Show("Löschen nicht möglich, weil es unterhalb Elemente gibt. Bitte diese vorab löschen.", "Löschen!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateProzessschemaView();
                    return;
                }
                {

                }
                if (XtraMessageBox.Show("Möchten Sie wirklich den Eintrag " + SelMainTreeBez + " endgültig löschen?", "Löschen?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (!DA_NMBSI.DelBSIStandard_MainTree(SelMainTreelID))
                    {
                        XtraMessageBox.Show("Löschen ist fehlgeschlagen. Prüfen Sie die LOG Datei", "Löschen!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }



                }
                UpdateProzessschemaView();
            }

        }

        /// <summary>
        /// Speichert in das TreeViewDataset den SortierIndex. Sortierung innerhalb des Knotens.
        /// Es wird die Sortierreihenfolge festgelegt mit den Druchlauf der Nodes. die Reiehnfolge ergibt sich aus dem derzeitigen Index (TreeList).
        /// Danach wird in die DatenbankSpalte jedes Items mit dem Index der Reihenfolge gespeichet für den nächten Aufruf der Datenbankeinträge. (Reihenfolge speichern SortID
        /// </summary>
        /// <param name="e"></param>
        private void SaveNewRecordPosition(NodeEventArgs e)
        {

            var nodes = e.Node.ParentNode == null ? e.Node.TreeList.Nodes
                        : e.Node.ParentNode.Nodes;
            for (var i = 0; i < nodes.Count; i++)
            {
                nodes[i].SetValue(treeListColumnMainTree_SortID, i);
                //Speichern der neuen SortID in der Datenbank
                //Bezeichnung Lang geändert

                TBL_AN_BSIStandard_MainTree tBL_AN_BSIStandard_MainTree = DA_NMBSI.GetBSIStandard_MainTree((int)nodes[i].GetValue(treeListColumnMainTree_ID));
                if (tBL_AN_BSIStandard_MainTree != null)
                {
                    tBL_AN_BSIStandard_MainTree.MainTree_SortID = i;
                    if (DA_NMBSI.SetBSIStandard_MainTree(tBL_AN_BSIStandard_MainTree) == 0)
                    {
                        //Fehler
                        XtraMessageBox.Show("Die Änderung Sortiereihenfolge konnte nicht gespeichert werden.", "Fehler", MessageBoxButtons.OK);
                        if (isLogEnabled) UCParameter.Logging.SetLogItem("SaveNewRecordPosition - Fehler : Die Änderung Sortiereihenfolge konnte nicht gespeichert werden.", 1, alternativDateiNameWithoutExtensions);

                    }
                }
            }
        }
        #endregion

        #region "Ereignisse TreeList"

        /// <summary>
        /// Ereignis Selektiertes Node hat sich geändert. UserChange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void treeListMainView_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            base.treeListMainView_FocusedNodeChanged(sender, e);
            if (e.Node != null)
            {
                SelMainTreelID = Convert.ToInt32(e.Node.GetValue(treeListColumnMainTree_ID));
                SelMainTreelTyp_ID = Convert.ToInt32(e.Node.GetValue(treeListColumnMainTree_Typ_ID));
                SelMainTreeBez = e.Node.GetValue(treeListColumnMainTree_Bez).ToString();
                SelMainTreeTagDel = (bool)e.Node.GetValue(treeListColumnMainTree_TagDel);
            }
            else
            {
                SelMainTreelID = 0;
                SelMainTreelTyp_ID = 0;
                SelMainTreeBez = string.Empty;
                SelMainTreeTagDel = false;
            }

            if (splitContainerMainView.Panel2.Controls.Count > 0)
            {
                if (!UC_GB_DetailGeneral.CanClose())
                {
                    return;
                }

                splitContainerMainView.Panel2.Controls.Clear();
                UCParameter.UCMessageBeforeDispose();
                UC_GB_DetailGeneral.Dispose();
            }

            //Kategorie 3 wird die Detailansicht geöffnet
            if (SelMainTreelTyp_ID == 3)
            {
                //test test = new test();
                //splitContainerMainView.Panel2.Controls.Add(test);
                //_ = new UCParameter();
                //UCParameter uCParameter = UCParameter;
                //BarButtonCollektion barButtonCollektion = new BarButtonCollektion();
                UCParameter.BarButtonItems.Add(barButtonItemEditItem);
                UCParameter.BarButtonItems.Add(barButtonItemSaveItem);
                UCParameter.BarButtonItems.Add(barButtonItemDiscriptionEdit);
                UC_GB_DetailGeneral = new UC_GB_DetailGeneral(UCParameter);
                //ribbonControl1.MergeRibbon(uC_ProzessBausteinMain.Ribbon);
                //IRibbon form = uC_ProzessBausteinMain as IRibbon;
                //if (form != null)
                //    ribbonControl1.MergeRibbon(form.Ribbon);
                //ribbonControl1.MergeRibbon(uC_ProzessBausteinMain.ribbonControl1); 
                splitContainerMainView.Panel2.Controls.Add(UC_GB_DetailGeneral);

            }



        }

        /// <summary>
        /// Ereignis AfterFocusNode - Nach dem das Node den Focus erhalten hat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void treeListMainView_AfterFocusNode(object sender, NodeEventArgs e)
        {
            base.treeListMainView_AfterFocusNode(sender, e);
            SelMainTreelID = Convert.ToInt32(e.Node.GetValue(treeListColumnMainTree_ID));
            SelMainTreelTyp_ID = Convert.ToInt32(e.Node.GetValue(treeListColumnMainTree_Typ_ID));
            SelMainTreeBez = e.Node.GetValue(treeListColumnMainTree_Bez).ToString();
            SelMainTreeTagDel = (bool)e.Node.GetValue(treeListColumnMainTree_TagDel);
        }

        /// <summary>
        /// Ereignis CellValueChanged - User hat eine Value einer Cell geändert 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void treeListMainView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            base.treeListMainView_CellValueChanged(sender, e);
            if (e.ChangedByUser)
            {
                if (SelMainTreelID > 0)
                {
                    if (e.Column == treeListColumnMainTree_Bez)
                    {
                        //Bezeichnung Kurz geändert
                        //dataAccess = new DataAccess(AppConnectString, logInUser, logging);
                        TBL_AN_BSIStandard_MainTree tBL_AN_BSIStandard_MainTree = DA_NMBSI.GetBSIStandard_MainTree(SelMainTreelID);
                        if (tBL_AN_BSIStandard_MainTree != null)
                        {
                            tBL_AN_BSIStandard_MainTree.MainTree_Bez = e.Value.ToString();
                            if (DA_NMBSI.SetBSIStandard_MainTree(tBL_AN_BSIStandard_MainTree) == 0)
                            {
                                //Fehler
                                XtraMessageBox.Show("Die Änderung MainTree_Bez konnte nicht gespeichert werden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                if (isLogEnabled) UCParameter.Logging.SetLogItem("treeList_Prozessschema_CellValueChanged - Fehler : Die Änderung MainTree_Bez konnte nicht gespeichert werden.", 1, alternativDateiNameWithoutExtensions);
                                e.Value = e.OldValue;
                            }
                        }
                    }
                    if (e.Column == treeListColumnMainTree_BezLang)
                    {
                        //Bezeichnung Lang geändert
                        //dataAccess = new DataAccess(AppConnectString, logInUser, logging);
                        TBL_AN_BSIStandard_MainTree tBL_AN_BSIStandard_MainTree = DA_NMBSI.GetBSIStandard_MainTree(SelMainTreelID);
                        if (tBL_AN_BSIStandard_MainTree != null)
                        {
                            tBL_AN_BSIStandard_MainTree.MainTree_BezLang = e.Value.ToString();
                            if (DA_NMBSI.SetBSIStandard_MainTree(tBL_AN_BSIStandard_MainTree) == 0)
                            {
                                //Fehler
                                XtraMessageBox.Show("Die Änderung MainTree_BezLang konnte nicht gespeichert werden.", "Fehler", MessageBoxButtons.OK);
                                if (isLogEnabled) UCParameter.Logging.SetLogItem("treeList_Prozessschema_CellValueChanged - Fehler : Die Änderung MainTree_Bez konnte nicht gespeichert werden.", 1, alternativDateiNameWithoutExtensions);
                                e.Value = e.OldValue;
                            }
                        }
                    }
                    //if (e.Column == treeListColumnMainTree_CSBProzessNummer)
                    //{
                    //    //Bezeichnung Lang geändert
                    //    //dataAccess = new DataAccess(AppConnectString, logInUser, logging);
                    //    TBL_AN_UNIProzesse_MainTree tBL_AN_CSBProzesse_MainTree = DA_CSBProzess.GetUNIProzesse_MainTree(SelMainTreelID);
                    //    if (tBL_AN_CSBProzesse_MainTree != null)
                    //    {
                    //        tBL_AN_CSBProzesse_MainTree.MainTree_CSBProzessNr = e.Value.ToString();
                    //        if (DA_CSBProzess.SetCSBProzesse_MainTree(tBL_AN_CSBProzesse_MainTree) == 0)
                    //        {
                    //            //Fehler
                    //            XtraMessageBox.Show("Die Änderung MainTree_CSB ProzessNr konnte nicht gespeichert werden.", "Fehler", MessageBoxButtons.OK);
                    //            if (isLogEnabled) UCParameter.Logging.SetLogItem("treeList_Prozessschema_CellValueChanged - Fehler : Die Änderung MainTree_Bez konnte nicht gespeichert werden.", 1, alternativDateiNameWithoutExtensions);
                    //            e.Value = e.OldValue;
                    //        }
                    //    }
                    //}

                }
                UpdateProzessschemaView();
            }
        }

        /// <summary>
        /// Edit Cell startet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void treeListMainView_ShownEditor(object sender, EventArgs e)
        {
            base.treeListMainView_ShownEditor(sender, e);
            var activeEditor = treeListMainView.ActiveEditor;
            if (treeListMainView.FocusedValue is int)
            {
                activeEditor.BackColor = ((int)treeListMainView.FocusedValue) < 25000 ? Color.LightPink : Color.LightGreen;
            }
            else
            {
                activeEditor.BackColor = Color.Yellow;
            }
        }

        /// <summary>
        /// Ereignis nach dem Absetzen eines Eintrages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void treeListMainView_AfterDragNode(object sender, AfterDragNodeEventArgs e)
        {
            base.treeListMainView_AfterDragNode(sender, e);
            //int test1 = (int)e.Node.GetValue(treeListColumnMainTree_ParentID);
            //int test2 = (int)e.Node.GetValue(treeListColumnMainTree_ID);

            SaveNewRecordPosition(e);
            UpdateProzessschemaView();
        }

        /// <summary>
        /// Ereignis Abschluss Drop Node an einer neuen Position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void treeListMainView_AfterDropNode(object sender, AfterDropNodeEventArgs e)
        {
            base.treeListMainView_AfterDropNode(sender, e);
            //int test1 = (int)e.Node.GetValue(treeListColumnMainTree_ParentID);
            //int test2 = (int)e.Node.GetValue(treeListColumnMainTree_ID);
            //int test3 = (int)e.DestinationNode.GetValue(treeListColumnMainTree_ParentID);
            //int test4 = (int)e.DestinationNode.GetValue(treeListColumnMainTree_ID);

            //ParentID geändert
            //dataAccess = new DataAccess(AppConnectString, logInUser, logging);
            TBL_AN_BSIStandard_MainTree tBL_AN_BSIStandard_MainTree = DA_NMBSI.GetBSIStandard_MainTree((int)e.Node.GetValue(treeListColumnMainTree_ID));
            if (tBL_AN_BSIStandard_MainTree != null)
            {
                tBL_AN_BSIStandard_MainTree.MainTree_ParentID = (int)e.DestinationNode.GetValue(treeListColumnMainTree_ID);
                if (DA_NMBSI.SetBSIStandard_MainTree(tBL_AN_BSIStandard_MainTree) == 0)
                {
                    //Fehler
                    XtraMessageBox.Show("Die Änderung Drag&Drop konnte nicht gespeichert werden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (isLogEnabled) UCParameter.Logging.SetLogItem("treeList_Prozessschema_AfterDropNode - Fehler : Die Änderung Drag&Drop konnte nicht gespeichert werden.", 1, alternativDateiNameWithoutExtensions);

                }
            }
        }

        /// <summary>
        /// Tritt auf, wenn ein Datenelement über die Grenzen des Steuerelements gezogen wird.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void treeListMainView_DragOver(object sender, DragEventArgs e)
        {
            base.treeListMainView_DragOver(sender, e);
            //List<TreeListNode> list = e.Data as List<TreeListNode>;
            //// You can prohibit to drop specific data.
            //if (list.Find((x) => x.GetValue(colDEPARTMENT1).ToString().Contains("Finance")) != null)
            //{
            //    e.Cursor = Cursors.No;
            //    e.Handled = true;
            //}
        }

        /// <summary>
        /// Ereignis Draw. Setzen von Zeileninhalte beim Zeichnen der Zeilen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void treeListMainView_GetSelectImage(object sender, GetSelectImageEventArgs e)
        {
            base.treeListMainView_GetSelectImage(sender, e);
            if (treeListMainView.IsAutoFilterNode(e.Node))
                return;
            //string[] groupNames = new string[] { "Administration", "Inventory", "Manufacturing", "Quality", "Research", "Sales" };
            if (e.Node.GetValue(treeListColumnMainTree_Typ_ID) != null)
            {
                int intTempTypID = (int)e.Node.GetValue(treeListColumnMainTree_Typ_ID);


                switch (intTempTypID)
                {
                    case 0:
                        //Hauptkategorie
                        e.NodeImageIndex = 0;
                        break;
                    case 1:
                        //Ordner
                        e.NodeImageIndex = 1;
                        break;
                    case 2:
                        //Elementare Gefährdung (MAIN)
                        e.NodeImageIndex = 0;
                        break;
                    case 3:
                        //Elementare Gefährdung (ITEM)
                        e.NodeImageIndex = 2;
                        break;
                    //case 4:
                    //    //Prozessbaustein IT
                    //    e.NodeImageIndex = 4;
                    //    break;
                    //case 5:
                    //    //Ordner Version
                    //    e.NodeImageIndex = 1;
                    //    break;
                    //case 6:
                    //    //Version
                    //    e.NodeImageIndex = 1;
                    //    break;
                    //case 7:
                    //    //UNI Prozess
                    //    e.NodeImageIndex = 6;
                    //    break;
                    default:
                        e.NodeImageIndex = -1;
                        break;
                }
            }
            //currentGroupName = (string)e.Node.GetValue("GroupName");
            //e.NodeImageIndex = Array.FindIndex(groupNames, new Predicate<string>(IsCurrentGroupName));
        }

        /// <summary>
        /// Ereignis bevor ein Element gezogen werden darf.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void treeListMainView_BeforeDragNode(object sender, BeforeDragNodeEventArgs e)
        {
            base.treeListMainView_BeforeDragNode(sender, e);
            if (e.Node.GetValue(treeListColumnMainTree_Typ_ID) != null)
            {

                int tempQNode = (int)e.Node.GetValue(treeListColumnMainTree_Typ_ID);
                switch (tempQNode)
                {
                    case 0:
                        //Hauptkategorie
                        e.CanDrag = false;
                        break;
                    case 1:
                        //Prozessordner
                        e.CanDrag = true;
                        break;
                    //case 2:
                    //    //ProzessGruppe
                    //    e.CanDrag = true;
                    //    break;
                    //case 3:
                    //    //Prozessbaustein CSB
                    //    e.CanDrag = true;
                    //    break;
                    //case 4:
                    //    //Prozessbaustein IT
                    //    e.CanDrag = true;
                    //    break;
                    //case 5:
                    //    //Ordner Version
                    //    e.CanDrag = false;
                    //    break;
                    //case 6:
                    //    //Version
                    //    e.CanDrag = false;
                    //    break;
                    //case 7:
                    //    //UNI Prozess
                    //    e.CanDrag = true;
                    //    break;
                    default:
                        e.CanDrag = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Ereignis bevor das Element abgelassen wird. Prüfung ob erlaubt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void treeListMainView_BeforeDropNode(object sender, BeforeDropNodeEventArgs e)
        {
            base.treeListMainView_BeforeDropNode(sender, e);
            if (e.SourceNode.GetValue(treeListColumnMainTree_Typ_ID) != null & e.DestinationNode.GetValue(treeListColumnMainTree_Typ_ID) != null)
            {
                int tempQNode = (int)e.SourceNode.GetValue(treeListColumnMainTree_Typ_ID);
                int tempDNode = (int)e.DestinationNode.GetValue(treeListColumnMainTree_Typ_ID);

                if (tempQNode == 1)
                {
                    //Order darf an 0 oder 1
                    if (tempDNode == 0 | tempDNode == 1)
                    {
                        e.Cancel = false;
                        return;
                    }
                    else
                    {
                        e.Cancel = true;
                        return;
                    }

                //}
                //if (tempQNode == 2)
                //{
                //    //Prozess darf an 1
                //    if (tempDNode == 1)
                //    {
                //        e.Cancel = false;
                //        return;
                //    }
                //    else
                //    {
                //        e.Cancel = true;
                //        return;
                //    }

                //}

                //if (tempQNode == 3)
                //{
                //    //ProzessGruppe darf an 2
                //    if (tempDNode == 2)
                //    {
                //        e.Cancel = false;
                //        return;
                //    }
                //    else
                //    {
                //        e.Cancel = true;
                //        return;
                //    }

                //}

                //if (tempQNode == 3)
                //{
                //    //Prozess CSB darf an 2
                //    if (tempDNode == 2)
                //    {
                //        e.Cancel = false;
                //        return;
                //    }
                //    else
                //    {
                //        e.Cancel = true;
                //        return;
                //    }

                //}
                //if (tempQNode == 4)
                //{
                //    //Prozess IT darf an 2
                //    if (tempDNode == 2)
                //    {
                //        e.Cancel = false;
                //        return;
                //    }
                //    else
                //    {
                //        e.Cancel = true;
                //        return;
                //    }

                //}

                //if (tempQNode == 5)
                //{
                //    //Ordner Version
                //    e.Cancel = true;
                //    return;

                //}

                //if (tempQNode == 6)
                //{
                //    // Version
                //    e.Cancel = true;
                //    return;

                //}
                //if (tempQNode == 7)
                //{
                //    //UNI Prozess darf an 2
                //    if (tempDNode == 2)
                //    {
                //        e.Cancel = false;
                //        return;
                //    }
                //    else
                //    {
                //        e.Cancel = true;
                //        return;
                //    }

                }
            }
        }

        /// <summary>
        /// Ereigniss nachdem ein Element gedrop ist (abgelegt). Nacharbeit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void treeListMainView_DragDrop(object sender, DragEventArgs e)
        {
            base.treeListMainView_DragDrop(sender, e);
            DXDragEventArgs args = treeListMainView.GetDXDragEventArgs(e);

            if (args.Data.GetDataPresent(typeof(TreeListNode)))
            {

                if (args.Effect == DragDropEffects.Move)
                {
                    //var DragNode = treeList_Prozessschema.GetDataRecordByNode(args.Node);
                    //var TargetNode = treeList_Prozessschema.GetDataRecordByNode(args.TargetNode);



                    //if (args.DragInsertPosition == DragInsertPosition.AsChild) // idzie w dol hierarchii  
                    //{
                    //    XtraMessageBox.Show("goes down");
                    //}
                    //else if (args.DragInsertPosition == DragInsertPosition.Before) //Idzie w gore hierarchii  
                    //{
                    //    XtraMessageBox.Show("goes up");
                    //}

                }
                if (args.Effect == DragDropEffects.Link)
                {
                    //var DragNode = treeList_Prozessschema.GetDataRecordByNode(args.Node);
                    //var TargetNode = treeList_Prozessschema.GetDataRecordByNode(args.TargetNode);

                }
            }
        }

        public override void treeListMainView_NodeCellStyle(object sender, GetCustomNodeCellStyleEventArgs e)
        {
            base.treeListMainView_NodeCellStyle(sender, e);
            if (e.Column.FieldName == "MainTree_Bez")
            {
                if (e.Node.GetValue(treeListColumnMainTree_TagDel) != null)
                {
                    if ((bool)e.Node.GetValue(treeListColumnMainTree_TagDel) == true)
                    {
                        //Datensatz als gelöscht markiert
                        e.Appearance.FontStyleDelta = FontStyle.Italic;
                        e.Appearance.FontStyleDelta = FontStyle.Strikeout;
                        e.Appearance.ForeColor = Color.Gray;

                    }
                }

            }
        }

        public override void treeListMainView_CustomDrawRow(object sender, CustomDrawRowEventArgs e)
        {
            base.treeListMainView_CustomDrawRow(sender, e);
        }

        public override void treeListMainView_ShowingEditor(object sender, CancelEventArgs e)
        {
            base.treeListMainView_ShowingEditor(sender, e);
        }

        public override void treeListMainView_LayoutUpdated(object sender, EventArgs e)
        {
            base.treeListMainView_LayoutUpdated(sender, e);
            treeListMainView.SaveLayoutToRegistry(regKey);
        }

        #endregion

        private void UC_BSIStandard_IsMainTreeIDChanged()
        {
            if (SelMainTreelID < 1)
            {
                //bt_BerechtigungNEU.Enabled = false;
                barBt_ElementareGefaehrdungDEL.Enabled = false;
            }
            else
            {
                //bt_BerechtigungNEU.Enabled = true;
                barBt_ElementareGefaehrdungDEL.Enabled = true;
            }
            //SelGrpID = 0;
            //barListItemSelTreeMainID.Caption = "SelMainTreeMainID = " + SelMainTreelID;
        }

        private void UC_BSIStandard_IsSelSelMainTreelTyp_IDChange()
        {
            switch (SelMainTreelTyp_ID)
            {
                case 0:
                    //Root, nicht löschbar
                    barBt_ElementareGefaehrdungNEU.Enabled = false;
                    barBt_ElementareGefaehrdungDEL.Enabled = false;
                    //barBtProzessBausteinNeu.Enabled = false;
                    barButtonItemDiscriptionEdit.Enabled = false;
                    
                    break;
                case 1:
                    //Ordner
                    barBt_ElementareGefaehrdungNEU.Enabled = false;
                    barBt_ElementareGefaehrdungDEL.Enabled = false;
                    //barBtProzessBausteinNeu.Enabled = false;
                    barButtonItemDiscriptionEdit.Enabled = false;
                    break;

                case 2:
                    //Elementare Gefährdung (Main)
                    barBt_ElementareGefaehrdungNEU.Enabled = true;
                    barBt_ElementareGefaehrdungDEL.Enabled = false;
                    barButtonItemDiscriptionEdit.Enabled = false;
                    break;

                case 3:
                    //Elementare Gefährdung (ITEM)
                    barBt_ElementareGefaehrdungNEU.Enabled = false;
                    barBt_ElementareGefaehrdungDEL.Enabled = true;
                    //barBtProzessBausteinNeu.Enabled = false;
                    barButtonItemDiscriptionEdit.Enabled = false;
                    break;

              

                default:
                    //Alle nicht genannten Typen
                    barBt_ElementareGefaehrdungNEU.Enabled = false;
                    barBt_ElementareGefaehrdungDEL.Enabled = false;
                    //barBtProzessBausteinNeu.Enabled = false;
                    barButtonItemDiscriptionEdit.Enabled = false;
                    break;
            }
            //barListItemSelTreeMainTypID.Caption = "SelMainTreeTypID = " + SelMainTreelTyp_ID;
        }

        private void barBt_ElementareGefaehrdungNEU_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            NewNode(3);
        }

        private void barBt_ElementareGefaehrdungDEL_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DelNode();
        }
    }
}
