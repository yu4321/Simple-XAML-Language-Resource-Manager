﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SimpleXAMLLocalizationHelper.Model;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using static SimpleXAMLLocalizationHelper.Utils.Common;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.Windows.Data;
using System.Globalization;
using SimpleXAMLLocalizationHelper.View;
using SimpleXAMLLocalizationHelper.Messages;
using GalaSoft.MvvmLight.Messaging;
using System.Data;

namespace SimpleXAMLLocalizationHelper.ViewModel
{
    /// <summary>
    /// 주 뷰모델 - 메인 윈도우 관련 요소들
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public partial class CoreViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the CoreViewModel class.
        /// </summary>

        #region properties and variables


        private ObservableCollection<LanguageBoxItem> _inputBoxes = new ObservableCollection<LanguageBoxItem>();

        public ObservableCollection<LanguageBoxItem> InputBoxes
        {
            get
            {
                return _inputBoxes;
            }
            set
            {
                Set(nameof(InputBoxes), ref _inputBoxes, value);
            }
        }

        private string _Kor;

        public string Kor
        {
            get
            {
                return _Kor;
            }
            set
            {
                Set(nameof(Kor), ref _Kor, value);
            }
        }

        private string _Eng;

        public string Eng
        {
            get
            {
                return _Eng;
            }
            set
            {
                Set(nameof(Eng), ref _Eng, value);
            }
        }

        private string _Jpn;

        public string Jpn
        {
            get
            {
                return _Jpn;
            }
            set
            {
                Set(nameof(Jpn), ref _Jpn, value);
            }
        }

        private string _Chns;

        public string Chns
        {
            get
            {
                return _Chns;
            }
            set
            {
                Set(nameof(Chns), ref _Chns, value);
            }
        }

        private string _ID;

        public string ID
        {
            get
            {
                return _ID;
            }
            set
            {
                Set(nameof(ID), ref _ID, value);
            }
        }

        private string _selectedID;

        public string SelectedID
        {
            get
            {
                return _selectedID;
            }
            set
            {
                Set(nameof(SelectedID), ref _selectedID, value);
            }
        }

        private DataRowView _nowindex;

        public DataRowView nowindex
        {
            get
            {
                return _nowindex;
            }
            set
            {
                Set(nameof(nowindex), ref _nowindex, value);
            }
        }

        private AttributeItem _nowindex_a1;

        public AttributeItem nowindex_a1
        {
            get
            {
                return _nowindex_a1;
            }
            set
            {
                Set(nameof(nowindex_a1), ref _nowindex_a1, value);
            }
        }

        private AttributeItem _nowindex_a2;

        public AttributeItem nowindex_a2
        {
            get
            {
                return _nowindex_a2;
            }
            set
            {
                Set(nameof(nowindex_a2), ref _nowindex_a2, value);
            }
        }

        private ObservableCollection<DataItem> _Items;

        public ObservableCollection<DataItem> Items

        {
            get
            {
                return _Items;
            }

            set
            {
                Set(nameof(Items), ref _Items, value);
            }
        }

        

        private ObservableCollection<AttributeItem> _ItemsA1;

        public ObservableCollection<AttributeItem> ItemsA1

        {
            get
            {
                return _ItemsA1;
            }

            set
            {
                Set(nameof(ItemsA1), ref _ItemsA1, value);
            }
        }

        private ObservableCollection<AttributeItem> _ItemsA2;

        public ObservableCollection<AttributeItem> ItemsA2

        {
            get
            {
                return _ItemsA2;
            }

            set
            {
                Set(nameof(ItemsA2), ref _ItemsA2, value);
            }
        }

        private bool _isAttrsUsing;

        public bool IsAttrsUsing
        {
            get
            {
                return _isAttrsUsing;
            }

            set
            {
                Set(nameof(IsAttrsUsing), ref _isAttrsUsing, value);
            }
        }

        

        private string _currentFolderPath= "현재 선택된 폴더 없음";
        public string CurrentFolderPath
        {
            get
            {
                return _currentFolderPath;
            }

            set
            {
                Set(nameof(CurrentFolderPath), ref _currentFolderPath, value);
            }
        }

