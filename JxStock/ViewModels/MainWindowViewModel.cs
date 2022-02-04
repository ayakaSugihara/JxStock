using JxStock.Models;
using Microsoft.Win32;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace JxStock.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region property

        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>("JxStock");
        public ReactiveProperty<string> Name { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<double> Price { get; } = new ReactiveProperty<double>();
        public ReactiveProperty<double> Quantity { get; } = new ReactiveProperty<double>();
        public ReactiveProperty<double> UnitPrice { get; } = new ReactiveProperty<double>();
        public ReactiveProperty<string> ImagePath { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Category { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<int> CategoryIndex { get; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> Index { get; } = new ReactiveProperty<int>(-1);
        public ReactiveProperty<string> ImageSource { get; } = new ReactiveProperty<string>();

        private List<Stocks> _dataList = new List<Stocks>();
        public List<Stocks> DataList
        {
            get { return _dataList; }
            set { SetProperty(ref _dataList, value); }
        }

        /// <summary>
        /// 画像の保存先
        /// </summary>
        private readonly string ImageFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Image");

        #endregion

        #region command

        public ReactiveCommand UpdateUnitPriceCommand { get; } = new ReactiveCommand();
        public ReactiveCommand RegistCommand { get; } = new ReactiveCommand();
        public ReactiveCommand DeleteCommand { get; } = new ReactiveCommand();
        public ReactiveCommand MasterItemSelectedChangeCommand { get; } = new ReactiveCommand();
        public ReactiveCommand AttachImageCommand { get; } = new ReactiveCommand();

        #endregion

        public MainWindowViewModel()
        {
            UpdateUnitPriceCommand.Subscribe(_ => UpdateUnitPrice());
            RegistCommand.Subscribe(_ => RegistStocks());
            DeleteCommand.Subscribe(_ => DeleteStocks());
            MasterItemSelectedChangeCommand.Subscribe(_ => MasterItemSelectedChange());
            AttachImageCommand.Subscribe(_ => AttachImage());
            DataList = DataBaseManager.Select();
        }

        /// <summary>
        /// 在庫を入力値で更新する
        /// </summary>
        private void RegistStocks()
        {
            try
            {
                var stock = new Stocks()
                {
                    Name = Name.Value,
                    Price = Price.Value,
                    Quantity = Quantity.Value,
                    UnitPrice = UnitPrice.Value,
                    ImagePath = ImagePath.Value,
                    Category = Category.Value,
                };

                if (Index.Value < 0)
                {
                    DataBaseManager.Insert(stock);
                }
                else
                {
                    var data = DataList[Index.Value];
                    stock.Id = data.Id;
                    DataBaseManager.Update(stock);
                }

                DataList = DataBaseManager.Select();
                Index.Value = -1;
                ClearMasterInput();
            }
            catch (Exception ex)
            {
                CatchException(ex);
            }
        }

        /// <summary>
        /// 在庫の削除
        /// </summary>
        private void DeleteStocks()
        {
            if (Index.Value < 0)
            {
                return;
            }

            try
            {
                var stock = DataList[Index.Value];
                DataBaseManager.Delete(stock.Id);
                DataList = DataBaseManager.Select();
                Index.Value = -1;
                ClearMasterInput();
            }
            catch (Exception ex)
            {
                CatchException(ex);
            }
        }

        /// <summary>
        /// 単価の計算
        /// </summary>
        private void UpdateUnitPrice()
        {
            try
            {
                UnitPrice.Value = Price.Value / Quantity.Value;
            }
            catch (Exception ex)
            {
                CatchException(ex);
            }
        }

        /// <summary>
        /// エラー時の対応
        /// </summary>
        /// <param name="ex"></param>
        private void CatchException(Exception ex)
        {
            MessageBox.Show(ex.Message, "error");
        }

        /// <summary>
        /// マスタ登録の入力項目をクリア
        /// </summary>
        private void ClearMasterInput()
        {
            Name.Value = string.Empty;
            CategoryIndex.Value = 0;
            Price.Value = 0;
            Quantity.Value = 0;
            UnitPrice.Value = 0;
            ImagePath.Value = string.Empty;
            ImageSource.Value = string.Empty;
        }

        /// <summary>
        /// マスタのグリッド選択
        /// </summary>
        private void MasterItemSelectedChange()
        {
            if (Index.Value < 0)
            {
                return;
            }

            var stock = DataList[Index.Value];
            Name.Value = stock.Name;
            Category.Value = stock.Category;
            Price.Value = stock.Price;
            Quantity.Value = stock.Quantity;
            UnitPrice.Value = stock.UnitPrice;
            ImagePath.Value = stock.ImagePath;

            if(string.IsNullOrEmpty(stock.ImagePath))
            {
                ImageSource.Value = stock.ImagePath;
            }
            else
            {
                string path = Path.Combine(ImageFolder, stock.ImagePath);
                ImageSource.Value = path;
            }
        }

        /// <summary>
        /// 画像ファイルの添付
        /// </summary>
        private void AttachImage()
        {
            try
            {
                // ファイル選択
                OpenFileDialog ofd = new OpenFileDialog()
                {
                    Multiselect = false,
                };

                bool? result = ofd.ShowDialog();

                if (result != true)
                {
                    return;
                }

                // ファイル名を表示
                ImagePath.Value = Path.GetFileName(ofd.FileName);

                // 画像をコピー
                Directory.CreateDirectory(ImageFolder);
                string path = Path.Combine(ImageFolder, ImagePath.Value);

                if (File.Exists(path))
                {
                    return;
                }

                File.Copy(ofd.FileName, path);

                // 画像を表示
                ImageSource.Value = path;
            }
            catch (Exception ex)
            {
                CatchException(ex);
                ImagePath.Value = string.Empty;
            }
        }
    }
}
