using JxStock.Models;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
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

        private List<Stocks> _dataList = new List<Stocks>();
        public List<Stocks> DataList
        {
            get { return _dataList; }
            set { SetProperty(ref _dataList, value); }
        }

        #endregion

        #region command

        public ReactiveCommand UpdateUnitPriceCommand { get; } = new ReactiveCommand();
        public ReactiveCommand RegistCommand { get; } = new ReactiveCommand();

        #endregion

        public MainWindowViewModel()
        {
            UpdateUnitPriceCommand.Subscribe(_ => UpdateUnitPrice());
            RegistCommand.Subscribe(_ => RegistStocks());
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
                    UnitPrice = UnitPrice.Value,
                    ImagePath = ImagePath.Value,
                    Category = Category.Value,
                };

                DataBaseManager.Regist(stock);
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

        private void UpdateGrid()
        {

        }
    }
}