        private DataTable _dataItems=new DataTable();
        public DataTable DataItems
        {
            get
            {
                return _dataItems;
            }
            set
            {
                Set(nameof(DataItems), ref _dataItems, value);
            }
        }

        private ObservableCollection<string> _langList = new ObservableCollection<string>();

        //private List<string> LangList = new List<string>();
        public ObservableCollection<string> LangList
        {
            get
            {
                return _langList;
            }
            set
            {
                Set(nameof(LangList), ref _langList, value);
            }
        }

        private string lastSelectedPath;

        public ICommand AddCommand { get; set; }
        public ICommand ModifyCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ClickCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public ICommand AddCommandA1 { get; set; }
        public ICommand ModifyCommandA1 { get; set; }
        public ICommand DeleteCommandA1 { get; set; }

        public ICommand AddCommandA2 { get; set; }
        public ICommand ModifyCommandA2 { get; set; }
        public ICommand DeleteCommandA2 { get; set; }

        public ICommand HomeCommand { get; set; }
        



        #endregion properties and variables

        #region constructor

        public CoreViewModel()
        {
            MakeLangList();
            Items = new ObservableCollection<DataItem>();
            ItemsA1 = new ObservableCollection<AttributeItem>();
            ItemsA2 = new ObservableCollection<AttributeItem>();
            NewItems = new ObservableCollection<DataItem>();
            ItemsA2.Add(new AttributeItem()
            {
                Content = new XAttribute(XNamespace.Xml + "space", "preserve")
            });
            ItemsA2.Add(new AttributeItem()
            {
                Content = new XAttribute(XNamespace.Xml + "space", "default")
            });
            AddCommand = new RelayCommand(() => ExecuteAddCommand());
            ModifyCommand = new RelayCommand(() => ExecuteModifyCommand());
            SearchCommand = new RelayCommand(() => ExecuteSearchCommand());
            ClickCommand = new RelayCommand(() => ExecuteClickCommand());
            DeleteCommand = new RelayCommand(() => ExecuteDeleteCommand());
            AddCommandA1 = new RelayCommand(() => ExecuteAddCommandA1());
            DeleteCommandA1 = new RelayCommand(() => ExecuteDeleteCommandA1());
            OpenAutoEditCommand = new RelayCommand(() =>ExecuteOpenAutoEditCommand());
            OpenFolderSelectCommand = new RelayCommand(() =>
              {
                  SetFolderPath();
                  Messenger.Default.Send<ResetMessage>(new ResetMessage());
              });
            OpenDataGridCommand = new RelayCommand<string>(ExecuteOpenDataGridCommand);
            OpenBaseFileCommand = new RelayCommand(() => ExecuteOpenBaseFileCommand());
            OpenBaseFolderCommand = new RelayCommand(() => ExecuteOpenBaseFolderCommand());
            PreviewEditCommand = new RelayCommand(() => ExecutePreviewEditCommand());
            BeginAutoEditCommand = new RelayCommand(() => ExecuteBeginAutoEditCommand());
            ChangeLangCommand = new RelayCommand<string>(ExecuteChangeLangCommand);
            ResetCommand = new RelayCommand(() => ExecuteResetCommand());
            HomeCommand = new RelayCommand(() => Messenger.Default.Send<GotoPageMessage>(new GotoPageMessage(PageName.Start)));
            Messenger.Default.Register<ResetMessage>(this,(x) => ReceiveMessage(x));
            if (!IsInDesignMode)
            {
                SetFolderPath(true);
                MakeTables();
            }
        }

        #endregion constructor

        #region methods


        private void MakeLangList()
        {
            //FOR TEST
            //LangList.Add(Properties.Settings.Default.KorFileName);
            //LangList.Add(Properties.Settings.Default.EngFileName);
            //LangList.Add(Properties.Settings.Default.JpnFileName);
            //LangList.Add(Properties.Settings.Default.ChnsFileName);
            foreach(var x in App.LanguageList)
            {
                LangList.Add(x);
            }
        }

        private void MakeInputBoxes()
        {
            InputBoxes.Clear();
            foreach(var x in LangList)
            {
                InputBoxes.Add(new LanguageBoxItem
                {
                    Language = x,
                    Content = ""
                });
            }
        }

