using System.Text;
using CommunityToolkit.Mvvm.Messaging;
using Tigrin.Litedb;
using Tigrin.Models;

namespace Tigrin.Views;

public partial class TransactionEdit : ContentPage
{

    private Transaction _transaction;
    private ITransactionRepository _repository;

    public TransactionEdit(ITransactionRepository repository)
    {
        InitializeComponent();
        _repository = repository;
    }

    public void SetTransactionToEdit(Transaction transaction)
    {
        _transaction = transaction;

        if (transaction.Type == TransactionType.Income)
            RadioIncome.IsChecked = true;
        else
            RadioExpense.IsChecked = true;

        EntryName.Text = transaction.Name;
        DatePic.Date = transaction.Date.Date;
        EntryValue.Text = transaction.Value.ToString();

        InitializeComponent();
        BindingContext = transaction;
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
    }

    private void SaveTrasnsactionInDb()
    {
        Transaction transaction = new Transaction()
        {
            Id = _transaction.Id,
            Name = EntryName.Text,
            Value = double.Parse(EntryValue.Text),
            Date = DatePic.Date,
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expense,
        };

        _repository.Update(transaction);
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
        if (!string.IsNullOrEmpty(EntryValue.Text) && !double.TryParse(EntryValue.Text, out result))
        {
            valid = false;
            sb.AppendLine("Valor é inválido.");
        }

        if (valid == false)
        {
            LabelError.IsVisible = true;
            LabelError.Text = sb.ToString();
        }

        return valid;

    }
}