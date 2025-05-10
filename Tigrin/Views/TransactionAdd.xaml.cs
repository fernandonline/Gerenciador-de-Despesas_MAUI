using System.Text;
using CommunityToolkit.Mvvm.Messaging;
using Tigrin.Litedb;
using Tigrin.Models;

namespace Tigrin.Views;

public partial class TransactionAdd : ContentPage
{
    private ITransactionRepository _repository;
	public TransactionAdd(ITransactionRepository repository)
	{
		InitializeComponent();
        _repository = repository;
    }

    private void TapGestureRecognizerTappedToClose(object sender, TappedEventArgs e)
    {
		Navigation.PopModalAsync();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (IsValidData() == false) return;
        SaveTrasnsactionInDb();
        Navigation.PopModalAsync();

        WeakReferenceMessenger.Default.Send<string>(string.Empty);

        var count = _repository.GetAll().Count;
        App.Current.MainPage.DisplayAlert("Mensagem!", $"Total de transações: {count}", "Ok");
    }

    private void SaveTrasnsactionInDb()
    {
        Transaction transaction = new Transaction()
        {
            Name = EntryName.Text,
            Value = double.Parse(EntryValue.Text),
            Date = DatePic.Date,
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expense,
        };

        _repository.Add(transaction);
    }

    private bool IsValidData()
    {
        bool valid = true;
        StringBuilder sb = new StringBuilder();

        if (string.IsNullOrEmpty(EntryName.Text) || string.IsNullOrWhiteSpace(EntryName.Text))
        {
            valid = false;
            sb.AppendLine("Nome deve ser preenchido!.");
        }
        if (string.IsNullOrEmpty(EntryValue.Text) || string.IsNullOrWhiteSpace(EntryValue.Text))
        {
            valid = false;
            sb.AppendLine("O valor deve ser preenchido!.");
        }

        double result;
        if(!string.IsNullOrEmpty(EntryValue.Text) && !double.TryParse(EntryValue.Text, out result))
        {
            valid = false;
            sb.AppendLine("Valor é inválido.");
        }

        if(valid == false)
        {
            LabelError.IsVisible = true;
            LabelError.Text = sb.ToString();
        }

        return valid;

    }
}