        private void MakeTables()
        {
            List<string> filenames = LangList.ToList();
            Dictionary<string, XElement[]> elements = new Dictionary<string, XElement[]>();
            Dictionary<string, XDocument> xDocs = new Dictionary<string, XDocument>();
            string lastfind;
            try
            {
                foreach (string name in filenames)
                {
                    lastfind = name;
                    using (var sr = new StreamReader(ResourcePath + name + ".xaml"))
                    {
                        XDocument xDoc = XDocument.Load(sr);
                        XElement[] xArray = (from w in xDoc.Descendants()
                                             where w.Attribute(xmn + "Key") != null && w.HasElements != true
                                             select w).ToArray();
                        xDocs.Add(name, xDoc);
                        elements.Add(name, xArray);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("{0}.xaml 파일을 찾을 수 없습니다! ");
                //if (DataItems.Rows.Count <= 0)
                //{
                //    //throw;
                //    //Messenger.Default.Send<GotoPageMessage>(new GotoPageMessage(PageName.Start));
                //    return;
                //    //Application.Current.Shutdown();
                //}
                //return;
                return;
            }
            catch
            {
                MessageBox.Show("올바른 폴더를 선택해주세요!\n폴더 안에 들어가는 각 파일 명은 설정과 동일해야 합니다. 또한 각 요소는 모두 어트리뷰트 \"x:Key\"를 고유한 값으로 지니고 있어야 합니다.");
                //if (DataItems.Rows.Count <= 0)
                //{
                //    //throw;
                //    //Messenger.Default.Send<GotoPageMessage>(new GotoPageMessage(PageName.Start));
                //    return;
                //    //Application.Current.Shutdown();
                //}
                return;
            }

            Resets();

            if (DataItems.Columns.Count <= 0)
            {
                DataItems.Columns.Add("ID");
                DataItems.PrimaryKey = new DataColumn[] { DataItems.Columns["ID"] };
            }
            else
            {
                DataItems.Rows.Clear();
            }

            foreach(var name in filenames)
            {
                if(DataItems.Columns[name]==null) DataItems.Columns.Add(name);
                foreach(var x in elements[name])
                {
                    var selectresult = DataItems.Select(string.Format("ID = '{0}'", x.Attribute(xmn + "Key").Value));
                    if (selectresult.Count()<=0)
                    {
                        DataRow dr = DataItems.NewRow();
                        dr["ID"] = x.Attribute(xmn + "Key").Value;
                        dr[name] = x.Value;
                        DataItems.Rows.Add(dr);
                    }
                    else
                    {
                        DataItems.Rows.Find((string)x.Attribute(xmn + "Key").Value)[name] = x.Value;
                    }
                }
            }
            if(InputBoxes.Count<=0)MakeInputBoxes();
        }

        private void Resets()
        {
            Items.Clear();
            ItemsA1.Clear();
            nowindex = null;
            nowindex_a1 = null;
            nowindex_a2 = null;
            SelectedID = null;
            Kor = null;
            Eng = null;
            Jpn = null;
            Chns = null;
            ID = null;
            CurrentFolderPath = lastSelectedPath;
            ExecuteResetCommand();
            InputBoxes.Clear();
        }

        private string GetFolderPath(bool isfirst = false)
        {
            string sresult;
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                dialog.Title = "언어 리소스 파일들이 들어있는 폴더를 선택해주세요.";
                if (isfirst) dialog.Title += "설정에 있는 파일명이 모두 필요합니다.";
                CommonFileDialogResult result = dialog.ShowDialog();
                try
                {
                    sresult = dialog.FileName;
                    lastSelectedPath = sresult;
                }
                catch
                {
                    sresult = "";
                }
            }
            sresult += "\\";
            return sresult;
        }

        private string GetFolderPath(Window basewindow)
        {
            string sresult;
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                dialog.Title = "언어 리소스 파일들이 들어있는 폴더를 선택해주세요.";
                CommonFileDialogResult result = dialog.ShowDialog(basewindow);
                try
                {
                    sresult = dialog.FileName;
                }
                catch
                {
                    sresult = "";
                }
            }
            sresult += "\\";
            return sresult;
        }

        private string GetFilePath()
        {
            string sresult;
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.Title = "언어 리소스 파일을 선택해주세요.";
                dialog.Filters.Add(new CommonFileDialogFilter("언어 리소스 파일", ".xaml"));
                CommonFileDialogResult result = dialog.ShowDialog();
                try
                {
                    sresult = dialog.FileName;
                    lastSelectedPath = sresult;
                }
                catch
                {
                    sresult = "";
                }
            }
            sresult += "\\";
            return sresult;
        }

        private string GetFilePath(Window basewindow)
        {
            string sresult;
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.Title = "언어 리소스 파일을 선택해주세요.";
                dialog.Filters.Add(new CommonFileDialogFilter("언어 리소스 파일", ".xaml"));
                CommonFileDialogResult result = dialog.ShowDialog(basewindow);
                try
                {
                    sresult = dialog.FileName;
                }
                catch
                {
                    sresult = "";
                }
            }
            sresult += "\\";
            return sresult;
        }

        private void SetFolderPath(bool isfirst = false)
        {
            if (!IsInDesignMode)
                ResourcePath = GetFolderPath(isfirst);
        }


        private static void SaveFiles(string kr, string en, string jp, string chns)
        {
            try
            {
                try
                {
                    using (StreamWriter wr = new StreamWriter(ResourcePath + Properties.Settings.Default.KorFileName + ".xaml"))
                    {
                        ReplaceCarriageReturns_String(ref kr);
                        wr.Write(kr);
                    }
                }
                catch
                {
                    MessageBox.Show("한글 파일의 저장에 실패하였습니다.");
                }
                try
                {
                    using (StreamWriter wr = new StreamWriter(ResourcePath + Properties.Settings.Default.EngFileName + ".xaml"))
                    {
                        ReplaceCarriageReturns_String(ref en);
                        wr.Write(en);
                    }
                }
                catch
                {
                    MessageBox.Show("영문 파일의 저장에 실패하였습니다.");
                }
                try
                {
                    using (StreamWriter wr = new StreamWriter(ResourcePath + Properties.Settings.Default.JpnFileName + ".xaml"))
                    {
                        ReplaceCarriageReturns_String(ref jp);
                        wr.Write(jp);
                    }
                }
                catch
                {
                    MessageBox.Show("일문 파일의 저장에 실패하였습니다.");
                }
                try
                {
                    using (StreamWriter wr = new StreamWriter(ResourcePath + Properties.Settings.Default.ChnsFileName + ".xaml"))
                    {
                        ReplaceCarriageReturns_String(ref chns);
                        wr.Write(chns);
                    }
                }
                catch
                {
                    MessageBox.Show("중문 파일의 저장에 실패하였습니다.");
                }
            }
            catch
            {
                MessageBox.Show("저장 과정 도중 오류가 발생했습니다. 파일이 다른 프로그램에서 열려있지는 않은지 봐주십시오.");
            }
        }

        private void SaveFiles(List<string> strdic)
        {
            try
            {
                for(int i=0;i<LangList.Count;i++)
                {
                    try
                    {
                        using (StreamWriter wr = new StreamWriter(ResourcePath + LangList[i] + ".xaml"))
                        {
                            strdic[i]= ReplaceCarriageReturns_String(strdic[i]);
                            wr.Write(strdic[i]);
                        }
                    }
                    catch
                    {
                        MessageBox.Show(LangList[i]+".xaml 파일의 저장에 실패하였습니다.");
                    }
                }
            }
            catch
            {
                MessageBox.Show("저장 과정 도중 오류가 발생했습니다. 파일이 다른 프로그램에서 열려있지는 않은지 봐주십시오.");
            }
        }

        public void SaveFiles(Dictionary<string,XDocument> xDocs)
        {
            List<string> param = new List<string>();
            try
            {
                foreach(string x in LangList)
                {
                    xDocs[x]= ReplaceCarriageReturns_XDoc(xDocs[x]);
                    param.Add(xDocs[x].ToString());
                }
            }
            catch (Exception e)
            {
                App.LoggerEx.Warn(e.Message);
            }
            SaveFiles(param);
        }

        public static void SaveFiles(XDocument kr, XDocument en, XDocument jp, XDocument ch)
        {
            try
            {
                ReplaceCarriageReturns_XDoc(ref kr);
                ReplaceCarriageReturns_XDoc(ref en);
                ReplaceCarriageReturns_XDoc(ref jp);
                ReplaceCarriageReturns_XDoc(ref ch);
            }
            catch (Exception e)
            {
                App.LoggerEx.Warn(e.Message);
            }
            SaveFiles(kr.ToString(), en.ToString(), jp.ToString(), ch.ToString());
        }

        private void GetAttributesFromID(string ID)
        {
            ItemsA1.Clear();
            nowindex_a1 = null;
            nowindex_a2 = null;
            try
            {
                List<XAttribute> allattributes = GetElementAttributesbyID(Properties.Settings.Default.KorFileName, ID);
                foreach (var x in allattributes)
                {
                    ItemsA1.Add(new AttributeItem() {
                        Content = x
                    });
                }
            }
            catch
            {

            }
        }

        private void ReceiveMessage(ResetMessage action)
        {
            MakeTables();
        }

        #endregion

        #region command methods

        private void ExecuteAddCommand()
        {
            Dictionary<string, XElement[]> elements = new Dictionary<string, XElement[]>();
            Dictionary<string, XDocument> xDocs = new Dictionary<string, XDocument>();
            string message = "";
            string message2 = "";
            if (ID != "" && ID != string.Empty && ID != null)
            { 
                for(int i = 0; i < LangList.Count; i++)
                {
                    if(InputBoxes[i].Content!=string.Empty && InputBoxes[i].Content != null)
                    {
                        XDocument xDoc = null;
                        if (AddElementwithDefaultKey(LangList[i], ID, InputBoxes[i].Content, ref xDoc) == true)
                        {
                            message += LangList[i] + ", ";
                            xDocs.Add(LangList[i],xDoc);
                        }
                        else MessageBox.Show("같은 ID를 가진 리소스가 " + LangList[i] + ".xaml 파일에 존재합니다.");
                    }
                }
                if (message.Length > 3)
                {
                    string finalmessage = message.Substring(0, message.Length - 2) + " 파일에 저장했습니다.";
                    if (message2.Length > 3) finalmessage += "\n\n" + message2.Substring(0, message2.Length - 2) + "파일은 공백값으로 처리했습니다.";
                    SaveFiles(xDocs);
                    MessageBox.Show(finalmessage);
                    Messenger.Default.Send<ResetMessage>(new ResetMessage());
                }
                else
                {
                    MessageBox.Show("최소 하나의 언어에는 값을 입력해주시기 바랍니다.");
                }
            }
            else
            {
                MessageBox.Show("ID를 공백문자로 만들 수 없습니다.");
            }
        }

        private void ExecuteModifyCommand()
        {
            Dictionary<string, XDocument> xDocs = new Dictionary<string, XDocument>();

            try
            {
                for(int i = 0; i < LangList.Count; i++)
                {
                    XDocument xDoc = null;
                    ModifyElementwithIDandValue(LangList[i], ID, InputBoxes[i].Content, ref xDoc);
                    xDocs.Add(LangList[i],xDoc);
                }
                SaveFiles(xDocs);
                MessageBox.Show("성공적으로 요소들을 수정하였습니다.");
                Messenger.Default.Send<ResetMessage>(new ResetMessage());
            }
            catch
            {
                MessageBox.Show("ID 검색 과정에서 오류가 발생했습니다. xaml 파일들을 다시 한번 점검해주십시오.");
            }
        }

        private void ExecuteDeleteCommand()
        {
            Dictionary<string, XDocument> xDocs = new Dictionary<string, XDocument>();

            int successes = 0;
            string nowid = "";

            if (nowindex != null)
            {
                nowid = (string)nowindex["ID"];
            }
            else if (ID != null && ID!=string.Empty && ID!="" && ID.Length!=0)
            {
                nowid = ID;
            }
            else
            {
                MessageBox.Show("삭제할 요소를 선택하거나 ID를 입력해주십시오.");
                return;
            }
            try
            {
                foreach(string lang in LangList)
                {
                    XDocument xDoc = null;
                    if (DeleteElementbyID(lang, nowid, ref xDoc) == false)
                    {
                        MessageBox.Show(string.Format("{0}.xaml 파일에서 해당 키 값을 찾지 못했습니다.", lang));
                    }
                    else
                    {
                        xDocs.Add(lang, xDoc);
                        successes++;
                    }
                }

                if (successes > 1)
                {
                    SaveFiles(xDocs);
                    MessageBox.Show("해당 ID(" + nowid + ") 를 성공적으로 삭제하였습니다.");
                    Messenger.Default.Send<ResetMessage>(new ResetMessage());
                }
                else
                {
                    return;
                }
            }
            catch
            {
                MessageBox.Show("ID 검색 과정에서 오류가 발생했습니다. xaml 파일들을 다시 한번 점검해주십시오.");
            }
           
        }

        private void ExecuteClickCommand()
        {
            ID = (string)nowindex["ID"];
            for(int i = 0; i < LangList.Count; i++)
            {
                try
                {
                    InputBoxes[i].Content = (string)nowindex[LangList[i]];
                }
                catch
                {
                    InputBoxes[i].Content = "NULL";
                }
            }
            if (IsAttrsUsing) GetAttributesFromID((string)nowindex["ID"]);
        }

        private void ExecuteSearchCommand()
        {
            for(int i=0;i<LangList.Count;i++)
            {
                InputBoxes[i].Content = GetValuefromID(LangList[i], ID);
            }
            try
            {
                nowindex = DataItems.DefaultView.FindRows(ID).Single();
            }
            catch
            {
                nowindex = null;
            }
        }

        private void ExecuteAddCommandA1()
        {
            if (nowindex_a2 == null)
            {
                MessageBox.Show("삽입할 어트리뷰트를 선택해주세요!");
                return;
            }

            if ((from w in ItemsA1 where w.Content.Name==nowindex_a2.Content.Name select w).Count()>0)
            {
                MessageBox.Show("동일한 어트리뷰트의 중복 삽입은 불가능합니다!");
                return;
            }

            Dictionary<string, XDocument> xDocs = new Dictionary<string, XDocument>();
            XAttribute target = nowindex_a2.Content;
            try
            {
                foreach(var name in LangList)
                {
                    XDocument xDoc = null;
                    AddAttributefromElementbyID(name, ID, target, ref xDoc);
                    xDocs.Add(name, xDoc);
                }
                SaveFiles(xDocs);
                MessageBox.Show("성공적으로 요소들을 수정하였습니다.");
                Messenger.Default.Send<ResetMessage>(new ResetMessage());
                GetAttributesFromID(ID);
            }
            catch
            {
                MessageBox.Show("ID 검색 과정에서 오류가 발생했습니다. xaml 파일들을 다시 한번 점검해주십시오.");
            }
        }

        private void ExecuteDeleteCommandA1()
        {
            if (nowindex_a1 == null)
            {
                MessageBox.Show("삭제할 어트리뷰트를 선택해주세요!");
                return;
            }

            if (nowindex_a1.Content.Name.LocalName.Contains("Key"))
            {
                MessageBox.Show("키 값의 삭제는 불가능합니다!");
                return;
            }
            
            Dictionary<string, XDocument> xDocs = new Dictionary<string, XDocument>();
            XAttribute target = nowindex_a1.Content;
            try
            {
                foreach (var name in LangList)
                {
                    XDocument xDoc = null;
                    DeleteAttributefromElementbyID(name, ID, target, ref xDoc);
                    xDocs.Add(name, xDoc);
                }
                SaveFiles(xDocs);
                MessageBox.Show("성공적으로 요소들을 수정하였습니다.");
                Messenger.Default.Send<ResetMessage>(new ResetMessage());
                GetAttributesFromID(ID);
            }
            catch
            {
                MessageBox.Show("ID 검색 과정에서 오류가 발생했습니다. xaml 파일들을 다시 한번 점검해주십시오.");
            }
            
        }

        private void ExecuteOpenAutoEditCommand()
        {
            WorkerViewModel = null;
            new View.AutoEditView().ShowDialog();
        }

        #endregion command methods
    }

    public class InverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (((bool)value)) return false;
            else return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}